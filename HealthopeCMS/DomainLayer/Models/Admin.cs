using System;

namespace DomainLayer.Models
{
    public class Admin
    {
        /// <summary>
        /// 管理員 Id
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 管理員帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 管理員加密密碼
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 管理員狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 職位描述(或權限描述)
        /// </summary>
        public string PositionDescription { get; set; }

        /// <summary>
        /// 身份(對應權限)
        /// </summary>
        public byte Identity { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
