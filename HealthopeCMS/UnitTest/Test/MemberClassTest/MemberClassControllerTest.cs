using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Controllers;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.MemberClass.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

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

        [TestMethod]
        public void 修改會員預約教練課程備註_成功_回傳成功()
        {
            // Arrange
            RequestEditMemberPersonalClassRemarkDto editMemberPersonalClassRemarkDto = new RequestEditMemberPersonalClassRemarkDto()
            {
                MemberPersonalClassId = 1,
                Remark = "123",
                UpdateTime = DateTime.Now,
            };

            bool successFlag = true;

            // Mock 設定
            memberClassServiceMock.Setup(s => s.EditMemberPersonalClassRemark(editMemberPersonalClassRemarkDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.EditMemberPersonalClassRemark(editMemberPersonalClassRemarkDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改會員預約教練課程備註_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditMemberPersonalClassRemarkDto editMemberPersonalClassRemarkDto = new RequestEditMemberPersonalClassRemarkDto()
            {
                MemberPersonalClassId = 0,
                Remark = "123",
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditMemberPersonalClassRemark(editMemberPersonalClassRemarkDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改會員預約教練課程備註_失敗_回傳失敗()
        {
            // Arrange
            RequestEditMemberPersonalClassRemarkDto editMemberPersonalClassRemarkDto = new RequestEditMemberPersonalClassRemarkDto()
            {
                MemberPersonalClassId = 1,
                Remark = "123",
                UpdateTime = DateTime.Now,
            };

            bool successFlag = false;

            // Mock 設定
            memberClassServiceMock.Setup(s => s.EditMemberPersonalClassRemark(editMemberPersonalClassRemarkDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.EditMemberPersonalClassRemark(editMemberPersonalClassRemarkDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.ModifiedFailed));
        }

        [TestMethod]
        public void 修改會員預約教練課程狀態_成功_回傳成功()
        {
            // Arrange
            RequestEditMemberPersonalClassStatusDto editStatusDto = new RequestEditMemberPersonalClassStatusDto()
            {
                MemberPersonalClassId = 1,
                Status = 5,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            memberClassServiceMock.Setup(s => s.EditMemberPersonalClassStatus(editStatusDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.EditMemberPersonalClassStatus(editStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改會員預約教練課程狀態_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditMemberPersonalClassStatusDto editStatusDto = new RequestEditMemberPersonalClassStatusDto()
            {
                MemberPersonalClassId = 1,
                Status = 10,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditMemberPersonalClassStatus(editStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改會員預約教練課程狀態_失敗_回傳失敗()
        {
            // Arrange
            RequestEditMemberPersonalClassStatusDto editStatusDto = new RequestEditMemberPersonalClassStatusDto()
            {
                MemberPersonalClassId = 1,
                Status = 5,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCode = ErrorCodeDefine.ModifiedFailed;

            // Mock 設定
            memberClassServiceMock.Setup(s => s.EditMemberPersonalClassStatus(editStatusDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.EditMemberPersonalClassStatus(editStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.ModifiedFailed));
        }
    }
}
