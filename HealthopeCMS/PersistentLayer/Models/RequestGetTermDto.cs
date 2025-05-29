using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public class RequestGetTermDto
    {
        /// <summary>
        /// 篩選分類
        /// </summary>
        public byte? Type { get; set; }

        /// <summary>
        /// 篩選分類
        /// </summary>
        public byte? Status { get; set; }

        /// <summary>
        /// 篩選分類
        /// </summary>
        public byte? ApplicableTarget { get; set; }

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
