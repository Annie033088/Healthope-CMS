namespace ApiLayer.Models.PlanTemplate.Request
{
    public class RequestAddPersonalTrainingPackageDto
    {
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
    }
}