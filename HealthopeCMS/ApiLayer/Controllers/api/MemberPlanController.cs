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
using ApiLayer.Models.MemberPlan.Request;
using ApiLayer.Models.Invoice;
using ApiLayer.Models.MemberPlan;
using ApiLayer.Interface;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class MemberPlanController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemberPlanService memberPlanService;

        public MemberPlanController(IMemberPlanService memberPlanService)
        {
            this.memberPlanService = memberPlanService;
        }

        /// <summary>
        /// 修改會籍狀態
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditMemberMembershipPlanStatus([FromBody] RequestMemberMembershipPlanStatusDto addInvoiceTrackNumberDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                ResultResponse response;


                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || addInvoiceTrackNumberDto.MemberMembershipPlanId < 1
                    || !Enum.IsDefined(typeof(MemberMembershipPlanStatus), addInvoiceTrackNumberDto.Status))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse()
                {
                    ErrorCode = memberPlanService.EditMemberMembershipPlanStatus(addInvoiceTrackNumberDto)
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
