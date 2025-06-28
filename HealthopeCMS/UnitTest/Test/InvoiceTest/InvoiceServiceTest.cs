using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Invoice.Response;
using ApiLayer.Models.Job;
using ApiLayer.Models.Order.Request;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.Invoice
{
    [TestClass]
    public class InvoiceServiceTest
    {
        private InvoiceService service;
        private Mock<IMapper> mapperMock;
        private Mock<IInvoiceRepository> invoiceRepositoryMock;
        private Mock<IHttpService> httpServiceMock;

        [TestInitialize]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            httpServiceMock = new Mock<IHttpService>();
            service = new InvoiceService(mapperMock.Object, invoiceRepositoryMock.Object, httpServiceMock.Object);
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
            InvoiceTrackNumber invoiceTrackNumber = new InvoiceTrackNumber()
            {
                TrackPrefix = "AA",
                StartNumber = 1,
                EndNumber = 1000,
                InvoicePeriod = 1151
            };

            bool successFlag = true;

            // Mock 設定
            mapperMock.Setup(s => s.Map<InvoiceTrackNumber>(addInvoiceTrackNumberDto)).Returns(invoiceTrackNumber);
            invoiceRepositoryMock.Setup(s
                => s.AddInvoiceTrackNumber(invoiceTrackNumber)).Returns(successFlag);

            // Act
            bool result = service.AddInvoiceTrackNumber(addInvoiceTrackNumberDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 新增字軌_失敗_回傳失敗()
        {
            // Arrange
            RequestAddInvoiceTrackNumberDto addInvoiceTrackNumberDto = new RequestAddInvoiceTrackNumberDto()
            {
                TrackPrefix = "AA",
                StartNumber = 1,
                EndNumber = 1000,
                InvoicePeriod = 1151
            };
            InvoiceTrackNumber invoiceTrackNumber = new InvoiceTrackNumber()
            {
                TrackPrefix = "AA",
                StartNumber = 1,
                EndNumber = 1000,
                InvoicePeriod = 1151
            };

            bool successFlag = false;

            // Mock 設定
            mapperMock.Setup(s => s.Map<InvoiceTrackNumber>(addInvoiceTrackNumberDto)).Returns(invoiceTrackNumber);
            invoiceRepositoryMock.Setup(s
                => s.AddInvoiceTrackNumber(invoiceTrackNumber)).Returns(successFlag);

            // Act
            bool result = service.AddInvoiceTrackNumber(addInvoiceTrackNumberDto);

            // Assert
            Assert.IsFalse(result);
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

            List<ResponseGetInvoiceTrackNumberDto> responseGet = new List<ResponseGetInvoiceTrackNumberDto>() {
                new ResponseGetInvoiceTrackNumberDto(){
                    InvoiceTrackNumberId = 1,
                    StartNumber = 1,
                    EndNumber = 1000,
                    CurrentNumber = 1,
                    InvoicePeriod = 1141,
                    Status = 1,
                    TrackPrefix = "AB",
                    UpdateTime = DateTime.Now,
                }
            };

            List<InvoiceTrackNumber> invoiceTrackNumbers = new List<InvoiceTrackNumber>()
            {
                new InvoiceTrackNumber()
                {
                    InvoiceTrackNumberId = 1,
                    StartNumber = 1,
                    EndNumber = 1000,
                    CurrentNumber = 1,
                    InvoicePeriod = 1141,
                    Status = 1,
                    TrackPrefix = "AB",
                    UpdateTime = DateTime.Now,
                }
            };

            int totalPage = 1;

            // Mock 設定
            mapperMock.Setup(s => s.Map<List<ResponseGetInvoiceTrackNumberDto>>(invoiceTrackNumbers)).Returns(responseGet);
            invoiceRepositoryMock.Setup(s
                => s.GetInvoiceTrackNumber(getInvoiceTrackNumberDto)).Returns((invoiceTrackNumbers, totalPage));

            // Act
            ResponseGetInvoiceTrackNumberListDto result = service.GetInvoiceTrackNumber(getInvoiceTrackNumberDto);

            // Assert
            CollectionAssert.AreEqual(responseGet, result.InvoiceTrackNumberList);
        }

        [TestMethod]
        public void 取得字軌清單_失敗_回傳空資料()
        {
            // Arrange
            RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto = new RequestGetInvoiceTrackNumberDto()
            {
                Time = true,
                Status = 3,
                Page = 1,
                RecordPerPage = 8
            };

            List<ResponseGetInvoiceTrackNumberDto> responseGet = null;
            List<InvoiceTrackNumber> invoiceTrackNumbers = null;
            int totalPage = 1;

            // Mock 設定
            mapperMock.Setup(s => s.Map<List<ResponseGetInvoiceTrackNumberDto>>(invoiceTrackNumbers)).Returns(responseGet);
            invoiceRepositoryMock.Setup(s
                => s.GetInvoiceTrackNumber(getInvoiceTrackNumberDto)).Returns((invoiceTrackNumbers, totalPage));

            // Act
            ResponseGetInvoiceTrackNumberListDto result = service.GetInvoiceTrackNumber(getInvoiceTrackNumberDto);

            // Assert
            CollectionAssert.AreEqual(responseGet, result.InvoiceTrackNumberList);
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

            InvoiceTrackNumber invoiceTrackNumber = new InvoiceTrackNumber()
            {
                Status = 2,
                InvoiceTrackNumberId = 1,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            mapperMock.Setup(s => s.Map<InvoiceTrackNumber>(editInvoiceTrackNumberStatusDto)).Returns(invoiceTrackNumber);
            invoiceRepositoryMock.Setup(s
                => s.EditInvoiceTrackNumberStatus(invoiceTrackNumber)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditInvoiceTrackNumberStatus(editInvoiceTrackNumberStatusDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 修改字軌狀態_失敗_資料已被異動()
        {
            // Arrange
            RequestEditInvoiceTrackNumberStatusDto editInvoiceTrackNumberStatusDto = new RequestEditInvoiceTrackNumberStatusDto()
            {
                Status = 2,
                InvoiceTrackNumberId = 1,
                UpdateTime = DateTime.Now,
            };

            InvoiceTrackNumber invoiceTrackNumber = new InvoiceTrackNumber()
            {
                Status = 2,
                InvoiceTrackNumberId = 1,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            mapperMock.Setup(s => s.Map<InvoiceTrackNumber>(editInvoiceTrackNumberStatusDto)).Returns(invoiceTrackNumber);
            invoiceRepositoryMock.Setup(s
                => s.EditInvoiceTrackNumberStatus(invoiceTrackNumber)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditInvoiceTrackNumberStatus(editInvoiceTrackNumberStatusDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.HasBeenModified);
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
            invoiceRepositoryMock.Setup(s
                => s.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto.InvoiceTrackNumberId)).Returns(successFlag);

            // Act
            bool result = service.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto);

            // Assert
            Assert.IsTrue(result);
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
            invoiceRepositoryMock.Setup(s
                => s.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto.InvoiceTrackNumberId)).Returns(successFlag);

            // Act
            bool result = service.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void 完成訂單和補印發票_成功_回傳成功()
        {
            // Arrange
            RequestOrderIdDto orderIdDto = new RequestOrderIdDto()
            {
                OrderId = 1,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            ElectronicInvoice electronicInvoice = new ElectronicInvoice
            {
                ElectronicInvoiceId = 1,
                InvoiceNumber = "EQ-12345677",
                RandomNumber = "1234",
                TotalAmount = 3000
            };

            string planName = "一個月會籍";

            RequestPrintInvoiceDto printInvoiceDto = new RequestPrintInvoiceDto()
            {
                ElectronicInvoiceId = electronicInvoice.ElectronicInvoiceId,
                InvoiceNumber = electronicInvoice.InvoiceNumber,
                PlanName = planName,
                RandomNumber = electronicInvoice.RandomNumber,
                TotalAmount = electronicInvoice.TotalAmount,
            };

            // Mock 設定
            invoiceRepositoryMock.Setup(s
                => s.EditOrderStateAndGetInvoiceNumber(orderIdDto.OrderId)).Returns((errorCodeNumber, electronicInvoice, planName));

            // Act
            (ErrorCodeDefine errorCode, RequestPrintInvoiceDto printInvoiceDto) result = service.EditOrderStateAndGetInvoiceNumber(orderIdDto);

            // Assert
            Assert.AreEqual(result.errorCode, (ErrorCodeDefine)errorCodeNumber);
        }

        [TestMethod]
        public void 完成訂單和補印發票_失敗_回傳失敗()
        {
            // Arrange
            RequestOrderIdDto orderIdDto = new RequestOrderIdDto()
            {
                OrderId = 1,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.ServerError;

            ElectronicInvoice electronicInvoice = null;

            string planName = "一個月會籍";

            // Mock 設定
            invoiceRepositoryMock.Setup(s
                => s.EditOrderStateAndGetInvoiceNumber(orderIdDto.OrderId)).Returns((errorCodeNumber, electronicInvoice, planName));

            // Act
            (ErrorCodeDefine errorCode, RequestPrintInvoiceDto printInvoiceDto) result = service.EditOrderStateAndGetInvoiceNumber(orderIdDto);

            // Assert
            Assert.AreEqual(result.errorCode, (ErrorCodeDefine)errorCodeNumber);
        }

        [TestMethod]
        public void 作廢發票_成功_回傳成功()
        {
            // Arrange
            RequestOrderIdDto orderIdDto = new RequestOrderIdDto()
            {
                OrderId = 1,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            invoiceRepositoryMock.Setup(s
                => s.VoidInvoice(orderIdDto.OrderId)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.VoidInvoice(orderIdDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 作廢發票_失敗_回傳失敗()
        {
            // Arrange
            RequestOrderIdDto orderIdDto = new RequestOrderIdDto()
            {
                OrderId = 1,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.ModifiedFailed;

            // Mock 設定
            invoiceRepositoryMock.Setup(s
                => s.VoidInvoice(orderIdDto.OrderId)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.VoidInvoice(orderIdDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.ModifiedFailed);
        }

        [TestMethod]
        public void 折讓發票_成功_回傳成功()
        {
            // Arrange
            RequestOrderIdDto orderIdDto = new RequestOrderIdDto()
            {
                OrderId = 1,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            invoiceRepositoryMock.Setup(s
                => s.DiscountInvoice(orderIdDto.OrderId)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.DiscountInvoice(orderIdDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 折讓發票_失敗_回傳失敗()
        {
            // Arrange
            RequestOrderIdDto orderIdDto = new RequestOrderIdDto()
            {
                OrderId = 1,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.ModifiedFailed;

            // Mock 設定
            invoiceRepositoryMock.Setup(s
                => s.DiscountInvoice(orderIdDto.OrderId)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.DiscountInvoice(orderIdDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.ModifiedFailed);
        }
    }
}
