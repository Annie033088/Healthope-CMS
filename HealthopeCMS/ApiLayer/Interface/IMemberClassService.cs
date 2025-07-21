using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.MemberClass.Request;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IMemberClassService
    {
        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        List<ResponseGetPersonalTrainingPackageAndCoachDto> GetPersonalTrainingPackageAndCoach(RequestMemberIdDto memberIdDto);

        /// <summary>
        /// 新增會員預約教練課程
        /// </summary>
        ErrorCodeDefine AddMemberPersonalClass(RequestAddMemberPersonalClassDto addMemberPersonalClassDto);

        /// <summary>
        /// 取得會員預約的教練課程列表
        /// </summary>
        ResponseGetMemberPersonalClassListDto GetMemberPersonalClass(RequestGetMemberPersonalClassDto getMemberPersonalClassDto);

        /// <summary>
        /// 修改預約課程備註
        /// </summary>
        bool EditMemberPersonalClassRemark(RequestEditMemberPersonalClassRemarkDto editMemberPersonalClassRemarkDto);

        /// <summary>
        /// 修改會員的教練預約課程狀態
        /// </summary>
        ErrorCodeDefine EditMemberPersonalClassStatus(RequestEditMemberPersonalClassStatusDto editStatusDto);

        /// <summary>
        /// 每日取消當日預約中的教練課程 (預約中課程於一天之前 無確認，即改為取消)
        /// </summary>
        Task AutoCancelReservingMemberPersonalClass();
    }
}
