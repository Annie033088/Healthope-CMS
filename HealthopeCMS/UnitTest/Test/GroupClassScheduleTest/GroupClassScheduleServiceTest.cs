using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Http;
using System;
using UnitTest.utils;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using Moq;
using ApiLayer.Service;
using PersistentLayer.Interface;
using AutoMapper;
using ApiLayer.Models.GroupClassShowcase.Response;
using DomainLayer.Models;
using PersistentLayer.Models;
using System.Linq;

namespace UnitTest.Test.GroupClassScheduleTest
{
    [TestClass]
    public class GroupClassScheduleServiceTest
    {
        private GroupClassScheduleService service;
        private readonly IMapper mapper;
        private Mock<IGroupClassScheduleRepository> groupClassScheduleRepositoryMock;
        private Mock<IMapper> mapperMock;

        [TestInitialize]
        public void Setup()
        {
            groupClassScheduleRepositoryMock = new Mock<IGroupClassScheduleRepository>();
            mapperMock = new Mock<IMapper>();
            service = new GroupClassScheduleService(mapperMock.Object, groupClassScheduleRepositoryMock.Object);
        }

        [TestMethod]
        public void 取得新增團體課程表前需要的資料_成功_回傳清單()
        {
            // Arrange
            RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto = new RequestGetShowcaseAndCoachDto()
            {
                Category = null
            };
            List<GroupClassShowcase> showcases = new List<GroupClassShowcase>()
            {
                new GroupClassShowcase()
                {
                        Category=1,
                        Icon=2,
                        Name="www"
                }
            };

            List<Coach> coaches = new List<Coach>()
             {
                 new Coach()
                 {
                        CoachId=1,
                        Name="wwwq",
                        UpdateTime = DateTime.Now,
                 }
             };

            ResponseGetShowcaseAndCoachDto responseGet = new ResponseGetShowcaseAndCoachDto()
            {
                ShowcaseList = new List<ScheduleGetShowcaseDto>()
                {
                    new ScheduleGetShowcaseDto()
                    {
                        Category=1,
                        Icon=2,
                        Name="www"
                    }
                },
                CoachList = new List<ScheduleGetCoachDto>()
                {
                    new ScheduleGetCoachDto()
                    {
                        CoachId=1,
                        Name="wwwq",
                        UpdateTime = DateTime.Now,
                    }
                },
            };

            // Mock 設定
            groupClassScheduleRepositoryMock.Setup(s => s.GetShowcaseAndCoach(getShowcaseAndCoachDto.Category))
                .Returns((showcases, coaches));
            mapperMock.Setup(s => s.Map<List<ScheduleGetCoachDto>>(coaches)).Returns(responseGet.CoachList);
            mapperMock.Setup(s => s.Map<List<ScheduleGetShowcaseDto>>(showcases)).Returns(responseGet.ShowcaseList);

            // Act
            ResponseGetShowcaseAndCoachDto response = service.GetShowcaseAndCoach(getShowcaseAndCoachDto);

            // Assert
            CollectionAssert.AreEqual(response.ShowcaseList, responseGet.ShowcaseList);
            CollectionAssert.AreEqual(response.CoachList, responseGet.CoachList);
        }

        [TestMethod]
        public void 取得新增團體課程表前需要的資料_失敗_取得空資料()
        {
            // Arrange
            RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto = new RequestGetShowcaseAndCoachDto()
            {
                Category = null
            };
            List<GroupClassShowcase> showcases = new List<GroupClassShowcase>();

            List<Coach> coaches = new List<Coach>();

            ResponseGetShowcaseAndCoachDto responseGet = new ResponseGetShowcaseAndCoachDto();

            // Mock 設定
            groupClassScheduleRepositoryMock.Setup(s => s.GetShowcaseAndCoach(getShowcaseAndCoachDto.Category))
                .Returns((showcases, coaches));
            mapperMock.Setup(s => s.Map<List<ScheduleGetCoachDto>>(coaches)).Returns(responseGet.CoachList);
            mapperMock.Setup(s => s.Map<List<ScheduleGetShowcaseDto>>(showcases)).Returns(responseGet.ShowcaseList);

            // Act
            ResponseGetShowcaseAndCoachDto response = service.GetShowcaseAndCoach(getShowcaseAndCoachDto);

            // Assert
            CollectionAssert.AreEqual(response.ShowcaseList, responseGet.ShowcaseList);
            CollectionAssert.AreEqual(response.CoachList, responseGet.CoachList);
        }

        [TestMethod]
        public void 新增團體課程表_成功_回傳成功()
        {
            // Arrange
            RequestAddScheduleDto addScheduleDto = new RequestAddScheduleDto()
            {
                Category = 1,
                ClassName = "wq",
                Coach = new ScheduleGetCoachDto()
                {
                    CoachId = 1,
                    UpdateTime = DateTime.Now,
                },
                Icon = 1,
                MaximumParticipant = 35,
                Place = "weq",
                Time = new DateTime(2025, 05, 25, 18, 40, 0),
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            groupClassScheduleRepositoryMock.Setup(s
                => s.AddSchedule(It.IsAny<GroupClassSchedule>(), It.IsAny<Coach>())).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.AddSchedule(addScheduleDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 新增團體課程表_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestAddScheduleDto addScheduleDto = new RequestAddScheduleDto()
            {
                Category = 1,
                ClassName = "wq",
                Coach = new ScheduleGetCoachDto()
                {
                    CoachId = 1,
                    UpdateTime = DateTime.Now,
                },
                Icon = 1,
                MaximumParticipant = 35,
                Place = "weq",
                Time = DateTime.Now.AddDays(-1),
            };

            int errorCodeNumber = (int)ErrorCodeDefine.InvalidFormatOrEntry;

            // Mock 設定
            groupClassScheduleRepositoryMock.Setup(s
                => s.AddSchedule(It.IsAny<GroupClassSchedule>(), It.IsAny<Coach>())).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.AddSchedule(addScheduleDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.InvalidFormatOrEntry);
        }

        [TestMethod]
        public void 新增團體課程表_失敗_課程時間重複()
        {
            // Arrange
            RequestAddScheduleDto addScheduleDto = new RequestAddScheduleDto()
            {
                Category = 1,
                ClassName = "wq",
                Coach = new ScheduleGetCoachDto()
                {
                    CoachId = 1,
                    UpdateTime = DateTime.Now,
                },
                Icon = 1,
                MaximumParticipant = 35,
                Place = "weq",
                Time = new DateTime(2025, 05, 25, 18, 40, 0),
            };

            int errorCodeNumber = (int)ErrorCodeDefine.DuplicatePlaceAndTime;

            // Mock 設定
            groupClassScheduleRepositoryMock.Setup(s
                => s.AddSchedule(It.IsAny<GroupClassSchedule>(), It.IsAny<Coach>())).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.AddSchedule(addScheduleDto);

            // Assert
            Assert.IsTrue(result == ErrorCodeDefine.DuplicatePlaceAndTime);
        }
    }
}
