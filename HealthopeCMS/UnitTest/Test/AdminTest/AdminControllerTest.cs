using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin.RequestAdminDto;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoListTest.utils;

namespace UnitTest.Test.AdminTest
{
    [TestClass]
    public class AdminControllerTest
    {
        private AdminController adminController;
        private Mock<IAdminService> adminServiceMock;

        [TestInitialize]
        public void Setup()
        {
            adminServiceMock = new Mock<IAdminService>();
            adminController = new AdminController(adminServiceMock.Object);
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
            bool success = true;
            adminServiceMock.Setup(s => s.AddAdmin(addAdminDto)).Returns(success);

            // Act
            IHttpActionResult result = adminController.AddAdmin(addAdminDto);

            ResponseErrorCodeIsEqual errorCodeIsEqual = new ResponseErrorCodeIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success)) return;

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 新增_失敗_管理者帳號重複()
        {
            // Arrange
            RequestAddAdminDto addAdminDto = new RequestAddAdminDto()
            {
                Account = "PB7m9JsNaRK0",
                Pwd = "g4556fgerger",
                Identity = "CoachManager"
            };

            // Mock 設定
            bool success = false;
            adminServiceMock.Setup(s => s.AddAdmin(addAdminDto)).Returns(success);

            // Act
            IHttpActionResult result = adminController.AddAdmin(addAdminDto);

            ResponseErrorCodeIsEqual errorCodeIsEqual = new ResponseErrorCodeIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.CreateFailed)) return;

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 取得權限_成功_回傳成功跟權限()
        {
            // Arrange
            List<AdminPermission> adminPermissions = new List<AdminPermission>() { AdminPermission.EditAdmin, AdminPermission.EditMember };

            // Mock 設定
            adminServiceMock.Setup(s => s.GetPermission()).Returns(adminPermissions);

            // Act
            IHttpActionResult result = adminController.GetPermission();

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse<List<AdminPermission>> resultWithoutData)
                {
                    if ((resultWithoutData.ErrorCode == ErrorCodeDefine.Success) & (adminPermissions.SequenceEqual(resultWithoutData.ApiDataObject))) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 取得權限_失敗_回傳無權限()
        {
            // Arrange
            List<AdminPermission> adminPermissions = null;

            // Mock 設定
            adminServiceMock.Setup(s => s.GetPermission()).Returns(adminPermissions);

            // Act
            IHttpActionResult result = adminController.GetPermission();

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.NoPermission) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
        }
    }
}
