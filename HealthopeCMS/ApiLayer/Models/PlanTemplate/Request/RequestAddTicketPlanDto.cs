namespace ApiLayer.Models.PlanTemplate.Request
{
    public class RequestAddTicketPlanDto
    {
        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 狀態 (有/無效)
        /// </summary>
        public bool Status { get; set; }
    }
}