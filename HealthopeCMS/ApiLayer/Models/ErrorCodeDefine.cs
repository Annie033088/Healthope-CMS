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

        /// <summary>
        /// 名稱重複
        /// </summary>
        DuplicateName = 21,

        /// <summary>
        /// 時間及地點重複
        /// </summary>
        DuplicatePlaceAndTime = 22,

        /// <summary>
        /// 時間及教練重複
        /// </summary>
        DuplicateCoachAndTime = 23,

        /// <summary>
        /// 已有啟用中的租約
        /// </summary>
        ActiveLeaseAgreement = 24,

        /// <summary>
        /// 已有啟用中的字軌
        /// </summary>
        ActiveInvoiceTrackNumber = 25,

        /// <summary>
        /// 會員被 ban
        /// </summary>
        MemberBaned = 26,

        /// <summary>
        /// 手機尚未驗證
        /// </summary>
        PhoneNotVerify = 27,

        /// <summary>
        /// 方案不可用(狀態為無效)
        /// </summary>
        PlanNotAvailable = 28,

        /// <summary>
        /// 字軌未設定(請使用者去設定)
        /// </summary>
        TrackNotSet = 29,

        /// <summary>
        /// 教練被設置無效
        /// </summary>
        CoachBaned = 30,

        /// <summary>
        /// 付款失敗
        /// </summary>
        PayFailed = 31,

        /// <summary>
        /// 刷卡成功但交易紀錄更新失敗
        /// </summary>
        CardPaySuccessTransactionUpdateFail = 32,

        /// <summary>
        /// 交易紀錄更新成功但訂單更新失敗
        /// </summary>
        TransactionSuccessOrderUpdateFail = 33,

        /// <summary>
        /// 時間/日期超過
        /// </summary>
        TimeExceeded = 34,

        /// <summary>
        /// 商品已使用
        /// </summary>
        ProductUsed = 35,

        /// <summary>
        /// 再次確認是否執行這個操作
        /// </summary>
        ConfirmAgain = 36,

        /// <summary>
        /// 已預約/預約中課程, 不得此時進行解約/退費 動作
        /// </summary>
        HasBooked = 37,

        /// <summary>
        /// 發票不存在(未開)
        /// </summary>
        InvoiceNotExist = 38,

        /// <summary>
        /// 無法開立跨期發票，請手動開立發票
        /// </summary>
        CantPrintCrossDateInvoice = 39,
    }
}