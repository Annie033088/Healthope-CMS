using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models.Other;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class GroupClassShowcaseController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMultipartRequestService<RequestAddShowcaseDto> multipartRequestAddService;
        private readonly IMultipartRequestService<RequestEditShowcaseDto> multipartRequestEditService;
        private readonly IGroupClassShowcaseService groupClassShowcaseService;

        public GroupClassShowcaseController(IMultipartRequestService<RequestAddShowcaseDto> multipartRequestAddService,
            IGroupClassShowcaseService groupClassShowcaseService,
            IMultipartRequestService<RequestEditShowcaseDto> multipartRequestEditService)
        {
            this.multipartRequestAddService = multipartRequestAddService;
            this.groupClassShowcaseService = groupClassShowcaseService;
            this.multipartRequestEditService = multipartRequestEditService;
        }

        /// <summary>
        /// 新增展示用團課
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> AddShowcase()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestAddShowcaseDto addShowcaseDto = new RequestAddShowcaseDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestAddService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addShowcaseDto, 2.file
                (addShowcaseDto, files) = await multipartRequestAddService.GetObjectAndFile(request);

                // 沒取到資料
                if (addShowcaseDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if (!ModelState.IsValid
                    || !formatValidation.ValidInput(true, 1, 15, addShowcaseDto.Name)
                    || !formatValidation.ValidInput(true, null, 80, addShowcaseDto.Summary)
                    || !formatValidation.ValidInput(true, null, 500, addShowcaseDto.DetailContent)
                    || !Enum.IsDefined(typeof(GroupClassCategory), addShowcaseDto.Category)
                    || addShowcaseDto.Icon < 1 || addShowcaseDto.Sort < 1
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

                (ErrorCodeDefine errorCode, Exception exception) = groupClassShowcaseService.AddShowcase(addShowcaseDto,
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
        /// 取得展示用課程
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetShowcase([FromBody] RequestGetShowcaseDto getShowcaseDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (getShowcaseDto.SearchName != null && getShowcaseDto.SearchName.Length > 20)
                    modelValidFlag = false;
                if (!((getShowcaseDto.SortOrder == "ascending")
                    || (getShowcaseDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getShowcaseDto.SortOption == "name") || (getShowcaseDto.SortOption == "sort")
                    || (getShowcaseDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getShowcaseDto.RecordPerPage == 8) || (getShowcaseDto.RecordPerPage == 12)
                    || (getShowcaseDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getShowcaseDto.Category != null &&
                    !Enum.IsDefined(typeof(GroupClassCategory), getShowcaseDto.Category))
                    modelValidFlag = false;
                if (getShowcaseDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetShowcaseListDto getShowcaseListDto = groupClassShowcaseService.GetShowcase(getShowcaseDto);
                response = new ResultResponse<ResponseGetShowcaseListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = getShowcaseListDto
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
        /// 取得展示用團課細項
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetShowcaseDetail([FromBody] RequestShowcaseIdDto showcaseIdDto)
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

        /// <summary>
        /// 取得修改展示用團課頁面的資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetShowcaseEditDataById([FromBody] RequestShowcaseIdDto showcaseIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (showcaseIdDto.GroupClassShowcaseId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetShowcaseEditDataDto responseGetShowcaseDto =
                    groupClassShowcaseService.GetShowcaseEditDataById(showcaseIdDto);
                response = new ResultResponse<ResponseGetShowcaseEditDataDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetShowcaseDto
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
        /// 修改展示用團課
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> EditShowcase()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                RequestEditShowcaseDto editShowcaseDto = new RequestEditShowcaseDto();
                List<FileDto> files = new List<FileDto>();
                HttpRequestMessage request = Request;

                // 檢查請求是否為 multipart/form-data ( MIME 類型，表明這是「多部分資料」的格式 )
                if (!multipartRequestEditService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addCoachDto, 2.file
                (editShowcaseDto, files) = await multipartRequestEditService.GetObjectAndFile(request);

                // 沒取到資料
                if (editShowcaseDto == default)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 格式驗證
                if ((editShowcaseDto.Icon != null && editShowcaseDto.Icon < 1)
                    || (editShowcaseDto.Sort != null && editShowcaseDto.Sort < 1)
                    || editShowcaseDto.GroupClassShowcaseId < 1
                    || !formatValidation.ValidInput(false, 1, 15, editShowcaseDto.Name)
                    || !formatValidation.ValidInput(false, null, 80, editShowcaseDto.Summary)
                    || !formatValidation.ValidInput(false, null, 500, editShowcaseDto.DetailContent)
                    || (editShowcaseDto.Category != null && !Enum.IsDefined(
                        typeof(GroupClassCategory), editShowcaseDto.Category)))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                if (files.Any() && !formatValidation.ValidImageFile(files[0].FileData, files[0].MimeType))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, Exception exception) = groupClassShowcaseService.EditShowcase(editShowcaseDto,
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
        /// 刪除展示用團課
        /// </summary>
        [HttpPost]
        public IHttpActionResult DeleteShowcase([FromBody] RequestShowcaseIdDto showcaseIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (showcaseIdDto.GroupClassShowcaseId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = groupClassShowcaseService.DeleteShowcase(showcaseIdDto);

                if (successFlag)
                    response = new ResultResponse() { ErrorCode = ErrorCodeDefine.Success };
                else
                    response = new ResultResponse() { ErrorCode = ErrorCodeDefine.DeleteFailed };

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
