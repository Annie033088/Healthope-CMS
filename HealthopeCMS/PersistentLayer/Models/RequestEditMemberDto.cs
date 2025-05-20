using System;

namespace PersistentLayer.Models
{
    public class RequestEditMemberDto
    {
        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public int? Phone { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
