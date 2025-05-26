using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.GroupClassScheduleTest
{
    [TestClass]
    public class GroupClassScheduleControllerTest
    {
        private GroupClassScheduleController controller;
        private Mock<IGroupClassScheduleService> groupClassScheduleServiceMock;

        [TestInitialize]
        public void Setup()
        {
            groupClassScheduleServiceMock = new Mock<IGroupClassScheduleService>();
            controller = new GroupClassScheduleController(groupClassScheduleServiceMock.Object);
        }

        [TestMethod]
        public void 取得新增團體課程表前需要的資料_成功_回傳清單()
        {
            // Arrange
            RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto = new RequestGetShowcaseAndCoachDto()
            {
                Category = null
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
            groupClassScheduleServiceMock.Setup(s
                => s.GetShowcaseAndCoach(getShowcaseAndCoachDto)).Returns(responseGet);

            // Act
            IHttpActionResult result = controller.GetShowcaseAndCoach(getShowcaseAndCoachDto);

            // Assert
            ResponseIsEqual<ResponseGetShowcaseAndCoachDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetShowcaseAndCoachDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseGet));
        }

        [TestMethod]
        public void 取得新增團體課程表前需要的資料_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto = new RequestGetShowcaseAndCoachDto()
            {
                Category = 30
            };

            // Act
            IHttpActionResult result = controller.GetShowcaseAndCoach(getShowcaseAndCoachDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 新增團體課程表_成功_回傳成功()
        {
            DateTime future = DateTime.Now.AddDays(2).Date;
            DateTime time = future.AddHours(18).AddMinutes(40);

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
                Time = time,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            groupClassScheduleServiceMock.Setup(s
                => s.AddSchedule(addScheduleDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.AddSchedule(addScheduleDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 新增團體課程表_失敗_請求參數格式錯誤()
        {
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
                Time = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.AddSchedule(addScheduleDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 新增團體課程表_失敗_課程時間重複()
        {
            DateTime future = DateTime.Now.AddDays(2).Date;
            DateTime time = future.AddHours(18).AddMinutes(40);
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
                Time = time,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.DuplicatePlaceAndTime;

            // Mock 設定
            groupClassScheduleServiceMock.Setup(s
                => s.AddSchedule(addScheduleDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.AddSchedule(addScheduleDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DuplicatePlaceAndTime));
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

            ResponseGetScheduleListDto responseGetScheduleListDto = new ResponseGetScheduleListDto()
            {
                ScheduleList = null,
                TotalPage = 1,
            };

            // Mock 設定
            groupClassScheduleServiceMock.Setup(s
                => s.GetSchedule(getGroupClassScheduleDto)).Returns(responseGetScheduleListDto);

            // Act
            IHttpActionResult result = controller.GetSchedule(getGroupClassScheduleDto);

            // Assert
            ResponseIsEqual<ResponseGetScheduleListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetScheduleListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseGetScheduleListDto));
        }

        [TestMethod]
        public void 取得團體課程表_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetGroupClassScheduleDto getGroupClassScheduleDto = new RequestGetGroupClassScheduleDto()
            {
                DateRangeFilter = "q",
                SortOption = null,
                Page = 1,
                RecordPerPage = 8,
                SortOrder = "ascending",
                SpecificDate = null,
                Status = 1
            };

            // Act
            IHttpActionResult result = controller.GetSchedule(getGroupClassScheduleDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
