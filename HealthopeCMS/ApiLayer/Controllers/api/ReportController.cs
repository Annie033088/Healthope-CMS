using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Report.Request;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class ReportController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        /// <summary>
        /// 取得收支資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetRevenueExpenseReport([FromBody] RequestGetRevenueExpenseReportDto getFinancialStatementDto)
        {
            try
            {
                ResultResponse response;

                // 格式驗證
                if (getFinancialStatementDto.Year < 1
                    || (getFinancialStatementDto.Month != null && (getFinancialStatementDto.Month > 12 || getFinancialStatementDto.Month < 1)))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                List<ResponseGetRevenueExpenseDto> responseGetRevenues = reportService.GetRevenueExpenseReport(getFinancialStatementDto);

                response = new ResultResponse<List<ResponseGetRevenueExpenseDto>>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGetRevenues
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }
    }
}
