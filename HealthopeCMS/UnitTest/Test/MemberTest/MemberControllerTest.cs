using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using ApiLayer.Models.Member;
using UnitTest.utils;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Member.Response;

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
                SortOption = "account", // 只允許 account 或 name 或 null
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
                SortOption = "account", // 只允許 account 或 name 或 null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 < 50 位數
                SearchPhone = 012, // 只允許 null 或是 3 位數字
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
    }
}
