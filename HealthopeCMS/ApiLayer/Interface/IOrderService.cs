using ApiLayer.Models;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// 新增訂單
        /// </summary>
        (ResponseAddOrderDto response, ErrorCodeDefine errorCode) AddOrder(RequestAddOrderDto addOrderDto);

        /// <summary>
        /// 現金付款
        /// </summary>
        (ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto) PayByCash(RequestPayByCashDto payByCashDto);
    }
}
