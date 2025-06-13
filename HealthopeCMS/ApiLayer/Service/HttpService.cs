using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using ApiLayer.Interface;

namespace ApiLayer.Service
{
    public class HttpService : IHttpService
    {

        public string GetControllerName(HttpActionContext actionContext)
        {
            try
            {
                return actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetActionName(HttpActionContext actionContext)
        {
            try
            {
                return actionContext.ActionDescriptor.ActionName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetRootPath()
        {
            try
            {
                return HttpContext.Current.Server.MapPath("~/");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SendPost(string url, StringContent content)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;
                return responseString;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}