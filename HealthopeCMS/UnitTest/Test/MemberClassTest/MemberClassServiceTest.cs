using System;
using System.Collections.Generic;
using System.Linq;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.MemberClass.Request;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.MemberClassTest
{
    [TestClass]
    public class MemberClassServiceTest
    {
        private MemberClassService service;
        private Mock<IMemberClassRepository> memberClassRepositoryMock;
        private Mock<IMapper> mapperMock;

        [TestInitialize]
        public void Setup()
        {
            memberClassRepositoryMock = new Mock<IMemberClassRepository>();
            mapperMock = new Mock<IMapper>();
            service = new MemberClassService(memberClassRepositoryMock.Object, mapperMock.Object);
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
            memberClassRepositoryMock.Setup(s => s.GetPersonalTrainingPackageAndCoach(memberIdDto.MemberId)).Returns(responseGets);

            // Act
            List<ResponseGetPersonalTrainingPackageAndCoachDto> result = service.GetPersonalTrainingPackageAndCoach(memberIdDto);

            // Assert
            Assert.IsTrue(result.SequenceEqual(responseGets));
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

            MemberPersonalClass memberPersonalClass = new MemberPersonalClass
            {
                MemberId = 1,
                CoachId = 2,
                MemberPersonalTrainingPackageId = 1,
                Time = DateTime.UtcNow,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            mapperMock.Setup(s => s.Map<MemberPersonalClass>(addMemberPersonalClassDto)).Returns(memberPersonalClass);
            memberClassRepositoryMock.Setup(s => s.AddMemberPersonalClass(memberPersonalClass)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.AddMemberPersonalClass(addMemberPersonalClassDto);

            // Assert
            Assert.AreEqual(result, (ErrorCodeDefine)errorCodeNumber);
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

            MemberPersonalClass memberPersonalClass = new MemberPersonalClass
            {
                MemberId = 1,
                CoachId = 2,
                MemberPersonalTrainingPackageId = 1,
                Time = DateTime.UtcNow,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.MemberTimeConflict;

            // Mock 設定
            mapperMock.Setup(s => s.Map<MemberPersonalClass>(addMemberPersonalClassDto)).Returns(memberPersonalClass);
            memberClassRepositoryMock.Setup(s => s.AddMemberPersonalClass(memberPersonalClass)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.AddMemberPersonalClass(addMemberPersonalClassDto);

            // Assert
            Assert.AreEqual(result, (ErrorCodeDefine)errorCodeNumber);
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
            memberClassRepositoryMock.Setup(s => s.GetMemberPersonalClass(getMemberPersonalClassDto)).Returns(responseGets);

            // Act
            ResponseGetMemberPersonalClassListDto result = service.GetMemberPersonalClass(getMemberPersonalClassDto);

            // Assert
            Assert.IsTrue(responseGets.Equals(responseGets));
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

            MemberPersonalClass memberPersonalClass = new MemberPersonalClass
            {
                MemberPersonalClassId = 1,
                Remark = "123",
                UpdateTime = DateTime.Now,
            };

            bool successFlag = true;

            // Mock 設定
            mapperMock.Setup(s => s.Map<MemberPersonalClass>(editMemberPersonalClassRemarkDto)).Returns(memberPersonalClass);
            memberClassRepositoryMock.Setup(s => s.EditMemberPersonalClassRemark(memberPersonalClass)).Returns(successFlag);

            // Act
            bool result = service.EditMemberPersonalClassRemark(editMemberPersonalClassRemarkDto);

            // Assert
            Assert.IsTrue(result);
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

            MemberPersonalClass memberPersonalClass = new MemberPersonalClass
            {
                MemberPersonalClassId = 1,
                Remark = "123",
                UpdateTime = DateTime.Now,
            };

            bool successFlag = false;

            // Mock 設定
            mapperMock.Setup(s => s.Map<MemberPersonalClass>(editMemberPersonalClassRemarkDto)).Returns(memberPersonalClass);
            memberClassRepositoryMock.Setup(s => s.EditMemberPersonalClassRemark(memberPersonalClass)).Returns(successFlag);

            // Act
            bool result = service.EditMemberPersonalClassRemark(editMemberPersonalClassRemarkDto);

            // Assert
            Assert.IsFalse(result);
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

            MemberPersonalClass memberPersonalClass = new MemberPersonalClass
            {
                MemberPersonalClassId = 1,
                Status = 5,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            mapperMock.Setup(s => s.Map<MemberPersonalClass>(editStatusDto)).Returns(memberPersonalClass);
            memberClassRepositoryMock.Setup(s => s.EditMemberPersonalClassStatus(memberPersonalClass)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditMemberPersonalClassStatus(editStatusDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.Success);
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

            MemberPersonalClass memberPersonalClass = new MemberPersonalClass
            {
                MemberPersonalClassId = 1,
                Status = 5,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.ModifiedFailed;

            // Mock 設定
            mapperMock.Setup(s => s.Map<MemberPersonalClass>(editStatusDto)).Returns(memberPersonalClass);
            memberClassRepositoryMock.Setup(s => s.EditMemberPersonalClassStatus(memberPersonalClass)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditMemberPersonalClassStatus(editStatusDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.ModifiedFailed);
        }
    }
}
