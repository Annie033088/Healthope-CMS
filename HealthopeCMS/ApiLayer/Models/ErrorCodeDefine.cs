namespace ApiLayer.Models
{
    public enum ErrorCodeDefine
    {
        /// <summary>
        /// 預設
        /// </summary>
        Default = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 被他人踢出
        /// </summary>
        KickOut = 2,

        /// <summary>
        /// 被Ban掉
        /// </summary>
        Baned = 3,

        /// <summary>
        /// 權限已被修改
        /// </summary>
        PermissionModified = 4,

        /// <summary>
        /// 無效輸入
        /// </summary>
        InvalidFormatOrEntry = 5,

        /// <summary>
        /// 伺服器錯誤
        /// </summary>
        ServerError = 6,

        /// <summary>
        /// 無權限
        /// </summary>
        NoPermission = 7,

        /// <summary>
        /// 使用者未登入
        /// </summary>
        UserNotLogin = 8,

        /// <summary>
        /// 登入失敗
        /// </summary>
        LoginFailed = 9,

        /// <summary>
        /// 創建失敗
        /// </summary>
        CreateFailed = 10,

        /// <summary>
        /// 修改失敗
        /// </summary>
        ModifiedFailed = 11,

        /// <summary>
        /// 刪除失敗
        /// </summary>
        DeleteFailed = 12,

        /// <summary>
        /// 取得特定資料失敗
        /// </summary>
        GetFailed = 13,

        /// <summary>
        /// 資料已被異動
        /// </summary>
        HasBeenModified = 14,

        /// <summary>
        /// 超級管理員不得修改
        /// </summary>
        ModifySuperAdminFailed = 15,

        /// <summary>
        /// 手機重複
        /// </summary>
        DuplicatePhone = 16,

        /// <summary>
        /// 帳號重複
        /// </summary>
        DuplicateAccount = 17,

        /// <summary>
        /// (手機)已驗證
        /// </summary>
        AlreadyVerify = 18,

        /// <summary>
        /// OTP 還在冷卻時間不可發送
        /// </summary>
        OtpCooldown = 19,

        /// <summary>
        /// 驗證失敗
        /// </summary>
        VerifyFail = 20,
    }
}