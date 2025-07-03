using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Invoice.Response;
using ApiLayer.Models.Job;
using ApiLayer.Models.Order.Request;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IInvoiceService
    {
        /// <summary>
        /// 新增發票字軌
        /// </summary>
        bool AddInvoiceTrackNumber(RequestAddInvoiceTrackNumberDto addInvoiceTrackNumberDto);

        /// <summary>
        /// 取得字軌
        /// </summary>
        ResponseGetInvoiceTrackNumberListDto GetInvoiceTrackNumber(
            RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto);

        /// <summary>
        /// 修改字軌狀態
        /// </summary>
        ErrorCodeDefine EditInvoiceTrackNumberStatus(RequestEditInvoiceTrackNumberStatusDto editInvoiceTrackNumberStatusDto);

        /// <summary>
        /// 修改字軌狀態
        /// </summary>
        bool DeleteInvoiceTrackNumber(InvoiceTrackNumberIdDto invoiceTrackNumberIdDto);

        /// <summary>
        /// 請求第三方開立發票
        /// </summary>
        Task<Task> PrintInvoice(RequestPrintInvoiceDto requestPrintInvoiceDto);

        /// <summary>
        /// 修改電子發票狀態
        /// </summary>
        bool EditElectronicInvoiceStatus(bool success, int electronicInvoiceId, string invocieTime);

        /// <summary>
        /// 取得發票號碼
        /// </summary>
        (ErrorCodeDefine errorCode, RequestPrintInvoiceDto printInvoiceDto) EditOrderStateAndGetInvoiceNumber(RequestOrderIdAndCategoryDto orderIdAndCategoryDto);

        /// <summary>
        /// 作廢發票
        /// </summary>
        ErrorCodeDefine VoidInvoice(RequestEditInvoiceStatusDto editInvoiceStatusDto);

        /// <summary>
        /// 折讓發票
        /// </summary>
        ErrorCodeDefine DiscountInvoice(RequestEditInvoiceStatusDto editInvoiceStatusDto);

        /// <summary>
        /// 修改發票狀態 => 待作廢
        /// </summary>
        ErrorCodeDefine PendingVoidInvoice(RequestEditInvoiceStatusDto editInvoiceStatusDto);

        /// <summary>
        /// 修改發票狀態 => 待折讓
        /// </summary>
        ErrorCodeDefine PendingDiscountInvoice(RequestEditInvoiceStatusDto editInvoiceStatusDto);

        /// <summary>
        /// 取得發票清單
        /// </summary>
        ResponseGetInvoiceListDto GetInvoice(RequestGetInvoiceDto getInvoiceDto);
    }
}
