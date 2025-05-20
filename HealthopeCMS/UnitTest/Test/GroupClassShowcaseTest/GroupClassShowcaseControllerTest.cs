using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.GroupClassShowcase.Request;
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

        [TestInitialize]
        public void Setup()
        {
            groupClassShowcaseServiceMock = new Mock<IGroupClassShowcaseService>();
            multipartRequestAddServiceMock =
                new Mock<IMultipartRequestService<RequestAddShowcaseDto>>();
            groupClassShowcaseController = new GroupClassShowcaseController(
                multipartRequestAddServiceMock.Object, groupClassShowcaseServiceMock.Object);
        }

        [TestMethod]
        public async Task 新增不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestAddShowcaseDto addShowcaseDto = new RequestAddShowcaseDto()
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
        public async Task 新增_失敗_名稱重複()
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
    }
}
