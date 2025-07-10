using System.Collections.Generic;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IMemberClassRepository
    {
        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        List<ResponseGetPersonalTrainingPackageAndCoachDto> GetPersonalTrainingPackageAndCoach(int memberId);
    }
}
