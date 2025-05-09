using System;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Models.Member;
using ApiLayer.Models.Member.Response;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    public class MemberController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        /// <summary>
        /// 取得會員列表
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetMember([FromBody] RequestGetMemberDto getMemberDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;
                // 手機號碼搜尋格視為末 3 碼
                if (getMemberDto.SearchPhone != null)
                    if (getMemberDto.SearchPhone < 100 || getMemberDto.SearchPhone > 999)
                        modelValidFlag = false;
                if (!string.IsNullOrEmpty(getMemberDto.SearchName))
                {
                    getMemberDto.SearchName = getMemberDto.SearchName.Trim();

                    if (getMemberDto.SearchName.Length > 50) modelValidFlag = false;
                }
                if (!((getMemberDto.SortOrder == "ascending") || (getMemberDto.SortOrder == "descending"))) modelValidFlag = false;
                if (!((getMemberDto.SortOption == "account") || (getMemberDto.SortOption == "status") || (getMemberDto.SortOption == null))) modelValidFlag = false;
                if (!((getMemberDto.RecordPerPage == 8) || (getMemberDto.RecordPerPage == 12) || (getMemberDto.RecordPerPage == 16))) modelValidFlag = false;
                if (getMemberDto.Page < 1) modelValidFlag = false;

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
        public IHttpActionResult GetMemberEditDataById([FromBody] RequestMemberIdDto getMemberByIdDto)
        {
            try
            {
                ResultResponse response;

                // 驗證前端傳遞的參數是否合法
                if (getMemberByIdDto.MemberId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetMemberEditDataByIdDto ResponseGetMemberEditDataByIdDto = memberService.GetMemberEditDataById(getMemberByIdDto);
                
                if (ResponseGetMemberEditDataByIdDto == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetMemberEditDataByIdDto> { ErrorCode = ErrorCodeDefine.Success, ApiDataObject = ResponseGetMemberEditDataByIdDto };
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
