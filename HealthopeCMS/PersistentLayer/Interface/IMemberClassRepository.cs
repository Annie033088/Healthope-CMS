using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IMemberClassRepository
    {
        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        List<ResponseGetPersonalTrainingPackageAndCoachDto> GetPersonalTrainingPackageAndCoach(int memberId);

        /// <summary>
        /// 新增會員預約教練課程
        /// </summary>
        int AddMemberPersonalClass(MemberPersonalClass memberPersonalClass);

        /// <summary>
        /// 取得會員預約的教練課程列表
        /// </summary>
        ResponseGetMemberPersonalClassListDto GetMemberPersonalClass(RequestGetMemberPersonalClassDto getMemberPersonalClassDto);
    }
}
