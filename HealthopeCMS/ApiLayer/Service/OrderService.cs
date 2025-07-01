using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Response;
using ApiLayer.Models.Job;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using ApiLayer.Models.Other;
using AutoMapper;
using DomainLayer.Models;
using DomainLayer.Utility;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IJobDispatcher jobDispatcher;
        private readonly IPaymentService paymentService;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IJobDispatcher jobDispatcher, IPaymentService paymentService)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.jobDispatcher = jobDispatcher;
            this.paymentService = paymentService;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        public (ResponseAddOrderDto response, ErrorCodeDefine errorCode) AddOrder(RequestAddOrderDto addOrderDto)
        {
            try
            {
                DateTime now = DateTime.Now;

                // 1. 日期部分 (YYMMDD)
                string datePart = now.ToString("yyMMdd");

                // 2. 時分秒轉為當天總秒數 (00000~86399)
                int totalSeconds = (int)(now.TimeOfDay.TotalSeconds);
                string secondsPart = totalSeconds.ToString("D5"); // 補零到5位

                // 3. 會員ID末7位 (補零)
                string memberPart = (addOrderDto.MemberId % 10_000_000).ToString("D7"); // 確保7位

                string orderNumberString = $"{datePart}{secondsPart}{memberPart}";
                long orderNumber = long.Parse(orderNumberString);

                Order addOrder = mapper.Map<Order>(addOrderDto);
                (Order order, int errorCodeNumber) = orderRepository.AddOrder(addOrder, orderNumber);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber) || order == null)
                    return (null, ErrorCodeDefine.ServerError);

                return (mapper.Map<ResponseAddOrderDto>(order), (ErrorCodeDefine)errorCodeNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 現金付款
        /// </summary>
        public (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) PayByCash(RequestPayByCashDto payByCashDto)
        {
            try
            {
                (int errorCodeNumber, DBResponsePaymentDto dbResponse) = orderRepository.PayByCash(payByCashDto);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return (ErrorCodeDefine.ServerError, null);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                // 排程 開立發票 任務
                if (errorCode == ErrorCodeDefine.Success)
                {
                    RequestPrintInvoiceDto printInvoiceDto = new RequestPrintInvoiceDto()
                    {
                        ElectronicInvoiceId = dbResponse.ElectronicInvoiceId,
                        InvoiceNumber = dbResponse.InvoiceNumber,
                        PlanName = dbResponse.PlanName,
                        RandomNumber = dbResponse.RandomNumber,
                        TotalAmount = dbResponse.TotalAmount,
                    };
                    jobDispatcher.Enqueue<RequestPrintInoviceJob, RequestPrintInvoiceDto>(printInvoiceDto);
                }

                // 若是票劵方案 需即時顯示票劵 qr code
                string qrCodeString = string.Empty;

                if (dbResponse != null && dbResponse.SingleEntryPassId != null)
                {
                    Hash hash = new Hash();
                    string qrCodeStringBefaoreHash = dbResponse.SingleEntryPassId.ToString() + payByCashDto.OrderId.ToString()
                        + dbResponse.TicketCode.ToString();

                    qrCodeString = dbResponse.SingleEntryPassId.ToString() + ";" + payByCashDto.OrderId.ToString()
                        + ";" + dbResponse.TicketCode.ToString() + ";" + hash.QrCodeStringHash(qrCodeStringBefaoreHash);
                }

                return ((ErrorCodeDefine)errorCodeNumber, new ResponseQrCodeStringDto { QrCodeString = qrCodeString });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 信用卡付款
        /// </summary>
        public async Task<(ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto)> PayByCard(RequestPayByCardDto payByCardDto)
        {
            try
            {
                // 新增交易紀錄
                (CreditCardTransaction creditCardTransaction, int errorCodeNumberAddTransaction)
                    = orderRepository.AddCreditCardTransaction(payByCardDto);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumberAddTransaction))
                    return (ErrorCodeDefine.ServerError, null);

                if ((ErrorCodeDefine)errorCodeNumberAddTransaction != ErrorCodeDefine.Success)
                    return (ErrorCodeDefine.ServerError, null);

                // 開始請求第三方進行交易
                RequestCardPaymentDto requestCardPayment = new RequestCardPaymentDto
                {
                    TransactionId = string.Empty,
                    OrderId = payByCardDto.OrderId,
                    Amount = creditCardTransaction.Amount,
                };

                (ErrorCodeDefine errorCode, DBResponsePaymentDto dbResponse) = await paymentService.PayByCard(
                    requestCardPayment, creditCardTransaction.CreditCardTransactionId, payByCardDto);

                string qrCodeString = string.Empty;

                if (dbResponse != null && dbResponse.SingleEntryPassId != null)
                {
                    Hash hash = new Hash();
                    string qrCodeStringBefaoreHash = dbResponse.SingleEntryPassId.ToString() + payByCardDto.OrderId.ToString()
                        + dbResponse.TicketCode.ToString();

                    qrCodeString = dbResponse.SingleEntryPassId.ToString() + ";" + payByCardDto.OrderId.ToString()
                        + ";" + dbResponse.TicketCode.ToString() + ";" + hash.QrCodeStringHash(qrCodeStringBefaoreHash);
                }

                return (errorCode, new ResponseQrCodeStringDto { QrCodeString = qrCodeString });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得訂單
        /// </summary>
        public ResponseGetOrderListDto GetOrder(RequestGetOrderDto getOrderDto)
        {
            try
            {
                ResponseGetOrderListDto orderList = orderRepository.GetOrder(getOrderDto);
                return orderList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根據 id 取得訂單
        /// </summary>
        public ResponseGetOrderDetailByIdDto GetOrderDetailById(RequestOrderIdDto orderIdDto)
        {
            try
            {
                (Order order, List<OrderState> orderStates) = orderRepository.GetOrderDetailById(orderIdDto.OrderId);

                if (order == null) return null;

                ResponseGetOrderDetailByIdDto response = new ResponseGetOrderDetailByIdDto
                {
                    Order = mapper.Map<ResponseGetOrderByIdDto>(order),
                    OrderStateList = mapper.Map<List<ResponseGetOrderStateByIdDto>>(orderStates),
                };

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 修改訂單狀態備註
        /// </summary>
        public bool EditOrderStateRemark(RequestEditOrderStateRemarkDto editOrderStateRemarkDto)
        {
            try
            {
                OrderState orderState = mapper.Map<OrderState>(editOrderStateRemarkDto);
                return orderRepository.EditOrderStateRemark(orderState);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改訂單備註
        /// </summary>
        public bool EditOrderRemark(RequestEditOrderRemarkDto editOrderRemarkDto)
        {
            try
            {
                Order order = mapper.Map<Order>(editOrderRemarkDto);
                return orderRepository.EditOrderRemark(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改訂單狀態：待付款 => 取消
        /// </summary>
        public bool CancelPendingOrder(RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                Order order = mapper.Map<Order>(editOrderStateDto);
                return orderRepository.CancelPendingOrder(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 訂單 7 日內無條件退款
        /// </summary>
        public (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) RefundIn7Days(RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                Order order = mapper.Map<Order>(editOrderStateDto);
                (int errorCodeNumber, string invoiceNumber) = orderRepository.RefundIn7Days(order);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return (ErrorCodeDefine.ServerError, null);

                return ((ErrorCodeDefine)errorCodeNumber, new ResponseInvoiceNumberDto { InvoiceNumber = invoiceNumber });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 確認是否可以無條件退費 若是=>請前端管理者確認是否要解約而不是無條件退費, 若否=>直接走解約流程
        /// </summary>
        public (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) CheckoutRefundQualifyAndTerminateOrder(RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                Order order = mapper.Map<Order>(editOrderStateDto);
                (int errorCodeNumber, bool haveRefundQualify) = orderRepository.CheckoutUnconditionalRefundQualify(order);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return (ErrorCodeDefine.ServerError, null);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                if (errorCode != ErrorCodeDefine.Success)
                    return (errorCode, null);

                // 判斷有 無條件退費資格!
                if (errorCode == ErrorCodeDefine.Success && haveRefundQualify)
                {
                    return (ErrorCodeDefine.ConfirmAgain, null);
                }

                // 沒有 無條件退費資格 => 照常執行解約
                return TerminateOrder(editOrderStateDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 解約訂單
        /// </summary>
        public (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) TerminateOrder(RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                Order order = mapper.Map<Order>(editOrderStateDto);
                (int errorCodeNumber, string invoiceNumber) = orderRepository.TerminateOrder(order);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return (ErrorCodeDefine.ServerError, null);

                return ((ErrorCodeDefine)errorCodeNumber, new ResponseInvoiceNumberDto { InvoiceNumber = invoiceNumber });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// 確認是否可以無條件退費 若是=>請前端管理者確認是否要設置違約而不是無條件退費, 若否=>直接走違約流程
        public (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) CheckoutRefundQualifyAndBreachOrder(RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                Order order = mapper.Map<Order>(editOrderStateDto);
                (int errorCodeNumber, bool haveRefundQualify) = orderRepository.CheckoutUnconditionalRefundQualify(order);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return (ErrorCodeDefine.ServerError, null);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                if (errorCode != ErrorCodeDefine.Success)
                    return (errorCode, null);

                // 判斷有 無條件退費資格!
                if (errorCode == ErrorCodeDefine.Success && haveRefundQualify)
                {
                    return (ErrorCodeDefine.ConfirmAgain, null);
                }

                // 沒有 無條件退費資格 => 照常執行解約
                return BreachOrder(editOrderStateDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 違約訂單
        /// </summary>
        public (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) BreachOrder(RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                Order order = mapper.Map<Order>(editOrderStateDto);
                (int errorCodeNumber, string invoiceNumber, DBResponsePrintInvoiceDto dbResponse) = orderRepository.BreachOrder(order);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return (ErrorCodeDefine.ServerError, null);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                // 排程 開立(違約金)發票 任務
                if (errorCode == ErrorCodeDefine.Success && dbResponse != null)
                {
                    RequestPrintInvoiceDto printInvoiceDto = new RequestPrintInvoiceDto()
                    {
                        ElectronicInvoiceId = dbResponse.ElectronicInvoiceId,
                        InvoiceNumber = dbResponse.InvoiceNumber,
                        PlanName = dbResponse.PlanName,
                        RandomNumber = dbResponse.RandomNumber,
                        TotalAmount = dbResponse.TotalAmount,
                    };
                    jobDispatcher.Enqueue<RequestPrintInoviceJob, RequestPrintInvoiceDto>(printInvoiceDto);
                }

                return (errorCode, new ResponseInvoiceNumberDto { InvoiceNumber = invoiceNumber });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}