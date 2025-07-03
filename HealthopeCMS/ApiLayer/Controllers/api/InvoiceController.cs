using System;
using System.Text.RegularExpressions;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.Invoice;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Invoice.Response;
using ApiLayer.Models.Job;
using ApiLayer.Models.Order.Request;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class InvoiceController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IJobDispatcher jobDispatcher;
        private readonly IInvoiceService invoiceService;

        public InvoiceController(IInvoiceService invoiceService, IJobDispatcher jobDispatcher)
        {
            this.invoiceService = invoiceService;
            this.jobDispatcher = jobDispatcher;
        }

        /// <summary>
        /// 新增發票字軌
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddInvoiceTrackNumber([FromBody] RequestAddInvoiceTrackNumberDto addInvoiceTrackNumberDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                ResultResponse response;


                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || !Regex.IsMatch(addInvoiceTrackNumberDto.TrackPrefix, "^[A-Z]{2}$")
                    || addInvoiceTrackNumberDto.StartNumber < 1 || addInvoiceTrackNumberDto.StartNumber > 99999999
                    || addInvoiceTrackNumberDto.EndNumber < 1 || addInvoiceTrackNumberDto.EndNumber > 99999999
                    || addInvoiceTrackNumberDto.StartNumber >= addInvoiceTrackNumberDto.EndNumber
                    || addInvoiceTrackNumberDto.InvoicePeriod > 9999
                    || addInvoiceTrackNumberDto.InvoicePeriod < 1000)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = invoiceService.AddInvoiceTrackNumber(addInvoiceTrackNumberDto);
                response = new ResultResponse()
                {
                    ErrorCode = successFlag ? ErrorCodeDefine.Success : ErrorCodeDefine.CreateFailed
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
        /// 取得字軌
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetInvoiceTrackNumber([FromBody] RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || (getInvoiceTrackNumberDto.Status != null
                        && !Enum.IsDefined(typeof(InvoiceTrackNumberStatus), getInvoiceTrackNumberDto.Status))
                    || (!((getInvoiceTrackNumberDto.RecordPerPage == 8) || (getInvoiceTrackNumberDto.RecordPerPage == 12)
                        || (getInvoiceTrackNumberDto.RecordPerPage == 16)))
                    || getInvoiceTrackNumberDto.Page < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetInvoiceTrackNumberListDto invoiceTrackNumbers
                    = invoiceService.GetInvoiceTrackNumber(getInvoiceTrackNumberDto);
                response = new ResultResponse<ResponseGetInvoiceTrackNumberListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = invoiceTrackNumbers
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
        /// 修改字軌狀態
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditInvoiceTrackNumberStatus(
            [FromBody] RequestEditInvoiceTrackNumberStatusDto editInvoiceTrackNumberStatusDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || (!Enum.IsDefined(typeof(InvoiceTrackNumberStatus), editInvoiceTrackNumberStatusDto.Status))
                    || ((editInvoiceTrackNumberStatusDto.Status != (int)InvoiceTrackNumberStatus.Active)
                        && (editInvoiceTrackNumberStatusDto.Status != (int)InvoiceTrackNumberStatus.Disabled))
                    || editInvoiceTrackNumberStatusDto.InvoiceTrackNumberId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse
                {
                    ErrorCode = invoiceService.EditInvoiceTrackNumberStatus(editInvoiceTrackNumberStatusDto),
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
        /// 刪除字軌
        /// </summary>
        [HttpPost]
        public IHttpActionResult DeleteInvoiceTrackNumber(
            [FromBody] InvoiceTrackNumberIdDto invoiceTrackNumberIdDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || invoiceTrackNumberIdDto.InvoiceTrackNumberId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = invoiceService.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto);
                response = new ResultResponse()
                {
                    ErrorCode = successFlag ? ErrorCodeDefine.Success : ErrorCodeDefine.DeleteFailed,
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
        /// 針對「現金發票列印失敗」或「刷卡未完成訂單狀態」或「刷卡發票列印失敗」進行補印與補狀態處理
        /// </summary>
        [HttpPost]
        public IHttpActionResult CompleteOrderAndPrintInvoice(
            [FromBody] RequestOrderIdAndCategoryDto orderIdAndCategoryDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || orderIdAndCategoryDto.OrderId < 1
                    || !Enum.IsDefined(typeof(ElectronicInvoiceCategory), orderIdAndCategoryDto.Category))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, RequestPrintInvoiceDto printInvoiceDto) = invoiceService.EditOrderStateAndGetInvoiceNumber(orderIdAndCategoryDto);

                if (printInvoiceDto == null || errorCode != ErrorCodeDefine.Success)
                {
                    response = new ResultResponse()
                    {
                        ErrorCode = errorCode,
                    };
                    return Ok(response);
                }

                // 請求第三放列印發票
                jobDispatcher.Enqueue<RequestPrintInoviceJob, RequestPrintInvoiceDto>(printInvoiceDto);

                response = new ResultResponse()
                {
                    ErrorCode = errorCode,
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
        /// 作廢發票
        /// </summary>
        [HttpPost]
        public IHttpActionResult VoidInvoice(
           [FromBody] RequestEditInvoiceStatusDto editInvoiceStatusDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || editInvoiceStatusDto.OrderId < 1
                    || !Enum.IsDefined(typeof(ElectronicInvoiceCategory), editInvoiceStatusDto.Category))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse()
                {
                    ErrorCode = invoiceService.VoidInvoice(editInvoiceStatusDto),
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
        /// 折讓發票
        /// </summary>
        [HttpPost]
        public IHttpActionResult DiscountInvoice(
           [FromBody] RequestEditInvoiceStatusDto editInvoiceStatusDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || editInvoiceStatusDto.OrderId < 1
                    || !Enum.IsDefined(typeof(ElectronicInvoiceCategory), editInvoiceStatusDto.Category))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse()
                {
                    ErrorCode = invoiceService.DiscountInvoice(editInvoiceStatusDto),
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
        /// 修改發票狀態 => 待作廢
        /// </summary>
        [HttpPost]
        public IHttpActionResult PendingVoidInvoice(
           [FromBody] RequestEditInvoiceStatusDto editInvoiceStatusDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || editInvoiceStatusDto.OrderId < 1
                    || !Enum.IsDefined(typeof(ElectronicInvoiceCategory), editInvoiceStatusDto.Category))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse()
                {
                    ErrorCode = invoiceService.PendingVoidInvoice(editInvoiceStatusDto),
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
        /// 修改發票狀態 => 待折讓
        /// </summary>
        [HttpPost]
        public IHttpActionResult PendingDiscountInvoice(
           [FromBody] RequestEditInvoiceStatusDto editInvoiceStatusDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || editInvoiceStatusDto.OrderId < 1
                    || !Enum.IsDefined(typeof(ElectronicInvoiceCategory), editInvoiceStatusDto.Category))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse()
                {
                    ErrorCode = invoiceService.PendingDiscountInvoice(editInvoiceStatusDto),
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
        /// 取得發票清單
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetInvoice([FromBody] RequestGetInvoiceDto getInvoiceDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || (getInvoiceDto.Status != null
                        && !Enum.IsDefined(typeof(ElectronicInvoiceStatus), getInvoiceDto.Status))
                    || (getInvoiceDto.Category != null
                        && !Enum.IsDefined(typeof(ElectronicInvoiceCategory), getInvoiceDto.Category))
                    || (!((getInvoiceDto.RecordPerPage == 8) || (getInvoiceDto.RecordPerPage == 12)
                        || (getInvoiceDto.RecordPerPage == 16)))
                    || getInvoiceDto.Page < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetInvoiceListDto invoiceList = invoiceService.GetInvoice(getInvoiceDto);
                response = new ResultResponse<ResponseGetInvoiceListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = invoiceList
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
