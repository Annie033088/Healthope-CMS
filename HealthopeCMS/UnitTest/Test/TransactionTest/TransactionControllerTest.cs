using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.Response.PlanTemplate;
using ApiLayer.Models.Transaction.Request;
using ApiLayer.Models.Transaction.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.TransactionTest
{
    [TestClass]
    public class TransactionControllerTest
    {
        private TransactionController controller;
        private Mock<ITransactionService> transactionServiceMock;

        [TestInitialize]
        public void Setup()
        {
            transactionServiceMock = new Mock<ITransactionService>();
            controller = new TransactionController(transactionServiceMock.Object);
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

            ResponseGetTransactionListDto response = new ResponseGetTransactionListDto()
            {
                TransactionList = null,
                TotalPage = 1,
            };

            // Mock 設定
            transactionServiceMock.Setup(s
                => s.GetTransaction(getTransactionDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetTransaction(getTransactionDto);

            // Assert
            ResponseIsEqual<ResponseGetTransactionListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetTransactionListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得付款紀錄清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetTransactionDto getTransactionDto = new RequestGetTransactionDto()
            {
                Status = 1,
                Method = 10,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "amount", // 只允許 amount | time | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            // Act
            IHttpActionResult result = controller.GetTransaction(getTransactionDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
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

            // Mock 設定
            transactionServiceMock.Setup(s
                => s.GetCreditCardCashFlowData(transactionIdDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetCreditCardCashFlowData(transactionIdDto);

            // Assert
            ResponseIsEqual<ResponsetGetCreditCardCashFlowDto> responseIsEqual =
                new ResponseIsEqual<ResponsetGetCreditCardCashFlowDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得金流資訊_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestTransactionIdDto transactionIdDto = new RequestTransactionIdDto()
            {
                TransactionId = -1,
            };

            // Act
            IHttpActionResult result = controller.GetCreditCardCashFlowData(transactionIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
