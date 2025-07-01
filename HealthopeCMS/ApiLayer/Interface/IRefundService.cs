using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistentLayer.Models;
using System.Web.Http;
using ApiLayer.Models.Refund.Response;

namespace ApiLayer.Interface
{
    public interface IRefundService
    {
        /// <summary>
        /// 取得退款紀錄
        /// </summary>
        ResponseGetRefundListDto GetRefund(RequestGetRefundDto getRefundDto);
    }
}
