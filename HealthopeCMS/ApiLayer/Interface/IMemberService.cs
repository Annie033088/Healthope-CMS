using ApiLayer.Models.Member;
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
    }
}
