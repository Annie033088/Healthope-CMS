using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.Job;
using ApiLayer.Models.Other;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.PaymentTest
{
    [TestClass]
    public class PaymentServiceTest
    {
        private Mock<IHttpService> httpServiceMock;
        private Mock<ITransactionRepository> transactionRepositoryMock;
        private Mock<IOrderRepository> orderRepositoryMock;
        private Mock<IJobDispatcher> jobDispatcherMock;
        private PaymentService service;

        [TestInitialize]
        public void Setup()
        {
            httpServiceMock = new Mock<IHttpService>();
            transactionRepositoryMock = new Mock<ITransactionRepository>();
            orderRepositoryMock = new Mock<IOrderRepository>();
            jobDispatcherMock = new Mock<IJobDispatcher>();
            service = new PaymentService(httpServiceMock.Object, transactionRepositoryMock.Object,
                orderRepositoryMock.Object, jobDispatcherMock.Object);
        }

        [TestMethod]
        public async Task 刷卡付款_成功_回傳成功()
        {
            // Arrange
            RequestCardPaymentDto requestCardPaymentDto = new RequestCardPaymentDto
            {
                Amount = 2000,
                OrderId = 5,
                TransactionId = ""
            };
            int creditCardTransactionId = 10;
            RequestPayByCardDto payByCardDto = new RequestPayByCardDto
            {
                CardReaderId = "ABC-1",
                CoachId = 22,
                OrderId = 5,
                UpdateTime = DateTime.UtcNow,
            };

            Dictionary<string, string> dictionaryContent = new Dictionary<string, string>
            {
                { "AuthCode", "ABND221" },
                { "CardLastFour", "1234" },
                { "CardType", "VISA" },
                { "Status", "true" },
                { "TransactionId", Guid.NewGuid().ToString() },
            };
            string json = JsonConvert.SerializeObject(dictionaryContent);

            string responseString = json;
            ResponseCardPaymentDto response = JsonConvert.DeserializeObject<ResponseCardPaymentDto>(responseString);

            bool editTransactionFlag = true;

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            DBResponsePaymentDto dbResponse = new DBResponsePaymentDto
            {
                ElectronicInvoiceId = creditCardTransactionId,
            };

            // Mock 設定
            httpServiceMock.Setup(s => s.SendPostAsync(It.IsAny<string>(), It.IsAny<StringContent>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(responseString);
            transactionRepositoryMock.Setup(s => s.EditCreditCardTransactionStatusSuccess(It.IsAny<CreditCardTransaction>()))
                .Returns(editTransactionFlag);
            orderRepositoryMock.Setup(s => s.PayByCardSuccess(It.IsAny<RequestPayByCardDto>())).Returns((errorCodeNumber, dbResponse));
            jobDispatcherMock.Setup(s => s.Enqueue<RequestPrintInoviceJob, RequestPrintInvoiceDto>(It.IsAny<RequestPrintInvoiceDto>()));

            // Act
            (ErrorCodeDefine errorCode, DBResponsePaymentDto dbResponse) result = await service.PayByCard(
                requestCardPaymentDto, creditCardTransactionId, payByCardDto);

            // Assert
            Assert.AreEqual(result.errorCode, ErrorCodeDefine.Success);
        }

        [TestMethod]
        public async Task 刷卡付款_失敗_回傳失敗()
        {
            // Arrange
            RequestCardPaymentDto requestCardPaymentDto = new RequestCardPaymentDto
            {
                Amount = 2000,
                OrderId = 5,
                TransactionId = ""
            };
            int creditCardTransactionId = 10;
            RequestPayByCardDto payByCardDto = new RequestPayByCardDto
            {
                CardReaderId = "ABC-1",
                CoachId = 22,
                OrderId = 5,
                UpdateTime = DateTime.UtcNow,
            };

            Dictionary<string, string> dictionaryContent = new Dictionary<string, string>
            {
                { "AuthCode", "ABND221" },
                { "CardLastFour", "1234" },
                { "CardType", "VISA" },
                { "Status", "false" },
                { "TransactionId", Guid.NewGuid().ToString() },
            };
            string json = JsonConvert.SerializeObject(dictionaryContent);

            string responseString = json;
            ResponseCardPaymentDto response = JsonConvert.DeserializeObject<ResponseCardPaymentDto>(responseString);

            bool successEditStatusFail = true;

            DBResponsePaymentDto dbResponse = new DBResponsePaymentDto
            {
                ElectronicInvoiceId = creditCardTransactionId,
            };

            // Mock 設定
            httpServiceMock.Setup(s => s.SendPostAsync(It.IsAny<string>(), It.IsAny<StringContent>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(responseString);
            transactionRepositoryMock.Setup(s => s.EditCreditCardTransactionStatusFail(It.IsAny<CreditCardTransaction>()))
                .Returns(successEditStatusFail);

            // Act
            (ErrorCodeDefine errorCode, DBResponsePaymentDto dbResponse) result = await service.PayByCard(
                requestCardPaymentDto, creditCardTransactionId, payByCardDto);

            // Assert
            Assert.AreEqual(result.errorCode, ErrorCodeDefine.PayFailed);
        }
    }
}
