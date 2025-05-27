using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models;
using ApiLayer.Service;
using DomainLayer.Utility;
using NLog;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Interface;
using System.Threading.Tasks;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.Other;
using ApiLayer.Filters;
using ApiLayer.Models.GroupClassShowcase.Response;
using PersistentLayer.Models;
using ApiLayer.Models.Response.PlanTemplate;
using ApiLayer.Models.PlanTemplate.Response;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class PlanTemplateController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IPlanTemplateService planTemplateService;
        private readonly IMultipartRequestService<RequestAddMembershipPlanDto> multipartRequestAddMembershipService;
        private readonly IMultipartRequestService<RequestAddPersonalTrainingPackageDto> multipartRequestAddAddPersonalTrainingService;

        public PlanTemplateController(IPlanTemplateService planTemplateService,
            IMultipartRequestService<RequestAddMembershipPlanDto> multipartRequestAddMembershipService,
            IMultipartRequestService<RequestAddPersonalTrainingPackageDto> multipartRequestAddAddPersonalTrainingService)
        {
            this.planTemplateService = planTemplateService;
            this.multipartRequestAddMembershipService = multipartRequestAddMembershipService;
            this.multipartRequestAddAddPersonalTrainingService = multipartRequestAddAddPersonalTrainingService;
        }

        /// <summary>
        /// 新增 一次性票劵方案
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddTicketPlan([FromBody] RequestAddTicketPlanDto addTicketPlanDto)
        {
            try
            {
                ResultResponse response;

                // 格式驗證
                if (!ModelState.IsValid
                    || addTicketPlanDto.Price < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = planTemplateService.AddTicketPlan(addTicketPlanDto);

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

        /// <summary>
        /// 新增 會籍方案
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> AddMembershipPlan()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestAddMembershipService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addShowcaseDto, 2.file
                (addMembershipPlanDto, files) = await multipartRequestAddMembershipService.GetObjectAndFile(request);

                // 沒取到資料
                if (addMembershipPlanDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if (!ModelState.IsValid
                    || !formatValidation.ValidInput(true, 1, 20, addMembershipPlanDto.Name)
                    || !formatValidation.ValidInput(true, null, 200, addMembershipPlanDto.Introduction)
                    || addMembershipPlanDto.Price < 1 || addMembershipPlanDto.Duration < 1
                    )
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                if (files.Any() && !formatValidation.ValidImageFile(files[0].FileData, files[0].MimeType))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (bool successFlag, Exception exception) = planTemplateService.AddMembershipPlan(addMembershipPlanDto,
                    files.Any() ? files[0] : null);

                // 如果有例外
                if (exception != null)
                {
                    logger.Error(exception);
                    response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                    return Ok(response);
                }

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

        /// <summary>
        /// 新增 教練課方案
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> AddPersonalTrainingPackage()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto = new RequestAddPersonalTrainingPackageDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestAddAddPersonalTrainingService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addShowcaseDto, 2.file
                (addPersonalTrainingPackageDto, files) = await multipartRequestAddAddPersonalTrainingService.GetObjectAndFile(request);

                // 沒取到資料
                if (addPersonalTrainingPackageDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if (!ModelState.IsValid
                    || !formatValidation.ValidInput(true, 1, 20, addPersonalTrainingPackageDto.Name)
                    || !formatValidation.ValidInput(true, null, 200, addPersonalTrainingPackageDto.Introduction)
                    || addPersonalTrainingPackageDto.Price < 1 || addPersonalTrainingPackageDto.SessionCount < 1
                    )
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                if (files.Any() && !formatValidation.ValidImageFile(files[0].FileData, files[0].MimeType))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (bool successFlag, Exception exception) = planTemplateService.AddPersonalTrainingPackage(
                    addPersonalTrainingPackageDto, files.Any() ? files[0] : null);

                // 如果有例外
                if (exception != null)
                {
                    logger.Error(exception);
                    response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                    return Ok(response);
                }

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

        /// <summary>
        /// 取得會籍方案
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetMembershipPlan([FromBody] RequestGetPlanDto getPlanDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (!((getPlanDto.SortOrder == "ascending")
                    || (getPlanDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getPlanDto.SortOption == "status") || (getPlanDto.SortOption == "price")
                    || (getPlanDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getPlanDto.RecordPerPage == 8) || (getPlanDto.RecordPerPage == 12)
                    || (getPlanDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getPlanDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetMembershipPlanListDto getMembershipPlanListDto = planTemplateService.GetMembershipPlan(getPlanDto);
                response = new ResultResponse<ResponseGetMembershipPlanListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = getMembershipPlanListDto
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
        /// 取得教練課方案
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetPersionalTrainingPackage([FromBody] RequestGetPlanDto getPlanDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (!((getPlanDto.SortOrder == "ascending")
                    || (getPlanDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getPlanDto.SortOption == "status") || (getPlanDto.SortOption == "price")
                    || (getPlanDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getPlanDto.RecordPerPage == 8) || (getPlanDto.RecordPerPage == 12)
                    || (getPlanDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getPlanDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetPersonalTrainingPackageListDto getPTPackage
                    = planTemplateService.GetPersionalTrainingPackage(getPlanDto);
                response = new ResultResponse<ResponseGetPersonalTrainingPackageListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = getPTPackage
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
        /// 取得票劵方案
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetTicketPlan([FromBody] RequestGetPlanDto getPlanDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (!((getPlanDto.SortOrder == "ascending")
                    || (getPlanDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getPlanDto.SortOption == "status") || (getPlanDto.SortOption == "price")
                    || (getPlanDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getPlanDto.RecordPerPage == 8) || (getPlanDto.RecordPerPage == 12)
                    || (getPlanDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getPlanDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetTicketPlanListDto getTicketPlan = planTemplateService.GetTicketPlan(getPlanDto);
                response = new ResultResponse<ResponseGetTicketPlanListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = getTicketPlan
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
