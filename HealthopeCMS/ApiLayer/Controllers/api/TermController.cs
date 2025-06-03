using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Member.Response;
using ApiLayer.Models.Term;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
using ApiLayer.Service;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
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

        /// <summary>
        /// 取得修改條款頁面的資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetTermEditDataById([FromBody] RequestTermIdDto termIdDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (termIdDto.TermId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetTermEditDataByIdDto term = termService.GetTermEditDataById(termIdDto);

                if (term == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetTermEditDataByIdDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = term
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
        /// 修改條款
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditTerm([FromBody] RequestEditTermDto editTermDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if ((editTermDto.DetailContent == null && editTermDto.VersionDescription == null)
                    || (editTermDto.DetailContent != null && editTermDto.DetailContent.Length > 7000)
                    || (editTermDto.VersionDescription != null && editTermDto.VersionDescription.Length > 200)
                    || editTermDto.TermId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse { ErrorCode = termService.EditTerm(editTermDto) };
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
        /// 修改條款狀態 (僅限草稿=>發布)
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditTermStatus([FromBody] RequestEditTermStatusDto editTermStatusDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid || editTermStatusDto.TermId < 1
                    || !Enum.IsDefined(typeof(TermStatus), editTermStatusDto.Status)
                    // 目前僅能 草稿 => 發布
                    || (TermStatus)editTermStatusDto.Status != TermStatus.Published)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse { ErrorCode = termService.EditTermStatus(editTermStatusDto) };
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
        /// 取得條款的詳細資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetTermDetail([FromBody] RequestTermIdDto termIdDto)
        {
            try
            {
                ResultResponse response;
                // 驗證前端傳遞的參數是否合法

                if (termIdDto.TermId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetTermDetailDto term = termService.GetTermDetail(termIdDto);

                if (term == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetTermDetailDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = term
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
        /// 刪除條款
        /// </summary>
        [HttpPost]
        public IHttpActionResult DeleteTerm([FromBody] RequestTermIdDto termIdDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                ResultResponse response;

                if (termIdDto.TermId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = termService.DeleteTerm(termIdDto);
                response = new ResultResponse()
                {
                    ErrorCode = successFlag ?
                   ErrorCodeDefine.Success : ErrorCodeDefine.DeleteFailed,
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
