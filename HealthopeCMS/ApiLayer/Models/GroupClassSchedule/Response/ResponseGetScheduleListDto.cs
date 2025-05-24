using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.GroupClassSchedule.Response
{
    public class ResponseGetScheduleListDto
    {
        /// <summary>
        /// 團課清單
        /// </summary>
        public List<ResponseGetScheduleDto> ScheduleList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetScheduleDto
    {
        /// <summary>
        /// 團課排程 Id
        /// </summary>
        public int GroupClassScheduleId { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 教練名稱
        /// </summary>
        public string CoachName { get; set; }

        /// <summary>
        /// 課程時間
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 課程地點
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 人數上限
        /// </summary>
        public int MaximumParticipant { get; set; }

        /// <summary>
        /// 目前預約人數
        /// </summary>
        public int ReserveParticipant { get; set; }

        /// <summary>
        /// 報到人數
        /// </summary>
        public int CheckInParticipant { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public int Status { get; set; }
    }
}