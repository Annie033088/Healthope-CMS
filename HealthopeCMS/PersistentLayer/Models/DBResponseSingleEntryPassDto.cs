using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public class DBResponseSingleEntryPassDto
    {
        /// <summary>
        /// 一次性入場劵 Id
        /// </summary>
        public int? SingleEntryPassId { get; set; }

        /// <summary>
        /// 隨機碼
        /// </summary>
        public Guid? TicketCode { get; set; }
    }
}
