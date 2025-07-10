using ApiLayer.Models.Refund.Response;
using PersistentLayer.Models;

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
