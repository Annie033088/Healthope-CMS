using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTest.utils;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Term.Response;
using PersistentLayer.Models;
using ApiLayer.Models.Invoice.Response;

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
    }
}
