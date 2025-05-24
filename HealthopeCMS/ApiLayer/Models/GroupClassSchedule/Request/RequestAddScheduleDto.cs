using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Models.GroupClassSchedule.Response;

namespace ApiLayer.Models.GroupClassSchedule.Request
{
    public class RequestAddScheduleDto
    {
        /// <summary>
        /// 教練
        /// </summary>
        public ScheduleGetCoachDto Coach { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// Icon
        /// </summary>
        public int Icon { get; set; }

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
        public byte MaximumParticipant { get; set; }
    }
}