using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models;
using ApiLayer.Service;
using DomainLayer.Utility;
using NLog;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.PlanTemplate;
using ApiLayer.Models.Order;
using ApiLayer.Interface;
using ApiLayer.Models.Order.Response;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class OrderController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        /// 新增訂單(會籍)
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddOrder([FromBody] RequestAddOrderDto addOrderDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || addOrderDto.PlanId < 1
                    || addOrderDto.MemberId < 1
                    || !Enum.IsDefined(typeof(PaymentMethod), addOrderDto.Method)
                    || !Enum.IsDefined(typeof(PlanType), addOrderDto.PlanType))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ResponseAddOrderDto result, ErrorCodeDefine errorCode) = orderService.AddOrder(addOrderDto);
                response = new ResultResponse<ResponseAddOrderDto>()
                {
                    ApiDataObject = result,
                    ErrorCode = result == null ? ErrorCodeDefine.CreateFailed : ErrorCodeDefine.Success
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        /// <summary>
        /// 現金付款
        /// </summary>
        [HttpPost]
        public IHttpActionResult PayByCash([FromBody] RequestPayByCashDto payByCashDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || payByCashDto.OrderId < 1
                    || (payByCashDto.CoachId != null && payByCashDto.CoachId < 1))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ResponseAddOrderDto result, ErrorCodeDefine errorCode) = orderService.AddOrder(payByCashDto);
                response = new ResultResponse<ResponseAddOrderDto>()
                {
                    ApiDataObject = result,
                    ErrorCode = result == null ? ErrorCodeDefine.CreateFailed : ErrorCodeDefine.Success
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }
    }
}
