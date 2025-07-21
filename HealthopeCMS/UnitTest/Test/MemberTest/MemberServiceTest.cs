using System;
using System.Collections.Generic;
using System.Linq;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.Member.Request;
using ApiLayer.Models.Member.Response;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

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
            mapperMock.Setup(s => s.Map<ResponseGetMemberEditDataByIdDto>(member)).Returns(response);

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
            List<MemberMembershipPlan> memberMembershipPlans = new List<MemberMembershipPlan>();
            List<MemberPersonalTrainingPackage> memberPersonalTrainingPackages = new List<MemberPersonalTrainingPackage>();
            List<Coach> coaches = new List<Coach>();

            ResponseGetMemberDetailDto response = new ResponseGetMemberDetailDto()
            {
                Member = new ResponseGetMemberDetailMemberDto
                {
                    Name = "okwopekq122",
                    Phone = 987654342,
                    Status = true,
                },
                MemberMembershipPlanList = new List<ResponseGetMemberDetailMembershipPlanDto>
                {
                    new ResponseGetMemberDetailMembershipPlanDto
                    {
                        Duration = 12,
                    }
                },
                MemberPersonalTrainingPackageList = new List<ResponseGetMemberDetailPersonalTrainingPackageDto>
                {
                    new ResponseGetMemberDetailPersonalTrainingPackageDto
                    {
                        CoachId = 1,
                    }
                },
                CoachList = new List<ResponseGetMemberDetailCoachDto>
                {
                    new ResponseGetMemberDetailCoachDto
                    {
                        CoachId = 1,
                    }
                }
            };

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberDetail(memberIdDto.MemberId))
                .Returns((member, memberMembershipPlans, memberPersonalTrainingPackages, coaches));
            mapperMock.Setup(s => s.Map<ResponseGetMemberDetailMemberDto>(member)).Returns(response.Member);
            mapperMock.Setup(s => s.Map<List<ResponseGetMemberDetailMembershipPlanDto>>(memberMembershipPlans))
                .Returns(response.MemberMembershipPlanList);
            mapperMock.Setup(s => s.Map<List<ResponseGetMemberDetailPersonalTrainingPackageDto>>(memberPersonalTrainingPackages))
                .Returns(response.MemberPersonalTrainingPackageList);
            mapperMock.Setup(s => s.Map<List<ResponseGetMemberDetailCoachDto>>(coaches)).Returns(response.CoachList);

            // Act
            ResponseGetMemberDetailDto result = memberService.GetMemberDetail(memberIdDto);

            // Assert
            Assert.AreEqual(result.Member, response.Member);
            Assert.IsTrue(result.MemberMembershipPlanList.SequenceEqual(response.MemberMembershipPlanList));
            Assert.IsTrue(result.MemberPersonalTrainingPackageList.SequenceEqual(response.MemberPersonalTrainingPackageList));
            Assert.IsTrue(result.CoachList.SequenceEqual(response.CoachList));
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
            List<MemberMembershipPlan> memberMembershipPlans = null;
            List<MemberPersonalTrainingPackage> memberPersonalTrainingPackages = null;
            List<Coach> coaches = null;

            ResponseGetMemberDetailDto response = null;

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberDetail(memberIdDto.MemberId))
                .Returns((member, memberMembershipPlans, memberPersonalTrainingPackages, coaches));

            // Act
            ResponseGetMemberDetailDto result = memberService.GetMemberDetail(memberIdDto);

            // Assert
            Assert.AreEqual(result, response);
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
            List<ResponseGetMemberByNameOrPhoneDto> response = new List<ResponseGetMemberByNameOrPhoneDto>()
            {
                new ResponseGetMemberByNameOrPhoneDto
                {
                    Name = "okwopekq122",
                    Phone = 987654342,
                    MemberId = 1,
                    PhoneVerified = true,
                }
            };

            List<Member> members = new List<Member>()
            {
                new Member()
                {
                    Name = "okwopekq122",
                    Phone = 987654342,
                    MemberId = 1,
                    PhoneVerified = true,
                    UpdateTime = DateTime.Now,
                }
            };


            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberByNameOrPhone(getMemberDto)).Returns(members);
            mapperMock.Setup(s => s.Map<List<ResponseGetMemberByNameOrPhoneDto>>(members)).Returns(response);

            // Act
            List<ResponseGetMemberByNameOrPhoneDto> result = memberService.GetMemberByNameOrPhone(getMemberDto);

            // Assert
            Assert.IsTrue(result.SequenceEqual(response));
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

            List<ResponseGetMemberByNameOrPhoneDto> response = null;
            List<Member> members = null;

            // Mock 設定
            memberRepositoryMock.Setup(s => s.GetMemberByNameOrPhone(getMemberDto)).Returns(members);
            mapperMock.Setup(s => s.Map<List<ResponseGetMemberByNameOrPhoneDto>>(members)).Returns(response);

            // Act
            List<ResponseGetMemberByNameOrPhoneDto> result = memberService.GetMemberByNameOrPhone(getMemberDto);

            // Assert
            Assert.IsNull(result);
        }
    }
}
