using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
