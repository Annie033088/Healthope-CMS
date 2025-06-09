using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Other;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.PlanTemplate.Response.GetAllType;
using ApiLayer.Models.Response.PlanTemplate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using UnitTest.utils;

namespace UnitTest.Test.PlanTemplateTest
{
    [TestClass]
    public class PlanTemplateControllerTest
    {
        private PlanTemplateController controller;
        private Mock<IPlanTemplateService> planTemplateServiceMock;
        private Mock<IMultipartRequestService<RequestAddMembershipPlanDto>> multipartRequestAddMembershipService;
        private Mock<IMultipartRequestService<RequestEditMembershipPlanDto>> multipartRequestEditMembershipService;
        private Mock<IMultipartRequestService<RequestAddPersonalTrainingPackageDto>> multipartRequestAddPersonalTrainingService;
        private Mock<IMultipartRequestService<RequestEditPersonalTrainingPackageDto>> multipartRequestEditPersonalTrainingService;

        [TestInitialize]
        public void Setup()
        {
            planTemplateServiceMock = new Mock<IPlanTemplateService>();
            multipartRequestAddMembershipService = new Mock<IMultipartRequestService<RequestAddMembershipPlanDto>>();
            multipartRequestEditMembershipService = new Mock<IMultipartRequestService<RequestEditMembershipPlanDto>>();
            multipartRequestAddPersonalTrainingService =
                new Mock<IMultipartRequestService<RequestAddPersonalTrainingPackageDto>>();
            multipartRequestEditPersonalTrainingService =
                new Mock<IMultipartRequestService<RequestEditPersonalTrainingPackageDto>>();
            controller = new PlanTemplateController(planTemplateServiceMock.Object, multipartRequestAddMembershipService.Object,
                multipartRequestAddPersonalTrainingService.Object, multipartRequestEditMembershipService.Object,
                multipartRequestEditPersonalTrainingService.Object);
        }

        [TestMethod]
        public void 新增票劵方案_成功_回傳成功()
        {
            // Arrange
            RequestAddTicketPlanDto addTicketPlanDto = new RequestAddTicketPlanDto()
            {
                Price = 100,
                Status = true
            };

            // Mock 設定
            bool success = true;
            planTemplateServiceMock.Setup(s => s.AddTicketPlan(addTicketPlanDto)).Returns(success);

            // Act
            IHttpActionResult result = controller.AddTicketPlan(addTicketPlanDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 新增票劵方案_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestAddTicketPlanDto addTicketPlanDto = new RequestAddTicketPlanDto()
            {
                Price = -1,
                Status = true
            };

            // Act
            IHttpActionResult result = controller.AddTicketPlan(addTicketPlanDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 新增會籍方案不包括圖擋_成功_回傳成功()
        {
            // Arrange
            RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto()
            {
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddMembershipService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddMembershipService.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addMembershipPlanDto, files));
            planTemplateServiceMock.Setup(s
                => s.AddMembershipPlan(addMembershipPlanDto, files.Any() ? files[0] : null)).Returns((success, exception));

            // Act
            IHttpActionResult result = await controller.AddMembershipPlan();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 新增會籍方案_失敗_回傳失敗()
        {
            // Arrange
            RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto()
            {
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            bool fail = false;
            multipartRequestAddMembershipService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddMembershipService.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addMembershipPlanDto, files));
            planTemplateServiceMock.Setup(s
                => s.AddMembershipPlan(addMembershipPlanDto, files.Any() ? files[0] : null)).Returns((fail, exception));

            // Act
            IHttpActionResult result = await controller.AddMembershipPlan();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.CreateFailed));
        }

        [TestMethod]
        public async Task 新增會籍方案_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto()
            {
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "",
                Price = 100,
                Status = true
            };
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddMembershipService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddMembershipService.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addMembershipPlanDto, files));

            // Act
            IHttpActionResult result = await controller.AddMembershipPlan();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 新增教練課方案不包括圖擋_成功_回傳成功()
        {
            // Arrange
            RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto = new RequestAddPersonalTrainingPackageDto()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddPersonalTrainingService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddPersonalTrainingService.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addPersonalTrainingPackageDto, files));
            planTemplateServiceMock.Setup(s
                => s.AddPersonalTrainingPackage(addPersonalTrainingPackageDto,
                files.Any() ? files[0] : null)).Returns((success, exception));

            // Act
            IHttpActionResult result = await controller.AddPersonalTrainingPackage();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 新增教練課方案_失敗_回傳失敗()
        {
            // Arrange
            RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto = new RequestAddPersonalTrainingPackageDto()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            bool fail = false;
            multipartRequestAddPersonalTrainingService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddPersonalTrainingService.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addPersonalTrainingPackageDto, files));
            planTemplateServiceMock.Setup(s
                => s.AddPersonalTrainingPackage(addPersonalTrainingPackageDto, files.Any() ?
                files[0] : null)).Returns((fail, exception));

            // Act
            IHttpActionResult result = await controller.AddPersonalTrainingPackage();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.CreateFailed));
        }

        [TestMethod]
        public async Task 新增教練課方案_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto = new RequestAddPersonalTrainingPackageDto()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = -1,
                Status = true
            };
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestAddPersonalTrainingService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddPersonalTrainingService.Setup(s
                => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>())).ReturnsAsync((addPersonalTrainingPackageDto, files));

            // Act
            IHttpActionResult result = await controller.AddPersonalTrainingPackage();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得會籍方案清單_成功_回傳清單()
        {
            // Arrange
            RequestGetPlanDto getPlanDto = new RequestGetPlanDto()
            {
                Status = true,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "status", // 只允許 status | price | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            ResponseGetMembershipPlanListDto response = new ResponseGetMembershipPlanListDto()
            {
                MembershipPlanList = null,
                TotalPage = 1,
            };


            // Mock 設定
            planTemplateServiceMock.Setup(s
                => s.GetMembershipPlan(getPlanDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetMembershipPlan(getPlanDto);

            // Assert
            ResponseIsEqual<ResponseGetMembershipPlanListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetMembershipPlanListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得會籍方案清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetPlanDto getPlanDto = new RequestGetPlanDto()
            {
                Status = true,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "name", // 只允許 status | price | null
                RecordPerPage = 10, // 只允許 8 或 12 或 16
            };

            ResponseGetMembershipPlanListDto response = new ResponseGetMembershipPlanListDto()
            {
                MembershipPlanList = null,
                TotalPage = 1,
            };

            // Act
            IHttpActionResult result = controller.GetMembershipPlan(getPlanDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得教練課方案清單_成功_回傳清單()
        {
            // Arrange
            RequestGetPlanDto getPlanDto = new RequestGetPlanDto()
            {
                Status = true,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "status", // 只允許 status | price | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            ResponseGetPersonalTrainingPackageListDto response = new ResponseGetPersonalTrainingPackageListDto()
            {
                PersonalTrainingPackageList = null,
                TotalPage = 1,
            };


            // Mock 設定
            planTemplateServiceMock.Setup(s
                => s.GetPersionalTrainingPackage(getPlanDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetPersionalTrainingPackage(getPlanDto);

            // Assert
            ResponseIsEqual<ResponseGetPersonalTrainingPackageListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetPersonalTrainingPackageListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得教練課方案清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetPlanDto getPlanDto = new RequestGetPlanDto()
            {
                Status = true,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "name",// 只允許 status | price | null
                RecordPerPage = 10, // 只允許 8 或 12 或 16
            };

            ResponseGetPersonalTrainingPackageListDto response = new ResponseGetPersonalTrainingPackageListDto()
            {
                PersonalTrainingPackageList = null,
                TotalPage = 1,
            };

            // Act
            IHttpActionResult result = controller.GetPersionalTrainingPackage(getPlanDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得票劵方案清單_成功_回傳清單()
        {
            // Arrange
            RequestGetPlanDto getPlanDto = new RequestGetPlanDto()
            {
                Status = true,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "status", // 只允許 status | price | null
                RecordPerPage = 8, // 只允許 8 或 12 或 16
            };

            ResponseGetTicketPlanListDto response = new ResponseGetTicketPlanListDto()
            {
                TicketPlanList = null,
                TotalPage = 1,
            };


            // Mock 設定
            planTemplateServiceMock.Setup(s
                => s.GetTicketPlan(getPlanDto)).Returns(response);

            // Act
            IHttpActionResult result = controller.GetTicketPlan(getPlanDto);

            // Assert
            ResponseIsEqual<ResponseGetTicketPlanListDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetTicketPlanListDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }

        [TestMethod]
        public void 取得票劵方案清單_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestGetPlanDto getPlanDto = new RequestGetPlanDto()
            {
                Status = true,
                Page = 1, // 必須>0
                SortOrder = "descending", // 只允許 descending 或 ascending
                SortOption = "name",// 只允許 status | price | null
                RecordPerPage = 10, // 只允許 8 或 12 或 16
            };

            ResponseGetTicketPlanListDto response = new ResponseGetTicketPlanListDto()
            {
                TicketPlanList = null,
                TotalPage = 1,
            };

            // Act
            IHttpActionResult result = controller.GetTicketPlan(getPlanDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 修改票劵方案狀態_成功_回傳成功()
        {
            // Arrange
            RequestEditStatusDto editStatusDto = new RequestEditStatusDto()
            {
                TicketPlanId = 10,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            bool successFlag = true;

            // Mock 設定
            planTemplateServiceMock.Setup(s => s.EditTicketPlanStatus(editStatusDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.EditTicketPlanStatus(editStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public void 修改票劵方案狀態_失敗_回傳失敗()
        {
            // Arrange
            RequestEditStatusDto editStatusDto = new RequestEditStatusDto()
            {
                TicketPlanId = 10,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            bool successFlag = false;

            // Mock 設定
            planTemplateServiceMock.Setup(s => s.EditTicketPlanStatus(editStatusDto)).Returns(successFlag);

            // Act
            IHttpActionResult result = controller.EditTicketPlanStatus(editStatusDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.ModifiedFailed));
        }

        [TestMethod]
        public void 取得修改會籍方案頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestMembershipPlanIdDto memebershipPlanIdDto = new RequestMembershipPlanIdDto()
            {
                MembershipPlanId = 1
            };

            ResponseGetMembershipPlanEditDataDto responseDto = new ResponseGetMembershipPlanEditDataDto()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            planTemplateServiceMock.Setup(s => s.GetMembershipPlanEditDataById(memebershipPlanIdDto)).Returns(responseDto);

            // Act
            IHttpActionResult result = controller.GetMembershipPlanEditDataById(memebershipPlanIdDto);

            // Assert
            ResponseIsEqual<ResponseGetMembershipPlanEditDataDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetMembershipPlanEditDataDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseDto));
        }

        [TestMethod]
        public void 取得修改會籍方案頁面需要的資料_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestMembershipPlanIdDto memebershipPlanIdDto = new RequestMembershipPlanIdDto()
            {
                MembershipPlanId = 0
            };

            ResponseGetMembershipPlanEditDataDto responseDto = new ResponseGetMembershipPlanEditDataDto()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            planTemplateServiceMock.Setup(s => s.GetMembershipPlanEditDataById(memebershipPlanIdDto)).Returns(responseDto);

            // Act
            IHttpActionResult result = controller.GetMembershipPlanEditDataById(memebershipPlanIdDto);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得修改教練課方案頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestPersonalTrainingPackageIdDto personalTrainingPackageId = new RequestPersonalTrainingPackageIdDto()
            {
                PersonalTrainingPackageId = 1
            };

            ResponseGetPersonalTrainingPackageEditDataDto responseDto = new ResponseGetPersonalTrainingPackageEditDataDto()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            planTemplateServiceMock.Setup(s => s.GetPersonalTrainingPackageEditDataById(personalTrainingPackageId))
                .Returns(responseDto);

            // Act
            IHttpActionResult result = controller.GetPersonalTrainingPackageEditDataById(personalTrainingPackageId);

            // Assert
            ResponseIsEqual<ResponseGetPersonalTrainingPackageEditDataDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetPersonalTrainingPackageEditDataDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, responseDto));
        }

        [TestMethod]
        public void 取得修改教練課方案頁面需要的資料_失敗_請求參數格式錯誤()
        {
            // Arrange
            RequestPersonalTrainingPackageIdDto personalTrainingPackageId = new RequestPersonalTrainingPackageIdDto()
            {
                PersonalTrainingPackageId = 0
            };

            ResponseGetPersonalTrainingPackageEditDataDto responseDto = new ResponseGetPersonalTrainingPackageEditDataDto()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            planTemplateServiceMock.Setup(s => s.GetPersonalTrainingPackageEditDataById(personalTrainingPackageId))
                .Returns(responseDto);

            // Act
            IHttpActionResult result = controller.GetPersonalTrainingPackageEditDataById(personalTrainingPackageId);

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 修改會籍方案不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestEditMembershipPlanDto editMembershipPlanDto = new RequestEditMembershipPlanDto()
            {
                MembershipPlanId = 1,
                Display = false,
                Status = null,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditMembershipService.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(success);
            multipartRequestEditMembershipService.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editMembershipPlanDto, files));
            planTemplateServiceMock.Setup(s => s.EditMembershipPlan(editMembershipPlanDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await controller.EditMembershipPlan();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 修改會籍方案_失敗_資料已被修改()
        {
            // Arrange
            RequestEditMembershipPlanDto editMembershipPlanDto = new RequestEditMembershipPlanDto()
            {
                MembershipPlanId = 1,
                Display = false,
                Status = null,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.HasBeenModified;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditMembershipService.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(success);
            multipartRequestEditMembershipService.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editMembershipPlanDto, files));
            planTemplateServiceMock.Setup(s => s.EditMembershipPlan(editMembershipPlanDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await controller.EditMembershipPlan();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.HasBeenModified));
        }

        [TestMethod]
        public async Task 修改會籍方案_失敗_格式錯誤()
        {
            // Arrange
            RequestEditMembershipPlanDto editMembershipPlanDto = new RequestEditMembershipPlanDto()
            {
                MembershipPlanId = 0,
                Display = false,
                Status = null,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditMembershipService.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(success);
            multipartRequestEditMembershipService.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editMembershipPlanDto, files));
            planTemplateServiceMock.Setup(s => s.EditMembershipPlan(editMembershipPlanDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await controller.EditMembershipPlan();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public async Task 修改教練課方案不包括圖檔_成功_回傳成功()
        {
            // Arrange
            RequestEditPersonalTrainingPackageDto editPlanDto = new RequestEditPersonalTrainingPackageDto()
            {
                PersonalTrainingPackageId = 1,
                Display = false,
                Status = null,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditPersonalTrainingService.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(success);
            multipartRequestEditPersonalTrainingService.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editPlanDto, files));
            planTemplateServiceMock.Setup(s => s.EditPersonalTrainingPackage(editPlanDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await controller.EditPersonalTrainingPackage();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        }

        [TestMethod]
        public async Task 修改教練課方案_失敗_資料已被修改()
        {
            // Arrange
            RequestEditPersonalTrainingPackageDto editPlanDto = new RequestEditPersonalTrainingPackageDto()
            {
                PersonalTrainingPackageId = 1,
                Display = false,
                Status = null,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.HasBeenModified;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditPersonalTrainingService.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(success);
            multipartRequestEditPersonalTrainingService.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editPlanDto, files));
            planTemplateServiceMock.Setup(s => s.EditPersonalTrainingPackage(editPlanDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await controller.EditPersonalTrainingPackage();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.HasBeenModified));
        }

        [TestMethod]
        public async Task 修改教練課方案_失敗_格式錯誤()
        {
            // Arrange
            RequestEditPersonalTrainingPackageDto editPlanDto = new RequestEditPersonalTrainingPackageDto()
            {
                PersonalTrainingPackageId = -1,
                Display = false,
                Status = null,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = DateTime.Now,
            };
            ErrorCodeDefine errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
            Exception exception = null;
            List<FileDto> files = new List<FileDto>();

            // Mock 設定
            bool success = true;
            multipartRequestEditPersonalTrainingService.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
                .Returns(success);
            multipartRequestEditPersonalTrainingService.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync((editPlanDto, files));
            planTemplateServiceMock.Setup(s => s.EditPersonalTrainingPackage(editPlanDto, files.Any() ? files[0] : null))
                .Returns((errorCode, exception));

            // Act
            IHttpActionResult result = await controller.EditPersonalTrainingPackage();

            // Assert
            ResponseIsEqual responseIsEqual = new ResponseIsEqual();
            Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        }

        [TestMethod]
        public void 取得全部有效方案清單_成功_回傳清單()
        {
            // Arrange
            ResponseGetAllTypePlanDto response = new ResponseGetAllTypePlanDto()
            {
                TicketPlanList = new List<ApiLayer.Models.PlanTemplate.Response.GetAllType.ResponseGetTicketPlanDto>()
                {
                    new ApiLayer.Models.PlanTemplate.Response.GetAllType.ResponseGetTicketPlanDto()
                    {
                        Price=100,
                        TicketPlanId=1,
                        UpdateTime=DateTime.Now,
                    }
                },
                MembershipPlanList = null,
                PersonalTrainingPackageList = null,
            };


            // Mock 設定
            planTemplateServiceMock.Setup(s
                => s.GetAllTypePlan()).Returns(response);

            // Act
            IHttpActionResult result = controller.GetAllTypePlan();

            // Assert
            ResponseIsEqual<ResponseGetAllTypePlanDto> responseIsEqual =
                new ResponseIsEqual<ResponseGetAllTypePlanDto>();
            Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
                ErrorCodeDefine.Success, response));
        }
    }
}
