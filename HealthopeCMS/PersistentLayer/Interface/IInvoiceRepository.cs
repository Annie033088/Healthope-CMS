using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IInvoiceRepository
    {
        /// <summary>
        /// 新增發票字軌
        /// </summary>
        bool AddInvoiceTrackNumber(InvoiceTrackNumber trackNumber);

        /// <summary>
        /// 取得字軌
        /// </summary>
        (List<InvoiceTrackNumber> invoiceTrackNumbers, int totalPage) GetInvoiceTrackNumber(
            RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto);

        /// <summary>
        /// 修改字軌狀態
        /// </summary>
        int EditInvoiceTrackNumberStatus(InvoiceTrackNumber invoiceTrackNumber);

        /// <summary>
        /// 修改字軌狀態
        /// </summary>
        bool DeleteInvoiceTrackNumber(int invoiceTrackNumberId);

        /// <summary>
        /// 根據開立成功與否設置
        /// </summary>
        bool EditElectronicInvoiceStatus(bool success, int electronicInvoiceId, string invocieTime);

        /// <summary>
        /// 取得發票號碼
        /// </summary>
        (int errorCodeNumber, ElectronicInvoice electronicInvoice, string planName) EditOrderStateAndGetInvoiceNumber(ElectronicInvoice invoice);

        /// <summary>
        /// 作廢發票
        /// </summary>
        int VoidInvoice(ElectronicInvoice invoice);

        /// <summary>
        /// 折讓發票
        /// </summary>
        int DiscountInvoice(ElectronicInvoice invoice);

        /// <summary>
        /// 修改發票狀態 => 待作廢
        /// </summary>
        int PendingVoidInvoice(ElectronicInvoice invoice);

        /// <summary>
        /// 修改發票狀態 => 待折讓
        /// </summary>
        int PendingDiscountInvoice(ElectronicInvoice invoice);

        /// <summary>
        /// 取得字軌
        /// </summary>
        (List<ElectronicInvoice> invoices, int totalPage) GetInvoice(RequestGetInvoiceDto getInvoiceDto);
    }
}
