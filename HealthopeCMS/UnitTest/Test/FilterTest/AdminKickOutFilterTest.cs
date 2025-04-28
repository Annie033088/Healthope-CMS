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
using ApiLayer.Models;
using ApiLayer.Models.Admin;
using ApiLayer.Service;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace UnitTest.Test.FilterTest
{
    [TestClass]
    public class AdminKickOutFilterTest
    {
        public Mock<ISessionService> sessionServiceMock;
        public Mock<IRedisService> redisServiceMock;
        public AdminKickOutFilter adminKickOutFilter;
        private HttpActionContext actionContext;
        private HttpRequestMessage request;

        [TestInitialize]
        public void Setup()
        {
            sessionServiceMock = new Mock<ISessionService>();
            redisServiceMock = new Mock<IRedisService>();
            adminKickOutFilter = new AdminKickOutFilter() { redisService = redisServiceMock.Object, sessionService = sessionServiceMock.Object };

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
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(It.Is<string>(key => key == adminSessionKey))).Returns(adminSession);

            // Act
            adminKickOutFilter.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            ResultResponse resultResponse = actionContext.Response.Content.ReadAsAsync<ResultResponse>().Result;
            Assert.AreEqual(ErrorCodeDefine.UserNotLogin, resultResponse.ErrorCode);
        }

        [TestMethod]
        public void 使用者被踢出帳號_被踢出_檢查redis裡的sessionId是否不同()
        {
            // Arrange
            string adminSessionKey = "AdminSession";
            AdminSession adminSession = new AdminSession() { AdminId = 1, Identity = AdminIdentity.Accountant };
            string redisKey = "Admin" + adminSession.AdminId;
            string sessionId = "asdawdpl21";
            AdminRedis adminRedis = new AdminRedis() { ErrorCode = ErrorCodeDefine.Success, SessionId="dkaopdka2222" };

            // 回傳設定
            ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.KickOut };

            // 把 response 序列化成 JSON 字串
            string json = JsonConvert.SerializeObject(response);

            // 設定請求的回傳
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            actionContext.Response = httpResponseMessage;

            // Mock 設定
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(It.Is<string>(key => key == adminSessionKey))).Returns(adminSession);
            sessionServiceMock.Setup(s => s.GetSessionId()).Returns(sessionId);
            sessionServiceMock.Setup(s => s.ClearSerssion());
            redisServiceMock.Setup(s => s.GetValue<AdminRedis>(It.Is<string>(key => key == redisKey))).Returns(adminRedis);
            redisServiceMock.Setup(s => s.DeleteKey(redisKey));

            // Act
            adminKickOutFilter.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            ResultResponse resultResponse = actionContext.Response.Content.ReadAsAsync<ResultResponse>().Result;
            Assert.AreEqual(ErrorCodeDefine.KickOut, resultResponse.ErrorCode);
        }

        [TestMethod]
        public void 使用者被禁用_是_回傳被禁用()
        {
            // Arrange
            string adminSessionKey = "AdminSession";
            AdminSession adminSession = new AdminSession() { AdminId = 1, Identity = AdminIdentity.Accountant };
            string redisKey = "Admin" + adminSession.AdminId;
            string sessionId = "asdawdpl21";
            AdminRedis adminRedis = new AdminRedis() { ErrorCode = ErrorCodeDefine.Baned, SessionId = sessionId };

            // 回傳設定
            ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.Baned };

            // 把 response 序列化成 JSON 字串
            string json = JsonConvert.SerializeObject(response);

            // 設定請求的回傳
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            actionContext.Response = httpResponseMessage;

            // Mock 設定
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(It.Is<string>(key => key == adminSessionKey))).Returns(adminSession);
            sessionServiceMock.Setup(s => s.GetSessionId()).Returns(sessionId);
            sessionServiceMock.Setup(s => s.ClearSerssion());
            redisServiceMock.Setup(s => s.GetValue<AdminRedis>(It.Is<string>(key => key == redisKey))).Returns(adminRedis);
            redisServiceMock.Setup(s => s.DeleteKey(redisKey));

            // Act
            adminKickOutFilter.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            ResultResponse resultResponse = actionContext.Response.Content.ReadAsAsync<ResultResponse>().Result;
            Assert.AreEqual(ErrorCodeDefine.Baned, resultResponse.ErrorCode);
        }

        [TestMethod]
        public void 使用者權限被更動_是_回傳被權限被更動()
        {
            // Arrange
            string adminSessionKey = "AdminSession";
            AdminSession adminSession = new AdminSession() { AdminId = 1, Identity = AdminIdentity.Accountant };
            string redisKey = "Admin" + adminSession.AdminId;
            string sessionId = "asdawdpl21";
            AdminRedis adminRedis = new AdminRedis() { ErrorCode = ErrorCodeDefine.PermissionModified, SessionId = sessionId };

            // 回傳設定
            ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.PermissionModified };

            // 把 response 序列化成 JSON 字串
            string json = JsonConvert.SerializeObject(response);

            // 設定請求的回傳
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            actionContext.Response = httpResponseMessage;

            // Mock 設定
            sessionServiceMock.Setup(s => s.GetSession<AdminSession>(It.Is<string>(key => key == adminSessionKey))).Returns(adminSession);
            sessionServiceMock.Setup(s => s.GetSessionId()).Returns(sessionId);
            sessionServiceMock.Setup(s => s.ClearSerssion());
            redisServiceMock.Setup(s => s.GetValue<AdminRedis>(It.Is<string>(key => key == redisKey))).Returns(adminRedis);
            redisServiceMock.Setup(s => s.DeleteKey(redisKey));

            // Act
            adminKickOutFilter.OnActionExecuting(actionContext);

            // Assert
            Assert.IsNotNull(actionContext.Response);
            ResultResponse resultResponse = actionContext.Response.Content.ReadAsAsync<ResultResponse>().Result;
            Assert.AreEqual(ErrorCodeDefine.PermissionModified, resultResponse.ErrorCode);
        }
    }
}
