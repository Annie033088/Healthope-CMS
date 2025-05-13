using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models;
using ApiLayer.Service;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using UnitTest.utils;
using ApiLayer.Models.Member;
using PersistentLayer.Models;
using DomainLayer.Models;
using ApiLayer.Models.Member.Response;
using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Interface;

namespace UnitTest.Test.MemberTest
{
    [TestClass]
    public class MemberServiceTest
    {
        private MemberService memberService;
        private Mock<IMemberRepository> memberRepositoryMock;
        private Mock<IMapper> mapperMock;
        private Mock<IRedisService> redisServiceMock;

        [TestInitialize]
        public void Setup()
        {
            memberRepositoryMock = new Mock<IMemberRepository>();
            mapperMock = new Mock<IMapper>();
            redisServiceMock = new Mock<IRedisService>();
            memberService = new MemberService(memberRepositoryMock.Object, mapperMock.Object, redisServiceMock.Object);
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
            DateTime time = DateTime.Now;
            List<Member> members = new List<Member>()
            {
                 new Member()
                {
                    MemberId = 1,
                    Name = "孫冬飽",
                    Phone = 987654321,
                    PhoneVerified = false,
                    Email = "",
                    MembershipExpiry = time,
                    Status = true,
                    AbsenceTime = 5,
                    AllowGroupClass = time,
                },
            };

            int totalPage = 1;
            List<ResponseGetMemberDto> expectedMappedResult = new List<ResponseGetMemberDto>() {
                new  ResponseGetMemberDto(){
                    MemberId = 1,
                    Name = "孫冬飽",
                    Phone = 987654321,
                    PhoneVerified = false,
                    Email = "",
                    MembershipExpiry = time,
                    Status = true,
                    AbsenceTime = 5,
                    AllowGroupClass = time,
                } 
            };
            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMember(getMemberDto)).Returns((members, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetMemberDto>>(members)).Returns(expectedMappedResult);

            // Act
            ResponseGetMemberListDto response = memberService.GetMember(getMemberDto);

            // Assert
            Assert.IsTrue(response.MemberList.SequenceEqual(expectedMappedResult));
        }

        [TestMethod]
        public void 取得會員清單_失敗_回傳空資料()
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
            DateTime time = DateTime.Now;
            List<Member> members = new List<Member>();

            int totalPage = 1;
            List<ResponseGetMemberDto> expectedMappedResult = new List<ResponseGetMemberDto>();

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMember(getMemberDto)).Returns((members, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetMemberDto>>(members)).Returns(expectedMappedResult);

            // Act
            ResponseGetMemberListDto response = memberService.GetMember(getMemberDto);

            // Assert
            Assert.IsTrue(response.MemberList.SequenceEqual(expectedMappedResult));
        }

        [TestMethod]
        public void 根據Id取得要修改會員的資料_成功_回傳會員資料()
        {
            // Arrange
            DateTime time = DateTime.Now;
            Member member = new Member()
            {
                Name = "okwopekq122",
                Phone = 987654342,
                Status = true,
                UpdateTime = time
            };

            ResponseGetMemberEditDataByIdDto response = new ResponseGetMemberEditDataByIdDto()
            {
                Name = "okwopekq122",
                Phone = 987654342,
                Status = true,
                UpdateTime = time,
            };

            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberEditDataById(memberIdDto.MemberId)).Returns(member);
            mapperMock.Setup(s=>s.Map<ResponseGetMemberEditDataByIdDto>(member)).Returns(response);

            // Act
            ResponseGetMemberEditDataByIdDto result = memberService.GetMemberEditDataById(memberIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 根據Id取得要修改會員的資料_失敗_回傳空資料()
        {
            // Arrange
            DateTime time = DateTime.Now;
            Member member = new Member();

            ResponseGetMemberEditDataByIdDto response = new ResponseGetMemberEditDataByIdDto();

            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberEditDataById(memberIdDto.MemberId)).Returns(member);
            mapperMock.Setup(s => s.Map<ResponseGetMemberEditDataByIdDto>(member)).Returns(response);

            // Act
            ResponseGetMemberEditDataByIdDto result = memberService.GetMemberEditDataById(memberIdDto);

            // Assert
            Assert.AreEqual(result, response);
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

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            memberRepositoryMock.Setup(s => s.EditMember(editMemberDto)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = memberService.EditMember(editMemberDto);

            // Assert
            Assert.IsTrue(result == (ErrorCodeDefine)errorCodeNumber);
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

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            memberRepositoryMock.Setup(s => s.EditMember(editMemberDto)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = memberService.EditMember(editMemberDto);

            // Assert
            Assert.IsTrue(result == (ErrorCodeDefine)errorCodeNumber);
        }

        [TestMethod]
        public void 修改會員_失敗_回傳修改的手機號碼重複()
        {
            // Arrange
            RequestEditMemberDto editMemberDto = new RequestEditMemberDto()
            {
                MemberId = 10,
                Phone = 987654321,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.DuplicatePhone;

            // Mock 設定
            memberRepositoryMock.Setup(s => s.EditMember(editMemberDto)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = memberService.EditMember(editMemberDto);

            // Assert
            Assert.IsTrue(result == (ErrorCodeDefine)errorCodeNumber);
        }
       
        [TestMethod]
        public void 根據Id取得會員詳細資料_成功_回傳會員資料()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            Member member = new Member()
            {
                Name = "okwopekq122",
                Phone = 987654342,
                Status = true,
            };

            ResponseGetMemberDetailDto response = new ResponseGetMemberDetailDto()
            {
                Name = "okwopekq122",
                Phone = 987654342,
                Status = true,
            };

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberDetail(memberIdDto.MemberId)).Returns(member);
            mapperMock.Setup(s=>s.Map<ResponseGetMemberDetailDto>(member)).Returns(response);

            // Act
            ResponseGetMemberDetailDto result = memberService.GetMemberDetail(memberIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 根據Id取得會員詳細資料_失敗_回傳空資料()
        {
            // Arrange
            RequestMemberIdDto memberIdDto = new RequestMemberIdDto()
            {
                MemberId = 1
            };

            Member member = null;

            ResponseGetMemberDetailDto response = null;

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberDetail(memberIdDto.MemberId)).Returns(member);

            // Act
            ResponseGetMemberDetailDto result = memberService.GetMemberDetail(memberIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }
    }
}
