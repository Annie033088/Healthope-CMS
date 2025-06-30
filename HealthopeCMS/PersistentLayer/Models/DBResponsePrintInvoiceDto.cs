using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public class DBResponsePrintInvoiceDto
    {
        /// <summary>
        /// 電子發票 Id
        /// </summary>
        public int ElectronicInvoiceId { get; set; }

        /// <summary>
        /// 發票號碼
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// 隨機碼
        /// </summary>
        public string RandomNumber { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// 方案(商品)名稱
        /// </summary>
        public string PlanName { get; set; }
    }
}
