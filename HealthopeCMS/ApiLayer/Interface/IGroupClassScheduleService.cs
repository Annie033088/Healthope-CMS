using ApiLayer.Models;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IGroupClassScheduleService
    {
        /// <summary>
        /// 取得 新增團體課程表前 需要的資料
        /// </summary>
        ResponseGetShowcaseAndCoachDto GetShowcaseAndCoach(RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto);

        /// <summary>
        /// 新增團課 schedule
        /// </summary>
        ErrorCodeDefine AddSchedule(RequestAddScheduleDto addScheduleDto);

        /// <summary>
        /// 取得團課 schedule
        /// </summary>
        ResponseGetScheduleListDto GetSchedule(RequestGetGroupClassScheduleDto getScheduleDto);
    }
}
