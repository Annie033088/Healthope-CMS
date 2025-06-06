using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
