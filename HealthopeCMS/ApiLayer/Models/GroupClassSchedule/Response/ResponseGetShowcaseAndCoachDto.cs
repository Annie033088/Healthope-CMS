using System;
using System.Collections.Generic;

namespace ApiLayer.Models.GroupClassSchedule.Response
{
    public class ResponseGetShowcaseAndCoachDto
    {
        /// <summary>
        /// 展示課列表
        /// </summary>
        public List<ScheduleGetShowcaseDto> ShowcaseList { get; set; }

        /// <summary>
        /// 教練列表
        /// </summary>
        public List<ScheduleGetCoachDto> CoachList { get; set; }
    }

    public class ScheduleGetShowcaseDto
    {
        /// <summary>
        /// 課程名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public int Icon { get; set; }
    }

    public class ScheduleGetCoachDto
    {
        /// <summary>
        /// 教練 ID，主鍵
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 教練姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}