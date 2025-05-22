using System;
using System.Collections.Generic;
using System.Linq;
using ApiLayer.Controllers.api;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models.Other;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using UnitTest.utils;
using ApiLayer.Models.Admin.RequestAdminDto;

namespace UnitTest.Test.GroupClassShowcaseTest
{
    [TestClass]
    public class GroupClassShowcaseServiceTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IFileService> fileServiceMock;
        private Mock<IHttpService> httpServiceMock;
        private Mock<IGroupClassShowcaseRepository> groupClassShowcaseRepositoryMock;
        private GroupClassShowcaseService groupClassShowcaseService;

        public GroupClassShowcaseServiceTest()
        {
            mapperMock = new Mock<IMapper>();
            fileServiceMock = new Mock<IFileService>();
            groupClassShowcaseRepositoryMock = new Mock<IGroupClassShowcaseRepository>();
            httpServiceMock = new Mock<IHttpService>();
            groupClassShowcaseService = new GroupClassShowcaseService(mapperMock.Object,
                fileServiceMock.Object, groupClassShowcaseRepositoryMock.Object, httpServiceMock.Object);
        }

        [TestMethod]
        public void 新增不包括圖檔_成功_回傳成功()
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
            GroupClassShowcase groupClassShowcase = new GroupClassShowcase()
            {
                Category = 1,
                DetailContent = "",
                Icon = 1,
                Name = "QQ123",
                Sort = 1,
                Summary = ""
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.AddShowcase(groupClassShowcase)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<GroupClassShowcase>(addShowcaseDto)).Returns(groupClassShowcase);

            // Act
            (ErrorCodeDefine errorCode, Exception exception)
                = groupClassShowcaseService.AddShowcase(addShowcaseDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.Success);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void 新增_失敗_名稱重複()
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
            GroupClassShowcase groupClassShowcase = new GroupClassShowcase()
            {
                Category = 1,
                DetailContent = "",
                Icon = 1,
                Name = "QQ123",
                Sort = 1,
                Summary = ""
            };

            int errorCodeNumber = (int)ErrorCodeDefine.DuplicateName;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.AddShowcase(groupClassShowcase)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<GroupClassShowcase>(addShowcaseDto)).Returns(groupClassShowcase);

            // Act
            (ErrorCodeDefine errorCode, Exception exception)
                = groupClassShowcaseService.AddShowcase(addShowcaseDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.DuplicateName);
            Assert.IsTrue(exception == null);
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

            List<GroupClassShowcase> showcases = new List<GroupClassShowcase>()
            {
                new GroupClassShowcase()
                {
                    GroupClassShowcaseId = 1,
                    Category=1,
                    Summary="",
                    Sort=1,
                    Icon=1,
                    Name="dd",
                }
            };
            int totalPage = 1;
            List<ResponseGetShowcaseDto> responseGetShowcaseDto = new List<ResponseGetShowcaseDto>()
             {
                 new ResponseGetShowcaseDto()
                 {
                    GroupClassShowcaseId = 1,
                    Category=1,
                    Summary="",
                    Sort=1,
                    Icon=1,
                    Name="dd",
                 }
             };

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.GetShowcase(getShowcaseDto)).Returns((showcases, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetShowcaseDto>>(showcases)).Returns(responseGetShowcaseDto);

            // Act
            ResponseGetShowcaseListDto response = groupClassShowcaseService.GetShowcase(getShowcaseDto);

            // Assert
            Assert.IsTrue(response.ShowcaseList.SequenceEqual(responseGetShowcaseDto));
        }

        [TestMethod]
        public void 取得展示用團課清單_失敗_請求參數格式錯誤()
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

            List<GroupClassShowcase> showcases = new List<GroupClassShowcase>();
            int totalPage = 1;
            List<ResponseGetShowcaseDto> responseGetShowcaseDto = new List<ResponseGetShowcaseDto>();

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.GetShowcase(getShowcaseDto)).Returns((showcases, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetShowcaseDto>>(showcases)).Returns(responseGetShowcaseDto);

            // Act
            ResponseGetShowcaseListDto response = groupClassShowcaseService.GetShowcase(getShowcaseDto);

            // Assert
            Assert.IsTrue(response.ShowcaseList.SequenceEqual(responseGetShowcaseDto));
        }

        [TestMethod]
        public void 根據Id取得展示用團課詳細資料_成功_回傳展示用團課資料()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 1
            };

            GroupClassShowcase groupClassShowcase = new GroupClassShowcase()
            {
                Name = "okwopekq122",
                Category = 5,
                DetailContent = "",
                Icon = 2,
                ImageUrl = "",
                Sort = 1,
                Summary = ""
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
            groupClassShowcaseRepositoryMock.Setup(s => s.GetShowcaseDetail(showcaseIdDto.GroupClassShowcaseId))
                .Returns(groupClassShowcase);
            mapperMock.Setup(s => s.Map<ResponseGetShowcaseDetailDto>(groupClassShowcase)).Returns(response);

            // Act
            ResponseGetShowcaseDetailDto result = groupClassShowcaseService.GetShowcaseDetail(showcaseIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 根據Id取得展示用團課詳細資料_失敗_回傳空資料()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 1
            };

            GroupClassShowcase groupClassShowcase = null;
            ResponseGetShowcaseDetailDto response = null;

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.GetShowcaseDetail(showcaseIdDto.GroupClassShowcaseId))
                .Returns(groupClassShowcase);
            mapperMock.Setup(s => s.Map<ResponseGetShowcaseDetailDto>(groupClassShowcase)).Returns(response);

            // Act
            ResponseGetShowcaseDetailDto result = groupClassShowcaseService.GetShowcaseDetail(showcaseIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 取得展示用團課修改頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 1
            };
            DateTime dateTime = DateTime.Now;
            GroupClassShowcase showcase = new GroupClassShowcase()
            {
                Name = "Jack",
                Sort = 1,
                Category = 2,
                DetailContent = "",
                Icon = 3,
                ImageUrl = "",
                Summary = "",
                UpdateTime = dateTime
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
                UpdateTime = dateTime
            };

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.GetShowcaseEditDataById(showcaseIdDto.GroupClassShowcaseId))
                .Returns(showcase);
            mapperMock.Setup(s => s.Map<ResponseGetShowcaseEditDataDto>(showcase)).Returns(responseDto);

            // Act
            ResponseGetShowcaseEditDataDto response = groupClassShowcaseService.GetShowcaseEditDataById(showcaseIdDto);
            responseDto.ImageUrl = "/" + responseDto.ImageUrl;

            // Assert
            Assert.IsTrue(response == responseDto);
        }

        [TestMethod]
        public void 取得展示用團課修改頁面需要的資料_失敗_回傳空資料()
        {
            // Arrange
            RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
            {
                GroupClassShowcaseId = 1
            };
            DateTime dateTime = DateTime.Now;
            GroupClassShowcase showcase = null;

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.GetShowcaseEditDataById(showcaseIdDto.GroupClassShowcaseId))
                .Returns(showcase);

            // Act
            ResponseGetShowcaseEditDataDto response = groupClassShowcaseService.GetShowcaseEditDataById(showcaseIdDto);

            // Assert
            Assert.IsTrue(response == null);
        }

        [TestMethod]
        public void  修改展示用團課不包括圖檔_成功_回傳成功()
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

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.EditShowcase(editShowcaseDto))
                .Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s => s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = groupClassShowcaseService.EditShowcase(editShowcaseDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.Success);
            Assert.IsTrue(exception == null);
        }

        [TestMethod]
        public void  修改_失敗_名稱重複()
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

            int errorCodeNumber = (int)ErrorCodeDefine.DuplicateName;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.EditShowcase(editShowcaseDto))
                .Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s => s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = groupClassShowcaseService.EditShowcase(editShowcaseDto, null);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.DuplicateName);
            Assert.IsTrue(exception == null);
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
            string oldImageUrl = "";

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.DeleteShowcase(showcaseIdDto.GroupClassShowcaseId))
                .Returns((successFlag, oldImageUrl));

            // Act
            bool result = groupClassShowcaseService.DeleteShowcase(showcaseIdDto);

            // Assert
            Assert.IsTrue(result);
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
            string oldImageUrl = "";

            // Mock 設定
            groupClassShowcaseRepositoryMock.Setup(s => s.DeleteShowcase(showcaseIdDto.GroupClassShowcaseId))
                .Returns((successFlag, oldImageUrl));

            // Act
            bool result = groupClassShowcaseService.DeleteShowcase(showcaseIdDto);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
