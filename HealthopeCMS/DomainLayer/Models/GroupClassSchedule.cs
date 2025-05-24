using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DomainLayer.Models
{
    public class GroupClassSchedule
    {
        /// <summary>
        /// 團課排程 Id
        /// </summary>
        public int GroupClassScheduleId { get; set; }

        /// <summary>
        /// 教練Id
        /// </summary>
        public int CoachId { get; set; }

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
        public byte MaximumParticipant { get; set; }

        /// <summary>
        /// 目前預約人數
        /// </summary>
        public byte ReserveParticipant { get; set; }

        /// <summary>
        /// 報到人數
        /// </summary>
        public byte CheckInParticipant { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        public byte Tag { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
