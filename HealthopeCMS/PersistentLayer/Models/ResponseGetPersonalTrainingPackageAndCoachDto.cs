using System;

namespace PersistentLayer.Models
{
    public class ResponseGetPersonalTrainingPackageAndCoachDto
    {
        /// <summary>
        /// 會員的教練課方案 Id
        /// </summary>
        public int MemberPersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 教練 Id
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 教練手機號碼
        /// </summary>
        public int CoachPhone { get; set; }

        /// <summary>
        /// 教練姓名
        /// </summary>
        public string CoachName { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 已使用課堂數
        /// </summary>
        public int UsedSession { get; set; }

        /// <summary>
        /// 課堂數
        /// </summary>
        public int SessionCount { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
