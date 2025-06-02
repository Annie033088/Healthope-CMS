using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member.Response;
using ApiLayer.Models.Member;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;
using ApiLayer.Models.Admin.RequestAdminDto;

namespace UnitTest.Test.TermTest
{
    [TestClass]
    public class TermControllerTest
    {
        private TermController controller;
        private Mock<ITermService> termServiceMock;

        [TestInitialize]
        public void Setup()
        {
            termServiceMock = new Mock<ITermService>();
            controller = new TermController(termServiceMock.Object);
        }

        [TestMethod]
        public void 取得舊條款清單_成功_回傳清單()
        {
            // Arrange
            RequestGetOldTermDto getOldTerm = new RequestGetOldTermDto()
            {
                ApplicableTarget = 1,
                Type = 2,
            };

            List<ResponseGetOldTermDto> response = new List<ResponseGetOldTermDto>()
            {
                new ResponseGetOldTermDto
                {
                    TermId = 1,
                    DetailContent="qwe",
                    Name="-",
                    Version=1,
                }
            };

            // Mock 設定
            termServiceMock.Setup(s
                => s.GetOldTerm(getOldTerm)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetOldTerm(getOldTerm);

            // Assert
            ResponseIsEqual<List<ResponseGetOldTermDto>> responseIsEqual =
                new ResponseIsEqual<List<ResponseGetOldTermDto>>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得舊條款清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetOldTermDto getOldTerm = new RequestGetOldTermDto()
            {
                ApplicableTarget = 0,
                Type = 2,
            };
            // Act
            IHttpActionResult result = controller.GetOldTerm(getOldTerm);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 新增_成功_回傳成功()
        {
            // Arrange
            RequestAddTermDto addTermDto = new RequestAddTermDto()
            {
                ApplicableTarget = 1,
                DetailContent = "w",
                Type = 2,
                VersionDescription = "2426"
            };

            bool successFlag = true;

            // Mock 設定
            termServiceMock.Setup(s
                => s.AddTerm(addTermDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.AddTerm(addTermDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 新增_失敗_回傳失敗()
        {
            // Arrange
            RequestAddTermDto addTermDto = new RequestAddTermDto()
            {
                ApplicableTarget = 1,
                DetailContent = "w",
                Type = 2,
                VersionDescription = "2426"
            };

            bool successFlag = false;

            // Mock 設定
            termServiceMock.Setup(s
                => s.AddTerm(addTermDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.AddTerm(addTermDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.CreateFailed));
        }

        [TestMethod]
        public void 取得條款清單_成功_回傳清單()
        {
            // Arrange
            RequestGetTermDto getTermDto = new RequestGetTermDto()
            {
                ApplicableTarget = 1,
                Type = 2,
                Status = 3,
                Page = 1,
                RecordPerPage = 8
            };

            ResponseGetTermListDto response = new ResponseGetTermListDto();

            // Mock 設定
            termServiceMock.Setup(s
                => s.GetTerm(getTermDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetTerm(getTermDto);

            // Assert
            ResponseIsEqual<ResponseGetTermListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetTermListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得條款清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetTermDto getTermDto = new RequestGetTermDto()
            {
                ApplicableTarget = 1,
                Type = 2,
                Status = 20,
                Page = 1,
                RecordPerPage = 8
            };

            // Act
            IHttpActionResult result = controller.GetTerm(getTermDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 根據Id取得要修改條款的資料_成功_回傳條款資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            ResponseGetTermEditDataByIdDto response = new ResponseGetTermEditDataByIdDto()
            {
                Name = "okwopekq122",
                DetailContent = "dwdq",
                VersionDescription = "000blob",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            termServiceMock.Setup(s => s.GetTermEditDataById(termIdDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetTermEditDataById(termIdDto);

            // Assert
            ResponseIsEqual<ResponseGetTermEditDataByIdDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetTermEditDataByIdDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 根據Id取得要修改條款的資料_失敗_回傳空資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            ResponseGetTermEditDataByIdDto response = null;

            // Mock 設定
            termServiceMock.Setup(s => s.GetTermEditDataById(termIdDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetTermEditDataById(termIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.GetFailed));
        }

        [TestMethod]
        public void 修改條款_成功_回傳成功()
        {
            // Arrange
            RequestEditTermDto editTermDto = new RequestEditTermDto()
            {
                TermId = 1,
                DetailContent = null,
                VersionDescription = "qwe",
                UpdateTime = DateTime.Now,
            };

            ErrorCodeDefine errorCdoe = ErrorCodeDefine.Success;

            // Mock 設定
            termServiceMock.Setup(s => s.EditTerm(editTermDto)).Returns(errorCdoe);

            // Act
            IHttpActionResult result = controller.EditTerm(editTermDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改條款_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditTermDto editTermDto = new RequestEditTermDto()
            {
                TermId = 1,
                DetailContent = null,
                VersionDescription = null,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditTerm(editTermDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改條款狀態_成功_回傳成功()
        {
            // Arrange
            RequestEditTermStatusDto editTermStatusDto = new RequestEditTermStatusDto()
            {
                TermId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;

            // Mock 設定
            termServiceMock.Setup(s => s.EditTermStatus(editTermStatusDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = controller.EditTermStatus(editTermStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改條款狀態_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestEditTermStatusDto editTermStatusDto = new RequestEditTermStatusDto()
            {
                TermId = 1,
                Status = 3,
                UpdateTime = DateTime.Now,
            };

            // Act
            IHttpActionResult result = controller.EditTermStatus(editTermStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得條款細項資料_成功_回傳條款資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            ResponseGetTermDetailDto response = new ResponseGetTermDetailDto()
            {
                Name = "okwopekq122",
                DetailContent = "dwdq",
                VersionDescription = "000blob",
                Version = 2,
            };

            // Mock 設定
            termServiceMock.Setup(s => s.GetTermDetail(termIdDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetTermDetail(termIdDto);

            // Assert
            ResponseIsEqual<ResponseGetTermDetailDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetTermDetailDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得條款細項資料_失敗_回傳空資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            ResponseGetTermDetailDto response = null;

            // Mock 設定
            termServiceMock.Setup(s => s.GetTermDetail(termIdDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetTermDetail(termIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.GetFailed));
        }

        [TestMethod]
        public void 刪圖條款_成功_回傳成功()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 10,
            };

            bool successFlag = true;

            // Mock 設定
            termServiceMock.Setup(s => s.DeleteTerm(termIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.DeleteTerm(termIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 刪除條款_失敗_回傳失敗()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1000,
            };

            bool successFlag = false;

            // Mock 設定
            termServiceMock.Setup(s => s.DeleteTerm(termIdDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.DeleteTerm(termIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DeleteFailed));
        }
    }
}
