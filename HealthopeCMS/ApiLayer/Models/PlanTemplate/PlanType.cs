using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.PlanTemplate
{
    public enum PlanType : byte
    {
        /// <summary>
        /// 會籍方案
        /// </summary>
        Membership = 1,
        
        /// <summary>
        /// 教練課方案
        /// </summary>
        Training = 2,
        
        /// <summary>
        /// 票劵方案
        /// </summary>
        Ticket = 3,
    }
}