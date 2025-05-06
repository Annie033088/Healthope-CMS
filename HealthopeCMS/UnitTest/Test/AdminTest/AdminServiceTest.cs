using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models.Admin;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Interface;
using DomainLayer.Models;
using DomainLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;

namespace UnitTest.Test.AdminTest
{
    [TestClass]
    public class AdminServiceTest
    {
        private Mock<IAdminRepository> adminRepositoryMock;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<IRedisService> redisServiceMock;
        private Mock<IAppSetting> appSettingMock;
        private Mock<IMapper> mapperMock;
        private AdminService adminService;

        [TestInitialize]
        public void Setup()
        {
            adminRepositoryMock = new Mock<IAdminRepository>();
            appSettingMock = new Mock<IAppSetting>();
            sessionServiceMock = new Mock<ISessionService>();
            redisServiceMock = new Mock<IRedisService>();
            mapperMock = new Mock<IMapper>();
            adminService = new AdminService(adminRepositoryMock.Object, appSettingMock.Object,
                sessionServiceMock.Object, mapperMock.Object, redisServiceMock.Object);
        }

        [TestMethod]
        public void 新增_成功_回傳成功()
        {
            // Arrange
            RequestAddAdminDto addAdminDto = new RequestAddAdminDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Identity = "CoachManager"
            };

            // Mock 設定
            adminRepositoryMock.Setup(s => s.AddAdmin(It.IsAny<Admin>())).Returns(true);
            appSettingMock.Setup(s => s.GetSuperAdminAccount()).Returns("PB7m9JsNaRK0");

            // Act
            bool successFlag = adminService.AddAdmin(addAdminDto);

            // Assert
            Assert.IsTrue(successFlag);
        }

        [TestMethod]
        public void 新增_失敗_帳號跟超級管理員重複()
        {
            // Arrange
            RequestAddAdminDto addAdminDto = new RequestAddAdminDto()
            {
                Account = "PB7m9JsNaRK0",
                Pwd = "g4556fgerger",
                Identity = "CoachManager"
            };
            Admin admin = new Admin();

            // Mock 設定
            appSettingMock.Setup(s => s.GetSuperAdminAccount()).Returns("PB7m9JsNaRK0");

            // Act
            bool successFlag = adminService.AddAdmin(addAdminDto);

            // Assert
            Assert.IsFalse(successFlag);
        }

        [TestMethod]
        public void 新增_失敗_身份錯誤()
        {
            // Arrange
            RequestAddAdminDto addAdminDto = new RequestAddAdminDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Identity = "qweqw"
            };
            Admin admin = new Admin();

            // Mock 設定
            appSettingMock.Setup(s => s.GetSuperAdminAccount()).Returns("PB7m9JsNaRK0");

            // Act
            bool successFlag = adminService.AddAdmin(addAdminDto);

            // Assert
            Assert.IsFalse(successFlag);
        }

        [TestMethod]
        public void 取得權限_成功_回傳權限()
        {
            // Arrange
            AdminSession adminSession = new AdminSession()
            {
                AdminId = 1,
                Identity = AdminIdentity.Admin
            };
            string adminSessionKey = "AdminSession";
            AdminPermissionUtility adminPermissionUtility = new AdminPermissionUtility();
            List<AdminPermission> adminPermissions = adminPermissionUtility.GetPermissions(adminSession.Identity);

            // Mock 設定
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(adminSessionKey)).Returns(adminSession);

            // Act
            List<AdminPermission> result = adminService.GetPermission();

            // Assert
            CollectionAssert.AreEqual(result, adminPermissions); // MSTest 專用
        }

        [TestMethod]
        public void 取得權限_成功_回傳無權限()
        {
            // Arrange
            AdminSession adminSession = new AdminSession()
            {
                AdminId = 1,
                Identity = AdminIdentity.Accountant
            };
            string adminSessionKey = "AdminSession";
            AdminPermissionUtility adminPermissionUtility = new AdminPermissionUtility();
            List<AdminPermission> adminPermissions = adminPermissionUtility.GetPermissions(adminSession.Identity);

            // Mock 設定
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(adminSessionKey)).Returns(adminSession);

            // Act
            List<AdminPermission> result = adminService.GetPermission();

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void 取得管理員清單_成功_回傳管理者清單()
        {
            // Arrange
            RequestGetAdminDto getAdminDto = new RequestGetAdminDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SearchAccount = null, // 只允許 null 或 長度>2的字串
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "account", // 只允許 account 或 name 或 null
                RecordPerPage = 8 // 只允許 8 或 12 或 16
            };
            DateTime time = DateTime.Now;

            List<Admin> admins = new List<Admin>()
            {
               new Admin()
               {
                   AdminId=1,
                   Account="wqekopqw111",
                   Identity=1,
                   Status=true,
                   UpdateTime=time
               }
            };
            int totalPage = 1;
            List<ResponseGetAdminDto> expectedMappedResult = new List<ResponseGetAdminDto>()
            {
                new ResponseGetAdminDto()
                {
                   AdminId=1,
                   Account="wqekopqw111",
                   Identity=1,
                   Status=true,
                   UpdateTime=time
                }
            };

            // Mock 設定
            adminRepositoryMock.Setup(s => s.GetAdmin(getAdminDto)).Returns((admins, totalPage));
            mapperMock.Setup(m => m.Map<List<ResponseGetAdminDto>>(admins)).Returns(expectedMappedResult);

            // Act
            ResponseGetAdminListDto response = adminService.GetAdmin(getAdminDto);

            // Assert
            Assert.AreEqual(expectedMappedResult, response.AdminList);
        }

        [TestMethod]
        public void 取得管理員清單_失敗_回傳空資料()
        {
            // Arrange
            RequestGetAdminDto getAdminDto = new RequestGetAdminDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SearchAccount = null, // 只允許 null 或 長度>2的字串
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "account", // 只允許 account 或 name 或 null
                RecordPerPage = 8 // 只允許 8 或 12 或 16
            };
            DateTime time = DateTime.Now;

            List<Admin> admins = new List<Admin>();
            int totalPage = 1;
            List<ResponseGetAdminDto> expectedMappedResult = new List<ResponseGetAdminDto>();

            // Mock 設定
            adminRepositoryMock.Setup(s => s.GetAdmin(getAdminDto)).Returns((admins, totalPage));
            mapperMock.Setup(m => m.Map<List<ResponseGetAdminDto>>(admins)).Returns(expectedMappedResult);

            // Act
            ResponseGetAdminListDto response = adminService.GetAdmin(getAdminDto);

            // Assert
            Assert.AreEqual(expectedMappedResult, response.AdminList);
        }

        [TestMethod]
        public void 根據Id取得管理員_成功_回傳管理員()
        {
            // Arrange
            RequestAdminIdDto getAdminIdDto = new RequestAdminIdDto()
            {
                AdminId = 1
            };

            ResponseGetAdminDto responseGetAdminDto = new ResponseGetAdminDto()
            {
                AdminId = 1,
                Account = "okwopekq122",
                Identity = 1,
                Status = true,
                UpdateTime = DateTime.Now,
            };

            Admin admin = new Admin()
            {
                AdminId = 1,
                Account = "okwopekq122",
                Identity = 1,
                Status = true,
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            adminRepositoryMock.Setup(s => s.GetAdminById(getAdminIdDto.AdminId)).Returns(admin);
            mapperMock.Setup(m => m.Map<ResponseGetAdminDto>(admin)).Returns(responseGetAdminDto);

            // Act
            ResponseGetAdminDto response = adminService.GetAdminById(getAdminIdDto);

            // Assert
            Assert.AreEqual(responseGetAdminDto, response);
        }

        [TestMethod]
        public void 根據Id取得管理員_失敗_回傳空資料()
        {
            // Arrange
            RequestAdminIdDto getAdminIdDto = new RequestAdminIdDto()
            {
                AdminId = 999
            };

            ResponseGetAdminDto responseGetAdminDto = null;
            Admin admin = null;

            // Mock 設定
            adminRepositoryMock.Setup(s => s.GetAdminById(getAdminIdDto.AdminId)).Returns(admin);
            mapperMock.Setup(m => m.Map<ResponseGetAdminDto>(admin)).Returns(responseGetAdminDto);

            // Act
            ResponseGetAdminDto response = adminService.GetAdminById(getAdminIdDto);

            // Assert
            Assert.AreEqual(responseGetAdminDto, response);
        }

        [TestMethod]
        public void 修改管理員_成功_回傳成功()
        {
            // Arrange
            RequestEditAdminDto editAdminDto = new RequestEditAdminDto()
            {
                AdminId = 10,
                Identity = null,
                Status = false,
                UpdateTime = DateTime.Now,
            };
            AdminRedis adminRedis = new AdminRedis()
            {
                SessionId = "kqowpekqw",
                ErrorCode = ApiLayer.Models.ErrorCodeDefine.Success
            };
            string redisKey = "Admin" + editAdminDto.AdminId;
            bool successFlag = true;

            // Mock 設定
            adminRepositoryMock.Setup(s => s.EditAdmin(editAdminDto)).Returns(successFlag);
            redisServiceMock.Setup(s => s.GetValue<AdminRedis>(redisKey))
                .Returns(adminRedis);
            adminRedis.ErrorCode = ApiLayer.Models.ErrorCodeDefine.PermissionModified;
            redisServiceMock.Setup(s => s.SetValue<AdminRedis>(
            It.Is<string>(k => k == redisKey),
            It.Is<AdminRedis>(v => v == adminRedis),
            It.IsAny<TimeSpan?>()));

            // Act
            bool result = adminService.EditAdmin(editAdminDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 修改管理員_失敗_回傳資料已被他人修改()
        {
            // Arrange
            RequestEditAdminDto editAdminDto = new RequestEditAdminDto()
            {
                AdminId = 10,
                Identity = null,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            bool successFlag = false;

            // Mock 設定
            adminRepositoryMock.Setup(s => s.EditAdmin(editAdminDto)).Returns(successFlag);

            // Act
            bool result = adminService.EditAdmin(editAdminDto);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void 刪除管理員_成功_回傳成功()
        {
            // Arrange
            RequestAdminIdDto adminIdDto = new RequestAdminIdDto()
            {
                AdminId = 10,
            };

            bool successFlag = true;

            // Mock 設定
            adminRepositoryMock.Setup(s => s.DeleteAdmin(adminIdDto.AdminId)).Returns(successFlag);

            // Act
            bool result = adminService.DeleteAdmin(adminIdDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 刪除管理員_失敗_回傳失敗()
        {
            // Arrange
            RequestAdminIdDto adminIdDto = new RequestAdminIdDto()
            {
                AdminId = 1000000000,
            };

            bool successFlag = false;

            // Mock 設定
            adminRepositoryMock.Setup(s => s.DeleteAdmin(adminIdDto.AdminId)).Returns(successFlag);

            // Act
            bool result = adminService.DeleteAdmin(adminIdDto);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
