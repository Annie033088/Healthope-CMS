using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models.Admin;
using ApiLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using ApiLayer.Controllers.api;
using ApiLayer.Service;
using DomainLayer.Models;

namespace UnitTest.Test.FilterTest
{
    [TestClass]
    public class AdminPermissionAuthFilterTest
    {
        public Mock<ISessionService> sessionServiceMock;
        public Mock<IHttpHelper> httpHelper;
        public AdminPermissionAuthFilter adminPermissionAuthFilter;
        private HttpActionContext actionContext;
        private HttpRequestMessage request; 
        [TestInitialize]
        public void Setup()
        {
            sessionServiceMock = new Mock<ISessionService>();
            httpHelper = new Mock<IHttpHelper>();
            adminPermissionAuthFilter = new AdminPermissionAuthFilter() { sessionService = sessionServiceMock.Object, httpHelper = httpHelper.Object };

            // 設定請求
            actionContext = new HttpActionContext();
            request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080")
            {
                Properties = { { System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            };
            actionContext.ControllerContext = new HttpControllerContext
            {
                Request = request
            };
        }

        [TestMethod]
        public void 使用者未登入_未登入_檢查沒有登入時的會話()
        {
            // Arrange
            string controllerName = "Admin";
            string actionName = "AddAdmin";
            string adminSessionKey = "AdminSession";
            AdminSession adminSession = null;
            ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.UserNotLogin };
            // 把 response 序列化成 JSON 字串
            string json = JsonConvert.SerializeObject(response);

            // 設定請求的回傳
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            actionContext.Response = httpResponseMessage;

            // Mock 設定
            httpHelper.Setup(s=>s.GetControllerName(actionContext)).Returns(controllerName);
            httpHelper.Setup(s=>s.GetActionName(actionContext)).Returns(actionName);
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(It.Is<string>(key => key == adminSessionKey))).Returns(adminSession);

            // Act
            adminPermissionAuthFilter.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            ResultResponse resultResponse = actionContext.Response.Content.ReadAsAsync<ResultResponse>().Result;
            Assert.AreEqual(ErrorCodeDefine.UserNotLogin, resultResponse.ErrorCode);
        }

        [TestMethod]
        public void 使用者沒權限_沒權限_回傳無權限()
        {
            // Arrange
            string controllerName = "Admin";
            string actionName = "AddAdmin";
            string adminSessionKey = "AdminSession";
            AdminSession adminSession = new AdminSession()
            {
                AdminId = 1,
                Identity = AdminIdentity.Admin
            };
            ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.NoPermission };
            // 把 response 序列化成 JSON 字串
            string json = JsonConvert.SerializeObject(response);

            // 設定請求的回傳
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            actionContext.Response = httpResponseMessage;

            // Mock 設定
            httpHelper.Setup(s => s.GetControllerName(actionContext)).Returns(controllerName);
            httpHelper.Setup(s => s.GetActionName(actionContext)).Returns(actionName);
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(It.Is<string>(key => key == adminSessionKey))).Returns(adminSession);

            // Act
            adminPermissionAuthFilter.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            ResultResponse resultResponse = actionContext.Response.Content.ReadAsAsync<ResultResponse>().Result;
            Assert.AreEqual(ErrorCodeDefine.NoPermission, resultResponse.ErrorCode);
        }
    }
}
