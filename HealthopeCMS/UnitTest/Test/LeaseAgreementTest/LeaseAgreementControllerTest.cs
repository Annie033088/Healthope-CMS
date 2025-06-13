using System;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models.LeaseAgreement.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

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

        [TestMethod]
        public void 修改租約狀態_成功_回傳成功()
        {
            // Arrange
            RequestEditLeaseAgreementStatusDto editLeaseAgreementStatusDto = new RequestEditLeaseAgreementStatusDto()
            {
                LeaseAgreementId = 1,
                Remark = null,
                Status = 2,
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            leaseAgreementServiceMock.Setup(s => s.EditLeaseAgreementStatus(editLeaseAgreementStatusDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.EditLeaseAgreementStatus(editLeaseAgreementStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改租約狀態_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditLeaseAgreementStatusDto editLeaseAgreementStatusDto = new RequestEditLeaseAgreementStatusDto()
            {
                LeaseAgreementId = 1,
                Remark = null,
                Status = 22,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditLeaseAgreementStatus(editLeaseAgreementStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改提醒狀態_成功_回傳成功()
        {
            // Arrange
            RequestEditLeaseAgreementRemindDto editLeaseAgreementRemindDto = new RequestEditLeaseAgreementRemindDto()
            {
                LeaseAgreementId = 1,
                Remind = false,
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            leaseAgreementServiceMock.Setup(s => s.EditLeaseAgreementRemind(editLeaseAgreementRemindDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.EditLeaseAgreementRemind(editLeaseAgreementRemindDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改提醒狀態_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditLeaseAgreementRemindDto editLeaseAgreementRemindDto = new RequestEditLeaseAgreementRemindDto()
            {
                LeaseAgreementId = 1,
                Remind = true,
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            leaseAgreementServiceMock.Setup(s => s.EditLeaseAgreementRemind(editLeaseAgreementRemindDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.EditLeaseAgreementRemind(editLeaseAgreementRemindDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 刪圖條款_成功_回傳成功()
        {
            // Arrange
            RequestLeaseAgreementIdDto leaseAgreementIdDto = new RequestLeaseAgreementIdDto()
            {
                LeaseAgreementId = 10,
            };

            bool successFlag = true;

            // Mock 設定
            leaseAgreementServiceMock.Setup(s => s.DeleteLeaseAgreement(leaseAgreementIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.DeleteLeaseAgreement(leaseAgreementIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 刪除條款_失敗_回傳失敗()
        {
            // Arrange
            RequestLeaseAgreementIdDto leaseAgreementIdDto = new RequestLeaseAgreementIdDto()
            {
                LeaseAgreementId = 10,
            };

            bool successFlag = false;

            // Mock 設定
            leaseAgreementServiceMock.Setup(s => s.DeleteLeaseAgreement(leaseAgreementIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.DeleteLeaseAgreement(leaseAgreementIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DeleteFailed));
        }
    }
}
