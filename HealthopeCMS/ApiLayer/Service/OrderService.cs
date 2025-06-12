using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;

namespace ApiLayer.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
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
    
        
    }
}