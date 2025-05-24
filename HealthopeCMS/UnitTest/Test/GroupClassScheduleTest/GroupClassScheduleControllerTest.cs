using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using System.Collections.Generic;
using System;

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
            // Arrange
            RequestAddScheduleDto addScheduleDto = new RequestAddScheduleDto()
            {
                Category=1,
                ClassName="wq",
                Coach= new ScheduleGetCoachDto()
                {
                    CoachId=1,
                    UpdateTime = DateTime.Now,
                },
                Icon=1,
                MaximumParticipant=35,
                Place="weq",
                Time = new DateTime(2025, 05, 25, 18, 40, 0),
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
    }
}
