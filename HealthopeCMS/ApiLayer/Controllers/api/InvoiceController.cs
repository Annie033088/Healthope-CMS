using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models;
using ApiLayer.Service;
using DomainLayer.Utility;
using NLog;
using ApiLayer.Models.Invoice.Request;
using System.Text.RegularExpressions;
using ApiLayer.Interface;
using ApiLayer.Filters;
using ApiLayer.Models.Term.Response;
using ApiLayer.Models.Term;
using PersistentLayer.Models;
using ApiLayer.Models.Invoice;
using ApiLayer.Models.Invoice.Response;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class InvoiceController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IInvoiceService invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
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
    }
}
