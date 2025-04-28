using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Service;
using DomainLayer.Interface;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;

namespace UnitTest.Test.AdminTest
{
    [TestClass]
    public class AdminServiceTest
    {
        private Mock<IAdminRepository> adminRepositoryMock;
        private Mock<IAppSetting> appSettingMock;
        private AdminService adminService;

        [TestInitialize]
        public void Setup()
        {
            adminRepositoryMock = new Mock<IAdminRepository>();
            appSettingMock = new Mock<IAppSetting>();
            adminService = new AdminService(adminRepositoryMock.Object, appSettingMock.Object);
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
    }
}
