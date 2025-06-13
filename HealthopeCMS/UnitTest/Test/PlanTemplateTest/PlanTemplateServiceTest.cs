using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.Response.PlanTemplate;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.PlanTemplateTest
{
    [TestClass]
    public class PlanTemplateServiceTest
    {
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IFileService> fileServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;
        private readonly Mock<IPlanTemplateRepository> planTemplateRepositoryMock;
        private readonly PlanTemplateService service;

        public PlanTemplateServiceTest()
        {
            mapperMock = new Mock<IMapper>();
            fileServiceMock = new Mock<IFileService>();
            planTemplateRepositoryMock = new Mock<IPlanTemplateRepository>();
            httpServiceMock = new Mock<IHttpService>();
            service = new PlanTemplateService(planTemplateRepositoryMock.Object, mapperMock.Object,
                fileServiceMock.Object, httpServiceMock.Object);
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

            TicketPlan ticketPlan = new TicketPlan()
            {
                Price = 100,
                Status = true
            };

            bool success = true;

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddTicketPlan(ticketPlan)).Returns(success);
            mapperMock.Setup(s => s.Map<TicketPlan>(addTicketPlanDto)).Returns(ticketPlan);

            // Act
            bool result = service.AddTicketPlan(addTicketPlanDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 新增票劵方案_失敗_回傳格式錯誤()
        {
            // Arrange
            RequestAddTicketPlanDto addTicketPlanDto = new RequestAddTicketPlanDto()
            {
                Price = 100,
                Status = true
            };

            TicketPlan ticketPlan = new TicketPlan()
            {
                Price = 100,
                Status = true
            };

            bool success = false;

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddTicketPlan(ticketPlan)).Returns(success);
            mapperMock.Setup(s => s.Map<TicketPlan>(addTicketPlanDto)).Returns(ticketPlan);

            // Act
            bool result = service.AddTicketPlan(addTicketPlanDto);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void 新增會籍方案不包括圖擋_成功_回傳成功()
        {
            // Arrange
            RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            MembershipPlan membershipPlan = new MembershipPlan()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddMembershipPlan(membershipPlan)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<MembershipPlan>(addMembershipPlanDto)).Returns(membershipPlan);

            // Act
            (bool successFlag, Exception exception) = service.AddMembershipPlan(addMembershipPlanDto, null);

            // Assert
            Assert.IsTrue(successFlag);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void 新增會籍方案_失敗_回傳失敗()
        {
            // Arrange
            RequestAddMembershipPlanDto addMembershipPlanDto = new RequestAddMembershipPlanDto()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            MembershipPlan membershipPlan = new MembershipPlan()
            {
                Price = 100,
                Status = true,
                Display = true,
                Duration = 12,
                Introduction = "",
                Name = "qq123"
            };

            int errorCodeNumber = (int)ErrorCodeDefine.CreateFailed;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddMembershipPlan(membershipPlan)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<MembershipPlan>(addMembershipPlanDto)).Returns(membershipPlan);

            // Act
            (bool successFlag, Exception exception) = service.AddMembershipPlan(addMembershipPlanDto, null);

            // Assert
            Assert.IsFalse(successFlag);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void 新增教練課方案不包括圖擋_成功_回傳成功()
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

            PersonalTrainingPackage personalTrainingPackage = new PersonalTrainingPackage()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddPersonalTrainingPackage(personalTrainingPackage)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<PersonalTrainingPackage>(addPersonalTrainingPackageDto)).Returns(personalTrainingPackage);

            // Act
            (bool successFlag, Exception exception) = service.AddPersonalTrainingPackage(addPersonalTrainingPackageDto, null);

            // Assert
            Assert.IsTrue(successFlag);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void 新增教練課方案_失敗_回傳失敗()
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

            PersonalTrainingPackage personalTrainingPackage = new PersonalTrainingPackage()
            {
                Display = true,
                SessionCount = 100,
                Introduction = "",
                Name = "QQ123",
                Price = 100,
                Status = true
            };

            int errorCodeNumber = (int)ErrorCodeDefine.CreateFailed;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.AddPersonalTrainingPackage(personalTrainingPackage)).Returns(operationResult);
            mapperMock.Setup(s => s.Map<PersonalTrainingPackage>(addPersonalTrainingPackageDto)).Returns(personalTrainingPackage);

            // Act
            (bool successFlag, Exception exception) = service.AddPersonalTrainingPackage(addPersonalTrainingPackageDto, null);

            // Assert
            Assert.IsFalse(successFlag);
            Assert.IsNull(exception);
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

            List<MembershipPlan> membershipPlans = new List<MembershipPlan>() {
                new MembershipPlan(){
                    MembershipPlanId  = 1,
                    Name = "QQ",
                    Price =100,
                    Duration =12,
                    Introduction ="",
                    Display =true,
                    Status =true,
                    UpdateTime =DateTime.Now,
                }
            };
            int totalPage = 1;

            List<ResponseGetMembershipPlanDto> membershipPlanList = new List<ResponseGetMembershipPlanDto>()
            {
                new ResponseGetMembershipPlanDto()
                {
                MembershipPlanId  = 1,
                Name = "QQ",
                Price =100,
                Duration =12,
                Introduction ="",
                Display =true,
                Status =true,
                UpdateTime =DateTime.Now,
                }
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s
                => s.GetMembershipPlan(getPlanDto)).Returns((membershipPlans, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetMembershipPlanDto>>(membershipPlans)).Returns(membershipPlanList);

            // Act
            ResponseGetMembershipPlanListDto response = service.GetMembershipPlan(getPlanDto);

            // Assert
            CollectionAssert.AreEqual(response.MembershipPlanList, membershipPlanList);
        }

        [TestMethod]
        public void 取得會籍方案清單_失敗_取得空資料()
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

            List<MembershipPlan> membershipPlans = new List<MembershipPlan>();
            int totalPage = 1;
            List<ResponseGetMembershipPlanDto> membershipPlanList = new List<ResponseGetMembershipPlanDto>();

            // Mock 設定
            planTemplateRepositoryMock.Setup(s
                => s.GetMembershipPlan(getPlanDto)).Returns((membershipPlans, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetMembershipPlanDto>>(membershipPlans)).Returns(membershipPlanList);

            // Act
            ResponseGetMembershipPlanListDto response = service.GetMembershipPlan(getPlanDto);

            // Assert
            CollectionAssert.AreEqual(response.MembershipPlanList, membershipPlanList);
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

            List<PersonalTrainingPackage> personalTrainingPackages = new List<PersonalTrainingPackage>() {
                new PersonalTrainingPackage(){
                    PersonalTrainingPackageId  = 1,
                    Name = "QQ",
                    Price =100,
                    SessionCount =30,
                    Introduction ="",
                    Display =true,
                    Status =true,
                    UpdateTime =DateTime.Now,
                }
            };
            int totalPage = 1;

            List<ResponseGetPersonalTrainingPackageDto> personalTrainingPackageList = new List<ResponseGetPersonalTrainingPackageDto>()
            {
                new ResponseGetPersonalTrainingPackageDto()
                {
                    PersonalTrainingPackageId  = 1,
                    Name = "QQ",
                    Price =100,
                    SessionCount =30,
                    Introduction ="",
                    Display =true,
                    Status =true,
                    UpdateTime =DateTime.Now,
                }
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s
                => s.GetPersionalTrainingPackage(getPlanDto)).Returns((personalTrainingPackages, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetPersonalTrainingPackageDto>>(personalTrainingPackages))
                .Returns(personalTrainingPackageList);

            // Act
            ResponseGetPersonalTrainingPackageListDto response = service.GetPersionalTrainingPackage(getPlanDto);

            // Assert
            CollectionAssert.AreEqual(response.PersonalTrainingPackageList, personalTrainingPackageList);
        }

        [TestMethod]
        public void 取得教練課方案清單_失敗_回傳空資料()
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

            List<PersonalTrainingPackage> personalTrainingPackages = new List<PersonalTrainingPackage>() {
                new PersonalTrainingPackage(){
                    PersonalTrainingPackageId  = 1,
                    Name = "QQ",
                    Price =100,
                    SessionCount =30,
                    Introduction ="",
                    Display =true,
                    Status =true,
                    UpdateTime =DateTime.Now,
                }
            };
            int totalPage = 1;

            List<ResponseGetPersonalTrainingPackageDto> personalTrainingPackageList = new List<ResponseGetPersonalTrainingPackageDto>()
            {
                new ResponseGetPersonalTrainingPackageDto()
                {
                    PersonalTrainingPackageId  = 1,
                    Name = "QQ",
                    Price =100,
                    SessionCount =30,
                    Introduction ="",
                    Display =true,
                    Status =true,
                    UpdateTime =DateTime.Now,
                }
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s
                => s.GetPersionalTrainingPackage(getPlanDto)).Returns((personalTrainingPackages, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetPersonalTrainingPackageDto>>(personalTrainingPackages))
                .Returns(personalTrainingPackageList);

            // Act
            ResponseGetPersonalTrainingPackageListDto response = service.GetPersionalTrainingPackage(getPlanDto);

            // Assert
            CollectionAssert.AreEqual(response.PersonalTrainingPackageList, personalTrainingPackageList);
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

            List<TicketPlan> tickePlans = new List<TicketPlan>() {
                new TicketPlan(){
                    TicketPlanId = 1,
                    Price =100,
                    Status =true,
                    UpdateTime =DateTime.Now,
                }
            };
            int totalPage = 1;

            List<ResponseGetTicketPlanDto> tickPlanList = new List<ResponseGetTicketPlanDto>()
            {
                new ResponseGetTicketPlanDto()
                {
                    TicketPlanId = 1,
                    Price =100,
                    Status =true,
                    UpdateTime =DateTime.Now,
                }
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s
                => s.GetTicketPlan(getPlanDto)).Returns((tickePlans, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetTicketPlanDto>>(tickePlans))
                .Returns(tickPlanList);

            // Act
            ResponseGetTicketPlanListDto response = service.GetTicketPlan(getPlanDto);

            // Assert
            CollectionAssert.AreEqual(response.TicketPlanList, tickPlanList);
        }

        [TestMethod]
        public void 取得票劵方案清單_失敗_回傳空資料()
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

            List<TicketPlan> tickePlans = new List<TicketPlan>();
            int totalPage = 1;
            List<ResponseGetTicketPlanDto> tickPlanList = new List<ResponseGetTicketPlanDto>();

            // Mock 設定
            planTemplateRepositoryMock.Setup(s
                => s.GetTicketPlan(getPlanDto)).Returns((tickePlans, totalPage));
            mapperMock.Setup(s => s.Map<List<ResponseGetTicketPlanDto>>(tickePlans))
                .Returns(tickPlanList);

            // Act
            ResponseGetTicketPlanListDto response = service.GetTicketPlan(getPlanDto);

            // Assert
            CollectionAssert.AreEqual(response.TicketPlanList, tickPlanList);
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

            TicketPlan ticketPlan = new TicketPlan()
            {
                TicketPlanId = 10,
                Status = false,
                UpdateTime = DateTime.Now,
            };

            bool successFlag = true;

            // Mock 設定
            planTemplateRepositoryMock
                .Setup(s => s.EditTicketPlanStatus(It.Is<TicketPlan>(t =>
                    t.TicketPlanId == editStatusDto.TicketPlanId &&
                    t.Status == editStatusDto.Status
                )))
                .Returns(successFlag);

            // Act
            bool response = service.EditTicketPlanStatus(editStatusDto);

            // Assert
            Assert.IsTrue(response);
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

            TicketPlan ticketPlan = new TicketPlan()
            {
                Status = editStatusDto.Status,
                TicketPlanId = editStatusDto.TicketPlanId,
                UpdateTime = editStatusDto.UpdateTime,
            };

            bool successFlag = false;

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.EditTicketPlanStatus(ticketPlan)).Returns(successFlag);

            // Act
            bool response = service.EditTicketPlanStatus(editStatusDto);

            // Assert
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void 取得修改會籍方案頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestMembershipPlanIdDto memebershipPlanIdDto = new RequestMembershipPlanIdDto()
            {
                MembershipPlanId = 1
            };

            DateTime dateTime = DateTime.Now;

            ResponseGetMembershipPlanEditDataDto responseDto = new ResponseGetMembershipPlanEditDataDto()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = dateTime,
            };

            MembershipPlan membershipPlan = new MembershipPlan()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = dateTime,
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.GetMembershipPlanEditDataById(memebershipPlanIdDto.MembershipPlanId))
                .Returns(membershipPlan);
            mapperMock.Setup(s => s.Map<ResponseGetMembershipPlanEditDataDto>(membershipPlan)).Returns(responseDto);

            // Act
            ResponseGetMembershipPlanEditDataDto response = service.GetMembershipPlanEditDataById(memebershipPlanIdDto);
            responseDto.ImageUrl = "/" + responseDto.ImageUrl;

            // Assert
            Assert.AreEqual(response, responseDto);
        }

        [TestMethod]
        public void 取得修改會籍方案頁面需要的資料_失敗_回傳空資料()
        {
            // Arrange
            RequestMembershipPlanIdDto memebershipPlanIdDto = new RequestMembershipPlanIdDto()
            {
                MembershipPlanId = 1
            };

            DateTime dateTime = DateTime.Now;

            MembershipPlan membershipPlan = null;

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.GetMembershipPlanEditDataById(memebershipPlanIdDto.MembershipPlanId))
                .Returns(membershipPlan);

            // Act
            ResponseGetMembershipPlanEditDataDto response = service.GetMembershipPlanEditDataById(memebershipPlanIdDto);

            // Assert
            Assert.IsNull(response);
        }

        [TestMethod]
        public void 取得修改教練課方案頁面需要的資料_成功_回傳資料()
        {
            // Arrange
            RequestPersonalTrainingPackageIdDto personalTrainingPackageIdDto = new RequestPersonalTrainingPackageIdDto()
            {
                PersonalTrainingPackageId = 1
            };

            DateTime dateTime = DateTime.Now;

            ResponseGetPersonalTrainingPackageEditDataDto responseDto = new ResponseGetPersonalTrainingPackageEditDataDto()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = dateTime,
            };

            PersonalTrainingPackage personalTrainingPackage = new PersonalTrainingPackage()
            {
                Name = "Jack",
                Status = true,
                Display = false,
                Introduction = "",
                ImageUrl = "",
                UpdateTime = dateTime,
            };

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.GetPersonalTrainingPackageEditDataById(
                personalTrainingPackageIdDto.PersonalTrainingPackageId)).Returns(personalTrainingPackage);
            mapperMock.Setup(s => s.Map<ResponseGetPersonalTrainingPackageEditDataDto>(personalTrainingPackage))
                .Returns(responseDto);

            // Act
            ResponseGetPersonalTrainingPackageEditDataDto response = service.GetPersonalTrainingPackageEditDataById(
                personalTrainingPackageIdDto);
            responseDto.ImageUrl = "/" + responseDto.ImageUrl;

            // Assert
            Assert.AreEqual(response, responseDto);
        }

        [TestMethod]
        public void 取得修改教練課方案頁面需要的資料_失敗_回傳空資料()
        {
            // Arrange
            RequestPersonalTrainingPackageIdDto personalTrainingPackageIdDto = new RequestPersonalTrainingPackageIdDto()
            {
                PersonalTrainingPackageId = 1
            };

            DateTime dateTime = DateTime.Now;

            PersonalTrainingPackage personalTrainingPackage = null;

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.GetPersonalTrainingPackageEditDataById(
                personalTrainingPackageIdDto.PersonalTrainingPackageId)).Returns(personalTrainingPackage);

            // Act
            ResponseGetPersonalTrainingPackageEditDataDto response = service.GetPersonalTrainingPackageEditDataById(
                personalTrainingPackageIdDto);

            // Assert
            Assert.IsNull(response);
        }

        [TestMethod]
        public void 修改會籍方案不包括圖檔_成功_回傳成功()
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

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.EditMembershipPlan(editMembershipPlanDto))
                .Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s => s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = service.EditMembershipPlan(editMembershipPlanDto, null);

            // Assert
            Assert.AreEqual(errorCode, ErrorCodeDefine.Success);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void 修改會籍方案_失敗_資料已被修改()
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

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.EditMembershipPlan(editMembershipPlanDto))
                .Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s => s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = service.EditMembershipPlan(editMembershipPlanDto, null);

            // Assert
            Assert.AreEqual(errorCode, ErrorCodeDefine.HasBeenModified);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void 修改教練課方案不包括圖檔_成功_回傳成功()
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

            int errorCodeNumber = (int)ErrorCodeDefine.Success;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.EditPersonalTrainingPackage(editPlanDto))
                .Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s => s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = service.EditPersonalTrainingPackage(editPlanDto, null);

            // Assert
            Assert.AreEqual(errorCode, ErrorCodeDefine.Success);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void 修改教練課方案_失敗_資料已被修改()
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

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;
            ResultWithException operationResult = new ResultWithException()
            {
                ErrorCodeNumber = errorCodeNumber,
                Exception = null
            };
            string oldPhotoUrl = "";

            // Mock 設定
            planTemplateRepositoryMock.Setup(s => s.EditPersonalTrainingPackage(editPlanDto))
                .Returns((operationResult, oldPhotoUrl));
            httpServiceMock.Setup(s => s.GetRootPath()).Returns("/");

            // Act
            (ErrorCodeDefine errorCode, Exception exception) = service.EditPersonalTrainingPackage(editPlanDto, null);

            // Assert
            Assert.AreEqual(errorCode, ErrorCodeDefine.HasBeenModified);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void 取得全部有效方案清單_成功_回傳清單()
        {
            // Arrange
            ApiLayer.Models.PlanTemplate.Response.GetAllType.ResponseGetAllTypePlanDto response
                = new ApiLayer.Models.PlanTemplate.Response.GetAllType.ResponseGetAllTypePlanDto()
                {
                    TicketPlanList = new List<ApiLayer.Models.PlanTemplate.Response.GetAllType.ResponseGetTicketPlanDto>()
                {
                    new ApiLayer.Models.PlanTemplate.Response.GetAllType.ResponseGetTicketPlanDto()
                    {
                        Price=100,
                        TicketPlanId=1,
                    }
                },
                    MembershipPlanList = null,
                    PersonalTrainingPackageList = null,
                };

            List<MembershipPlan> membershipPlans = new List<MembershipPlan>()
            {
                new MembershipPlan()
                {
                    MembershipPlanId=1,
                    Name="qq",
                    Price=100,
                    UpdateTime = DateTime.Now,
                }
            };

            List<PersonalTrainingPackage> personalTrainingPackageList = new List<PersonalTrainingPackage>()
            {
                new PersonalTrainingPackage()
                {
                    PersonalTrainingPackageId=1,
                    Name="qq",
                    Price=100,
                    UpdateTime = DateTime.Now,
                }
            };

            List<TicketPlan> TicketPlanList = new List<TicketPlan>()
            {
                new TicketPlan()
                {
                    TicketPlanId=1,
                    Price=100,
                    UpdateTime = DateTime.Now,
                }
            };


            // Mock 設定
            planTemplateRepositoryMock.Setup(s
                => s.GetAllTypePlan()).Returns((membershipPlans, personalTrainingPackageList, TicketPlanList));

            // Act
            ApiLayer.Models.PlanTemplate.Response.GetAllType.ResponseGetAllTypePlanDto result = service.GetAllTypePlan();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
