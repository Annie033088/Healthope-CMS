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
using ApiLayer.Models.Other;
using DomainLayer.Utility;
using NLog;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class GroupClassShowcaseController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMultipartRequestService<RequestAddShowcaseDto> multipartRequestAddService;
        private readonly IGroupClassShowcaseService groupClassShowcaseService;

        public GroupClassShowcaseController(IMultipartRequestService<RequestAddShowcaseDto> multipartRequestAddService,
            IGroupClassShowcaseService groupClassShowcaseService)
        {
            this.multipartRequestAddService = multipartRequestAddService;
            this.groupClassShowcaseService = groupClassShowcaseService;
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
    }
}
