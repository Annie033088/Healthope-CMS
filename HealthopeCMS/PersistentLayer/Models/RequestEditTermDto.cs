using System;

namespace PersistentLayer.Models
{
    public class RequestEditTermDto
    {
        /// <summary>
        /// 條款 Id
        /// </summary>
        public int TermId { get; set; }

        /// <summary>
        /// 內文
        /// </summary>
        public string DetailContent { get; set; }

        /// <summary>
        /// 描述更新內容
        /// </summary>
        public string VersionDescription { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
