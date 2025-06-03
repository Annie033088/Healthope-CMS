using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;
using UnitTest.utils;
using ApiLayer.Service;
using PersistentLayer.Interface;
using AutoMapper;
using DomainLayer.Models;
using ApiLayer.Models.LeaseAgreement.Response;
using PersistentLayer.Models;
using ApiLayer.Models.Term.Response;

namespace UnitTest.Test.LeaseAgreementTest
{
    [TestClass]
    public class LeaseAgreementServiceTest
    {
        private LeaseAgreementService service;
        private Mock<IMapper> mapperMock;
        private Mock<ILeaseAgreementRepository> leaseAgreementRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            leaseAgreementRepositoryMock = new Mock<ILeaseAgreementRepository>();
            service = new LeaseAgreementService(mapperMock.Object, leaseAgreementRepositoryMock.Object);
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
    }
}
