using System;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Refund;
using ApiLayer.Models.Refund.Response;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class RefundController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IRefundService refundService;

        public RefundController(IRefundService refundService)
        {
            this.refundService = refundService;
        }

        /// <summary>
        /// 取得退款紀錄
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetRefund([FromBody] RequestGetRefundDto getRefundDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (getRefundDto.RefundType != null && !Enum.IsDefined(typeof(RefundType), getRefundDto.RefundType))
                    modelValidFlag = false;
                if (getRefundDto.Status != null && !Enum.IsDefined(typeof(RefundStatus), getRefundDto.Status))
                    modelValidFlag = false;
                if (!((getRefundDto.SortOrder == "ascending") || (getRefundDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getRefundDto.SortOption == "status") || (getRefundDto.SortOption == "createTime")
                    || (getRefundDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getRefundDto.RecordPerPage == 8) || (getRefundDto.RecordPerPage == 12)
                    || (getRefundDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getRefundDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetRefundListDto responseGet = refundService.GetRefund(getRefundDto);
                response = new ResultResponse<ResponseGetRefundListDto>
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
    }
}
