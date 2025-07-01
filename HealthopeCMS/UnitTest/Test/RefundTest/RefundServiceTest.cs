using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Util;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.Refund.Response;
using ApiLayer.Models.Response.PlanTemplate;
using ApiLayer.Models.Transaction.Request;
using ApiLayer.Models.Transaction.Response;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.TransactionTest
{
    [TestClass]
    public class RefundServiceTest
    {
        private RefundService service;
        private Mock<IRefundRepository> refundRepositoryMock;
        private Mock<IMapper> mapperMock;

        [TestInitialize]
        public void Setup()
        {
            refundRepositoryMock = new Mock<IRefundRepository>();
            mapperMock = new Mock<IMapper>();
            service = new RefundService(refundRepositoryMock.Object, mapperMock.Object);
        }

        [TestMethod]
        public void 取得退款紀錄清單_成功_回傳清單()
        {
            // Arrange
            RequestGetRefundDto getRefundDto = new RequestGetRefundDto()
            {
                Status = 1,
                RefundType = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = null, // 只允許 status | createTime | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            List<Refund> refunds = new List<Refund>
            {
                new Refund
                {
                    RefundId = 1,
                    OrderId = 1,
                    RefundType = 2,
                    Status = 1,
                    RefundAmount = 1000,
                    PenaltyAmount =200,
                    CreateTime = DateTime.Now,
                }
            };

            int totalPage = 1;

            // Mock 設定
            refundRepositoryMock.Setup(s
                 => s.GetRefund(getRefundDto)).Returns((refunds, totalPage));

            // Act
            ResponseGetRefundListDto result = service.GetRefund(getRefundDto);

            // Assert
            Assert.IsTrue(result.RefundList.SequenceEqual(refunds));
        }

        [TestMethod]
        public void 取得退款紀錄清單_失敗_回傳空資料()
        {
            // Arrange
            RequestGetRefundDto getRefundDto = new RequestGetRefundDto()
            {
                Status = 1,
                RefundType = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = null, // 只允許 status | createTime | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };
            List<Refund> refunds = null;
            int totalPage = 1;

            // Mock 設定
            refundRepositoryMock.Setup(s
                 => s.GetRefund(getRefundDto)).Returns((refunds, totalPage));

            // Act
            ResponseGetRefundListDto result = service.GetRefund(getRefundDto);

            // Assert
            Assert.IsNull(result.RefundList);
        }
    }
}
