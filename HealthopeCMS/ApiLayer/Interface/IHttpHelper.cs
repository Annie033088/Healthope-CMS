using System.Web.Http.Controllers;

namespace ApiLayer.Interface
{
    public interface IHttpHelper
    {
        /// <summary>
        /// 取得 controller 名
        /// </summary>
        string GetControllerName(HttpActionContext actionContext);

        /// <summary>
        /// 取得 action 名
        /// </summary>
        string GetActionName(HttpActionContext actionContext);
    }
}
