using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestAccountAccessDto;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTest.utils;

namespace UnitTest.Test.AccountAccessTest
{
    [TestClass]
    public class AccountAccessControllerTest
    {
        private AccountAccessController controller;
        private Mock<IAccountAccessService> accountAccessServiceMock;

        [TestInitialize]
        public void Setup()
        {
            accountAccessServiceMock = new Mock<IAccountAccessService>();
            controller = new AccountAccessController(accountAccessServiceMock.Object);
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
            accountAccessServiceMock.Setup(s => s.AdminLogin(loginDto))
                .Returns((success, admin));

            // Act
            IHttpActionResult result = controller.VerifyAdminLogin(loginDto);

            ResponseIsEqual<List<AdminPermission>> errorCodeIsEqual = new ResponseIsEqual<List<AdminPermission>>();
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
            accountAccessServiceMock.Setup(s => s.AdminLogin(loginDto))
                .Returns((success, admin));

            // Act
            IHttpActionResult result = controller.VerifyAdminLogin(loginDto);

            ResponseIsEqual errorCodeIsEqual = new ResponseIsEqual();
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
            accountAccessServiceMock.Setup(s => s.AdminLogin(loginDto))
                .Returns((success, admin));

            // Act
            IHttpActionResult result = controller.VerifyAdminLogin(loginDto);

            ResponseIsEqual errorCodeIsEqual = new ResponseIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Baned)) return;

            // Assert
            Assert.Fail("測試出錯");
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

            // Mock 設定
            ErrorCodeDefine errorCodeDefine = ErrorCodeDefine.Success;

            accountAccessServiceMock.Setup(s => s.EditSelfPwd(editSelfPwdDto))
                .Returns(errorCodeDefine);

            // Act
            IHttpActionResult result = controller.EditSelfPwd(editSelfPwdDto);

            ResponseIsEqual errorCodeIsEqual = new ResponseIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success)) return;

            // Assert
            Assert.Fail("測試出錯");
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

            // Mock 設定
            ErrorCodeDefine errorCodeDefine = ErrorCodeDefine.ModifySuperAdminFailed;

            accountAccessServiceMock.Setup(s => s.EditSelfPwd(editSelfPwdDto))
                .Returns(errorCodeDefine);

            // Act
            IHttpActionResult result = controller.EditSelfPwd(editSelfPwdDto);

            ResponseIsEqual errorCodeIsEqual = new ResponseIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.ModifySuperAdminFailed)) return;

            // Assert
            Assert.Fail("測試出錯");
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

            // Mock 設定
            ErrorCodeDefine errorCodeDefine = ErrorCodeDefine.ModifiedFailed;

            accountAccessServiceMock.Setup(s => s.EditSelfPwd(editSelfPwdDto))
                .Returns(errorCodeDefine);

            // Act
            IHttpActionResult result = controller.EditSelfPwd(editSelfPwdDto);

            ResponseIsEqual errorCodeIsEqual = new ResponseIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.ModifiedFailed)) return;

            // Assert
            Assert.Fail("測試出錯");
        }
    }
}
