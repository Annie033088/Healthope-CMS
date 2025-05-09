namespace PersistentLayer.Models
{
    public class RequestGetMemberDto
    {
        /// <summary>
        /// 篩選狀態
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
        /// 搜尋的名稱
        /// </summary>
        public string SearchName { get; set; }

        /// <summary>
        /// 搜尋的手機末三碼
        /// </summary>
        public int? SearchPhone { get; set; }

        /// <summary>
        /// 搜尋的頁數
        /// </summary>
        public int Page { get; set; }
    }
}