using System.Threading.Tasks;
using System.Web.Http;
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

        /// <summary>
        /// 刷卡付款
        /// </summary>
        Task<(ErrorCodeDefine errorCode, ResponseQrCodeStringDto QrCodeStringDto)> PayByCard(RequestPayByCardDto payByCardDto);

        /// <summary>
        /// 取得訂單
        /// </summary>
        ResponseGetOrderListDto GetOrder(RequestGetOrderDto getOrderDto);
    }
}
