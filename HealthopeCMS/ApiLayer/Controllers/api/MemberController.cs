using System;
using System.Text.RegularExpressions;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Models.Member;
using ApiLayer.Models.Member.Response;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class MemberController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }
        // TODO: 查詢會籍/查詢教練課程
        /// <summary>
        /// 取得會員列表
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetMember([FromBody] RequestGetMemberDto getMemberDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if(!ModelState.IsValid) 
                    modelValidFlag = false;
                if (!formatValidation.ValidSearchPhone(getMemberDto.SearchPhone))
                    modelValidFlag = false;
                if (getMemberDto.SearchName != null && getMemberDto.SearchName.Length > 50)
                    modelValidFlag = false;
                if (!((getMemberDto.SortOrder == "ascending") || (getMemberDto.SortOrder == "descending"))) 
                    modelValidFlag = false;
                if (!((getMemberDto.SortOption == "name") || (getMemberDto.SortOption == "status")
                    || (getMemberDto.SortOption == "membershipExpiry") || (getMemberDto.SortOption == null))) 
                    modelValidFlag = false;
                if (!((getMemberDto.RecordPerPage == 8) || (getMemberDto.RecordPerPage == 12) 
                    || (getMemberDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getMemberDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetMemberListDto responseGetMemberListDto = memberService.GetMember(getMemberDto);
                response = new ResultResponse<ResponseGetMemberListDto> { ErrorCode = ErrorCodeDefine.Success, ApiDataObject = responseGetMemberListDto };
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
        /// 根據 id 取得修改會員時需要的資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetMemberEditDataById([FromBody] RequestMemberIdDto memberIdDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (memberIdDto.MemberId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetMemberEditDataByIdDto responseGetMemberEditDataByIdDto =
                    memberService.GetMemberEditDataById(memberIdDto);

                if (responseGetMemberEditDataByIdDto == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetMemberEditDataByIdDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetMemberEditDataByIdDto
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
        /// 修改會員資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditMember([FromBody] RequestEditMemberDto editMemberDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (!ModelState.IsValid) modelValidFlag = false;
                if (editMemberDto.MemberId < 1) modelValidFlag = false;
                if (editMemberDto.Phone == null && editMemberDto.Status == null) modelValidFlag = false;

                if (editMemberDto.Phone != null && !formatValidation.ValidPhone(editMemberDto.Phone.Value))
                    modelValidFlag = false;

                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse { ErrorCode = memberService.EditMember(editMemberDto) };
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
        /// 取得會員詳細資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetMemberDetail([FromBody] RequestMemberIdDto memberIdDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (memberIdDto.MemberId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetMemberDetailDto responseGetMemberDetail =
                    memberService.GetMemberDetail(memberIdDto);

                if (responseGetMemberDetail == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetMemberDetailDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetMemberDetail
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
