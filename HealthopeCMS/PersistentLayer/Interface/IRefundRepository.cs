using System.Collections.Generic;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IRefundRepository
    {
        /// <summary>
        /// 取得退款紀錄
        /// </summary>
        (List<ResponseGetRefundDto> refunds, int totalPage) GetRefund(RequestGetRefundDto getRefundDto);
    }
}
