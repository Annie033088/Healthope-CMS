using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Response;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
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

        [TestMethod]
        public void 取得訂單_成功_回傳訂單()
        {
            // Arrange
            RequestGetOrderDto getOrderDto = new RequestGetOrderDto()
            {
                State = 1,
                Method = null,
                SortOption = null,
                SortOrder = "ascending",
                RecordPerPage = 8,
                Page = 1
            };

            List<ResponseGetOrderDto> responseGetOrder = new List<ResponseGetOrderDto>()
            {
                new ResponseGetOrderDto
                {
                    Amount=200,
                    MemberId=1,
                    MemberName="AA",
                    MemberPhone=987654321,
                    Method=2,
                    OrderId=1,
                    OrderNumber="250106000010000001",
                    PlanName="健身體驗",
                    PlanType=1,
                    State=1,
                    UpdateTime=DateTime.Now,
                }
            };

            ResponseGetOrderListDto response = new ResponseGetOrderListDto
            {
                OrderList = responseGetOrder,
                TotalPage = 1,
            };

            // Mock 設定
            orderServiceMock.Setup(s
                => s.GetOrder(getOrderDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetOrder(getOrderDto);

            // Assert
            ResponseIsEqual<ResponseGetOrderListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetOrderListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 取得訂單_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestGetOrderDto getOrderDto = new RequestGetOrderDto()
            {
                State = 1,
                Method = null,
                SortOption = "null",
                SortOrder = "ascending",
                RecordPerPage = 8,
                Page = 1
            };

            // Act
            IHttpActionResult result = controller.GetOrder(getOrderDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 訂單用信用卡付款_成功_回傳成功()
        {
            // Arrange
            RequestPayByCardDto payByCardDto = new RequestPayByCardDto()
            {
                CoachId = 1,
                OrderId = 1,
                CardReaderId = "WEQXX-1",
                UpdateTime = DateTime.Now,
            };

            ResponseQrCodeStringDto response = new ResponseQrCodeStringDto()
            {
                QrCodeString = "wqelw22pqw1111"
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            orderServiceMock.Setup(s => s.PayByCard(payByCardDto)).ReturnsAsync((errorCode, response));

            // Act
            IHttpActionResult result = await controller.PayByCard(payByCardDto);

            // Assert
            ResponseIsEqual<ResponseQrCodeStringDto> responseIsEqual = new ResponseIsEqual<ResponseQrCodeStringDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public async Task 訂單用信用卡付款_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestPayByCardDto payByCardDto = new RequestPayByCardDto()
            {
                CoachId = 1,
                OrderId = 0,
                CardReaderId = "WEQXX-1",
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = await controller.PayByCard(payByCardDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 訂單用信用卡付款_失敗_回傳失敗()
        {
            // Arrange
            RequestPayByCardDto payByCardDto = new RequestPayByCardDto()
            {
                CoachId = 1,
                OrderId = 1,
                CardReaderId = "WEQXX-1",
                UpdateTime = DateTime.Now,
            };

            ResponseQrCodeStringDto response = new ResponseQrCodeStringDto()
            {
                QrCodeString = "wqelw22pqw1111"
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.TrackNotSet;

            // Mock 設定
            orderServiceMock.Setup(s => s.PayByCard(payByCardDto)).ReturnsAsync((errorCode, response));

            // Act
            IHttpActionResult result = await controller.PayByCard(payByCardDto);

            // Assert
            ResponseIsEqual<ResponseQrCodeStringDto> responseIsEqual = new ResponseIsEqual<ResponseQrCodeStringDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.TrackNotSet, response));
        }

        [TestMethod]
        public void 取得訂單細項_成功_回傳訂單細項()
        {
            // Arrange
            RequestOrderIdDto requestOrderIdDto = new RequestOrderIdDto { OrderId = 1 };

            ResponseGetOrderDetailByIdDto responseGet = new ResponseGetOrderDetailByIdDto
            {
                Order = new ResponseGetOrderByIdDto
                {
                    Amount = 2000
                },
                OrderStateList = new List<ResponseGetOrderStateByIdDto>
                {
                    new ResponseGetOrderStateByIdDto
                    {
                        CreateTime = DateTime.Now,
                    }
                }
            };

            // Mock 設定
            orderServiceMock.Setup(s
                => s.GetOrderDetailById(requestOrderIdDto)).Returns(responseGet);

            // Act
            IHttpActionResult result = controller.GetOrderDetailById(requestOrderIdDto);

            // Assert
            ResponseIsEqual<ResponseGetOrderDetailByIdDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetOrderDetailByIdDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 取得訂單細項_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestOrderIdDto requestOrderIdDto = new RequestOrderIdDto { OrderId = 0 };

            // Act
            IHttpActionResult result = controller.GetOrderDetailById(requestOrderIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改訂單狀態備註_成功_回傳成功()
        {
            // Arrange
            RequestEditOrderStateRemarkDto requestEdit = new RequestEditOrderStateRemarkDto
            {
                OrderStateId = 1,
                Remark = "今天第一次",
                UpdateTime = DateTime.Now,
            };

            bool successFlag = true;

            // Mock 設定
            orderServiceMock.Setup(s
                => s.EditOrderStateRemark(requestEdit)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.EditOrderStateRemark(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改訂單狀態備註_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditOrderStateRemarkDto requestEdit = new RequestEditOrderStateRemarkDto
            {
                OrderStateId = 0,
                Remark = "今天第一次",
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditOrderStateRemark(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改訂單備註_成功_回傳成功()
        {
            // Arrange
            RequestEditOrderRemarkDto requestEdit = new RequestEditOrderRemarkDto
            {
                OrderId = 1,
                Remark = "今天第一次",
                UpdateTime = DateTime.Now,
            };

            bool successFlag = true;

            // Mock 設定
            orderServiceMock.Setup(s
                => s.EditOrderRemark(requestEdit)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.EditOrderRemark(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改訂單備註_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditOrderRemarkDto requestEdit = new RequestEditOrderRemarkDto
            {
                OrderId = 0,
                Remark = "今天第一次",
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditOrderRemark(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改訂單狀態為取消_成功_回傳成功()
        {
            // Arrange
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 1,
                UpdateTime = DateTime.Now,
            };

            bool successFlag = true;

            // Mock 設定
            orderServiceMock.Setup(s
                => s.CancelPendingOrder(requestEdit)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.CancelPendingOrder(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改訂單狀態為取消_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 0,
                UpdateTime = DateTime.Now,
            };


            // Act
            IHttpActionResult result = controller.CancelPendingOrder(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改訂單狀態為取消_失敗_回傳失敗()
        {
            // Arrange
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 1,
                UpdateTime = DateTime.Now,
            };

            bool successFlag = false;

            // Mock 設定
            orderServiceMock.Setup(s
                => s.CancelPendingOrder(requestEdit)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.CancelPendingOrder(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.ModifiedFailed));
        }

        [TestMethod]
        public void 修改訂單狀態為7日內退款_成功_回傳成功()
        {
            // Arrange
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 1,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            ResponseInvoiceNumberDto invoiceNumberDto = new ResponseInvoiceNumberDto
            {
                InvoiceNumber = "QC-12456778"
            };

            // Mock 設定
            orderServiceMock.Setup(s
                => s.RefundIn7Days(requestEdit)).Returns((errorCode, invoiceNumberDto));

            // Act
            IHttpActionResult result = controller.RefundIn7Days(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改訂單狀態為7日內退款_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 0,
                UpdateTime = DateTime.Now,
            };


            // Act
            IHttpActionResult result = controller.RefundIn7Days(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改訂單狀態為7日內退款_失敗_回傳失敗()
        {
            // Arrange
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 1,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.TimeExceeded;
            ResponseInvoiceNumberDto invoiceNumberDto = new ResponseInvoiceNumberDto
            {
                InvoiceNumber = "QC-12456778"
            };

            // Mock 設定
            orderServiceMock.Setup(s
                => s.RefundIn7Days(requestEdit)).Returns((errorCode, invoiceNumberDto));

            // Act
            IHttpActionResult result = controller.RefundIn7Days(requestEdit);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.TimeExceeded));
        }
    }
}
