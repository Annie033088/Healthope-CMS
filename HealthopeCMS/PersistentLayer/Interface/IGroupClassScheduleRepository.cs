using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IGroupClassScheduleRepository
    {
        /// <summary>
        /// 取得 新增團體課程表前 需要的資料
        /// </summary>
        (List<GroupClassShowcase> showcases, List<Coach> coaches) GetShowcaseAndCoach(int? category);

        /// <summary>
        /// 新增團課 schedule
        /// </summary>
        int AddSchedule(GroupClassSchedule schedule, Coach coach);

        /// <summary>
        /// 取得團課 schedule
        /// </summary>
        (List<GroupClassSchedule> schedules, int totalPage) GetSchedule(RequestGetGroupClassScheduleDto getScheduleDto);
    }
}
