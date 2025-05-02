using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Admin.ResponseAdminDto;
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

        [TestMethod]
        public void 取得管理員列表_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetAdminDto getAdminDto = new RequestGetAdminDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SearchAccount = "1", // 只允許 null 或 長度>2的字串
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "account", // 只允許 account 或 name 或 null
                RecordPerPage = 8 // 只允許 8 或 12 或 16
            };

            // Mock 設定

            // Act
            IHttpActionResult result = adminController.GetAdmin(getAdminDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.InvalidFormatOrEntry) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 取得管理員清單_成功_回傳管理員清單()
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

            ResponseGetAdminListDto responseGetAdminDto = new ResponseGetAdminListDto()
            {
                AdminList = null,
                TotalPage = 1,
            };


            // Mock 設定
            adminServiceMock.Setup(s => s.GetAdmin(getAdminDto)).Returns(responseGetAdminDto);

            // Act
            IHttpActionResult result = adminController.GetAdmin(getAdminDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.Success) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
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


            // Mock 設定
            adminServiceMock.Setup(s => s.GetAdminById(getAdminIdDto)).Returns(responseGetAdminDto);

            // Act
            IHttpActionResult result = adminController.GetAdminById(getAdminIdDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.Success) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 根據Id取得管理員_失敗_回傳失敗()
        {
            // Arrange
            RequestAdminIdDto getAdminIdDto = new RequestAdminIdDto()
            {
                AdminId = 1
            };

            ResponseGetAdminDto responseGetAdminDto = null; // null

            // Mock 設定
            adminServiceMock.Setup(s => s.GetAdminById(getAdminIdDto)).Returns(responseGetAdminDto);

            // Act
            IHttpActionResult result = adminController.GetAdminById(getAdminIdDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.GetFailed) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
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

            bool successFlag = true;

            // Mock 設定
            adminServiceMock.Setup(s => s.EditAdmin(editAdminDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = adminController.EditAdmin(editAdminDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.Success) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 修改管理員_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditAdminDto editAdminDto = new RequestEditAdminDto()
            {
                AdminId = 10,
                Identity = 200, // 沒有此身份 (無法轉換成功)
                Status = null,
                UpdateTime = DateTime.Now,
            };

            bool successFlag = false;

            // Mock 設定
            adminServiceMock.Setup(s => s.EditAdmin(editAdminDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = adminController.EditAdmin(editAdminDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.InvalidFormatOrEntry) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
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
            adminServiceMock.Setup(s => s.EditAdmin(editAdminDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = adminController.EditAdmin(editAdminDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.HasBeenModified) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
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
            adminServiceMock.Setup(s => s.DeleteAdmin(adminIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = adminController.DeleteAdmin(adminIdDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.Success) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
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
            adminServiceMock.Setup(s => s.DeleteAdmin(adminIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = adminController.DeleteAdmin(adminIdDto);

            if (result is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    if (resultWithoutData.ErrorCode == ErrorCodeDefine.DeleteFailed) return;
                }
            }

            // Assert
            Assert.Fail("測試出錯");
        }
    }
}
