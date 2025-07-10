using System;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.MemberPlan;
using ApiLayer.Models.MemberPlan.Request;
using NLog;

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
        public IHttpActionResult EditMemberMembershipPlanStatus([FromBody] RequestMemberMembershipPlanStatusDto editMemberMembershipPlanStatusDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || editMemberMembershipPlanStatusDto.MemberMembershipPlanId < 1
                    || !Enum.IsDefined(typeof(MemberMembershipPlanStatus), editMemberMembershipPlanStatusDto.Status))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse()
                {
                    ErrorCode = memberPlanService.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto)
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
        /// 修改教練課方案的教練
        /// </summary>
        public IHttpActionResult EditMemberPersonalTrainingPackageCoach(
            [FromBody] RequestEditMemberPersonalTrainingPackageCoachDto editMemberPersonalTrainingPackageCoachDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid
                    || editMemberPersonalTrainingPackageCoachDto.MemberPersonalTrainingPackageId < 1
                    || editMemberPersonalTrainingPackageCoachDto.CoachId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse()
                {
                    ErrorCode = memberPlanService.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto)
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
