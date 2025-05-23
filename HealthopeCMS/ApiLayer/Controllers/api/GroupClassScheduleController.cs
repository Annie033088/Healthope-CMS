using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models;
using ApiLayer.Service;
using NLog;
using ApiLayer.Models.GroupClassSchedule.Request;

namespace ApiLayer.Controllers.api
{
    public class GroupClassScheduleController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 取得 新增團體課程表前 需要的資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetShowcaseAndCoach([FromBody] RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (showcaseIdDto.GroupClassShowcaseId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetShowcaseDetailDto responseGetShowcaseDetail =
                    groupClassShowcaseService.GetShowcaseDetail(showcaseIdDto);

                if (responseGetShowcaseDetail == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetShowcaseDetailDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetShowcaseDetail
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
