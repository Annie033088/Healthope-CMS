using System;

namespace PersistentLayer.Models
{
    public class RequestEditAdminDto
    {
        /// <summary>
        /// 管理員 Id
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 管理員狀態
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 身份(對應權限)
        /// </summary>
        public byte? Identity { get; set; }


        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}