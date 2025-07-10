using System.Collections.Generic;
using ApiLayer.Models.Report.Request;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IReportService
    {
        /// <summary>
        /// 取得收支資料
        /// </summary>
        List<ResponseGetRevenueExpenseDto> GetRevenueExpenseReport(RequestGetRevenueExpenseReportDto getFinancialStatementDto);
    }
}
