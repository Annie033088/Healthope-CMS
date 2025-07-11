using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models.MemberPlan.Request;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;
using UnitTest.utils;
using ApiLayer.Controllers;
using ApiLayer.Models.Member;
using ApiLayer.Service;
using PersistentLayer.Models;
using ApiLayer.Models.MemberClass.Request;

namespace UnitTest.Test.MemberClassTest
{
    [TestClass]
    public class MemberClassControllerTest
    {
        private MemberClassController controller;
        private Mock<IMemberClassService> memberClassServiceMock;

        [TestInitialize]
        public void Setup()
        {
            memberClassServiceMock = new Mock<IMemberClassService>();
            controller = new MemberClassController(memberClassServiceMock.Object);
        }

        [TestMethod]
        public void 取得新增教練課時的教練課跟教練資料_成功_回傳成功()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            List<ResponseGetPersonalTrainingPackageAndCoachDto> responseGets = new List<ResponseGetPersonalTrainingPackageAndCoachDto>
            {
                new ResponseGetPersonalTrainingPackageAndCoachDto
                {
                    CoachId = 1,
                }
            };

            // Mock 設定
            memberClassServiceMock.Setup(s => s.GetPersonalTrainingPackageAndCoach(memberIdDto)).Returns(responseGets);

            // Act
            IHttpActionResult result = controller.GetPersonalTrainingPackageAndCoach(memberIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 取得新增教練課時的教練課跟教練資料_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 0
            };

            // Act
            IHttpActionResult result = controller.GetPersonalTrainingPackageAndCoach(memberIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 新增會員預約教練課程_成功_回傳成功()
        {
            // Arrange
            RequestAddMemberPersonalClassDto addMemberPersonalClassDto = new RequestAddMemberPersonalClassDto()
            {
                MemberId = 1,
                CoachId = 2,
                MemberPersonalTrainingPackageId = 1,
                Time = DateTime.UtcNow,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            memberClassServiceMock.Setup(s => s.AddMemberPersonalClass(addMemberPersonalClassDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.AddMemberPersonalClass(addMemberPersonalClassDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 新增會員預約教練課程_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestAddMemberPersonalClassDto addMemberPersonalClassDto = new RequestAddMemberPersonalClassDto()
            {
                MemberId = -1,
                CoachId = 2,
                MemberPersonalTrainingPackageId = 1,
                Time = DateTime.UtcNow,
            };

            // Act
            IHttpActionResult result = controller.AddMemberPersonalClass(addMemberPersonalClassDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 新增會員預約教練課程_失敗_回傳會員時間衝突()
        {
            // Arrange
            RequestAddMemberPersonalClassDto addMemberPersonalClassDto = new RequestAddMemberPersonalClassDto()
            {
                MemberId = 1,
                CoachId = 2,
                MemberPersonalTrainingPackageId = 1,
                Time = DateTime.UtcNow,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.MemberTimeConflict;

            // Mock 設定
            memberClassServiceMock.Setup(s => s.AddMemberPersonalClass(addMemberPersonalClassDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.AddMemberPersonalClass(addMemberPersonalClassDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.MemberTimeConflict));
        }

        [TestMethod]
        public void 取得會員預約的教練課程列表_成功_回傳成功()
        {
            // Arrange
            RequestGetMemberPersonalClassDto getMemberPersonalClassDto = new RequestGetMemberPersonalClassDto()
            {
                SearchPhone = "123",
                Page = 1,
                RecordPerPage = 8,
                SortOption = "time",
                SortOrder = "descending",
                Status = 1
            };

            ResponseGetMemberPersonalClassListDto responseGets = new ResponseGetMemberPersonalClassListDto
            {
                MemberPersonalClassList = new List<ResponseGetMemberPersonalClassDto>
                {
                    new ResponseGetMemberPersonalClassDto { MemberId = 2 },
                },
                TotalPage = 1,
            };

            // Mock 設定
            memberClassServiceMock.Setup(s => s.GetMemberPersonalClass(getMemberPersonalClassDto)).Returns(responseGets);

            // Act
            IHttpActionResult result = controller.GetMemberPersonalClass(getMemberPersonalClassDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 取得會員預約的教練課程列表_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestGetMemberPersonalClassDto getMemberPersonalClassDto = new RequestGetMemberPersonalClassDto()
            {
                SearchPhone = "123",
                Page = 1,
                RecordPerPage = 8,
                SortOption = "time",
                SortOrder = "www",
                Status = 1
            };
            // Act
            IHttpActionResult result = controller.GetMemberPersonalClass(getMemberPersonalClassDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
