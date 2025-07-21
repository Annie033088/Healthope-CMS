namespace DomainLayer.Interface
{
    public interface IAppConfigProvider
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

        /// <summary>
        /// 取得 config 檔的 connection string
        /// </summary>
        string GetConnectionString();
    }
}
