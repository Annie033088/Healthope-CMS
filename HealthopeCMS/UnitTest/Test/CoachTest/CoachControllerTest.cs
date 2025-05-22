using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Coach.Response;
using ApiLayer.Models.Other;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.CoachTest
{
    [TestClass]
    public class CoachControllerTest
    {
        private CoachController coachController;
        private Mock<ICoachService> coachServiceMock;
        private Mock<IMultipartRequestService<RequestAddCoachDto>> multipartRequestAddServiceMock;
        private Mock<IMultipartRequestService<RequestEditCoachDto>> multipartRequestEditServiceMock;

        [TestInitialize]
        public void Setup()
        {
            coachServiceMock = new Mock<ICoachService>();
            multipartRequestAddServiceMock =
                new Mock<IMultipartRequestService<RequestAddCoachDto>>();
            multipartRequestEditServiceMock =
                new Mock<IMultipartRequestService<RequestEditCoachDto>>();
            coachController = new CoachController(
                multipartRequestAddServiceMock.Object, coachServiceMock.Object,
                multipartRequestEditServiceMock.Object);
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
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addCoachDto, files));
            coachServiceMock.Setup(s => s.AddCoach(addCoachDto, files.Any() ? files[0] : null)).Returns((errorCode, exception));

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
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addCoachDto, files));
            coachServiceMock.Setup(s => s.AddCoach(addCoachDto, files.Any() ? files[0] : null)).Returns((errorCode, exception));

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
            multipartRequestAddServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addCoachDto, files));

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
        public void 取得教練清單_失敗_請求參數格式錯誤()
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

        [TestMethod]
        public void 取得教練修改頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestCoachIdDto coachIdDto = new RequestCoachIdDto()
            {
                CoachId = 1
            };

            ResponseGetCoachEditDataByIdDto responseDto = new ResponseGetCoachEditDataByIdDto()
            {
                Email = "",
                Certification = "",
                ContractStartTime = DateTime.Now,
                ContractEndTime = DateTime.Now.AddDays(DateTime.DaysInMonth(1, 6)),
                Specialty = "",
                Introduction = "",
                Name = "Jack",
                Status = true,
                Phone = 987896543,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            coachServiceMock.Setup(s => s.GetCoachEditDataById(coachIdDto)).Returns(responseDto);

            // Act
            IHttpActionResult result = coachController.GetCoachEditDataById(coachIdDto);

            // Assert
            ResponseIsEqual<ResponseGetCoachEditDataByIdDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetCoachEditDataByIdDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseDto));
        }

        [TestMethod]
        public void 取得教練修改頁面需要的資料_失敗_請求參數格是錯誤()
        {
            // Arrange
            RequestCoachIdDto coachIdDto = new RequestCoachIdDto()
            {
                CoachId = 0
            };

            ResponseGetCoachEditDataByIdDto responseDto = new ResponseGetCoachEditDataByIdDto()
            {
                Email = "",
                Certification = "",
                ContractStartTime = DateTime.Now,
                ContractEndTime = DateTime.Now.AddDays(DateTime.DaysInMonth(1, 6)),
                Specialty = "",
                Introduction = "",
                Name = "Jack",
                Status = true,
                Phone = 987896543,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            coachServiceMock.Setup(s => s.GetCoachEditDataById(coachIdDto)).Returns(responseDto);

            // Act
            IHttpActionResult result = coachController.GetCoachEditDataById(coachIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 修改教練不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestEditCoachDto editCoachDto = new RequestEditCoachDto()
            {
                CoachId = 1,
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                ContractEndTime = null,
                ContractStartTime = null,
                Status = true,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((editCoachDto, files));
            coachServiceMock.Setup(s => s.EditCoach(editCoachDto, files.Any() ? files[0] : null)).Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await coachController.EditCoach();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 修改_失敗_手機重複()
        {
            // Arrange
            RequestEditCoachDto editCoachDto = new RequestEditCoachDto()
            {
                CoachId = 1,
                Email = "",
                Phone = 987654321,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                ContractEndTime = null,
                ContractStartTime = null,
                Status = true,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.DuplicatePhone;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();


            // Mock 設定
            bool success = true;
            multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((editCoachDto, files));
            coachServiceMock.Setup(s => s.EditCoach(editCoachDto, files.Any() ? files[0] : null)).Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await coachController.EditCoach();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DuplicatePhone));
        }

        [TestMethod]
        public async Task 修改_失敗_格式錯誤()
        {
            // Arrange
            RequestEditCoachDto editCoachDto = new RequestEditCoachDto()
            {
                CoachId = 1,
                Email = "",
                Phone = 98765432,
                Name = "蘑菇",
                Introduction = "",
                Specialty = "",
                Certification = "",
                ContractEndTime = null,
                ContractStartTime = null,
                Status = null,
                PhotoUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();


            // Mock 設定
            bool success = true;
            multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((editCoachDto, files));
            coachServiceMock.Setup(s => s.EditCoach(editCoachDto, files.Any() ? files[0] : null)).Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await coachController.EditCoach();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }
    }
}
