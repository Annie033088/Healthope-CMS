using System;
using System.Collections.Generic;
using System.Linq;
using ApiLayer.Models.Report.Request;
using ApiLayer.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.ReportTest
{
    [TestClass]
    public class ReportServiceTest
    {
        private ReportService service;
        private Mock<IReportRepository> reportRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            reportRepositoryMock = new Mock<IReportRepository>();
            service = new ReportService(reportRepositoryMock.Object);
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
            reportRepositoryMock.Setup(s
                => s.GetRevenueExpenseReport(getFinancialStatementDto)).Returns(response);

            // Act
            List<ResponseGetRevenueExpenseDto> result = service.GetRevenueExpenseReport(getFinancialStatementDto);

            // Assert
            Assert.IsTrue(result.SequenceEqual(response));
        }
    }
}
