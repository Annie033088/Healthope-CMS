using System;
using System.Web.Http;
using ApiLayer.Models;
using ApiLayer.Models.Admin.ResponseAdminDto;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    public class MemberController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                getMemberDto.SearchName = getMemberDto.SearchName.Trim();
                if (getMemberDto.SearchPhone < 100 || getMemberDto.SearchPhone > 999) modelValidFlag = false;
                if (!((getMemberDto.SearchName == null) || (getMemberDto.SearchName.Length > 1))) modelValidFlag = false;
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

                ResponseGetAdminListDto responseGetAdminListDto = adminService.GetAdmin(getMemberDto);
                response = new ResultResponse<ResponseGetAdminListDto> { ErrorCode = ErrorCodeDefine.Success, ApiDataObject = responseGetAdminListDto };
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
