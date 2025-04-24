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
    }
}
