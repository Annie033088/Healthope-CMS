using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models.Other;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.GroupClassShowcaseTest
{
    [TestClass]
    public class GroupClassShowcaseControllerTest
    {
        private GroupClassShowcaseController groupClassShowcaseController;
        private Mock<IGroupClassShowcaseService> groupClassShowcaseServiceMock;
        private Mock<IMultipartRequestService<RequestAddShowcaseDto>> multipartRequestAddServiceMock;
        private Mock<IMultipartRequestService<RequestEditShowcaseDto>> multipartRequestEditServiceMock;

        [TestInitialize]
        public void Setup()
        {
            groupClassShowcaseServiceMock = new Mock<IGroupClassShowcaseService>();
            multipartRequestAddServiceMock =
                new Mock<IMultipartRequestService<RequestAddShowcaseDto>>();
            multipartRequestEditServiceMock =
                new Mock<IMultipartRequestService<RequestEditShowcaseDto>>();
            groupClassShowcaseController = new GroupClassShowcaseController(
                multipartRequestAddServiceMock.Object, groupClassShowcaseServiceMock.Object,
                multipartRequestEditServiceMock.Object);
        }

        [TestMethod]
        public async Task 新增不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestAddShowcaseDto addShowcaseDto = new RequestAddShowcaseDto()
            {
                Category = 1,
                DetailContent = "",
                Icon = 1,
                Name = "QQ123",
                Sort = 1,
                Summary = ""
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddServiceMock.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddServiceMock.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addShowcaseDto, files));
            groupClassShowcaseServiceMock.Setup(s
                => s.AddShowcase(addShowcaseDto, files.Any() ? files[0] : null)).Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await groupClassShowcaseController.AddShowcase();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 新增_失敗_名稱重複()
        {
            // Arrange
            RequestAddShowcaseDto addShowcaseDto = new RequestAddShowcaseDto()
            {
                Category = 1,
                DetailContent = "",
                Icon = 1,
                Name = "QQ123",
                Sort = 1,
                Summary = ""
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.DuplicateName;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddServiceMock.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddServiceMock.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addShowcaseDto, files));
            groupClassShowcaseServiceMock.Setup(s
                => s.AddShowcase(addShowcaseDto, files.Any() ? files[0] : null)).Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await groupClassShowcaseController.AddShowcase();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DuplicateName));
        }

        [TestMethod]
        public void 取得展示用團課清單_成功_回傳清單()
        {
            // Arrange
            RequestGetShowcaseDto getShowcaseDto = new RequestGetShowcaseDto()
            {
                Category = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "name", // 只允許 name | sort | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 null 或 < 20 位
            };

            ResponseGetShowcaseListDto responseGetShowcaseListDto = new ResponseGetShowcaseListDto()
            {
                ShowcaseList = null,
                TotalPage = 1,
            };


            // Mock 設定
            groupClassShowcaseServiceMock.Setup(s
                => s.GetShowcase(getShowcaseDto)).Returns(responseGetShowcaseListDto);

            // Act
            IHttpActionResult result = groupClassShowcaseController.GetShowcase(getShowcaseDto);

            // Assert
            ResponseIsEqual<ResponseGetShowcaseListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetShowcaseListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseGetShowcaseListDto));
        }

        [TestMethod]
        public void 取得展示用團課清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetShowcaseDto getShowcaseDto = new RequestGetShowcaseDto()
            {
                Category = 20, // 必須被定義在 category enum 裡
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "name", // 只允許 name | sort | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 null 或 < 20 位
            };

            ResponseGetShowcaseListDto responseGetShowcaseListDto = new ResponseGetShowcaseListDto()
            {
                ShowcaseList = null,
                TotalPage = 1,
            };


            // Mock 設定
            groupClassShowcaseServiceMock.Setup(s
                => s.GetShowcase(getShowcaseDto)).Returns(responseGetShowcaseListDto);


            // Act
            IHttpActionResult result = groupClassShowcaseController.GetShowcase(getShowcaseDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 根據Id取得展示用團課詳細資料_成功_回傳展示用團課資料()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 1
            };

            ResponseGetShowcaseDetailDto response = new ResponseGetShowcaseDetailDto()
            {
                Name = "okwopekq122",
                Category = 5,
                DetailContent = "",
                Icon = 2,
                ImageUrl = "",
                Sort = 1,
                Summary = ""
            };

            // Mock 設定
            groupClassShowcaseServiceMock.Setup(s => s.GetShowcaseDetail(showcaseIdDto)).Returns(response);

            // Act
            IHttpActionResult result = groupClassShowcaseController.GetShowcaseDetail(showcaseIdDto);

            // Assert
            ResponseIsEqual<ResponseGetShowcaseDetailDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetShowcaseDetailDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 根據Id取得展示用團課詳細資料_失敗_回傳空資料()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 1
            };

            ResponseGetShowcaseDetailDto response = null;

            // Mock 設定
            groupClassShowcaseServiceMock.Setup(s => s.GetShowcaseDetail(showcaseIdDto)).Returns(response);

            // Act
            IHttpActionResult result = groupClassShowcaseController.GetShowcaseDetail(showcaseIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.GetFailed));
        }

        [TestMethod]
        public void 取得展示用團課修改頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 1
            };

            ResponseGetShowcaseEditDataDto responseDto = new ResponseGetShowcaseEditDataDto()
            {
                Name = "Jack",
                Sort = 1,
                Category = 2,
                DetailContent = "",
                Icon = 3,
                ImageUrl = "",
                Summary = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            groupClassShowcaseServiceMock.Setup(s => s.GetShowcaseEditDataById(showcaseIdDto)).Returns(responseDto);

            // Act
            IHttpActionResult result = groupClassShowcaseController.GetShowcaseEditDataById(showcaseIdDto);

            // Assert
            ResponseIsEqual<ResponseGetShowcaseEditDataDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetShowcaseEditDataDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseDto));
        }

        [TestMethod]
        public void 取得展示用團課修改頁面需要的資料_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 0
            };

            // Act
            IHttpActionResult result = groupClassShowcaseController.GetShowcaseEditDataById(showcaseIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 修改展示用團課不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestEditShowcaseDto editShowcaseDto = new RequestEditShowcaseDto()
            {
                GroupClassShowcaseId = 1,
                Category = 1,
                DetailContent = "",
                Icon = 1,
                ImageUrl = "",
                Sort = 3,
                Summary = "",
                Name = "蘑菇",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editShowcaseDto, files));
            groupClassShowcaseServiceMock.Setup(s => s.EditShowcase(editShowcaseDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await groupClassShowcaseController.EditShowcase();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 修改_失敗_名稱重複()
        {
            // Arrange
            RequestEditShowcaseDto editShowcaseDto = new RequestEditShowcaseDto()
            {
                GroupClassShowcaseId = 1,
                Category = 1,
                DetailContent = "",
                Icon = 1,
                ImageUrl = "",
                Sort = 3,
                Summary = "",
                Name = "蘑菇",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.DuplicateName;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editShowcaseDto, files));
            groupClassShowcaseServiceMock.Setup(s => s.EditShowcase(editShowcaseDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await groupClassShowcaseController.EditShowcase();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DuplicateName));
        }

        [TestMethod]
        public async Task 修改_失敗_格式錯誤()
        {
            // Arrange
            RequestEditShowcaseDto editShowcaseDto = new RequestEditShowcaseDto()
            {
                GroupClassShowcaseId = 1,
                Category = 20,
                DetailContent = "",
                Icon = 1,
                ImageUrl = "",
                Sort = 3,
                Summary = "",
                Name = "蘑菇",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(success);
            multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editShowcaseDto, files));
            groupClassShowcaseServiceMock.Setup(s => s.EditShowcase(editShowcaseDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await groupClassShowcaseController.EditShowcase();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 刪除_成功_回傳成功()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 10,
            };

            bool successFlag = true;

            // Mock 設定
            groupClassShowcaseServiceMock.Setup(s => s.DeleteShowcase(showcaseIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = groupClassShowcaseController.DeleteShowcase(showcaseIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 刪除_失敗_回傳失敗()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 10,
            };

            bool successFlag = false;

            // Mock 設定
            groupClassShowcaseServiceMock.Setup(s => s.DeleteShowcase(showcaseIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = groupClassShowcaseController.DeleteShowcase(showcaseIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DeleteFailed));
        }
    }
}
