using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Report.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.ReportTest
{
    [TestClass]
    public class ReportControllerTest
    {
        private ReportController controller;
        private Mock<IReportService> reportServiceMock;

        [TestInitialize]
        public void Setup()
        {
            reportServiceMock = new Mock<IReportService>();
            controller = new ReportController(reportServiceMock.Object);
        }

        [TestMethod]
        public void 取得收支資料_成功_回傳資料()
        {
            // Arrange
            RequestGetRevenueExpenseReportDto getFinancialStatementDto = new RequestGetRevenueExpenseReportDto()
            {
                Month = 1,
                Year = 2000,
            };

            List<ResponseGetRevenueExpenseDto> response = new List<ResponseGetRevenueExpenseDto>()
            {
                new ResponseGetRevenueExpenseDto
                {
                    RefundExpense=1000,
                    Day = 1,
                    MembershipRevenue=2000,
                }
            };

            // Mock 設定
            reportServiceMock.Setup(s
                => s.GetRevenueExpenseReport(getFinancialStatementDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetRevenueExpenseReport(getFinancialStatementDto);

            // Assert
            ResponseIsEqual<List<ResponseGetRevenueExpenseDto>> responseIsEqual =
                new ResponseIsEqual<List<ResponseGetRevenueExpenseDto>>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得收支資料_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetRevenueExpenseReportDto getFinancialStatementDto = new RequestGetRevenueExpenseReportDto()
            {
                Month = 0,
                Year = 2000,
            };

            // Act
            IHttpActionResult result = controller.GetRevenueExpenseReport(getFinancialStatementDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
