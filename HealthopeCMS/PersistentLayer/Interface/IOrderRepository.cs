using System.Collections.Generic;
using ApiLayer.Models.Order.Response;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IOrderRepository
    {
        /// <summary>
        /// 新增訂單
        /// </summary>
        (Order order, int errorCodeNumber) AddOrder(Order addOrder, long orderNumber);

        /// <summary>
        /// 現金付款
        /// </summary>
        (int errorCodeNumber, DBResponsePayByCashDto dBResponsePayByCashDto) PayByCash(RequestPayByCashDto payByCashDto);

        /// <summary>
        /// 刷卡付款
        /// </summary>
        (int errorCodeNumber, DBResponsePayByCashDto dBResponsePayByCashDto) PayByCard(RequestPayByCardDto payByCardDto);

        /// <summary>
        /// 取得訂單
        /// </summary>
        ResponseGetOrderListDto GetOrder(RequestGetOrderDto getOrderDto);
    }
}
