using System;

namespace ApiLayer.Models.GroupClassShowcase.Response
{
    public class ResponseGetShowcaseEditDataDto
    {
        /// <summary>
        /// 課程名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 簡介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 內文
        /// </summary>
        public string DetailContent { get; set; }

        /// <summary>
        /// 展示圖路徑
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public int Icon { get; set; }

        /// <summary>
        /// 排序(可重複)
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}