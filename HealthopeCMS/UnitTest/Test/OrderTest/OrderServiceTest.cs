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
using ApiLayer.Models.Other;
using DomainLayer.Utility;

namespace UnitTest.Test.OrderTest
{
    [TestClass]
    public class OrderServiceTest
    {
        private OrderService service;
        private Mock<IOrderRepository> orderRepositoryMock;
        private Mock<IMapper> mapperMock;
        private Mock<IJobDispatcher> jobDispatcherMock;
        private Mock<IPaymentService> paymentServiceMock;

        [TestInitialize]
        public void Setup()
        {
            orderRepositoryMock = new Mock<IOrderRepository>();
            mapperMock = new Mock<IMapper>();
            jobDispatcherMock = new Mock<IJobDispatcher>();
            paymentServiceMock = new Mock<IPaymentService>();
            service = new OrderService(orderRepositoryMock.Object, mapperMock.Object, jobDispatcherMock.Object, paymentServiceMock.Object);
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
            DBResponsePaymentDto dbResponse = new DBResponsePaymentDto
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
            DBResponsePaymentDto dbResponse = new DBResponsePaymentDto
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
            orderRepositoryMock.Setup(s
                => s.GetOrder(getOrderDto)).Returns(response);

            // Act
            ResponseGetOrderListDto result = service.GetOrder(getOrderDto);

            // Assert
            Assert.IsTrue(result.OrderList.SequenceEqual(responseGetOrder));
        }

        [TestMethod]
        public void 取得訂單_失敗_回傳空資料()
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

            ResponseGetOrderListDto response = null;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.GetOrder(getOrderDto)).Returns(response);

            // Act
            ResponseGetOrderListDto result = service.GetOrder(getOrderDto);

            // Assert
            Assert.AreEqual(result, null);
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

            CreditCardTransaction creditCardTransaction = new CreditCardTransaction()
            {
                Amount = 1000,
                CreditCardTransactionId = 2
            };

            int errorCodeNumberAddTransaction = (int)ErrorCodeDefine.Success;

            RequestCardPaymentDto requestCardPayment = new RequestCardPaymentDto()
            {
                Amount = creditCardTransaction.Amount,
                OrderId = payByCardDto.OrderId,
                TransactionId = string.Empty,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            DBResponsePaymentDto dbResponse = new DBResponsePaymentDto()
            {
                SingleEntryPassId = 1,
                TicketCode = Guid.NewGuid(),
            };

            Hash hash = new Hash();
            string qrCodeStringBefaoreHash = dbResponse.SingleEntryPassId.ToString() + payByCardDto.OrderId.ToString()
                + dbResponse.TicketCode.ToString();

            string qrCodeString = dbResponse.SingleEntryPassId.ToString() + ";" + payByCardDto.OrderId.ToString()
                + ";" + dbResponse.TicketCode.ToString() + ";" + hash.QrCodeStringHash(qrCodeStringBefaoreHash);

            ResponseQrCodeStringDto response = new ResponseQrCodeStringDto()
            {
                QrCodeString = qrCodeString,
            };

            // Mock 設定
            orderRepositoryMock.Setup(s => s.AddCreditCardTransaction(payByCardDto))
                .Returns((creditCardTransaction, errorCodeNumberAddTransaction));
            paymentServiceMock.Setup(s => s.PayByCard(It.IsAny<RequestCardPaymentDto>(), It.IsAny<int>(), It.IsAny<RequestPayByCardDto>()))
                .ReturnsAsync((errorCode, dbResponse));

            // Act
            (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) result = await service.PayByCard(payByCardDto);

            // Assert
            Assert.AreEqual(result.errorCode, ErrorCodeDefine.Success);
            Assert.AreEqual(result.QrCodeStringDto.QrCodeString, response.QrCodeString);
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

            CreditCardTransaction creditCardTransaction = new CreditCardTransaction()
            {
                Amount = 1000,
                CreditCardTransactionId = 2
            };

            int errorCodeNumberAddTransaction = (int)ErrorCodeDefine.Success;

            RequestCardPaymentDto requestCardPayment = new RequestCardPaymentDto()
            {
                Amount = creditCardTransaction.Amount,
                OrderId = payByCardDto.OrderId,
                TransactionId = string.Empty,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.PayFailed;
            DBResponsePaymentDto dbResponse = null;

            string qrCodeString = string.Empty;

            ResponseQrCodeStringDto response = new ResponseQrCodeStringDto()
            {
                QrCodeString = qrCodeString,
            };

            // Mock 設定
            orderRepositoryMock.Setup(s => s.AddCreditCardTransaction(payByCardDto))
                .Returns((creditCardTransaction, errorCodeNumberAddTransaction));
            paymentServiceMock.Setup(s => s.PayByCard(It.IsAny<RequestCardPaymentDto>(), It.IsAny<int>(), It.IsAny<RequestPayByCardDto>()))
                .ReturnsAsync((errorCode, dbResponse));

            // Act
            (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) result = await service.PayByCard(payByCardDto);

            // Assert
            Assert.AreEqual(result.errorCode, ErrorCodeDefine.PayFailed);
            Assert.AreEqual(result.QrCodeStringDto.QrCodeString, response.QrCodeString);
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
                        OrderStateId = 20,
                    }
                }
            };

            Order order = new Order
            {
                Amount = 2000
            };

            List<OrderState> orderStates = new List<OrderState>
            {
                new OrderState
                {
                    OrderStateId = 20,
                }
            };

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.GetOrderDetailById(requestOrderIdDto.OrderId)).Returns((order, orderStates));
            mapperMock.Setup(s => s.Map<ResponseGetOrderByIdDto>(order)).Returns(responseGet.Order);
            mapperMock.Setup(s => s.Map<List<ResponseGetOrderStateByIdDto>>(orderStates)).Returns(responseGet.OrderStateList);

            // Act
            ResponseGetOrderDetailByIdDto result = service.GetOrderDetailById(requestOrderIdDto);

            // Assert
            Assert.AreEqual(result.Order, responseGet.Order);
            Assert.IsTrue(result.OrderStateList.SequenceEqual(responseGet.OrderStateList));
        }

        [TestMethod]
        public void 取得訂單細項_失敗_回傳空資料()
        {
            // Arrange
            RequestOrderIdDto requestOrderIdDto = new RequestOrderIdDto { OrderId = 1 };
            Order order = null;
            List<OrderState> orderStates = null;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.GetOrderDetailById(requestOrderIdDto.OrderId)).Returns((order, orderStates));

            // Act
            ResponseGetOrderDetailByIdDto result = service.GetOrderDetailById(requestOrderIdDto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void 修改訂單狀態備註_成功_回傳訂單細項()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            RequestEditOrderStateRemarkDto requestEdit = new RequestEditOrderStateRemarkDto
            {
                OrderStateId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            OrderState orderState = new OrderState
            {
                OrderStateId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            bool successFlag = true;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.EditOrderStateRemark(orderState)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<OrderState>(requestEdit)).Returns(orderState);

            // Act
            bool result = service.EditOrderStateRemark(requestEdit);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 修改訂單狀態備註_失敗_回傳失敗()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            RequestEditOrderStateRemarkDto requestEdit = new RequestEditOrderStateRemarkDto
            {
                OrderStateId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            OrderState orderState = new OrderState
            {
                OrderStateId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            bool successFlag = false;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.EditOrderStateRemark(orderState)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<OrderState>(requestEdit)).Returns(orderState);

            // Act
            bool result = service.EditOrderStateRemark(requestEdit);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void 修改訂單備註_成功_回傳成功()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            RequestEditOrderRemarkDto requestEdit = new RequestEditOrderRemarkDto
            {
                OrderId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            Order order = new Order
            {
                OrderId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            bool successFlag = true;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.EditOrderRemark(order)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<Order>(requestEdit)).Returns(order);

            // Act
            bool result = service.EditOrderRemark(requestEdit);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 修改訂單備註_失敗_回傳格式錯誤()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            RequestEditOrderRemarkDto requestEdit = new RequestEditOrderRemarkDto
            {
                OrderId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            Order order = new Order
            {
                OrderId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            bool successFlag = false;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.EditOrderRemark(order)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<Order>(requestEdit)).Returns(order);

            // Act
            bool result = service.EditOrderRemark(requestEdit);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void 修改訂單狀態為取消_成功_回傳成功()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 1,
                UpdateTime = dateTime,
            };

            Order order = new Order
            {
                OrderId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            bool successFlag = true;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.CancelPendingOrder(order)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<Order>(requestEdit)).Returns(order);

            // Act
            bool result = service.CancelPendingOrder(requestEdit);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 修改訂單狀態為取消_失敗_回傳失敗()
        {
            // Arrange
            DateTime dateTime = DateTime.Now;
            RequestEditOrderStateDto requestEdit = new RequestEditOrderStateDto
            {
                OrderId = 1,
                UpdateTime = dateTime,
            };

            Order order = new Order
            {
                OrderId = 1,
                Remark = "今天第一次",
                UpdateTime = dateTime,
            };

            bool successFlag = false;

            // Mock 設定
            orderRepositoryMock.Setup(s
                => s.CancelPendingOrder(order)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<Order>(requestEdit)).Returns(order);

            // Act
            bool result = service.CancelPendingOrder(requestEdit);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
