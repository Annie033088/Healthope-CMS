using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models.LeaseAgreement.Response;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.LeaseAgreementTest
{
    [TestClass]
    public class LeaseAgreementServiceTest
    {
        private LeaseAgreementService service;
        private Mock<IMapper> mapperMock;
        private Mock<ILeaseAgreementRepository> leaseAgreementRepositoryMock;
        private Mock<IJobDispatcher> jobDispatcherMock;

        [TestInitialize]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            leaseAgreementRepositoryMock = new Mock<ILeaseAgreementRepository>();
            jobDispatcherMock = new Mock<IJobDispatcher>();
            service = new LeaseAgreementService(mapperMock.Object, leaseAgreementRepositoryMock.Object, jobDispatcherMock.Object);
        }

        [TestMethod]
        public void 新增租約_成功_回傳成功()
        {
            DateTime startTime = DateTime.UtcNow.AddDays(-1);
            DateTime endTime = DateTime.UtcNow.AddDays(1);

            // Arrange
            RequestAddLeaseAgreementDto addLeaseAgreementDto = new RequestAddLeaseAgreementDto()
            {
                StartTime = startTime,
                EndTime = endTime,
                ReminderLeadTime = 60
            };

            bool successFlag = true;

            LeaseAgreement leaseAgreement = new LeaseAgreement()
            {
                StartTime = startTime,
                EndTime = endTime,
                ReminderLeadTime = 60,
            };

            // Mock 設定
            mapperMock.Setup(s => s.Map<LeaseAgreement>(addLeaseAgreementDto)).Returns(leaseAgreement);
            leaseAgreementRepositoryMock.Setup(s
                => s.AddLeaseAgreement(leaseAgreement)).Returns(successFlag);

            // Act
            bool result = service.AddLeaseAgreement(addLeaseAgreementDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 新增租約_失敗_回傳失敗()
        {
            DateTime startTime = DateTime.UtcNow.AddDays(-1);
            DateTime endTime = DateTime.UtcNow.AddDays(1);

            // Arrange
            RequestAddLeaseAgreementDto addLeaseAgreementDto = new RequestAddLeaseAgreementDto()
            {
                StartTime = startTime,
                EndTime = endTime,
                ReminderLeadTime = 60
            };

            bool successFlag = false;

            LeaseAgreement leaseAgreement = new LeaseAgreement()
            {
                StartTime = startTime,
                EndTime = endTime,
                ReminderLeadTime = 60,
            };

            // Mock 設定
            mapperMock.Setup(s => s.Map<LeaseAgreement>(addLeaseAgreementDto)).Returns(leaseAgreement);
            leaseAgreementRepositoryMock.Setup(s
                => s.AddLeaseAgreement(leaseAgreement)).Returns(successFlag);

            // Act
            bool result = service.AddLeaseAgreement(addLeaseAgreementDto);

            // Assert
            Assert.IsFalse(result);
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

            DateTime startTime = DateTime.UtcNow.AddDays(-1);
            DateTime endTime = DateTime.UtcNow.AddDays(1);

            List<ResponseGetLeaseAgreementDto> responseGetLeaseAgreement = new List<ResponseGetLeaseAgreementDto>()
            {
                new ResponseGetLeaseAgreementDto()
                {
                    StartTime = startTime,
                    EndTime= endTime,
                    LeaseAgreementId =1,
                    Remark="",
                    Remind=true,
                    ReminderLeadTime=60,
                    UpdateTime=startTime,
                    Status = 3,
                }
            };

            List<LeaseAgreement> leaseAgreements = new List<LeaseAgreement>()
            {
                new LeaseAgreement()
                {
                    StartTime = startTime,
                    EndTime= endTime,
                    LeaseAgreementId =1,
                    Remark="",
                    Remind=true,
                    ReminderLeadTime=60,
                    UpdateTime=startTime,
                    Status = 3,
                }
            };
            int totalPage = 1;

            // Mock 設定
            leaseAgreementRepositoryMock.Setup(s
                => s.GetLeaseAgreement(getLeaseAgreementDto)).Returns((leaseAgreements, totalPage));
            mapperMock.Setup(s
                => s.Map<List<ResponseGetLeaseAgreementDto>>(leaseAgreements)).Returns(responseGetLeaseAgreement);

            // Act
            ResponseGetLeaseAgreementListDto result = service.GetLeaseAgreement(getLeaseAgreementDto);

            // Assert
            CollectionAssert.AreEqual(responseGetLeaseAgreement, result.LeaseAgreementList);
        }

        [TestMethod]
        public void 取得租約清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetLeaseAgreementDto getLeaseAgreementDto = new RequestGetLeaseAgreementDto()
            {
                Status = 3,
                Page = 1,
                RecordPerPage = 8
            };
            List<ResponseGetLeaseAgreementDto> responseGetLeaseAgreement = null;
            List<LeaseAgreement> leaseAgreements = null;
            int totalPage = 1;

            // Mock 設定
            leaseAgreementRepositoryMock.Setup(s
                => s.GetLeaseAgreement(getLeaseAgreementDto)).Returns((leaseAgreements, totalPage));
            mapperMock.Setup(s
                => s.Map<List<ResponseGetLeaseAgreementDto>>(leaseAgreements)).Returns(responseGetLeaseAgreement);

            // Act
            ResponseGetLeaseAgreementListDto result = service.GetLeaseAgreement(getLeaseAgreementDto);

            // Assert
            CollectionAssert.AreEqual(responseGetLeaseAgreement, result.LeaseAgreementList);
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
            LeaseAgreement leaseAgreement = new LeaseAgreement()
            {
                LeaseAgreementId = 1,
                Remark = null,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            bool sendEmailFlag = false;
            DateTime leaseEndTime = DateTime.MinValue;

            // Mock 設定
            mapperMock.Setup(s
                => s.Map<LeaseAgreement>(editLeaseAgreementStatusDto)).Returns(leaseAgreement);
            leaseAgreementRepositoryMock.Setup(s => s.EditLeaseAgreementStatus(leaseAgreement))
                .Returns((errorCodeNumber, sendEmailFlag, leaseEndTime));

            // Act
            ErrorCodeDefine result = service.EditLeaseAgreementStatus(editLeaseAgreementStatusDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 修改租約狀態_失敗_回傳資料已被修改()
        {
            // Arrange
            RequestEditLeaseAgreementStatusDto editLeaseAgreementStatusDto = new RequestEditLeaseAgreementStatusDto()
            {
                LeaseAgreementId = 1,
                Remark = null,
                Status = 2,
                UpdateTime = DateTime.Now,
            };
            LeaseAgreement leaseAgreement = new LeaseAgreement()
            {
                LeaseAgreementId = 1,
                Remark = null,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;
            bool sendEmailFlag = false;
            DateTime leaseEndTime = DateTime.MinValue;

            // Mock 設定
            mapperMock.Setup(s
                => s.Map<LeaseAgreement>(editLeaseAgreementStatusDto)).Returns(leaseAgreement);
            leaseAgreementRepositoryMock.Setup(s => s.EditLeaseAgreementStatus(leaseAgreement))
                .Returns((errorCodeNumber, sendEmailFlag, leaseEndTime));

            // Act
            ErrorCodeDefine result = service.EditLeaseAgreementStatus(editLeaseAgreementStatusDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.HasBeenModified);
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

            LeaseAgreement leaseAgreement = new LeaseAgreement()
            {
                LeaseAgreementId = 1,
                Remind = false,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            mapperMock.Setup(s
                => s.Map<LeaseAgreement>(editLeaseAgreementRemindDto)).Returns(leaseAgreement);
            leaseAgreementRepositoryMock.Setup(s => s.EditLeaseAgreementRemind(leaseAgreement))
                .Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditLeaseAgreementRemind(editLeaseAgreementRemindDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 修改提醒狀態_失敗_回傳資料已被修改()
        {
            // Arrange
            RequestEditLeaseAgreementRemindDto editLeaseAgreementRemindDto = new RequestEditLeaseAgreementRemindDto()
            {
                LeaseAgreementId = 1,
                Remind = false,
                UpdateTime = DateTime.Now,
            };

            LeaseAgreement leaseAgreement = new LeaseAgreement()
            {
                LeaseAgreementId = 1,
                Remind = false,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            mapperMock.Setup(s
                => s.Map<LeaseAgreement>(editLeaseAgreementRemindDto)).Returns(leaseAgreement);
            leaseAgreementRepositoryMock.Setup(s => s.EditLeaseAgreementRemind(leaseAgreement))
                .Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditLeaseAgreementRemind(editLeaseAgreementRemindDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.HasBeenModified);
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
            leaseAgreementRepositoryMock.Setup(s => s.DeleteLeaseAgreement(leaseAgreementIdDto.LeaseAgreementId))
                .Returns(successFlag);

            // Act
            bool result = service.DeleteLeaseAgreement(leaseAgreementIdDto);

            // Assert
            Assert.IsTrue(result);
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
            leaseAgreementRepositoryMock.Setup(s => s.DeleteLeaseAgreement(leaseAgreementIdDto.LeaseAgreementId))
                .Returns(successFlag);

            // Act
            bool result = service.DeleteLeaseAgreement(leaseAgreementIdDto);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
