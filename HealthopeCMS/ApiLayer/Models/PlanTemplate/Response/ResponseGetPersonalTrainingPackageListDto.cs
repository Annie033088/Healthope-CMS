using System;
using System.Collections.Generic;

namespace ApiLayer.Models.PlanTemplate.Response
{
    public class ResponseGetPersonalTrainingPackageListDto
    {
        /// <summary>
        /// 教練課方案清單
        /// </summary>
        public List<ResponseGetPersonalTrainingPackageDto> PersonalTrainingPackageList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }
    public class ResponseGetPersonalTrainingPackageDto
    {
        /// <summary>
        /// 教練課方案 Id
        /// </summary>
        public int PersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 課堂數
        /// </summary>
        public int SessionCount { get; set; }

        /// <summary>
        /// 介紹
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 是否顯示在前台
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 狀態 (有/無效)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}