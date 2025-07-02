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
        (Order order, List<OrderState> orderStates, List<ElectronicInvoice> electronicInvoices) GetOrderDetailById(int orderId);

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
        /// 訂單 7 日內無條件退款
        /// </summary>
        (int errorCodeNumber, string invoiceNumber) RefundIn7Days(Order order);

        /// <summary>
        /// 確認是否有無條件退費資格
        /// </summary>
        (int errorCodeNumber, bool haveRefundQualify) CheckoutUnconditionalRefundQualify(Order order);

        /// <summary>
        /// 解約訂單
        /// </summary>
        (int errorCodeNumber, string invoiceNumber) TerminateOrder(Order order);

        /// <summary>
        /// 違約訂單
        /// </summary>
        (int errorCodeNumber, string invoiceNumber, DBResponsePrintInvoiceDto dbResponse) BreachOrder(Order order);
    }
}
