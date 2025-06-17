using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.Job;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
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

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IJobDispatcher jobDispatcher)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.jobDispatcher = jobDispatcher;
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
                (int errorCodeNumber, DBResponsePayByCashDto dbResponse) = orderRepository.PayByCash(payByCashDto);

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
    }
}