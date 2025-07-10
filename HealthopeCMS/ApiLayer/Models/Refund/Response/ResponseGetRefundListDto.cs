using System.Collections.Generic;

namespace ApiLayer.Models.Refund.Response
{
    public class ResponseGetRefundListDto
    {
        /// <summary>
        /// 退款清單
        /// </summary>
        public List<DomainLayer.Models.Refund> RefundList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }
}