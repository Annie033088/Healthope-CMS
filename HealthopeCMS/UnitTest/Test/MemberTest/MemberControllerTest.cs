using System;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.Member.Request;
using ApiLayer.Models.Member.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.MemberTest
{
    [TestClass]
    public class MemberControllerTest
    {
        private MemberController memberController;
        private Mock<IMemberService> memberServiceMock;

        [TestInitialize]
        public void Setup()
        {
            memberServiceMock = new Mock<IMemberService>();
            memberController = new MemberController(memberServiceMock.Object);
        }

        [TestMethod]
        public void 取得會員清單_成功_回傳會員清單()
        {
            // Arrange
            RequestGetMemberDto getMemberDto = new RequestGetMemberDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "membershipExpiry", // 只允許 membershipExpiry 或 name 或 status 或 null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 < 50 位數
                SearchPhone = null, // 只允許 null 或是 3 位數字
            };

            ResponseGetMemberListDto responseGetMemberDto = new ResponseGetMemberListDto()
            {
                MemberList = null,
                TotalPage = 1,
            };


            // Mock 設定
            memberServiceMock.Setup(s => s.GetMember(getMemberDto)).Returns(responseGetMemberDto);

            // Act
            IHttpActionResult result = memberController.GetMember(getMemberDto);

            // Assert
            ResponseIsEqual<ResponseGetMemberListDto> responseIsEqual = new ResponseIsEqual<ResponseGetMemberListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, responseGetMemberDto));
        }

        [TestMethod]
        public void 取得會員清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetMemberDto getMemberDto = new RequestGetMemberDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "account", // 只允許 membershipExpiry 或 name 或 status 或 null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 < 50 位數
                SearchPhone = "012", // 只允許 null 或是 3 位數字
            };

            ResponseGetMemberListDto responseGetMemberDto = new ResponseGetMemberListDto()
            {
                MemberList = null,
                TotalPage = 1,
            };

            // Mock 設定
            memberServiceMock.Setup(s => s.GetMember(getMemberDto)).Returns(responseGetMemberDto);

            // Act
            IHttpActionResult result = memberController.GetMember(getMemberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 根據Id取得要修改會員的資料_成功_回傳會員資料()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            ResponseGetMemberEditDataByIdDto response = new ResponseGetMemberEditDataByIdDto()
            {
                Name = "okwopekq122",
                Phone = 987654342,
                Status = true,
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            memberServiceMock.Setup(s => s.GetMemberEditDataById(memberIdDto)).Returns(response);

            // Act
            IHttpActionResult result = memberController.GetMemberEditDataById(memberIdDto);

            // Assert
            ResponseIsEqual<ResponseGetMemberEditDataByIdDto> responseIsEqual = new ResponseIsEqual<ResponseGetMemberEditDataByIdDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 根據Id取得要修改會員的資料_失敗_回傳空資料()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            ResponseGetMemberEditDataByIdDto response = null;

            // Mock 設定
            memberServiceMock.Setup(s => s.GetMemberEditDataById(memberIdDto)).Returns(response);

            // Act
            IHttpActionResult result = memberController.GetMemberEditDataById(memberIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.GetFailed));
        }

        [TestMethod]
        public void 修改會員_成功_回傳成功()
        {
            // Arrange
            RequestEditMemberDto editMemberDto = new RequestEditMemberDto()
            {
                MemberId = 10,
                Phone = 987654321,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCdoe = ErrorCodeDefine.Success;

            // Mock 設定
            memberServiceMock.Setup(s => s.EditMember(editMemberDto)).Returns(errorCdoe);

            // Act
            IHttpActionResult result = memberController.EditMember(editMemberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改會員_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditMemberDto editMemberDto = new RequestEditMemberDto()
            {
                MemberId = 10,
                Phone = 97654321,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = memberController.EditMember(editMemberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改會員_失敗_回傳資料已被他人修改()
        {
            // Arrange
            RequestEditMemberDto editMemberDto = new RequestEditMemberDto()
            {
                MemberId = 10,
                Phone = 987654321,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCdoe = ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            memberServiceMock.Setup(s => s.EditMember(editMemberDto)).Returns(errorCdoe);

            // Act
            IHttpActionResult result = memberController.EditMember(editMemberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.HasBeenModified));
        }

        [TestMethod]
        public void 根據Id取得會員詳細資料_成功_回傳會員資料()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            ResponseGetMemberDetailDto response = new ResponseGetMemberDetailDto()
            {
                Name = "okwopekq122",
                Phone = 987654342,
                Status = true,
            };

            // Mock 設定
            memberServiceMock.Setup(s => s.GetMemberDetail(memberIdDto)).Returns(response);

            // Act
            IHttpActionResult result = memberController.GetMemberDetail(memberIdDto);

            // Assert
            ResponseIsEqual<ResponseGetMemberDetailDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetMemberDetailDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 根據Id取得會員詳細資料_失敗_回傳空資料()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            ResponseGetMemberDetailDto response = null;

            // Mock 設定
            memberServiceMock.Setup(s => s.GetMemberDetail(memberIdDto)).Returns(response);

            // Act
            IHttpActionResult result = memberController.GetMemberDetail(memberIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.GetFailed));
        }

        [TestMethod]
        public void 根據名稱或手機取得會員_成功_回傳會員資料()
        {
            // Arrange
            RequestGetMemberByNameOrPhoneDto getMemberDto = new RequestGetMemberByNameOrPhoneDto()
            {
                Name = null,
                Phone = 0987654321
            };

            ResponseGetMemberByNameOrPhoneDto response = new ResponseGetMemberByNameOrPhoneDto()
            {
                Name = "okwopekq122",
                Phone = 987654342,
                MemberId = 1,
                PhoneVerified = true,
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            memberServiceMock.Setup(s => s.GetMemberByNameOrPhone(getMemberDto)).Returns(response);

            // Act
            IHttpActionResult result = memberController.GetMemberByNameOrPhone(getMemberDto);

            // Assert
            ResponseIsEqual<ResponseGetMemberByNameOrPhoneDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetMemberByNameOrPhoneDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 根據名稱或手機取得會員_失敗_回傳空資料()
        {
            // Arrange
            RequestGetMemberByNameOrPhoneDto getMemberDto = new RequestGetMemberByNameOrPhoneDto()
            {
                Name = null,
                Phone = 0987654321
            };

            ResponseGetMemberByNameOrPhoneDto response = null;

            // Mock 設定
            memberServiceMock.Setup(s => s.GetMemberByNameOrPhone(getMemberDto)).Returns(response);

            // Act
            IHttpActionResult result = memberController.GetMemberByNameOrPhone(getMemberDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.GetFailed));
        }
    }
}
