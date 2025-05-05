using System;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestAccountAccessDto;
using ApiLayer.Models.Admin;
using ApiLayer.Service;
using DomainLayer.Interface;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.AccountAccessTest
{
    [TestClass]
    public class AccountAccessServiceTest
    {
        private AccountAccessService accountAccessService;
        private Mock<IAdminRepository> adminRepositoryMock;
        private Mock<IAppSetting> appSettingMock;
        private Mock<IRedisService> redisServiceMock;
        private Mock<ISessionService> sessionServiceMock;

        [TestInitialize]
        public void Setup()
        {
            adminRepositoryMock = new Mock<IAdminRepository>();
            appSettingMock = new Mock<IAppSetting>();
            redisServiceMock = new Mock<IRedisService>();
            sessionServiceMock = new Mock<ISessionService>();
            accountAccessService = new AccountAccessService(adminRepositoryMock.Object, appSettingMock.Object, redisServiceMock.Object, sessionServiceMock.Object);
        }

        [TestMethod]
        public void 登入SA_成功_回傳管理者資料()
        {
            // Arrange
            string account = "PB7m9JsNaRK0";
            string pwd = "wf9MqJAPT0wG168MeVbR";
            RequestLoginDto loginDto = new RequestLoginDto() { Account = account, Pwd = pwd };

            AdminIdentity identity = AdminIdentity.SuperAdmin;
            Admin admin = new Admin() { AdminId = 1, Identity = (byte)identity, Status = true };
            string sessionId = "qwlep12312ql";
            TimeSpan expiry = TimeSpan.FromHours(12);
            string key = "Admin" + admin.AdminId;

            // Mock
            appSettingMock.Setup(s => s.GetSuperAdminAccount()).Returns("PB7m9JsNaRK0");
            appSettingMock.Setup(s => s.GetSuperAdminHash()).Returns("5a4ca2d4-2f0b-42cd-9bc9-e91a59a4c897472b2d0896a0b59331c1e4b21306f62c046bcfc1eb60ae9603239b4aeb94bc8c");

            sessionServiceMock.Setup(s => s.SaveSession("AdminSession", new AdminSession() { AdminId = admin.AdminId, Identity = identity }));
            sessionServiceMock.Setup(s => s.GetSessionId()).Returns(sessionId);

            AdminRedis adminRedis = new AdminRedis() { SessionId = sessionId, ErrorCode = ErrorCodeDefine.Success };

            redisServiceMock.Setup(s => s.SetValue(key, adminRedis, expiry));

            // ACT
            (bool success, Admin returnAdmin) = accountAccessService.AdminLogin(loginDto);
            Assert.AreEqual(success, true);
            Assert.AreEqual(returnAdmin.AdminId, 1);
            Assert.AreEqual(returnAdmin.Identity, (byte)AdminIdentity.SuperAdmin);
            Assert.AreEqual(returnAdmin.Status, true);
        }

        [TestMethod]
        public void 登入SA_失敗_回傳失敗()
        {
            // Arrange
            string account = "PB7m9JsNaRK0";
            string pwd = "dasdl78gsd9dd48";
            RequestLoginDto loginDto = new RequestLoginDto() { Account = account, Pwd = pwd };

            // Mock
            appSettingMock.Setup(s => s.GetSuperAdminAccount()).Returns("PB7m9JsNaRK0");
            appSettingMock.Setup(s => s.GetSuperAdminHash()).Returns("5a4ca2d4-2f0b-42cd-9bc9-e91a59a4c897472b2d0896a0b59331c1e4b21306f62c046bcfc1eb60ae9603239b4aeb94bc8c");

            // ACT
            (bool success, Admin admin) = accountAccessService.AdminLogin(loginDto);
            Assert.AreEqual(success, false);
            Assert.AreEqual(admin, null);
        }


        [TestMethod]
        public void 修改密碼_成功_回傳成功()
        {
            // Arrange
            RequestEditSelfPwdDto editSelfPwdDto = new RequestEditSelfPwdDto()
            {
                OldPwd = "12qwekw23",
                NewPwd = "qowkep9999"
            };
            AdminSession adminSession = new AdminSession()
            {
                AdminId = 2,
                Identity = AdminIdentity.Admin
            };
            EditPwdDto editPwdDto = new EditPwdDto()
            {
                AdminId = adminSession.AdminId,
                OldPwd = editSelfPwdDto.OldPwd,
                NewPwd = editSelfPwdDto.NewPwd
            };
            string adminSessionKey = "AdminSession";

            // Mock 設定
            adminRepositoryMock.Setup(s => s.EditSelfPwd(It.Is<EditPwdDto>(dto =>
            dto.AdminId == 2 &&
            dto.OldPwd == "12qwekw23" &&
            dto.NewPwd == "qowkep9999"
            ))).Returns(true);
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(adminSessionKey)).Returns(adminSession);

            // Act
            ErrorCodeDefine result = accountAccessService.EditSelfPwd(editSelfPwdDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 修改密碼_失敗_回傳修改超級管理員失敗()
        {
            // Arrange
            RequestEditSelfPwdDto editSelfPwdDto = new RequestEditSelfPwdDto()
            {
                OldPwd = "12qwekw23",
                NewPwd = "qowkep9999"
            };
            AdminSession adminSession = new AdminSession()
            {
                AdminId = 1,
                Identity = AdminIdentity.Admin
            };
            string adminSessionKey = "AdminSession";

            // Mock 設定
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(adminSessionKey)).Returns(adminSession);

            // Act
            ErrorCodeDefine result = accountAccessService.EditSelfPwd(editSelfPwdDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.ModifySuperAdminFailed);
        }

        [TestMethod]
        public void 修改密碼_失敗_回傳修改失敗()
        {
            // Arrange
            RequestEditSelfPwdDto editSelfPwdDto = new RequestEditSelfPwdDto()
            {
                OldPwd = "12qwekw23",
                NewPwd = "qowkep9999"
            };
            AdminSession adminSession = new AdminSession()
            {
                AdminId = 2,
                Identity = AdminIdentity.Admin
            };
            EditPwdDto editPwdDto = new EditPwdDto()
            {
                AdminId = adminSession.AdminId,
                OldPwd = editSelfPwdDto.OldPwd,
                NewPwd = editSelfPwdDto.NewPwd
            };
            string adminSessionKey = "AdminSession";

            // Mock 設定
            adminRepositoryMock.Setup(s => s.EditSelfPwd(It.Is<EditPwdDto>(dto =>
            dto.AdminId == 2 &&
            dto.OldPwd == "12qwekw23" &&
            dto.NewPwd == "qowkep9999"
            ))).Returns(false);
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(adminSessionKey)).Returns(adminSession);

            // Act
            ErrorCodeDefine result = accountAccessService.EditSelfPwd(editSelfPwdDto);

            // Assert
            Assert.AreEqual(result, ErrorCodeDefine.ModifiedFailed);
        }
        // TODO: 之後補上 HavePermission 檢查邏輯

        //[TestMethod]
        //public void 登入一般管理者_成功_回傳管理者資料()
        //{
        //    // Arrange
        //    string account = "qwp59felp484";
        //    string pwd = "dasdl78gsd9dd48";
        //    RequestLoginDto loginDto = new RequestLoginDto() { Account = account, Pwd = pwd };

        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void 登入一般管理者_失敗_回傳失敗()
        //{
        //    // Arrange
        //    string account = "qwp59felp484";
        //    string pwd = "ABCDWWG322156";
        //    RequestLoginDto loginDto = new RequestLoginDto() { Account = account, Pwd = pwd };

        //    Assert.Fail();
        //}
    }
}