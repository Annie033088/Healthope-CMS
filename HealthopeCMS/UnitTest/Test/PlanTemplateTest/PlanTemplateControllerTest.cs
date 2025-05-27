using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models.Other;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Models;
using System.Web.Http;
using UnitTest.utils;
using ApiLayer.Models.PlanTemplate.Request;
using System.Net.Http;
using ApiLayer.Models.Response.PlanTemplate;
using ApiLayer.Models.PlanTemplate.Response;

namespace UnitTest.Test.PlanTemplateTest
{
    [TestClass]
    public class PlanTemplateControllerTest
    {
        private PlanTemplateController controller;
        private Mock<IPlanTemplateService> planTemplateServiceMock;
        private Mock<IMultipartRequestService<RequestAddMembershipPlanDto>> multipartRequestAddMembershipService;
        private Mock<IMultipartRequestService<RequestAddPersonalTrainingPackageDto>> multipartRequestAddAddPersonalTrainingService;

        [TestInitialize]
        public void Setup()
        {
            planTemplateServiceMock = new Mock<IPlanTemplateService>();
            multipartRequestAddMembershipService = new Mock<IMultipartRequestService<RequestAddMembershipPlanDto>>();
            multipartRequestAddAddPersonalTrainingService =
                new Mock<IMultipartRequestService<RequestAddPersonalTrainingPackageDto>>();
            controller = new PlanTemplateController(planTemplateServiceMock.Object, multipartRequestAddMembershipService.Object,
                multipartRequestAddAddPersonalTrainingService.Object);
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
            multipartRequestAddAddPersonalTrainingService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddAddPersonalTrainingService.Setup(s
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
            multipartRequestAddAddPersonalTrainingService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddAddPersonalTrainingService.Setup(s
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
            multipartRequestAddAddPersonalTrainingService.Setup(s
                => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
            multipartRequestAddAddPersonalTrainingService.Setup(s
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

        //[TestMethod]
        //public void 根據Id取得展示用團課詳細資料_成功_回傳展示用團課資料()
        //{
        //    // Arrange
        //    RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
        //    {
        //        GroupClassShowcaseId = 1
        //    };

        //    ResponseGetShowcaseDetailDto response = new ResponseGetShowcaseDetailDto()
        //    {
        //        Name = "okwopekq122",
        //        Category = 5,
        //        DetailContent = "",
        //        Icon = 2,
        //        ImageUrl = "",
        //        Sort = 1,
        //        Summary = ""
        //    };

        //    // Mock 設定
        //    groupClassShowcaseServiceMock.Setup(s => s.GetShowcaseDetail(showcaseIdDto)).Returns(response);

        //    // Act
        //    IHttpActionResult result = groupClassShowcaseController.GetShowcaseDetail(showcaseIdDto);

        //    // Assert
        //    ResponseIsEqual<ResponseGetShowcaseDetailDto> responseIsEqual =
        //        new ResponseIsEqual<ResponseGetShowcaseDetailDto>();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result, ErrorCodeDefine.Success, response));
        //}

        //[TestMethod]
        //public void 根據Id取得展示用團課詳細資料_失敗_回傳空資料()
        //{
        //    // Arrange
        //    RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
        //    {
        //        GroupClassShowcaseId = 1
        //    };

        //    ResponseGetShowcaseDetailDto response = null;

        //    // Mock 設定
        //    groupClassShowcaseServiceMock.Setup(s => s.GetShowcaseDetail(showcaseIdDto)).Returns(response);

        //    // Act
        //    IHttpActionResult result = groupClassShowcaseController.GetShowcaseDetail(showcaseIdDto);

        //    // Assert
        //    ResponseIsEqual responseIsEqual = new ResponseIsEqual();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.GetFailed));
        //}

        //[TestMethod]
        //public void 取得展示用團課修改頁面需要的資料_成功_回傳資料()
        //{
        //    // Arrange
        //    RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
        //    {
        //        GroupClassShowcaseId = 1
        //    };

        //    ResponseGetShowcaseEditDataDto responseDto = new ResponseGetShowcaseEditDataDto()
        //    {
        //        Name = "Jack",
        //        Sort = 1,
        //        Category = 2,
        //        DetailContent = "",
        //        Icon = 3,
        //        ImageUrl = "",
        //        Summary = "",
        //        UpdateTime = DateTime.Now,
        //    };

        //    // Mock 設定
        //    groupClassShowcaseServiceMock.Setup(s => s.GetShowcaseEditDataById(showcaseIdDto)).Returns(responseDto);

        //    // Act
        //    IHttpActionResult result = groupClassShowcaseController.GetShowcaseEditDataById(showcaseIdDto);

        //    // Assert
        //    ResponseIsEqual<ResponseGetShowcaseEditDataDto> responseIsEqual =
        //        new ResponseIsEqual<ResponseGetShowcaseEditDataDto>();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeAndObjectIsEqual(result,
        //        ErrorCodeDefine.Success, responseDto));
        //}

        //[TestMethod]
        //public void 取得展示用團課修改頁面需要的資料_失敗_請求參數格式錯誤()
        //{
        //    // Arrange
        //    RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
        //    {
        //        GroupClassShowcaseId = 0
        //    };

        //    // Act
        //    IHttpActionResult result = groupClassShowcaseController.GetShowcaseEditDataById(showcaseIdDto);

        //    // Assert
        //    ResponseIsEqual responseIsEqual = new ResponseIsEqual();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        //}

        //[TestMethod]
        //public async Task 修改展示用團課不包括圖檔_成功_回傳成功()
        //{
        //    // Arrange
        //    RequestEditShowcaseDto editShowcaseDto = new RequestEditShowcaseDto()
        //    {
        //        GroupClassShowcaseId = 1,
        //        Category = 1,
        //        DetailContent = "",
        //        Icon = 1,
        //        ImageUrl = "",
        //        Sort = 3,
        //        Summary = "",
        //        Name = "蘑菇",
        //        UpdateTime = DateTime.Now,
        //    };
        //    ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
        //    Exception exception = null;
        //    List<FileDto> files = new List<FileDto>();

        //    // Mock 設定
        //    bool success = true;
        //    multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
        //    multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
        //        .ReturnsAsync((editShowcaseDto, files));
        //    groupClassShowcaseServiceMock.Setup(s => s.EditShowcase(editShowcaseDto, files.Any() ? files[0] : null))
        //        .Returns((errorCode, exception));

        //    // Act
        //    IHttpActionResult result = await groupClassShowcaseController.EditShowcase();

        //    // Assert
        //    ResponseIsEqual responseIsEqual = new ResponseIsEqual();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        //}

        //[TestMethod]
        //public async Task 修改_失敗_名稱重複()
        //{
        //    // Arrange
        //    RequestEditShowcaseDto editShowcaseDto = new RequestEditShowcaseDto()
        //    {
        //        GroupClassShowcaseId = 1,
        //        Category = 1,
        //        DetailContent = "",
        //        Icon = 1,
        //        ImageUrl = "",
        //        Sort = 3,
        //        Summary = "",
        //        Name = "蘑菇",
        //        UpdateTime = DateTime.Now,
        //    };
        //    ErrorCodeDefine errorCode = ErrorCodeDefine.DuplicateName;
        //    Exception exception = null;
        //    List<FileDto> files = new List<FileDto>();

        //    // Mock 設定
        //    bool success = true;
        //    multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>())).Returns(success);
        //    multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
        //        .ReturnsAsync((editShowcaseDto, files));
        //    groupClassShowcaseServiceMock.Setup(s => s.EditShowcase(editShowcaseDto, files.Any() ? files[0] : null))
        //        .Returns((errorCode, exception));

        //    // Act
        //    IHttpActionResult result = await groupClassShowcaseController.EditShowcase();

        //    // Assert
        //    ResponseIsEqual responseIsEqual = new ResponseIsEqual();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DuplicateName));
        //}

        //[TestMethod]
        //public async Task 修改_失敗_格式錯誤()
        //{
        //    // Arrange
        //    RequestEditShowcaseDto editShowcaseDto = new RequestEditShowcaseDto()
        //    {
        //        GroupClassShowcaseId = 1,
        //        Category = 20,
        //        DetailContent = "",
        //        Icon = 1,
        //        ImageUrl = "",
        //        Sort = 3,
        //        Summary = "",
        //        Name = "蘑菇",
        //        UpdateTime = DateTime.Now,
        //    };
        //    ErrorCodeDefine errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
        //    Exception exception = null;
        //    List<FileDto> files = new List<FileDto>();

        //    // Mock 設定
        //    bool success = true;
        //    multipartRequestEditServiceMock.Setup(s => s.IsMultipartRequest(It.IsAny<HttpRequestMessage>()))
        //        .Returns(success);
        //    multipartRequestEditServiceMock.Setup(s => s.GetObjectAndFile(It.IsAny<HttpRequestMessage>()))
        //        .ReturnsAsync((editShowcaseDto, files));
        //    groupClassShowcaseServiceMock.Setup(s => s.EditShowcase(editShowcaseDto, files.Any() ? files[0] : null))
        //        .Returns((errorCode, exception));

        //    // Act
        //    IHttpActionResult result = await groupClassShowcaseController.EditShowcase();

        //    // Assert
        //    ResponseIsEqual responseIsEqual = new ResponseIsEqual();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry));
        //}

        //[TestMethod]
        //public void 刪除_成功_回傳成功()
        //{
        //    // Arrange
        //    RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
        //    {
        //        GroupClassShowcaseId = 10,
        //    };

        //    bool successFlag = true;

        //    // Mock 設定
        //    groupClassShowcaseServiceMock.Setup(s => s.DeleteShowcase(showcaseIdDto)).Returns(successFlag);

        //    // Act
        //    IHttpActionResult result = groupClassShowcaseController.DeleteShowcase(showcaseIdDto);

        //    // Assert
        //    ResponseIsEqual responseIsEqual = new ResponseIsEqual();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success));
        //}

        //[TestMethod]
        //public void 刪除_失敗_回傳失敗()
        //{
        //    // Arrange
        //    RequestShowcaseIdDto showcaseIdDto = new RequestShowcaseIdDto()
        //    {
        //        GroupClassShowcaseId = 10,
        //    };

        //    bool successFlag = false;

        //    // Mock 設定
        //    groupClassShowcaseServiceMock.Setup(s => s.DeleteShowcase(showcaseIdDto)).Returns(successFlag);

        //    // Act
        //    IHttpActionResult result = groupClassShowcaseController.DeleteShowcase(showcaseIdDto);

        //    // Assert
        //    ResponseIsEqual responseIsEqual = new ResponseIsEqual();
        //    Assert.IsTrue(responseIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DeleteFailed));
        //}
    }
}
