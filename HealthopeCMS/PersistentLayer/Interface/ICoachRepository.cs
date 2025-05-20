using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface ICoachRepository
    {
        /// <summary>
        /// 新增教練
        /// </summary>
        ResultWithException AddCoach(Coach coach);

        /// <summary>
        /// 取得教練清單
        /// </summary>
        (List<Coach> coaches, int totalPage) GetCoach(RequestGetCoachDto getCoachDto);

        /// <summary>
        /// 取得修改教練頁面的資料
        /// </summary>
        Coach GetCoachEditDataById(int coachId);

        /// <summary>
        /// 修改教練
        /// </summary>
        (ResultWithException result, string oldPhotoUrl) EditCoach(RequestEditCoachDto editCoachDto);
    }
}
