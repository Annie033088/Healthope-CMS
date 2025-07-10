namespace PersistentLayer.Models
{
    public class ResponseGetRevenueExpenseDto
    {
        /// <summary>
        /// 會籍收入
        /// </summary>
        public int MembershipRevenue { get; set; }

        /// <summary>
        /// 教練課收入
        /// </summary>
        public int PersonalTrainingRevenue { get; set; }

        /// <summary>
        /// 單次票卷收入
        /// </summary>
        public int SingleEntryRevenue { get; set; }

        /// <summary>
        /// 退款支出 包括 1.訂單取消 ; 2.解約 ; 3.違約， 有各自支出的計算方式
        /// </summary>
        public int RefundExpense { get; set; }

        /// <summary>
        /// 違約金收入
        /// </summary>
        public int PenaltyIncome { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public int Day { get; set; }
    }
}
