using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models;
using NLog;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using ApiLayer.Models.Coach.Request;
using System.Collections.ObjectModel;
using DomainLayer.Utility;
using ApiLayer.Service;
using ApiLayer.Models.Other;
using System.Threading.Tasks;

namespace ApiLayer.Controllers.api
{
    public class CoachController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMultipartRequestService<RequestAddCoachDto> multipartRequestService;
        private readonly ICoachService coachService;

        public CoachController(IMultipartRequestService<RequestAddCoachDto> multipartRequestService, ICoachService coachService)
        {
            this.multipartRequestService = multipartRequestService;
            this.coachService = coachService;
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
                if (!multipartRequestService.IsMultipartRequest(request))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 取得請求的 form data 包括 1.addCoachDto, 2.file
                (addCoachDto, files) = await multipartRequestService.GetObjectAndFile(request);

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
                    || !formatValidation.ValidContractTime(addCoachDto.ContractStartTime, addCoachDto.ContractEndTime)
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

                if (files != null && !formatValidation.ValidImageFile(files[0].FileData, files[0].MimeType))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (ErrorCodeDefine errorCode, Exception exception) = coachService.AddCoach(addCoachDto, files == null ? null : files[0]);

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
