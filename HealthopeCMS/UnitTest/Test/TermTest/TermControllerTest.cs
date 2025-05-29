using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

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
    }
}
