namespace ApiLayer.Models.PlanTemplate.Request
{
    public class RequestGetPlanDto
    {
        /// <summary>
        /// 篩選分類
        /// </summary>
        public bool? Status { get; set; }

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