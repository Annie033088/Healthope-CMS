namespace ApiLayer.Interface
{
    public interface ISessionService
    {
        /// <summary>
        /// SessionId
        /// </summary>
        string GetSessionId();

        /// <summary>
        /// 設定 Session
        /// </summary>
        void SaveSession<T>(string key, T sessionObject);

        /// <summary>
        /// 取得 Session
        /// </summary>
        T GetSession<T>(string key);

        /// <summary>
        /// 設定 Cookie
        /// </summary>
        void SaveCookie<T>(string name, T value, int expiryHour);

        /// <summary>
        /// 清除當前會話(登出)
        /// </summary>
        void ClearSerssion();
    }
}
