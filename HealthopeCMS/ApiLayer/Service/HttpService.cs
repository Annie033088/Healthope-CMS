using System;
using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<string> SendPostAsync(string url, StringContent content, TimeSpan? timeOut = null)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    if (timeOut != null) httpClient.Timeout = timeOut.Value; // ✅ 設定 timeout

                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    response.EnsureSuccessStatusCode(); // 如果不是 2xx，會丟出例外
                    string responseString = await response.Content.ReadAsStringAsync();
                    return responseString;
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException("連線逾時", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}