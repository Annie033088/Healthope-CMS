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
        (int errorCodeNumber, ElectronicInvoice electronicInvoice, string planName) EditOrderStateAndGetInvoiceNumber(int orderId);
    }
}
