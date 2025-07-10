using System;
using ApiLayer.Models;
using ApiLayer.Models.MemberPlan.Request;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;

namespace UnitTest.Test.MemberPlanTest
{
    [TestClass]
    public class MemberPlanServiceTest
    {
        private MemberPlanService service;
        private Mock<IMemberPlanRepository> memberPlanRepositoryMock;
        private Mock<IMapper> mapperMock;

        [TestInitialize]
        public void Setup()
        {
            memberPlanRepositoryMock = new Mock<IMemberPlanRepository>();
            mapperMock = new Mock<IMapper>();
            service = new MemberPlanService(memberPlanRepositoryMock.Object, mapperMock.Object);
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
            MemberMembershipPlan memberMembershipPlan = new MemberMembershipPlan
            {
                MemberMembershipPlanId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };
            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            memberPlanRepositoryMock.Setup(s => s.EditMemberMembershipPlanStatus(memberMembershipPlan)).Returns(errorCodeNumber);
            mapperMock.Setup(s => s.Map<MemberMembershipPlan>(editMemberMembershipPlanStatusDto)).Returns(memberMembershipPlan);

            // Act
            ErrorCodeDefine result = service.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto);

            // Assert
            Assert.AreEqual(result, (ErrorCodeDefine)errorCodeNumber);
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
            MemberMembershipPlan memberMembershipPlan = new MemberMembershipPlan
            {
                MemberMembershipPlanId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            memberPlanRepositoryMock.Setup(s => s.EditMemberMembershipPlanStatus(memberMembershipPlan)).Returns(errorCodeNumber);
            mapperMock.Setup(s => s.Map<MemberMembershipPlan>(editMemberMembershipPlanStatusDto)).Returns(memberMembershipPlan);

            // Act
            ErrorCodeDefine result = service.EditMemberMembershipPlanStatus(editMemberMembershipPlanStatusDto);

            // Assert
            Assert.AreEqual(result, (ErrorCodeDefine)errorCodeNumber);
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
            MemberPersonalTrainingPackage memberPersonalTrainingPackage = new MemberPersonalTrainingPackage
            {
                CoachId = 1,
                MemberPersonalTrainingPackageId = 1,
                UpdateTime = DateTime.Now,
            };
            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            memberPlanRepositoryMock.Setup(s => s.EditMemberPersonalTrainingPackageCoach(memberPersonalTrainingPackage))
                .Returns(errorCodeNumber);
            mapperMock.Setup(s => s.Map<MemberPersonalTrainingPackage>(editMemberPersonalTrainingPackageCoachDto))
                .Returns(memberPersonalTrainingPackage);

            // Act
            ErrorCodeDefine result = service.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto);

            // Assert
            Assert.AreEqual(result, (ErrorCodeDefine)errorCodeNumber);
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
            MemberPersonalTrainingPackage memberPersonalTrainingPackage = new MemberPersonalTrainingPackage
            {
                CoachId = 1,
                MemberPersonalTrainingPackageId = 1,
                UpdateTime = DateTime.Now,
            };
            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            memberPlanRepositoryMock.Setup(s => s.EditMemberPersonalTrainingPackageCoach(memberPersonalTrainingPackage))
                .Returns(errorCodeNumber);
            mapperMock.Setup(s => s.Map<MemberPersonalTrainingPackage>(editMemberPersonalTrainingPackageCoachDto))
                .Returns(memberPersonalTrainingPackage);

            // Act
            ErrorCodeDefine result = service.EditMemberPersonalTrainingPackageCoach(editMemberPersonalTrainingPackageCoachDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.HasBeenModified);
        }
    }
}
