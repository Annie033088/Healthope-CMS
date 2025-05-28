using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Other;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.Response.PlanTemplate;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

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
        private readonly IMultipartRequestService<RequestEditMembershipPlanDto> multipartRequestEditdMembershipService;
        private readonly IMultipartRequestService<RequestAddPersonalTrainingPackageDto> multipartRequestAddPersonalTrainingService;
        private readonly IMultipartRequestService<RequestEditPersonalTrainingPackageDto> multipartRequestEditPersonalTrainingService;

        public PlanTemplateController(IPlanTemplateService planTemplateService,
            IMultipartRequestService<RequestAddMembershipPlanDto> multipartRequestAddMembershipService,
            IMultipartRequestService<RequestAddPersonalTrainingPackageDto> multipartRequestAddPersonalTrainingService,
            IMultipartRequestService<RequestEditMembershipPlanDto> multipartRequestEditdMembershipService,
            IMultipartRequestService<RequestEditPersonalTrainingPackageDto> multipartRequestEditPersonalTrainingService)
        {
            this.planTemplateService = planTemplateService;
            this.multipartRequestAddMembershipService = multipartRequestAddMembershipService;
            this.multipartRequestAddPersonalTrainingService = multipartRequestAddPersonalTrainingService;
            this.multipartRequestEditdMembershipService = multipartRequestEditdMembershipService;
            this.multipartRequestEditPersonalTrainingService = multipartRequestEditPersonalTrainingService;
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
                if (!multipartRequestAddPersonalTrainingService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addShowcaseDto, 2.file
                (addPersonalTrainingPackageDto, files) = await multipartRequestAddPersonalTrainingService.GetObjectAndFile(request);

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

        /// <summary>
        /// 修改票劵方案狀態
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditTicketPlanStatus(RequestEditStatusDto editStatusDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                ResultResponse response;

                if (!ModelState.IsValid || editStatusDto.TicketPlanId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = planTemplateService.EditTicketPlanStatus(editStatusDto);

                response = new ResultResponse()
                {
                    ErrorCode = successFlag ?
                    ErrorCodeDefine.Success : ErrorCodeDefine.ModifiedFailed,
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
        /// 取得修改會籍方案頁面資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetMembershipPlanEditDataById([FromBody] RequestMembershipPlanIdDto memebershipPlanIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (memebershipPlanIdDto.MembershipPlanId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetMembershipPlanEditDataDto responseData =
                    planTemplateService.GetMembershipPlanEditDataById(memebershipPlanIdDto);
                response = new ResultResponse<ResponseGetMembershipPlanEditDataDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseData
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
        /// 取得修改教練課方案頁面資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetPersonalTrainingPackageEditDataById(
            [FromBody] RequestPersonalTrainingPackageIdDto personalTrainingPackageIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (personalTrainingPackageIdDto.PersonalTrainingPackageId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetPersonalTrainingPackageEditDataDto responseData =
                    planTemplateService.GetPersonalTrainingPackageEditDataById(personalTrainingPackageIdDto);
                response = new ResultResponse<ResponseGetPersonalTrainingPackageEditDataDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseData
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
        /// 修改會籍方案
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> EditMembershipPlan()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestEditMembershipPlanDto editMembershipPlanDto = new RequestEditMembershipPlanDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestEditdMembershipService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (editMembershipPlanDto, files) = await multipartRequestEditdMembershipService.GetObjectAndFile(request);

                // 沒取到資料
                if (editMembershipPlanDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if (editMembershipPlanDto.MembershipPlanId < 1
                    || !formatValidation.ValidInput(false, null, 200, editMembershipPlanDto.Introduction))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                if (files.Any() && !formatValidation.ValidImageFile(files[0].FileData, files[0].MimeType))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, Exception exception) = planTemplateService.EditMembershipPlan(editMembershipPlanDto,
                    files.Any() ? files[0] : null);

                // 如果有例外
                if (exception != null)
                {
                    logger.Error(exception);
                    response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                    return Ok(response);
                }

                response = new ResultResponse() { ErrorCode = errorCode };
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
        /// 修改教練課方案
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> EditPersonalTrainingPackage()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestEditPersonalTrainingPackageDto editPlanDto = new RequestEditPersonalTrainingPackageDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestEditPersonalTrainingService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (editPlanDto, files) = await multipartRequestEditPersonalTrainingService.GetObjectAndFile(request);

                // 沒取到資料
                if (editPlanDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if (editPlanDto.PersonalTrainingPackageId < 1
                    || !formatValidation.ValidInput(false, null, 200, editPlanDto.Introduction))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                if (files.Any() && !formatValidation.ValidImageFile(files[0].FileData, files[0].MimeType))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, Exception exception) = planTemplateService.EditPersonalTrainingPackage(editPlanDto,
                    files.Any() ? files[0] : null);

                // 如果有例外
                if (exception != null)
                {
                    logger.Error(exception);
                    response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                    return Ok(response);
                }

                response = new ResultResponse() { ErrorCode = errorCode };
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
