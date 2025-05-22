using System;
using System.Collections.Generic;
using System.Linq;
using ApiLayer.Controllers.api;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Coach.Response;
using ApiLayer.Models.Other;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.CoachTest
{
    [TestClass]
    public class CoachServiceTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<ICoachRepository> coachRepositoryMock;
        private Mock<IRedisService> redisServiceMock;
        private Mock<IFileService> fileServiceMock;
        private  Mock<IHttpService> httpServiceMock;
        private CoachService coachService;

        [TestInitialize]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            coachRepositoryMock = new Mock<ICoachRepository>();
            fileServiceMock = new Mock<IFileService>();
            redisServiceMock = new Mock<IRedisService>();
            httpServiceMock = new Mock<IHttpService>();
            coachService = new CoachService(mapperMock.Object, coachRepositoryMock.Object, 
                fileServiceMock.Object, redisServiceMock.Object, httpServiceMock.Object);
        }

        [TestMethod]
        public void 新增不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestAddCoachDto addCoachDto = new RequestAddCoachDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                Type = 1,
                ContractEndTime = null,
                ContractStartTime = null,
            };
            Coach coach = new Coach()
            {
                Account = "eqweqw123",
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                Type = 1,
                ContractEndTime = DateTime.MinValue,
                ContractStartTime = DateTime.MinValue,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            coachRepositoryMock.Setup(s => s.AddCoach(coach)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<Coach>(addCoachDto)).Returns(coach);

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = coachService.AddCoach(addCoachDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.Success);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void 新增_失敗_帳號重複()
        {
            // Arrange
            RequestAddCoachDto addCoachDto = new RequestAddCoachDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                Type = 1,
                ContractEndTime = null,
                ContractStartTime = null,
            };
            Coach coach = new Coach()
            {
                Account = "eqweqw123",
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                Type = 1,
                ContractEndTime = DateTime.MinValue,
                ContractStartTime = DateTime.MinValue,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.DuplicateAccount;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            coachRepositoryMock.Setup(s => s.AddCoach(coach)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<Coach>(addCoachDto)).Returns(coach);

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = coachService.AddCoach(addCoachDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.DuplicateAccount);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void 取得教練清單_成功_回傳教練清單()
        {
            // Arrange
            RequestGetCoachDto getCoachDto = new RequestGetCoachDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "contractEndTime", // 只允許 contractEndTime | name | status | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 null 或 < 15 位
                SearchPhone = null, // 只允許 null 或是 3 位數字
            };

            List<Coach> coaches = new List<Coach>()
            {
                new Coach()
                {
                     CoachId = 1,
                     ContractEndTime = DateTime.MinValue,
                     ContractStartTime = DateTime.MinValue,
                     Name = "草莓族",
                     Phone = 978678521,
                     Status = true,
                     Type = 1
                }
            };
            int totalPage = 1;

            List<ResponseGetCoachDto> responseGetCoachDto = new List<ResponseGetCoachDto>()
             {
                 new ResponseGetCoachDto()
                 {
                     CoachId = 1,
                     ContractEndTime = DateTime.MinValue,
                     ContractStartTime = DateTime.MinValue,
                     Name = "草莓族",
                     Phone = 978678521,
                     Status = true,
                     Type = 1
                 }
             };

            // Mock 設定
            coachRepositoryMock.Setup(s => s.GetCoach(getCoachDto)).Returns((coaches, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetCoachDto>>(coaches)).Returns(responseGetCoachDto);

            // Act
            ResponseGetCoachListDto response = coachService.GetCoach(getCoachDto);

            // Assert
            Assert.IsTrue(response.CoachList.SequenceEqual(responseGetCoachDto));
        }

        [TestMethod]
        public void 取得會員清單_失敗_回傳空資料()
        {
            // Arrange
            RequestGetCoachDto getCoachDto = new RequestGetCoachDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "contractEndTime", // 只允許 contractEndTime | name | status | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 null 或 < 15 位
                SearchPhone = null, // 只允許 null 或是 3 位數字
            };

            List<Coach> coaches = new List<Coach>();
            int totalPage = 1;
            List<ResponseGetCoachDto> responseGetCoachDto = new List<ResponseGetCoachDto>();

            // Mock 設定
            coachRepositoryMock.Setup(s => s.GetCoach(getCoachDto)).Returns((coaches, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetCoachDto>>(coaches)).Returns(responseGetCoachDto);

            // Act
            ResponseGetCoachListDto response = coachService.GetCoach(getCoachDto);

            // Assert
            Assert.IsTrue(response.CoachList.SequenceEqual(responseGetCoachDto));
        }

        [TestMethod]
        public void 取得教練修改頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestCoachIdDto coachIdDto = new RequestCoachIdDto()
            {
                CoachId = 1
            };
            Coach coach = new Coach()
            {
                Email = "",
                Certification = "",
                ContractStartTime = DateTime.Now,
                ContractEndTime = DateTime.Now.AddDays(DateTime.DaysInMonth(1, 6)),
                Specialty = "",
                Introduction = "",
                Name = "Jack",
                Status = true,
                Phone = 987896543,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };
            ResponseGetCoachEditDataByIdDto responseDto = new ResponseGetCoachEditDataByIdDto()
            {
                Email = "",
                Certification = "",
                ContractStartTime = DateTime.Now,
                ContractEndTime = DateTime.Now.AddDays(DateTime.DaysInMonth(1, 6)),
                Specialty = "",
                Introduction = "",
                Name = "Jack",
                Status = true,
                Phone = 987896543,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            coachRepositoryMock.Setup(s => s.GetCoachEditDataById(coachIdDto.CoachId)).Returns(coach);
            mapperMock.Setup(s => s.Map<ResponseGetCoachEditDataByIdDto>(coach)).Returns(responseDto);

            // Act
            ResponseGetCoachEditDataByIdDto response = coachService.GetCoachEditDataById(coachIdDto);
            responseDto.PhotoUrl = "/" + responseDto.PhotoUrl;

            // Assert
            Assert.IsTrue(response == responseDto);
        }

        [TestMethod]
        public void 取得教練修改頁面需要的資料_失敗_回傳空資料()
        {
            // Arrange
            RequestCoachIdDto coachIdDto = new RequestCoachIdDto()
            {
                CoachId = 1
            };
            Coach coach = null;

            // Mock 設定
            coachRepositoryMock.Setup(s => s.GetCoachEditDataById(coachIdDto.CoachId)).Returns(coach);

            // Act
            ResponseGetCoachEditDataByIdDto response = coachService.GetCoachEditDataById(coachIdDto);

            // Assert
            Assert.IsTrue(response == null);
        }

        [TestMethod]
        public void  修改教練不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestEditCoachDto editCoachDto = new RequestEditCoachDto()
            {
                CoachId = 1,
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                ContractEndTime = null,
                ContractStartTime = null,
                Status = true,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            coachRepositoryMock.Setup(s => s.EditCoach(editCoachDto)).Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s=>s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = coachService.EditCoach(editCoachDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.Success);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void 修改_失敗_手機重複()
        {
            // Arrange
            RequestEditCoachDto editCoachDto = new RequestEditCoachDto()
            {
                CoachId = 1,
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                ContractEndTime = null,
                ContractStartTime = null,
                Status = true,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.DuplicatePhone;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            coachRepositoryMock.Setup(s => s.EditCoach(editCoachDto)).Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s => s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = coachService.EditCoach(editCoachDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.DuplicatePhone);
            Assert.IsTrue(exception == null);
        }
    }
}
