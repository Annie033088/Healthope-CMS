using System;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.MemberPlan.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTest.utils;

namespace UnitTest.Test.MemberPlanTest
{
    [TestClass]
    public class MemberPlanControllerTest
    {
        private MemberPlanController controller;
        private Mock<IMemberPlanService> memberPlanServiceMock;

        [TestInitialize]
        public void Setup()
        {
            memberPlanServiceMock = new Mock<IMemberPlanService>();
            controller = new MemberPlanController(memberPlanServiceMock.Object);
        }

        [TestMethod]
        public void 修改會員會籍狀態_成功_回傳成功()
        {
            // Arrange
            RequestMemberMembershipPlanStatusDto editMemberMembershipPlanStatusDto = new RequestMemberMembershipPlanStatusDto()
            {
                MemberMembershipPlanId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCdoe = ErrorCodeDefine.Success;

            // Mock 設定
            memberPlanServiceMock.Setup(s => s.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto)).Returns(errorCdoe);

            // Act
            IHttpActionResult result = controller.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改會員會籍狀態_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestMemberMembershipPlanStatusDto editMemberMembershipPlanStatusDto = new RequestMemberMembershipPlanStatusDto()
            {
                MemberMembershipPlanId = 0,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改會員會籍狀態_失敗_回傳資料已被他人修改()
        {
            // Arrange
            RequestMemberMembershipPlanStatusDto editMemberMembershipPlanStatusDto = new RequestMemberMembershipPlanStatusDto()
            {
                MemberMembershipPlanId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCdoe = ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            memberPlanServiceMock.Setup(s => s.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto)).Returns(errorCdoe);

            // Act
            IHttpActionResult result = controller.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.HasBeenModified));
        }

        [TestMethod]
        public void 修改會員教練課方案教練_成功_回傳成功()
        {
            // Arrange
            RequestEditMemberPersonalTrainingPackageCoachDto editMemberPersonalTrainingPackageCoachDto = new RequestEditMemberPersonalTrainingPackageCoachDto()
            {
                CoachId = 1,
                MemberPersonalTrainingPackageId = 1,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCdoe = ErrorCodeDefine.Success;

            // Mock 設定
            memberPlanServiceMock.Setup(s => s.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto)).Returns(errorCdoe);

            // Act
            IHttpActionResult result = controller.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改會員教練課方案教練_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditMemberPersonalTrainingPackageCoachDto editMemberPersonalTrainingPackageCoachDto = new RequestEditMemberPersonalTrainingPackageCoachDto()
            {
                CoachId = -1,
                MemberPersonalTrainingPackageId = 1,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改會員教練課方案教練_失敗_回傳資料已被他人修改()
        {
            // Arrange
            RequestEditMemberPersonalTrainingPackageCoachDto editMemberPersonalTrainingPackageCoachDto = new RequestEditMemberPersonalTrainingPackageCoachDto()
            {
                CoachId = 1,
                MemberPersonalTrainingPackageId = 1,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCdoe = ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            memberPlanServiceMock.Setup(s => s.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto)).Returns(errorCdoe);

            // Act
            IHttpActionResult result = controller.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.HasBeenModified));
        }
    }
}
