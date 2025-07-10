using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models.Report.Request;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            this.reportRepository = reportRepository;
        }

        public List<ResponseGetRevenueExpenseDto> GetRevenueExpenseReport(RequestGetRevenueExpenseReportDto getFinancialStatementDto)
        {
            try
            {
                return reportRepository.GetRevenueExpenseReport(getFinancialStatementDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}