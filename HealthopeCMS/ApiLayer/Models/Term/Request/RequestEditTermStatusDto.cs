using System;

namespace ApiLayer.Models.Term.Request
{
    public class RequestEditTermStatusDto
    {
        /// <summary>
        /// 條款 Id
        /// </summary>
        public int TermId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}