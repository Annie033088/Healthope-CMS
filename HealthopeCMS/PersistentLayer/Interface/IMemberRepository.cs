using System.Collections.Generic;
using ApiLayer.Models.Member.Request;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IMemberRepository
    {
        /// <summary>
        /// 取得會員列表
        /// </summary>
        (List<Member> members, int totalPage) GetMember(RequestGetMemberDto getMemberDto);

        /// <summary>
        /// 根據 id 取得修改會員時需要的資料
        /// </summary>
        Member GetMemberEditDataById(int memberId);

        /// <summary>
        /// 修改會員手機或狀態
        /// </summary>
        int EditMember(RequestEditMemberDto editMemberDto);

        /// <summary>
        /// 取得會員詳細資料
        /// </summary>
        Member GetMemberDetail(int memberId);

        /// <summary>
        /// 根據電話或名稱取得會員
        /// </summary>
        List<Member> GetMemberByNameOrPhone(RequestGetMemberByNameOrPhoneDto getMemberDto);
    }
}
