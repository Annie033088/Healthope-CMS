namespace ApiLayer.Models.Order.Request
{
    public class RequestAddOrderDto
    {
        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 方案編號(EX:教練課/票劵/會籍 的方案Id)
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// 方案類別
        /// </summary>
        public byte PlanType { get; set; }

        /// <summary>
        /// 付款方式 (1.現金 ; 2.信用卡)
        /// </summary>
        public byte Method { get; set; }
    }
}