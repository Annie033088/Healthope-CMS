using System;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Response;
using ApiLayer.Models.Member.Response;
using ApiLayer.Models.Order;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using ApiLayer.Models.PlanTemplate;
using ApiLayer.Service;
using DomainLayer.Utility;
using NLog;
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
                    ErrorCode = errorCode
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

                (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) = orderService.PayByCash(payByCashDto);
                response = new ResultResponse<ResponseQrCodeStringDto>()
                {
                    ApiDataObject = QrCodeStringDto,
                    ErrorCode = errorCode
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
        /// 刷卡付款
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> PayByCard([FromBody] RequestPayByCardDto payByCardDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || payByCardDto.OrderId < 1
                    || (payByCardDto.CoachId != null && payByCardDto.CoachId < 1))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) = await orderService.PayByCard(payByCardDto);
                response = new ResultResponse<ResponseQrCodeStringDto>()
                {
                    ApiDataObject = QrCodeStringDto,
                    ErrorCode = errorCode
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
        /// 取得訂單
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetOrder([FromBody] RequestGetOrderDto getOrderDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (getOrderDto.Method != null && !Enum.IsDefined(typeof(PaymentMethod), getOrderDto.Method))
                    modelValidFlag = false;
                if (getOrderDto.State != null && !Enum.IsDefined(typeof(OrderState), getOrderDto.State))
                    modelValidFlag = false;
                if (!((getOrderDto.SortOrder == "ascending") || (getOrderDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getOrderDto.SortOption == "amount") || (getOrderDto.SortOption == "state")
                    || (getOrderDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getOrderDto.RecordPerPage == 8) || (getOrderDto.RecordPerPage == 12)
                    || (getOrderDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getOrderDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetOrderListDto responseGet = orderService.GetOrder(getOrderDto);
                response = new ResultResponse<ResponseGetOrderListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGet
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
        /// 根據 id 取得訂單
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetOrderDetailById([FromBody] RequestOrderIdDto orderIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid || orderIdDto.OrderId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetOrderDetailByIdDto responseGet = orderService.GetOrderDetailById(orderIdDto);
                response = new ResultResponse<ResponseGetOrderDetailByIdDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGet
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
        /// 修改訂單狀態備註
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditOrderStateRemark([FromBody] RequestEditOrderStateRemarkDto editOrderStateRemarkDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid
                    || editOrderStateRemarkDto.OrderStateId < 1
                    || editOrderStateRemarkDto.Remark.Length > 50)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = orderService.EditOrderStateRemark(editOrderStateRemarkDto);
                response = new ResultResponse
                {
                    ErrorCode = successFlag ? ErrorCodeDefine.Success : ErrorCodeDefine.ModifiedFailed,
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
        /// 修改訂單備註
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditOrderRemark([FromBody] RequestEditOrderRemarkDto editOrderRemarkDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid
                    || editOrderRemarkDto.OrderId < 1
                    || editOrderRemarkDto.Remark.Length > 50)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = orderService.EditOrderRemark(editOrderRemarkDto);
                response = new ResultResponse
                {
                    ErrorCode = successFlag ? ErrorCodeDefine.Success : ErrorCodeDefine.ModifiedFailed,
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
        /// 修改訂單狀態：待付款 => 取消
        /// </summary>
        [HttpPost]
        public IHttpActionResult CancelPendingOrder([FromBody] RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid
                    || editOrderStateDto.OrderId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = orderService.CancelPendingOrder(editOrderStateDto);
                response = new ResultResponse
                {
                    ErrorCode = successFlag ? ErrorCodeDefine.Success : ErrorCodeDefine.ModifiedFailed,
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
        /// 修改訂單狀態：已付款 => 7日內退款
        /// </summary>
        [HttpPost]
        public IHttpActionResult RefundIn7Days([FromBody] RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid
                    || editOrderStateDto.OrderId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) = orderService.RefundIn7Days(editOrderStateDto);
                response = new ResultResponse<ResponseInvoiceNumberDto>
                {
                    ErrorCode = errorCode,
                    ApiDataObject = invoiceNumberDto,
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
        /// 確認是否可以無條件退費 若是=>請前端管理者確認是否要解約而不是無條件退費, 若否=>直接走解約流程
        /// </summary>
        [HttpPost]
        public IHttpActionResult CheckoutRefundQualifyAndTerminateOrder([FromBody] RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid
                    || editOrderStateDto.OrderId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) = orderService.TerminateOrder(editOrderStateDto);
                response = new ResultResponse<ResponseInvoiceNumberDto>
                {
                    ErrorCode = errorCode,
                    ApiDataObject = invoiceNumberDto,
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
        /// 修改訂單狀態：已付款 => 解約
        /// </summary>
        [HttpPost]
        public IHttpActionResult TerminateOrder([FromBody] RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid
                    || editOrderStateDto.OrderId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) = orderService.TerminateOrder(editOrderStateDto);
                response = new ResultResponse<ResponseInvoiceNumberDto>
                {
                    ErrorCode = errorCode,
                    ApiDataObject = invoiceNumberDto,
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
