using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using ApiLayer.Models;
using ApiLayer.Service;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using UnitTest.utils;
using DomainLayer.Models;
using PersistentLayer.Models;
using System.Net;
using ApiLayer.Models.Job;
using System.Web.Http.Results;

namespace UnitTest.Test.OrderTest
{
    [TestClass]
    public class OrderServiceTest
    {
        private OrderService service;
        private Mock<IOrderRepository> orderRepositoryMock;
        private Mock<IMapper> mapperMock;
        private Mock<IJobDispatcher> jobDispatcherMock;

        [TestInitialize]
        public void Setup()
        {
            orderRepositoryMock = new Mock<IOrderRepository>();
            mapperMock = new Mock<IMapper>();
            jobDispatcherMock = new Mock<IJobDispatcher>();
            service = new OrderService(orderRepositoryMock.Object, mapperMock.Object, jobDispatcherMock.Object);
        }

        [TestMethod]
        public void 新增訂單_成功_回傳成功()
        {
            // Arrange
            DateTime time = DateTime.Now;
            RequestAddOrderDto addOrderDto = new RequestAddOrderDto()
            {
                MemberId = 1,
                Method = 1,
                PlanId = 1,
                PlanType = 1,
            };

            Order addOrder = new Order
            {
                MemberId = 1,
                Method = 1,
                PlanId = 1,
                PlanType = 1,
            };

            ResponseAddOrderDto response = new ResponseAddOrderDto()
            {
                OrderId = 1,
                UpdateTime = time,
            };

            string datePart = time.ToString("yyMMdd");
            int totalSeconds = (int)(time.TimeOfDay.TotalSeconds);
            string secondsPart = totalSeconds.ToString("D5"); // 補零到5位
            string memberPart = (addOrderDto.MemberId % 10_000_000).ToString("D7"); // 確保7位
            string orderNumberString = $"{datePart}{secondsPart}{memberPart}";
            long orderNumber = long.Parse(orderNumberString);

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            Order order = new Order
            {
                OrderId = 1,
                UpdateTime = time,
            };

            // Mock 設定
            orderRepositoryMock.Setup(s => s.AddOrder(addOrder, orderNumber)).Returns((order, errorCodeNumber));
            mapperMock.Setup(s => s.Map<Order>(addOrderDto)).Returns(addOrder);
            mapperMock.Setup(s => s.Map<ResponseAddOrderDto>(order)).Returns(response);

            // Act
            (ResponseAddOrderDto result, ErrorCodeDefine errorCode) = service.AddOrder(addOrderDto);

            // Assert
            Assert.AreEqual(response, result);
            Assert.AreEqual(errorCode, ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 新增訂單_失敗_回傳失敗()
        {
            // Arrange
            DateTime time = DateTime.Now;
            RequestAddOrderDto addOrderDto = new RequestAddOrderDto()
            {
                MemberId = 1,
                Method = 1,
                PlanId = 1,
                PlanType = 1,
            };

            Order addOrder = new Order
            {
                MemberId = 1,
                Method = 1,
                PlanId = 1,
                PlanType = 1,
            };

            ResponseAddOrderDto response = new ResponseAddOrderDto()
            {
                OrderId = 1,
                UpdateTime = time,
            };

            string datePart = time.ToString("yyMMdd");
            int totalSeconds = (int)(time.TimeOfDay.TotalSeconds);
            string secondsPart = totalSeconds.ToString("D5"); // 補零到5位
            string memberPart = (addOrderDto.MemberId % 10_000_000).ToString("D7"); // 確保7位
            string orderNumberString = $"{datePart}{secondsPart}{memberPart}";
            long orderNumber = long.Parse(orderNumberString);

            int errorCodeNumber = (int)ErrorCodeDefine.MemberBaned;

            Order order = new Order
            {
                OrderId = 1,
                UpdateTime = time,
            };

            // Mock 設定
            orderRepositoryMock.Setup(s => s.AddOrder(addOrder, orderNumber)).Returns((order, errorCodeNumber));
            mapperMock.Setup(s => s.Map<Order>(addOrderDto)).Returns(addOrder);
            mapperMock.Setup(s => s.Map<ResponseAddOrderDto>(order)).Returns(response);

            // Act
            (ResponseAddOrderDto result, ErrorCodeDefine errorCode) = service.AddOrder(addOrderDto);

            // Assert
            Assert.AreEqual(response, result);
            Assert.AreEqual(errorCode, ErrorCodeDefine.MemberBaned);
        }

        [TestMethod]
        public void 訂單用現金付款_成功_回傳成功()
        {
            // Arrange
            RequestPayByCashDto payByCashDto = new RequestPayByCashDto()
            {
                CoachId = 1,
                OrderId = 0,
                UpdateTime = DateTime.Now,
            };
            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            DBResponsePayByCashDto dbResponse = new DBResponsePayByCashDto
            {
                ElectronicInvoiceId = 1,
                InvoiceNumber = "wq12345678",
                PlanName = "一個月會籍",
                RandomNumber = "1234",
                TotalAmount = 1000,
                SingleEntryPassId = null,
            };

            RequestPrintInvoiceDto printInvoiceDto = new RequestPrintInvoiceDto()
            {
                ElectronicInvoiceId = dbResponse.ElectronicInvoiceId,
                InvoiceNumber = dbResponse.InvoiceNumber,
                PlanName = dbResponse.PlanName,
                RandomNumber = dbResponse.RandomNumber,
                TotalAmount = dbResponse.TotalAmount,
            };
            // Mock 設定
            orderRepositoryMock.Setup(s => s.PayByCash(payByCashDto)).Returns((errorCodeNumber, dbResponse));
            jobDispatcherMock.Setup(s => s.Enqueue<RequestPrintInoviceJob, RequestPrintInvoiceDto>(printInvoiceDto));

            // Act
            (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) = service.PayByCash(payByCashDto);

            // Assert
            Assert.AreEqual(QrCodeStringDto.QrCodeString, string.Empty);
            Assert.AreEqual(errorCode, ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 訂單用現金付款_失敗_回傳失敗()
        {
            // Arrange
            RequestPayByCashDto payByCashDto = new RequestPayByCashDto()
            {
                CoachId = 1,
                OrderId = 0,
                UpdateTime = DateTime.Now,
            };
            int errorCodeNumber = (int)ErrorCodeDefine.TrackNotSet;
            DBResponsePayByCashDto dbResponse = new DBResponsePayByCashDto
            {
                ElectronicInvoiceId = 1,
                InvoiceNumber = "wq12345678",
                PlanName = "一個月會籍",
                RandomNumber = "1234",
                TotalAmount = 1000,
                SingleEntryPassId = null,
            };

            // Mock 設定
            orderRepositoryMock.Setup(s => s.PayByCash(payByCashDto)).Returns((errorCodeNumber, dbResponse));

            // Act
            (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) = service.PayByCash(payByCashDto);

            // Assert
            Assert.AreEqual(QrCodeStringDto.QrCodeString, string.Empty);
            Assert.AreEqual(errorCode, ErrorCodeDefine.TrackNotSet);
        }
    }
}
