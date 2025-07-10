using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Refund.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.TransactionTest
{
    [TestClass]
    public class RefundControllerTest
    {
        private RefundController controller;
        private Mock<IRefundService> refundServiceMock;

        [TestInitialize]
        public void Setup()
        {
            refundServiceMock = new Mock<IRefundService>();
            controller = new RefundController(refundServiceMock.Object);
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

            ResponseGetRefundListDto response = new ResponseGetRefundListDto()
            {
                RefundList = null,
                TotalPage = 1,
            };

            // Mock 設定
            refundServiceMock.Setup(s
                => s.GetRefund(getRefundDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetRefund(getRefundDto);

            // Assert
            ResponseIsEqual<ResponseGetRefundListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetRefundListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得退款紀錄清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetRefundDto getRefundDto = new RequestGetRefundDto()
            {
                Status = 1,
                RefundType = null,
                Page = 1, // 必須>0
                SortOrder = "abc", // 只允許 descending 或 ascending
                SortOption = null, // 只允許 status | createTime | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            // Act
            IHttpActionResult result = controller.GetRefund(getRefundDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
