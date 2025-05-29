using System;

namespace DomainLayer.Models
{
    public class Term
    {
        /// <summary>
        /// 條款 Id
        /// </summary>
        public int TermId { get; set; }

        /// <summary>
        /// 版本號
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 內文
        /// </summary>
        public string DetailContent { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 適用對象
        /// </summary>
        public byte ApplicableTarget { get; set; }

        /// <summary>
        /// 描述更新內容
        /// </summary>
        public string VersionDescription { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 條款生效日
        /// </summary>
        public DateTime EffectiveTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
