using System.Collections.Generic;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.Member.Request;
using ApiLayer.Models.Member.Response;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IMemberService
    {
        /// <summary>
        /// 取得會員列表
        /// </summary>
        ResponseGetMemberListDto GetMember(RequestGetMemberDto getMemberDto);

        /// <summary>
        /// 根據 id 取得修改會員時需要的資料
        /// </summary>
        ResponseGetMemberEditDataByIdDto GetMemberEditDataById(RequestMemberIdDto getMemberByIdDto);

        /// <summary>
        /// 修改會員手機或狀態
        /// </summary>
        ErrorCodeDefine EditMember(RequestEditMemberDto editMemberDto);

        /// <summary>
        /// 取得會員詳細資料
        /// </summary>
        ResponseGetMemberDetailDto GetMemberDetail(RequestMemberIdDto memberIdDto);

        /// <summary>
        /// 根據電話或名稱取得會員
        /// </summary>
        List<ResponseGetMemberByNameOrPhoneDto> GetMemberByNameOrPhone(RequestGetMemberByNameOrPhoneDto getMemberDto);
    }
}
