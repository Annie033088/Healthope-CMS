using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Controllers.api;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using UnitTest.utils;
using ApiLayer.Interface;
using Moq;
using ApiLayer.Service;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Other;
using System.Net.Http;
using System.IO;
using ApiLayer.Models.Member.Response;
using PersistentLayer.Models;
using ApiLayer.Models.Coach.Response;

namespace UnitTest.Test.CoachTest
{
    [TestClass]
    public class CoachControllerTest
    {
        private CoachController coachController;
        private Mock<ICoachService> coachServiceMock;
        private Mock<IMultipartRequestService<RequestAddCoachDto>> multipartRequestServiceMock;

        [TestInitialize]
        public void Setup()
        {
            coachServiceMock = new Mock<ICoachService>();
            multipartRequestServiceMock = new Mock<IMultipartRequestService<RequestAddCoachDto>>();
            coachController = new CoachController(multipartRequestServiceMock.Object, coachServiceMock.Object);
        }

        [TestMethod]
        public async Task 新增不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestAddCoachDto addCoachDto = new RequestAddCoachDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                Type = 1,
                ContractEndTime = null,
                ContractStartTime = null,
            };
            HttpRequestMessage request = new HttpRequestMessage();
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            Exception exception = null;

            // Mock 設定
            bool success = true;
            multipartRequestServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addCoachDto, null));
            coachServiceMock.Setup(s => s.AddCoach(addCoachDto, null)).Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await coachController.AddCoach();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 新增_失敗_帳號重複()
        {
            // Arrange
            RequestAddCoachDto addCoachDto = new RequestAddCoachDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                Type = 1,
                ContractEndTime = null,
                ContractStartTime = null,
            };
            HttpRequestMessage request = new HttpRequestMessage();
            ErrorCodeDefine errorCode = ErrorCodeDefine.DuplicateAccount;
            Exception exception = null;

            // Mock 設定
            bool success = true;
            multipartRequestServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addCoachDto, null));
            coachServiceMock.Setup(s => s.AddCoach(addCoachDto, null)).Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await coachController.AddCoach();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DuplicateAccount));
        }

        [TestMethod]
        public async Task 新增_失敗_圖片格式錯誤()
        {
            // Arrange
            RequestAddCoachDto addCoachDto = new RequestAddCoachDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                Type = 1,
                ContractEndTime = null,
                ContractStartTime = null,
            };
            HttpRequestMessage request = new HttpRequestMessage();
            List<FileDto> files = new List<FileDto>() {
                new FileDto(){
                    FileData = File.ReadAllBytes("C:\\Users\\User\\Pictures\\改開頭btye測試png.png"),
                    MimeType = "image/png",
                }
            };

            // Mock 設定
            bool success = true;
            multipartRequestServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addCoachDto, files));

            // Act
            IHttpActionResult result = await coachController.AddCoach();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得教練清單_成功_回傳教練清單()
        {
            // Arrange
            RequestGetCoachDto getCoachDto = new RequestGetCoachDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "contractEndTime", // 只允許 contractEndTime | name | status | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 null 或 < 15 位
                SearchPhone = null, // 只允許 null 或是 3 位數字
            };

            ResponseGetCoachListDto responseGetCoachDto = new ResponseGetCoachListDto()
            {
                CoachList = null,
                TotalPage = 1,
            };


            // Mock 設定
            coachServiceMock.Setup(s => s.GetCoach(getCoachDto)).Returns(responseGetCoachDto);

            // Act
            IHttpActionResult result = coachController.GetCoach(getCoachDto);

            // Assert
            ResponseIsEqual<ResponseGetCoachListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetCoachListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseGetCoachDto));
        }

        [TestMethod]
        public void 取得會員清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetCoachDto getCoachDto = new RequestGetCoachDto()
            {
                Status = null,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "name", // 只允許 contractEndTime | name | status | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
                SearchName = null, // 只允許 null 或 < 15 位
                SearchPhone = "null", // 只允許 null 或是 3 位數字
            };

            ResponseGetCoachListDto responseGetCoachDto = new ResponseGetCoachListDto()
            {
                CoachList = null,
                TotalPage = 1,
            };

            // Mock 設定
            coachServiceMock.Setup(s => s.GetCoach(getCoachDto)).Returns(responseGetCoachDto);

            // Act
            IHttpActionResult result = coachController.GetCoach(getCoachDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
