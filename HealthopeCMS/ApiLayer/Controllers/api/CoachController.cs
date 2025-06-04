using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Coach.Response;
using ApiLayer.Models.Other;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class CoachController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMultipartRequestService<RequestAddCoachDto> multipartRequestAddService;
        private readonly IMultipartRequestService<RequestEditCoachDto> multipartRequestEditService;
        private readonly ICoachService coachService;

        public CoachController(IMultipartRequestService<RequestAddCoachDto> multipartRequestAddService,
            ICoachService coachService,
            IMultipartRequestService<RequestEditCoachDto> multipartRequestEditService)
        {
            this.multipartRequestAddService = multipartRequestAddService;
            this.coachService = coachService;
            this.multipartRequestEditService = multipartRequestEditService;
        }

        /// <summary>
        /// 新增教練
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> AddCoach()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestAddCoachDto addCoachDto = new RequestAddCoachDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestAddService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addCoachDto, 2.file
                (addCoachDto, files) = await multipartRequestAddService.GetObjectAndFile(request);

                // 沒取到資料
                if (addCoachDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if (!formatValidation.ValidAccount(addCoachDto.Account)
                    || !formatValidation.ValidPwd(addCoachDto.Pwd)
                    || addCoachDto.Account == addCoachDto.Pwd
                    || !formatValidation.ValidPhone(addCoachDto.Phone)
                    || !formatValidation.ValidEmail(addCoachDto.Email)
                    || !formatValidation.ValidInput(
                        requireNonNull: true, minLength: 1, maxLength: 15, addCoachDto.Name)
                    || !formatValidation.ValidContractTime(addCoachDto.ContractStartTime,
                    addCoachDto.ContractEndTime)
                    || !formatValidation.ValidCoachType(addCoachDto.Type)
                    || !formatValidation.ValidInput(
                        requireNonNull: true, minLength: null, maxLength: 50, addCoachDto.Introduction)
                    || !formatValidation.ValidInput(
                        requireNonNull: true, minLength: null, maxLength: 200, addCoachDto.Specialty)
                    || !formatValidation.ValidInput(
                        requireNonNull: true, minLength: null, maxLength: 200, addCoachDto.Certification)
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

                (ErrorCodeDefine errorCode, Exception exception) = coachService.AddCoach(addCoachDto,
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
        /// 取得教練
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetCoach([FromBody] RequestGetCoachDto getCoachDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (!formatValidation.ValidSearchPhone(getCoachDto.SearchPhone))
                    modelValidFlag = false;
                if (getCoachDto.SearchName != null && getCoachDto.SearchName.Length > 50)
                    modelValidFlag = false;
                if (!((getCoachDto.SortOrder == "ascending") || (getCoachDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getCoachDto.SortOption == "name") || (getCoachDto.SortOption == "status")
                    || (getCoachDto.SortOption == "contractEndTime") || (getCoachDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getCoachDto.RecordPerPage == 8) || (getCoachDto.RecordPerPage == 12)
                    || (getCoachDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getCoachDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetCoachListDto responseGetCoachListDto = coachService.GetCoach(getCoachDto);
                response = new ResultResponse<ResponseGetCoachListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetCoachListDto
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
        /// 取得修改教練頁面的資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetCoachEditDataById([FromBody] RequestCoachIdDto coachIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (coachIdDto.CoachId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetCoachEditDataByIdDto responseGetCoachDto =
                    coachService.GetCoachEditDataById(coachIdDto);

                if (responseGetCoachDto == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetCoachEditDataByIdDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetCoachDto
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
        /// 修改教練
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> EditCoach()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestEditCoachDto editCoachDto = new RequestEditCoachDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestEditService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addCoachDto, 2.file
                (editCoachDto, files) = await multipartRequestEditService.GetObjectAndFile(request);

                // 沒取到資料
                if (editCoachDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if (!ModelState.IsValid
                    || editCoachDto.CoachId < 1
                    || !formatValidation.ValidEmail(editCoachDto.Email)
                    || (editCoachDto.Phone != null && !formatValidation.ValidPhone(editCoachDto.Phone.Value))
                    || !formatValidation.ValidInput(
                        requireNonNull: false, minLength: 1, maxLength: 15, editCoachDto.Name)
                    || !formatValidation.ValidContractTime(editCoachDto.ContractStartTime,
                    editCoachDto.ContractEndTime)
                    || !formatValidation.ValidInput(
                        requireNonNull: false, minLength: null, maxLength: 50, editCoachDto.Introduction)
                    || !formatValidation.ValidInput(
                        requireNonNull: false, minLength: null, maxLength: 200, editCoachDto.Specialty)
                    || !formatValidation.ValidInput(
                        requireNonNull: false, minLength: null, maxLength: 200, editCoachDto.Certification)
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

                (ErrorCodeDefine errorCode, Exception exception) = coachService.EditCoach(editCoachDto,
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
