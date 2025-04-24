using System;
using System.Text;
using System.Web;
using ApiLayer.Interface;
using Newtonsoft.Json;

namespace ApiLayer.Service
{
    public class SessionService : ISessionService
    {
        /// <summary>
        /// SessionId
        /// </summary>
        public string GetSessionId()
        {
            try
            {
                return HttpContext.Current.Session.SessionID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得 Session
        /// </summary>
        public T GetSession<T>(string key)
        {
            try
            {
                return (T)HttpContext.Current.Session[key];
            }
            catch (Exception)
            {
                // 不記 log，由 controller 統一處理
                throw;
            }
        }

        /// <summary>
        /// 設定 Session
        /// </summary>
        public void SaveSession<T>(string key, T sessionObject)
        {
            try
            {
                HttpContext.Current.Session[key] = sessionObject;
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 設定 cookie
        /// </summary>
        public void SaveCookie<T>(string name, T value, int expiryHour)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(value);
                string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
                HttpCookie cookie = new HttpCookie(name, base64)
                {
                    HttpOnly = false,
                    Secure = false,
                    Expires = DateTime.Now.AddHours(expiryHour)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 清除當前會話(登出)
        /// </summary>
        public void ClearSerssion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            foreach (string cookieName in HttpContext.Current.Request.Cookies.AllKeys)
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            return;
        }
    }
}