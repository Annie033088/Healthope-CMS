using System;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.LeaseAgreement;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models.LeaseAgreement.Response;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    public class LeaseAgreementController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ILeaseAgreementService leaseAgreementService;

        public LeaseAgreementController(ILeaseAgreementService leaseAgreementService)
        {
            this.leaseAgreementService = leaseAgreementService;
        }

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
                int currentYear = DateTime.UtcNow.Year;
                DateTime minDate = new DateTime(currentYear - 100, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime maxDate = new DateTime(currentYear + 100, 12, 31, 23, 59, 59, DateTimeKind.Utc);

                if (!ModelState.IsValid || addLeaseAgreementDto.ReminderLeadTime < 1
                    || addLeaseAgreementDto.StartTime > addLeaseAgreementDto.EndTime
                    || addLeaseAgreementDto.StartTime < minDate || addLeaseAgreementDto.StartTime > maxDate
                    || addLeaseAgreementDto.EndTime < minDate || addLeaseAgreementDto.EndTime > maxDate)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = leaseAgreementService.AddLeaseAgreement(addLeaseAgreementDto);
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
        /// 取得條款
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetLeaseAgreement([FromBody] RequestGetLeaseAgreementDto getLeaseAgreementDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || (getLeaseAgreementDto.Status != null
                        && !Enum.IsDefined(typeof(LeaseAgreementStatus), getLeaseAgreementDto.Status))
                    || (!((getLeaseAgreementDto.RecordPerPage == 8) || (getLeaseAgreementDto.RecordPerPage == 12)
                        || (getLeaseAgreementDto.RecordPerPage == 16)))
                    || getLeaseAgreementDto.Page < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetLeaseAgreementListDto leaseAgreemets = leaseAgreementService.GetLeaseAgreement(getLeaseAgreementDto);
                response = new ResultResponse<ResponseGetLeaseAgreementListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = leaseAgreemets
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
        /// 修改租約狀態 (僅限未啟用=>啟用, 啟用=>已完成、取消)
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditLeaseAgreementStatus([FromBody] RequestEditLeaseAgreementStatusDto editLeaseAgreementStatusDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid || editLeaseAgreementStatusDto.LeaseAgreementId < 1
                    || !Enum.IsDefined(typeof(LeaseAgreementStatus), editLeaseAgreementStatusDto.Status)
                    // 狀態不能轉成 未啟用
                    || (LeaseAgreementStatus)editLeaseAgreementStatusDto.Status == LeaseAgreementStatus.Inactive
                    || ((editLeaseAgreementStatusDto.Remark != null) && (editLeaseAgreementStatusDto.Remark.Length > 50)))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse
                {
                    ErrorCode = leaseAgreementService.EditLeaseAgreementStatus(
                    editLeaseAgreementStatusDto)
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
        /// 修改是否提醒
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditLeaseAgreementRemind([FromBody] RequestEditLeaseAgreementRemindDto editLeaseAgreementRemindDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid || editLeaseAgreementRemindDto.LeaseAgreementId < 1
                    || editLeaseAgreementRemindDto.Remind != false)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse
                {
                    ErrorCode = leaseAgreementService.EditLeaseAgreementRemind(editLeaseAgreementRemindDto)
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
        /// 刪除租約(僅限未啟用租約)
        /// </summary>
        [HttpPost]
        public IHttpActionResult DeleteLeaseAgreement([FromBody] RequestLeaseAgreementIdDto leaseAgreementIdDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid || leaseAgreementIdDto.LeaseAgreementId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = leaseAgreementService.DeleteLeaseAgreement(leaseAgreementIdDto);
                response = new ResultResponse
                {
                    ErrorCode = successFlag ? ErrorCodeDefine.Success : ErrorCodeDefine.DeleteFailed
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
