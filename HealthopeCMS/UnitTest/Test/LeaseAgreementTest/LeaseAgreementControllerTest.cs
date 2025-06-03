using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using ApiLayer.Models;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.utils;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Controllers.api;
using Moq;
using ApiLayer.Interface;
using ApiLayer.Models.Term.Response;
using PersistentLayer.Models;
using ApiLayer.Models.LeaseAgreement.Response;

namespace UnitTest.Test.LeaseAgreementTest
{
    [TestClass]
    public class LeaseAgreementControllerTest
    {
        private LeaseAgreementController controller;
        private Mock<ILeaseAgreementService> leaseAgreementServiceMock;

        [TestInitialize]
        public void Setup()
        {
            leaseAgreementServiceMock = new Mock<ILeaseAgreementService>();
            controller = new LeaseAgreementController(leaseAgreementServiceMock.Object);
        }

        [TestMethod]
        public void 新增租約_成功_回傳成功()
        {
            // Arrange
            RequestAddLeaseAgreementDto addLeaseAgreementDto = new RequestAddLeaseAgreementDto()
            {
                StartTime = DateTime.UtcNow.AddDays(-1),
                EndTime = DateTime.UtcNow.AddDays(1),
                ReminderLeadTime = 60
            };

            bool successFlag = true;

            // Mock 設定
            leaseAgreementServiceMock.Setup(s
                => s.AddLeaseAgreement(addLeaseAgreementDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.AddLeaseAgreement(addLeaseAgreementDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 新增租約_失敗_請求參數格式錯誤()
        {
            RequestAddLeaseAgreementDto addLeaseAgreementDto = new RequestAddLeaseAgreementDto()
            {
                StartTime = DateTime.UtcNow.AddDays(1),
                EndTime = DateTime.UtcNow.AddDays(-1),
                ReminderLeadTime = 60
            };

            // Act
            IHttpActionResult result = controller.AddLeaseAgreement(addLeaseAgreementDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得租約清單_成功_回傳清單()
        {
            // Arrange
            RequestGetLeaseAgreementDto getLeaseAgreementDto = new RequestGetLeaseAgreementDto()
            {
                Status = 3,
                Page = 1,
                RecordPerPage = 8
            };

            ResponseGetLeaseAgreementListDto response = new ResponseGetLeaseAgreementListDto();

            // Mock 設定
            leaseAgreementServiceMock.Setup(s
                => s.GetLeaseAgreement(getLeaseAgreementDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetLeaseAgreement(getLeaseAgreementDto);

            // Assert
            ResponseIsEqual<ResponseGetLeaseAgreementListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetLeaseAgreementListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得租約清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetLeaseAgreementDto getLeaseAgreementDto = new RequestGetLeaseAgreementDto()
            {
                Status = 230,
                Page = 1,
                RecordPerPage = 8
            };

            // Act
            IHttpActionResult result = controller.GetLeaseAgreement(getLeaseAgreementDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
