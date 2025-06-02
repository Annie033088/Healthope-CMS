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
using ApiLayer.Models.LeaseAgreement.Request;

namespace ApiLayer.Controllers.api
{
    public class LeaseAgreementController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 新增條款
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddLeaseAgreement([FromBody] RequestAddLeaseAgreementDto addLeaseAgreementDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法
                int currentYear = DateTime.Now.Year;
                DateTime minDate = new DateTime(currentYear - 100, 1, 1);
                DateTime maxDate = new DateTime(currentYear + 100, 12, 31);

                if (!ModelState.IsValid || addLeaseAgreementDto.ReminderLeadTime < 1
                    || addLeaseAgreementDto.StartTime > addLeaseAgreementDto.EndTime
                    || addLeaseAgreementDto.StartTime < minDate || addLeaseAgreementDto.StartTime > maxDate
                    || addLeaseAgreementDto.EndTime < minDate || addLeaseAgreementDto.EndTime > maxDate)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = termService.AddTerm(addTermDto);
                response = new ResultResponse()
                {
                    ErrorCode = successFlag ?
                   ErrorCodeDefine.Success : ErrorCodeDefine.CreateFailed
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
