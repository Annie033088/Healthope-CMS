using System;
using System.Collections.Generic;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

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

        [TestMethod]
        public void 取得團體課程表_成功_回傳清單()
        {
            // Arrange
            RequestGetGroupClassScheduleDto getGroupClassScheduleDto = new RequestGetGroupClassScheduleDto()
            {
                DateRangeFilter = "all",
                SortOption = null,
                Page = 1,
                RecordPerPage = 8,
                SortOrder = "ascending",
                SpecificDate = null,
                Status = 1
            };
            List<ResponseGetScheduleDto> responseGetScheduleListDto = new List<ResponseGetScheduleDto>()
                {
                    new ResponseGetScheduleDto()
                    {
                        GroupClassScheduleId=1,
                        Category=1,
                        CheckInParticipant=1,
                        ClassName="ww",
                        CoachName="alecks",
                        Status=1,
                        MaximumParticipant=60,
                        Place="A",
                        ReserveParticipant=1,
                        Tag=1,
                        Time=DateTime.Now,
                    }
                };

            List<GroupClassSchedule> schedules = new List<GroupClassSchedule>()
            {
                new GroupClassSchedule()
                {
                        GroupClassScheduleId=1,
                        Category=1,
                        CheckInParticipant=1,
                        ClassName="ww",
                        CoachName="alecks",
                        Status=1,
                        MaximumParticipant=60,
                        Place="A",
                        ReserveParticipant=1,
                        Tag=1,
                        Time=DateTime.Now,
                }
            };

            int totalPage = 1;

            // Mock 設定
            groupClassScheduleRepositoryMock.Setup(s
                => s.GetSchedule(getGroupClassScheduleDto)).Returns((schedules, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetScheduleDto>>(schedules)).Returns(responseGetScheduleListDto);

            // Act
            ResponseGetScheduleListDto result = service.GetSchedule(getGroupClassScheduleDto);

            // Assert
            CollectionAssert.AreEqual(result.ScheduleList, responseGetScheduleListDto);
        }

        [TestMethod]
        public void 取得團體課程表_失敗_回傳空資料()
        {
            // Arrange
            RequestGetGroupClassScheduleDto getGroupClassScheduleDto = new RequestGetGroupClassScheduleDto()
            {
                DateRangeFilter = "all",
                SortOption = null,
                Page = 1,
                RecordPerPage = 8,
                SortOrder = "ascending",
                SpecificDate = null,
                Status = 1
            };
            List<ResponseGetScheduleDto> responseGetScheduleListDto = new List<ResponseGetScheduleDto>();
            List<GroupClassSchedule> schedules = new List<GroupClassSchedule>();

            int totalPage = 1;

            // Mock 設定
            groupClassScheduleRepositoryMock.Setup(s
                => s.GetSchedule(getGroupClassScheduleDto)).Returns((schedules, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetScheduleDto>>(schedules)).Returns(responseGetScheduleListDto);

            // Act
            ResponseGetScheduleListDto result = service.GetSchedule(getGroupClassScheduleDto);

            // Assert
            CollectionAssert.AreEqual(result.ScheduleList, responseGetScheduleListDto);
        }
    }
}
