using System;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Invoice.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.Invoice
{
    [TestClass]
    public class InvoiceControllerTest
    {
        private InvoiceController controller;
        private Mock<IInvoiceService> invoiceServiceMock;

        [TestInitialize]
        public void Setup()
        {
            invoiceServiceMock = new Mock<IInvoiceService>();
            controller = new InvoiceController(invoiceServiceMock.Object);
        }

        [TestMethod]
        public void 新增字軌_成功_回傳成功()
        {
            // Arrange
            RequestAddInvoiceTrackNumberDto addInvoiceTrackNumberDto = new RequestAddInvoiceTrackNumberDto()
            {
                TrackPrefix = "AA",
                StartNumber = 1,
                EndNumber = 1000,
                InvoicePeriod = 1151
            };

            bool successFlag = true;

            // Mock 設定
            invoiceServiceMock.Setup(s
                => s.AddInvoiceTrackNumber(addInvoiceTrackNumberDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.AddInvoiceTrackNumber(addInvoiceTrackNumberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 新增字軌_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestAddInvoiceTrackNumberDto addInvoiceTrackNumberDto = new RequestAddInvoiceTrackNumberDto()
            {
                TrackPrefix = "AA",
                StartNumber = 1,
                EndNumber = 1000,
                InvoicePeriod = 115
            };

            // Act
            IHttpActionResult result = controller.AddInvoiceTrackNumber(addInvoiceTrackNumberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得字軌清單_成功_回傳清單()
        {
            // Arrange
            RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto = new RequestGetInvoiceTrackNumberDto()
            {
                Time = true,
                Status = 3,
                Page = 1,
                RecordPerPage = 8
            };

            ResponseGetInvoiceTrackNumberListDto response = new ResponseGetInvoiceTrackNumberListDto();

            // Mock 設定
            invoiceServiceMock.Setup(s
                => s.GetInvoiceTrackNumber(getInvoiceTrackNumberDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetInvoiceTrackNumber(getInvoiceTrackNumberDto);

            // Assert
            ResponseIsEqual<ResponseGetInvoiceTrackNumberListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetInvoiceTrackNumberListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得字軌清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto = new RequestGetInvoiceTrackNumberDto()
            {
                Time = true,
                Status = 30,
                Page = 1,
                RecordPerPage = 8
            };

            // Act
            IHttpActionResult result = controller.GetInvoiceTrackNumber(getInvoiceTrackNumberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改字軌狀態_成功_回傳成功()
        {
            // Arrange
            RequestEditInvoiceTrackNumberStatusDto editInvoiceTrackNumberStatusDto = new RequestEditInvoiceTrackNumberStatusDto()
            {
                Status = 2,
                InvoiceTrackNumberId = 1,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            invoiceServiceMock.Setup(s
                => s.EditInvoiceTrackNumberStatus(editInvoiceTrackNumberStatusDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.EditInvoiceTrackNumberStatus(editInvoiceTrackNumberStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改字軌狀態_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestEditInvoiceTrackNumberStatusDto editInvoiceTrackNumberStatusDto = new RequestEditInvoiceTrackNumberStatusDto()
            {
                Status = 1,
                InvoiceTrackNumberId = 1,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditInvoiceTrackNumberStatus(editInvoiceTrackNumberStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 刪除字軌_成功_回傳成功()
        {
            // Arrange
            InvoiceTrackNumberIdDto invoiceTrackNumberIdDto = new InvoiceTrackNumberIdDto()
            {
                InvoiceTrackNumberId = 1,
            };

            bool successFlag = true;

            // Mock 設定
            invoiceServiceMock.Setup(s
                => s.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 刪除字軌_失敗_回傳失敗()
        {
            // Arrange
            InvoiceTrackNumberIdDto invoiceTrackNumberIdDto = new InvoiceTrackNumberIdDto()
            {
                InvoiceTrackNumberId = 1,
            };

            bool successFlag = false;

            // Mock 設定
            invoiceServiceMock.Setup(s
                => s.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DeleteFailed));
        }
    }
}
