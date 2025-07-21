using System.Collections.Generic;
using System.Threading.Tasks;
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

        /// <summary>
        /// 修改預約課程備註
        /// </summary>
        bool EditMemberPersonalClassRemark(MemberPersonalClass memberPersonalClass);

        /// <summary>
        /// 修改會員的教練預約課程狀態
        /// </summary>
        int EditMemberPersonalClassStatus(MemberPersonalClass memberPersonalClass);

        /// <summary>
        /// 每日取消當日預約中的教練課程 (預約中課程於一天之前 無確認，即改為取消)
        /// </summary>
        Task AutoCancelReservingMemberPersonalClass();
    }
}
