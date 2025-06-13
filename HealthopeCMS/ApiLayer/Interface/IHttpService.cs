using System.Net.Http;
using System.Web.Http.Controllers;

namespace ApiLayer.Interface
{
    public interface IHttpService
    {
        /// <summary>
        /// 取得 controller 名
        /// </summary>
        string GetControllerName(HttpActionContext actionContext);

        /// <summary>
        /// 取得 action 名
        /// </summary>
        string GetActionName(HttpActionContext actionContext);

        /// <summary>
        /// 取得當前源地址
        /// </summary>
        string GetRootPath();

        /// <summary>
        /// 發送 post, 回傳以 json string 接收
        /// </summary>
        string SendPost(string url, StringContent content);
    }
}
