using System.Collections.Generic;
using ApiLayer.Models.Member;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IMemberClassService
    {
        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        List<ResponseGetPersonalTrainingPackageAndCoachDto> GetPersonalTrainingPackageAndCoach(RequestMemberIdDto memberIdDto);
    }
}
