using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Invoice.Response;
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
    }
}
