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
        public void 新增訂單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestAddOrderDto addOrderDto = new RequestAddOrderDto()
            {
                MemberId = 1,
                Method = 1,
                PlanId = 1,
                PlanType = 10,
            };

            // Act
            IHttpActionResult result = controller.AddOrder(addOrderDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 新增訂單_失敗_回傳失敗()
        {
            // Arrange
            RequestAddOrderDto addOrderDto = new RequestAddOrderDto()
            {
                MemberId = 1,
                Method = 1,
                PlanId = 1,
                PlanType = 1,
            };

            ResponseAddOrderDto response = null;

            ErrorCodeDefine errorCode = ErrorCodeDefine.MemberBaned;

            // Mock 設定
            orderService.Setup(s => s.AddOrder(addOrderDto)).Returns((response, errorCode));

            // Act
            IHttpActionResult result = controller.AddOrder(addOrderDto);

            // Assert
            ResponseIsEqual<ResponseAddOrderDto> responseIsEqual = new ResponseIsEqual<ResponseAddOrderDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.MemberBaned));
        }
    }
}
