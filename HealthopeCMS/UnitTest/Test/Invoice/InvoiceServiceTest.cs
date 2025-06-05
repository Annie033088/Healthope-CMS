using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Job;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models;
using ApiLayer.Service;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using UnitTest.utils;
using DomainLayer.Models;
using ApiLayer.Models.Invoice.Response;
using PersistentLayer.Models;

namespace UnitTest.Test.Invoice
{
    [TestClass]
    public class InvoiceServiceTest
    {
        private InvoiceService service;
        private Mock<IMapper> mapperMock;
        private Mock<IInvoiceRepository> invoiceRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            service = new InvoiceService(mapperMock.Object, invoiceRepositoryMock.Object);
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
    }
}
