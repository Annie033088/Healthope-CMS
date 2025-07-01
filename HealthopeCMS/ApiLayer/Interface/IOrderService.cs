using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Response;
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

        /// <summary>
        /// 根據 id 取得訂單
        /// </summary>
        ResponseGetOrderDetailByIdDto GetOrderDetailById(RequestOrderIdDto orderIdDto);

        /// <summary>
        /// 修改訂單狀態備註
        /// </summary>
        bool EditOrderStateRemark(RequestEditOrderStateRemarkDto editOrderStateRemarkDto);

        /// <summary>
        /// 修改訂單備註
        /// </summary>
        bool EditOrderRemark(RequestEditOrderRemarkDto editOrderRemarkDto);

        /// <summary>
        /// 修改訂單狀態：待付款 => 取消
        /// </summary>
        bool CancelPendingOrder(RequestEditOrderStateDto editOrderStateDto);

        /// <summary>
        /// 訂單 7 日內無條件退款
        /// </summary>
        (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) RefundIn7Days(RequestEditOrderStateDto editOrderStateDto);

        /// <summary>
        /// 確認是否可以無條件退費 若是=>請前端管理者確認是否要解約而不是無條件退費, 若否=>直接走解約流程
        /// </summary>
        (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) CheckoutRefundQualifyAndTerminateOrder(
            RequestEditOrderStateDto editOrderStateDto);

        /// <summary>
        /// 解約訂單
        /// </summary>
        (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) TerminateOrder(RequestEditOrderStateDto editOrderStateDto);

        /// <summary>
        /// 確認是否可以無條件退費 若是=>請前端管理者確認是否要設置違約而不是無條件退費, 若否=>直接走違約流程
        /// </summary>
        (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) CheckoutRefundQualifyAndBreachOrder(
            RequestEditOrderStateDto editOrderStateDto);

        /// <summary>
        /// 違約訂單
        /// </summary>
        (ErrorCodeDefine errorCode, ResponseInvoiceNumberDto invoiceNumberDto) BreachOrder(RequestEditOrderStateDto editOrderStateDto);
    }
}
