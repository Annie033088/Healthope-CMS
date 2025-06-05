using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Models.Term.Response;

namespace ApiLayer.Models.Invoice.Response
{
    public class ResponseGetInvoiceTrackNumberListDto
    {
        /// <summary>
        /// 字軌清單
        /// </summary>
        public List<ResponseGetInvoiceTrackNumberDto> InvoiceTrackNumberList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }
    public class ResponseGetInvoiceTrackNumberDto
    {
        /// <summary>
        /// 字軌 Id
        /// </summary>
        public int InvoiceTrackNumberId { get; set; }

        /// <summary>
        /// 發票號碼前兩碼
        /// </summary>
        public string TrackPrefix { get; set; }

        /// <summary>
        /// 起始碼
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// 結束碼
        /// </summary>
        public int EndNumber { get; set; }

        /// <summary>
        /// 當前已使用號碼
        /// </summary>
        public int CurrentNumber { get; set; }

        /// <summary>
        /// 哪年哪期的發票 (EX:1141(114年1期), 1142...)
        /// </summary>
        public int InvoicePeriod { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}