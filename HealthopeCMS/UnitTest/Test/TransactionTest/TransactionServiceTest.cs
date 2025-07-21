using System;
using System.Collections.Generic;
using System.Linq;
using ApiLayer.Models.Transaction.Request;
using ApiLayer.Models.Transaction.Response;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.TransactionTest
{
    [TestClass]
    public class TransactionServiceTest
    {
        private TransactionService service;
        private Mock<ITransactionRepository> transactionRepositoryMock;
        private Mock<IMapper> mapperMock;

        [TestInitialize]
        public void Setup()
        {
            transactionRepositoryMock = new Mock<ITransactionRepository>();
            mapperMock = new Mock<IMapper>();
            service = new TransactionService(transactionRepositoryMock.Object, mapperMock.Object);
        }

        [TestMethod]
        public void 取得付款紀錄清單_成功_回傳清單()
        {
            // Arrange
            RequestGetTransactionDto getTransactionDto = new RequestGetTransactionDto()
            {
                Status = 1,
                Method = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "amount", // 只允許 amount | time | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            List<ResponseGetTransactionDto> TransactionList = new List<ResponseGetTransactionDto>
                {
                    new ResponseGetTransactionDto
                    {
                        TransactionId = 1,
                        OrderId = 1,
                        Status = 1,
                        Amount = 1000,
                        Method =2,
                        Time = DateTime.Now,
                    }
                };

            ResponseGetTransactionListDto responseGetTransactionList = new ResponseGetTransactionListDto
            {
                TotalPage = 1,
                TransactionList = TransactionList
            };

            // Mock 設定
            transactionRepositoryMock.Setup(s
                => s.GetTransaction(getTransactionDto)).Returns(responseGetTransactionList);

            // Act
            ResponseGetTransactionListDto result = service.GetTransaction(getTransactionDto);

            // Assert
            Assert.IsTrue(result.TransactionList.SequenceEqual(TransactionList));
        }

        [TestMethod]
        public void 取得付款紀錄清單_失敗_回傳空資料()
        {
            // Arrange
            RequestGetTransactionDto getTransactionDto = new RequestGetTransactionDto()
            {
                Status = 1,
                Method = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "amount", // 只允許 amount | time | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            ResponseGetTransactionListDto responseGetTransactionList = null;

            // Mock 設定
            transactionRepositoryMock.Setup(s
                => s.GetTransaction(getTransactionDto)).Returns(responseGetTransactionList);

            // Act
            ResponseGetTransactionListDto result = service.GetTransaction(getTransactionDto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void 取得金流資訊_成功_回傳清單()
        {
            // Arrange
            RequestTransactionIdDto transactionIdDto = new RequestTransactionIdDto()
            {
                TransactionId = 1,
            };

            ResponsetGetCreditCardCashFlowDto response = new ResponsetGetCreditCardCashFlowDto()
            {
                AuthCode = "",
                GatewayTransactionId = "dd"
            };

            PaymentTransaction transaction = new PaymentTransaction()
            {
                AuthCode = "",
                GatewayTransactionId = "dd"
            };

            // Mock 設定
            transactionRepositoryMock.Setup(s
                => s.GetCreditCardCashFlowData(transactionIdDto.TransactionId)).Returns(transaction);
            mapperMock.Setup(s => s.Map<ResponsetGetCreditCardCashFlowDto>(transaction)).Returns(response);

            // Act
            ResponsetGetCreditCardCashFlowDto result = service.GetCreditCardCashFlowData(transactionIdDto);

            // Assert
            Assert.AreEqual(response, result);
        }

        [TestMethod]
        public void 取得金流資訊_失敗_回傳空資料()
        {
            // Arrange
            RequestTransactionIdDto transactionIdDto = new RequestTransactionIdDto()
            {
                TransactionId = -1,
            };

            ResponsetGetCreditCardCashFlowDto response = null;

            PaymentTransaction transaction = null;

            // Mock 設定
            transactionRepositoryMock.Setup(s
                => s.GetCreditCardCashFlowData(transactionIdDto.TransactionId)).Returns(transaction);
            mapperMock.Setup(s => s.Map<ResponsetGetCreditCardCashFlowDto>(transaction)).Returns(response);

            // Act
            ResponsetGetCreditCardCashFlowDto result = service.GetCreditCardCashFlowData(transactionIdDto);

            // Assert
            Assert.IsNull(result);
        }
    }
}
