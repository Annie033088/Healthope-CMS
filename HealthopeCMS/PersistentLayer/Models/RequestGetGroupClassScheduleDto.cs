using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public class RequestGetGroupClassScheduleDto
    {
        /// <summary>
        /// 篩選分類
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 搜尋的日期範圍
        /// </summary>
        public string DateRangeFilter { get; set; }

        /// <summary>
        /// 搜尋的明確日期
        /// </summary>
        public DateTime? SpecificDate { get; set; }

        /// <summary>
        /// 升/降序
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 排序選項
        /// </summary>
        public string SortOption { get; set; }

        /// <summary>
        /// 一頁顯示 x 筆
        /// </summary>
        public int RecordPerPage { get; set; }

        /// <summary>
        /// 搜尋的頁數
        /// </summary>
        public int Page { get; set; }
    }
}
