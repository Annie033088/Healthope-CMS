namespace DomainLayer.Interface
{
    public interface IAppSetting
    {
        /// <summary>
        /// sa 帳號
        /// </summary>
        string GetSuperAdminAccount();

        /// <summary>
        /// sa Hash
        /// </summary>
        string GetSuperAdminHash();

        /// <summary>
        /// 取得 config 擋 key - value
        /// </summary>
        string GetConfigurationAppsetting(string key);
    }
}
