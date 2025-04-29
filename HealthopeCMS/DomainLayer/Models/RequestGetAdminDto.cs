using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Admin.RequestAdminDto
{
    public class RequestGetAdminDto
    {
        /// <summary>
        /// 篩選狀態
        /// </summary>
        public bool? Status {  get; set; }

        /// <summary>
        /// 升/降序
        /// </summary>
        public string SortOrder {  get; set; }

        /// <summary>
        /// 排序選項
        /// </summary>
        public string SortOption {  get; set; }

        /// <summary>
        /// 一頁顯示 x 筆
        /// </summary>
        public int RecordPerPage {  get; set; }

        /// <summary>
        /// 搜尋的帳號
        /// </summary>
        public string SearchAccount {  get; set; }

        /// <summary>
        /// 搜尋的頁數
        /// </summary>
        public int Page { get; set; }
    }
}