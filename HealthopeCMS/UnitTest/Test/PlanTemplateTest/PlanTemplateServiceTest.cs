using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.Other;
using System.Web.Http;
using UnitTest.utils;

namespace UnitTest.Test.PlanTemplateTest
{
    [TestClass]
    public class PlanTemplateServiceTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IFileService> fileServiceMock;
        private Mock<IHttpService> httpServiceMock;
        private Mock<IPlanTemplateRepository> planTemplateRepositoryMock;
        private PlanTemplateService service;

        public PlanTemplateServiceTest()
        {
            mapperMock = new Mock<IMapper>();
            fileServiceMock = new Mock<IFileService>();
            planTemplateRepositoryMock = new Mock<IPlanTemplateRepository>();
            httpServiceMock = new Mock<IHttpService>();
            service = new PlanTemplateService(planTemplateRepositoryMock.Object, mapperMock.Object,
                fileServiceMock.Object, httpServiceMock.Object);
        }

        [TestMethod]
        public void 新增票劵方案_成功_回傳成功()
        {
            // Arrange
            RequestAddTicketPlanDto addTicketPlanDto = new RequestAddTicketPlanDto()
            {
                Price = 100,
                Status = true
            };

            TicketPlan ticketPlan = new TicketPlan()
            {
                Price = 100,
                Status = true
            };

            bool success = true;

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddTicketPlan(ticketPlan)).Returns(success);
            mapperMock.Setup(s => s.Map<TicketPlan>(addTicketPlanDto)).Returns(ticketPlan);

            // Act
            bool result = service.AddTicketPlan(addTicketPlanDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 新增票劵方案_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestAddTicketPlanDto addTicketPlanDto = new RequestAddTicketPlanDto()
            {
                Price = 100,
                Status = true
            };

            TicketPlan ticketPlan = new TicketPlan()
            {
                Price = 100,
                Status = true
            };

            bool success = false;

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddTicketPlan(ticketPlan)).Returns(success);
            mapperMock.Setup(s => s.Map<TicketPlan>(addTicketPlanDto)).Returns(ticketPlan);

            // Act
            bool result = service.AddTicketPlan(addTicketPlanDto);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void 新增會籍方案不包括圖擋_成功_回傳成功()
        {
            // Arrange
            RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            MembershipPlan membershipPlan = new MembershipPlan()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddMembershipPlan(membershipPlan)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<MembershipPlan>(addMembershipPlanDto)).Returns(membershipPlan);

            // Act
            (bool successFlag, Exception exception) = service.AddMembershipPlan(addMembershipPlanDto, null);

            // Assert
            Assert.IsTrue(successFlag);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void 新增會籍方案_失敗_回傳失敗()
        {
            // Arrange
            RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            MembershipPlan membershipPlan = new MembershipPlan()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            int errorCodeNumber = (int)ErrorCodeDefine.CreateFailed;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddMembershipPlan(membershipPlan)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<MembershipPlan>(addMembershipPlanDto)).Returns(membershipPlan);

            // Act
            (bool successFlag, Exception exception) = service.AddMembershipPlan(addMembershipPlanDto, null);

            // Assert
            Assert.IsFalse(successFlag);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void 新增教練課方案不包括圖擋_成功_回傳成功()
        {
            // Arrange
            RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto = new RequestAddPersonalTrainingPackageDto()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };

            PersonalTrainingPackage personalTrainingPackage = new PersonalTrainingPackage()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddPersonalTrainingPackage(personalTrainingPackage)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<PersonalTrainingPackage>(addPersonalTrainingPackageDto)).Returns(personalTrainingPackage);

            // Act
            (bool successFlag, Exception exception) = service.AddPersonalTrainingPackage(addPersonalTrainingPackageDto, null);

            // Assert
            Assert.IsTrue(successFlag);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void 新增教練課方案_失敗_回傳失敗()
        {
            // Arrange
            RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto = new RequestAddPersonalTrainingPackageDto()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };

            PersonalTrainingPackage personalTrainingPackage = new PersonalTrainingPackage()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };

            int errorCodeNumber = (int)ErrorCodeDefine.CreateFailed;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddPersonalTrainingPackage(personalTrainingPackage)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<PersonalTrainingPackage>(addPersonalTrainingPackageDto)).Returns(personalTrainingPackage);

            // Act
            (bool successFlag, Exception exception) = service.AddPersonalTrainingPackage(addPersonalTrainingPackageDto, null);

            // Assert
            Assert.IsFalse(successFlag);
            Assert.IsTrue(exception == null);
        }
    }
}
