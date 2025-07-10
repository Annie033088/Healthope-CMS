using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class MemberClassController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemberClassService memberClassService;

        public MemberClassController(IMemberClassService memberClassService)
        {
            this.memberClassService = memberClassService;
        }

        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetPersonalTrainingPackageAndCoach([FromBody] RequestMemberIdDto memberIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式驗證
                if (memberIdDto.MemberId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                List<ResponseGetRevenueExpenseDto> responseGetRevenues = reportService.GetRevenueExpenseReport(getFinancialStatementDto);

                response = new ResultResponse<List<ResponseGetRevenueExpenseDto>>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetRevenues
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
