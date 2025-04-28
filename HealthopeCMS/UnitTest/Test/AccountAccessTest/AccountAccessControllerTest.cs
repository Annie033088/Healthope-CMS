using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestAccountAccessDto;
using DomainLayer.Models;
using DomainLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoListTest.utils;

namespace UnitTest.Test.AccountAccessTest
{
    [TestClass]
    public class AccountAccessControllerTest
    {
        private readonly AdminPermissionUtility adminPermissionUtility = new AdminPermissionUtility();
        private AccountAccessController controller;
        private Mock<IAccountAccessService> loginServiceMock;

        [TestInitialize]
        public void Setup()
        {
            loginServiceMock = new Mock<IAccountAccessService>();
            controller = new AccountAccessController(loginServiceMock.Object);
        }

        [TestMethod]
        public void 登入_成功_帳密正確()
        {
            // Arrange
            string account = "PB7m9JsNaRK0";
            string pwd = "wf9MqJAPT0wG168MeVbR";
            RequestLoginDto loginDto = new RequestLoginDto() { Account = account, Pwd = pwd };

            // Mock 設定
            bool success = true;
            Admin admin = new Admin
            {
                AdminId = 1,
                Identity = 1,
                Status = true
            };
            loginServiceMock.Setup(s => s.AdminLogin(loginDto))
                .Returns((success, admin));

            // Act
            IHttpActionResult result = controller.VerifyAdminLogin(loginDto);

            ResponseErrorCodeIsEqual<List<AdminPermission>> errorCodeIsEqual = new ResponseErrorCodeIsEqual<List<AdminPermission>>();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success)) return;

            Assert.Fail();
        }

        [TestMethod]
        public void 登入_失敗_帳密錯誤()
        {
            // Arrange
            string account = "eqweqw123";
            string pwd = "g4556fgerger";
            RequestLoginDto loginDto = new RequestLoginDto { Account = account, Pwd = pwd };

            // Mock 設定
            bool success = false;
            Admin admin = new Admin();
            loginServiceMock.Setup(s => s.AdminLogin(loginDto))
                .Returns((success, admin));

            // Act
            IHttpActionResult result = controller.VerifyAdminLogin(loginDto);

            ResponseErrorCodeIsEqual errorCodeIsEqual = new ResponseErrorCodeIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.LoginFailed)) return;

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 登入_失敗_狀態禁用()
        {
            // Arrange
            string account = "eqweqw123";
            string pwd = "4556fgerger";
            RequestLoginDto loginDto = new RequestLoginDto { Account = account, Pwd = pwd };

            // Mock 設定
            bool success = true;
            Admin admin = new Admin()
            {
                AdminId = 10,
                Identity = 5,
                Status = false
            };
            loginServiceMock.Setup(s => s.AdminLogin(loginDto))
                .Returns((success, admin));

            // Act
            IHttpActionResult result = controller.VerifyAdminLogin(loginDto);

            ResponseErrorCodeIsEqual errorCodeIsEqual = new ResponseErrorCodeIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Baned)) return;

            // Assert
            Assert.Fail("測試出錯");
        }
    }
}
