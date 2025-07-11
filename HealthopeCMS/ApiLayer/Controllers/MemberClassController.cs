using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.MemberClass.Request;
using ApiLayer.Models.Term.Response;
using ApiLayer.Models.Term;
using ApiLayer.Service;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;
using ApiLayer.Models.MemberClass;

namespace ApiLayer.Controllers
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class MemberClassController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemberClassService memberClassService;

        public MemberClassController(IMemberClassService memberClassService)
        {
            this.memberClassService = memberClassService;
        }

        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetPersonalTrainingPackageAndCoach([FromBody] RequestMemberIdDto memberIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式驗證
                if (memberIdDto.MemberId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                List<ResponseGetPersonalTrainingPackageAndCoachDto> responseGets =
                    memberClassService.GetPersonalTrainingPackageAndCoach(memberIdDto);

                response = new ResultResponse<List<ResponseGetPersonalTrainingPackageAndCoachDto>>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGets
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
        /// 新增會員預約教練課程
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddMemberPersonalClass([FromBody] RequestAddMemberPersonalClassDto addMemberPersonalClassDto)
        {
            try
            {
                ResultResponse response;

                // 格式驗證
                if (addMemberPersonalClassDto.MemberId < 1
                    || addMemberPersonalClassDto.MemberPersonalTrainingPackageId < 1
                    || addMemberPersonalClassDto.CoachId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse
                {
                    ErrorCode = memberClassService.AddMemberPersonalClass(addMemberPersonalClassDto),
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
        /// 取得會員預約的教練課程列表
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetMemberPersonalClass([FromBody] RequestGetMemberPersonalClassDto getMemberPersonalClassDto)
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法

                if (!ModelState.IsValid
                    || (getMemberPersonalClassDto.Status != null
                        && !Enum.IsDefined(typeof(MemberPersonalClassStatus), getMemberPersonalClassDto.Status))
                    || !formatValidation.ValidSearchPhone(getMemberPersonalClassDto.SearchPhone)
                    || ((getMemberPersonalClassDto.SortOption != "time") && (getMemberPersonalClassDto.SortOption != "caochId")
                        && (getMemberPersonalClassDto.SortOption != null))
                    || (getMemberPersonalClassDto.SortOrder != "ascending" && getMemberPersonalClassDto.SortOrder != "descending")
                    || (!((getMemberPersonalClassDto.RecordPerPage == 8) || (getMemberPersonalClassDto.RecordPerPage == 12)
                        || (getMemberPersonalClassDto.RecordPerPage == 16)))
                    || getMemberPersonalClassDto.Page < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetMemberPersonalClassListDto responseGet = memberClassService.GetMemberPersonalClass(getMemberPersonalClassDto);
                response = new ResultResponse<ResponseGetMemberPersonalClassListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGet
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
