using System.Collections.Generic;
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
    }
}
