using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Term;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    public class TermController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ITermService termService;

        public TermController(ITermService termService)
        {
            this.termService = termService;
        }

        /// <summary>
        /// 取得舊條款
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetOldTerm([FromBody] RequestGetOldTermDto getOldTerm)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid || getOldTerm.Type < 1 || getOldTerm.ApplicableTarget < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                List<ResponseGetOldTermDto> oldTerms = termService.GetOldTerm(getOldTerm);
                response = new ResultResponse<List<ResponseGetOldTermDto>>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = oldTerms
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
        /// 新增條款
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddTerm([FromBody] RequestAddTermDto addTermDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                Console.WriteLine(addTermDto);

                if (!ModelState.IsValid || addTermDto.Type < 1 || addTermDto.ApplicableTarget < 1
                    || !formatValidation.ValidInput(true, null, 7000, addTermDto.DetailContent)
                    || !formatValidation.ValidInput(true, null, 200, addTermDto.VersionDescription))
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

        /// <summary>
        /// 取得條款
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetTerm([FromBody] RequestGetTermDto getTermDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid || getTermDto.Type < 1 || getTermDto.ApplicableTarget < 1
                    || (getTermDto.Status != null && !Enum.IsDefined(typeof(TermStatus), getTermDto.Status))
                    || (!((getTermDto.RecordPerPage == 8) || (getTermDto.RecordPerPage == 12)
                        || (getTermDto.RecordPerPage == 16)))
                    || getTermDto.Page < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetTermListDto terms = termService.GetTerm(getTermDto);
                response = new ResultResponse<ResponseGetTermListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = terms
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
