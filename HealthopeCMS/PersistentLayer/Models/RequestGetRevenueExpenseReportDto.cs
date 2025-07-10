namespace ApiLayer.Models.Report.Request
{
    public class RequestGetRevenueExpenseReportDto
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int? Month { get; set; }
    }
}