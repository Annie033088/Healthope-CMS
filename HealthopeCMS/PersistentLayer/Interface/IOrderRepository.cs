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
        (int errorCodeNumber, DBResponsePaymentDto dBResponsePaymentDto) PayByCash(RequestPayByCashDto payByCashDto);

        /// <summary>
        /// 刷卡付款
        /// </summary>
        (int errorCodeNumber, DBResponsePaymentDto dBResponsePaymentDto) PayByCardSuccess(RequestPayByCardDto payByCardDto);

        /// <summary>
        /// 取得訂單
        /// </summary>
        ResponseGetOrderListDto GetOrder(RequestGetOrderDto getOrderDto);

        /// <summary>
        /// 新增信用卡交易紀錄 (待付款)
        /// </summary>
        (CreditCardTransaction creditCardTransaction, int errorCodeNumber) AddCreditCardTransaction(
            RequestPayByCardDto payByCardDto);

        /// <summary>
        /// 根據 id 取得訂單
        /// </summary>
        (Order order, List<OrderState> orderStates) GetOrderDetailById(int orderId);

        /// <summary>
        /// 修改訂單狀態備註
        /// </summary>
        bool EditOrderStateRemark(OrderState orderState);

        /// <summary>
        /// 修改訂單備註
        /// </summary>
        bool EditOrderRemark(Order order);

        /// <summary>
        /// 修改訂單狀態：待付款 => 取消
        /// </summary>
        bool CancelPendingOrder(Order order);

        /// <summary>
        /// 修改訂單狀態：已付款 => 7日內退款
        /// </summary>
        (int errorCodeNumber, string invoiceNumber) RefundIn7Days(Order order);

        /// <summary>
        /// 確認是否可以無條件退費 若是=>請前端管理者確認是否要解約而不是無條件退費, 若否=>直接走解約流程
        /// </summary>
        (int errorCodeNumber, bool haveRefundQualify) CheckoutRefundQualify(Order order);

        /// <summary>
        /// 修改訂單狀態：已付款 => 解約
        /// </summary>
        (int errorCodeNumber, string invoiceNumber) TerminateOrder(Order order);
    }
}
