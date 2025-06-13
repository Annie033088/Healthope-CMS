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
    }
}
