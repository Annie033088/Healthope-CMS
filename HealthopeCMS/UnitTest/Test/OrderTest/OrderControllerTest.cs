using System;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.OrderTest
{
    [TestClass]
    public class OrderControllerTest
    {
        private OrderController controller;
        private Mock<IOrderService> orderServiceMock;

        [TestInitialize]
        public void Setup()
        {
            orderServiceMock = new Mock<IOrderService>();
            controller = new OrderController(orderServiceMock.Object);
        }

        [TestMethod]
        public void 新增訂單_成功_回傳成功()
        {
            // Arrange
            RequestAddOrderDto addOrderDto = new RequestAddOrderDto()
            {
                MemberId = 1,
                Method = 1,
                PlanId = 1,
                PlanType = 1,
            };

            ResponseAddOrderDto response = new ResponseAddOrderDto()
            {
                OrderId = 1,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            orderServiceMock.Setup(s => s.AddOrder(addOrderDto)).Returns((response, errorCode));

            // Act
            IHttpActionResult result = controller.AddOrder(addOrderDto);

            // Assert
            ResponseIsEqual<ResponseAddOrderDto> responseIsEqual = new ResponseIsEqual<ResponseAddOrderDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
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
            orderServiceMock.Setup(s => s.AddOrder(addOrderDto)).Returns((response, errorCode));

            // Act
            IHttpActionResult result = controller.AddOrder(addOrderDto);

            // Assert
            ResponseIsEqual<ResponseAddOrderDto> responseIsEqual = new ResponseIsEqual<ResponseAddOrderDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.MemberBaned));
        }

        [TestMethod]
        public void 訂單用現金付款_成功_回傳成功()
        {
            // Arrange
            RequestPayByCashDto payByCashDto = new RequestPayByCashDto()
            {
                CoachId = 1,
                OrderId = 1,
                UpdateTime = DateTime.Now,
            };

            ResponseQrCodeStringDto response = new ResponseQrCodeStringDto()
            {
                QrCodeString = "wqelw22pqw1111"
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            orderServiceMock.Setup(s => s.PayByCash(payByCashDto)).Returns((errorCode, response));

            // Act
            IHttpActionResult result = controller.PayByCash(payByCashDto);

            // Assert
            ResponseIsEqual<ResponseQrCodeStringDto> responseIsEqual = new ResponseIsEqual<ResponseQrCodeStringDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 訂單用現金付款_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestPayByCashDto payByCashDto = new RequestPayByCashDto()
            {
                CoachId = 1,
                OrderId = 0,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.PayByCash(payByCashDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 訂單用現金付款_失敗_回傳失敗()
        {
            // Arrange
            RequestPayByCashDto payByCashDto = new RequestPayByCashDto()
            {
                CoachId = 1,
                OrderId = 1,
                UpdateTime = DateTime.Now,
            };

            ResponseQrCodeStringDto response = new ResponseQrCodeStringDto()
            {
                QrCodeString = "wqelw22pqw1111"
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.TrackNotSet;

            // Mock 設定
            orderServiceMock.Setup(s => s.PayByCash(payByCashDto)).Returns((errorCode, response));

            // Act
            IHttpActionResult result = controller.PayByCash(payByCashDto);

            // Assert
            ResponseIsEqual<ResponseQrCodeStringDto> responseIsEqual = new ResponseIsEqual<ResponseQrCodeStringDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.TrackNotSet));
        }
    }
}
