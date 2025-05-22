using System.Collections.Generic;

namespace ApiLayer.Models.GroupClassShowcase.Response
{
    public class ResponseGetShowcaseListDto
    {
        /// <summary>
        /// 教練清單
        /// </summary>
        public List<ResponseGetShowcaseDto> ShowcaseList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetShowcaseDto
    {
        /// <summary>
        /// 展示用團課 Id
        /// </summary>
        public int GroupClassShowcaseId { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 簡介
        /// </summary>
        public string Summary { get; set; }

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
    }
}