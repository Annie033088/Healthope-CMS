using System.Collections.Generic;
using ApiLayer.Models.Report.Request;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IReportRepository
    {
        /// <summary>
        /// 取得收支資料
        /// </summary>
        List<ResponseGetRevenueExpenseDto> GetRevenueExpenseReport(RequestGetRevenueExpenseReportDto getFinancialStatementDto);
    }
}
