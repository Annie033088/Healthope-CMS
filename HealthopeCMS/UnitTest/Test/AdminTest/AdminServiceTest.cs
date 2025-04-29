using System.Collections.Generic;
using System.Linq;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Service;
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
        private Mock<IAppSetting> appSettingMock;
        private AdminService adminService;

        [TestInitialize]
        public void Setup()
        {
            adminRepositoryMock = new Mock<IAdminRepository>();
            appSettingMock = new Mock<IAppSetting>();
            sessionServiceMock = new Mock<ISessionService>();
            adminService = new AdminService(adminRepositoryMock.Object, appSettingMock.Object, sessionServiceMock.Object);
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
            if (adminPermissions.SequenceEqual(result)) return;
            Assert.Fail();
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
            if (result == null) return;
            Assert.Fail();
        }
    }
}
