using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IRefundRepository
    {
        /// <summary>
        /// 取得退款紀錄
        /// </summary>
        (List<Refund> refunds, int totalPage) GetRefund(RequestGetRefundDto getRefundDto);
    }
}
