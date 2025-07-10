using System;

namespace DomainLayer.Models
{
    public class MemberPersonalTrainingPackage
    {
        /// <summary>
        /// 會員的教練課方案 Id
        /// </summary>
        public int MemberPersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 教練 Id
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 課堂數
        /// </summary>
        public int SessionCount { get; set; }

        /// <summary>
        /// 狀態 1:進行中 ; 2:終止 ; 3:完成
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
