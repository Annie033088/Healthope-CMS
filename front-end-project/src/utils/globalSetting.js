export const errorCodeDefine = {
    //預設
    Default: 0,

    //成功
    Success: 1,

    //被他人踢出
    KickOut: 2,

    //被Ban掉
    Baned: 3,

    //權限已被修改
    PermissionModified: 4,

    //無效輸入
    InvalidFormatOrEntry: 5,

    //伺服器錯誤
    ServerError: 6,

    //無權限
    NoPermission: 7,

    //使用者未登入
    UserNotLogin: 8,

    //登入失敗
    LoginFailed: 9,

    //創建失敗
    CreateFailed: 10,

    //修改失敗
    ModifiedFailed: 11,

    //刪除失敗
    DeleteFailed: 12,

    //取得特定資料失敗
    GetFailed: 13,

    // 資料已被異動
    HasBeenModified: 14,

    // 超級管理員不得修改
    ModifySuperAdminFailed: 15,

    // 手機重複
    DuplicatePhone: 16,

    // 帳號重複
    DuplicateAccount: 17,

    // (手機)已驗證
    AlreadyVerify: 18,

    // OTP 還在冷卻時間不可發送
    OtpCooldown: 19,

    // 驗證失敗
    VerifyFail: 20,

    // 名稱重複
    DuplicateName: 21,

    // 時間及地點重複
    DuplicatePlaceAndTime: 22,

    // 時間及教練重複
    DuplicateCoachAndTime: 23,

    // 已有啟用中的租約
    ActiveLeaseAgreement: 24,

    // 已有啟用中的字軌
    ActiveInvoiceTrackNumber: 25,

    // 會員被 ban
    MemberBaned: 26,

    // 手機尚未驗證
    PhoneNotVerify: 27,

    // 方案不可用(狀態為無效)
    PlanNotAvailable: 28,

    // 字軌未設定(請使用者去設定)
    TrackNotSet: 29,

    // 教練被設置無效
    CoachBaned: 30,

    // 付款失敗
    PayFailed: 31,

    // 刷卡成功但交易紀錄更新失敗
    CardPaySuccessTransactionUpdateFail: 32,

    // 交易紀錄更新成功但訂單更新失敗
    TransactionSuccessOrderUpdateFail: 33,

    // 時間/日期超過
    TimeExceeded: 34,

    // 商品已使用
    ProductUsed: 35,

    // 再次確認是否執行這個操作
    ConfirmAgain: 36,

    /// 已預約/預約中課程, 不得此時進行解約/退費 動作
    HasBooked: 37,

    /// 發票不存在(未開)
    InvoiceNotExist: 38,
};


//設定errorCode對應資料
export function errorCodeToMessage(errorCode) {
    let message;

    switch (errorCode) {
        case 1:
            message = "成功!";
            return message;
        case 2:
            message = "您的帳號已被其他使用者踢出";
            return message;
        case 3:
            message = "您的帳號已被禁用";
            return message;
        case 4:
            message = "您的權限已被更動，請重新登入";
            return message;
        case 5:
            message = "請求格式錯誤或無效數據";
            return message;
        case 6:
            message = "伺服器錯誤，請再試一次";
            return message;
        case 7:
            message = "沒有此權限";
            return message;
        case 8:
            message = "使用者未登入";
            return message;
        case 9:
            message = "登入失敗，請再試一次";
            return message;
        case 10:
            message = "新增失敗，請再試一次";
            return message;
        case 11:
            message = "修改失敗，請再試一次";
            return message;
        case 12:
            message = "刪除失敗，請再試一次";
            return message;
        case 13:
            message = "取得資料失敗，請再試一次";
            return message;
        case 14:
            message = "資料已被異動";
            return message;
        case 15:
            message = "超級管理員資料不得修改";
            return message;
        case 16:
            message = "輸入的手機號碼已被註冊";
            return message;
        case 17:
            message = "輸入的帳號已被註冊";
            return message;
        case 18:
            message = "已驗證，不需再驗證";
            return message;
        case 19:
            message = "OTP 還在冷卻時間不可發送";
            return message;
        case 20:
            message = "驗證失敗";
            return message;
        case 21:
            message = "輸入的名稱重複";
            return message;
        case 22:
            message = "時間及地點重複";
            return message;
        case 23:
            message = "時間及教練重複";
            return message;
        case 24:
            message = "已有啟用中的租約";
            return message;
        case 25:
            message = "已有啟用中的字軌，請先中斷啟用中的字軌";
            return message;
        case 26:
            message = "會員被禁用";
            return message;
        case 27:
            message = "手機尚為驗證";
            return message;
        case 28:
            message = "方案不可用";
            return message;
        case 29:
            message = "字軌未設定，請前去設定";
            return message;
        case 30:
            message = "教練狀態無效";
            return message;
        case 31:
            message = "付款失敗";
            return message;
        case 32:
            message = "刷卡成功但交易紀錄更新失敗!";
            return message;
        case 33:
            message = "交易紀錄更新成功但訂單更新失敗!";
            return message;
        case 34:
            message = "時間/日期超過";
            return message;
        case 35:
            message = "商品已使用";
            return message;
        case 37:
            message = "已有 預約中/已預約 課程，請先取消課程再執行動作";
            return message;
        case 38:
            message = "發票尚未開立";
            return message;
        default:
            message = "";
            return message;
    }
}

export const adminPermission = {
    // 無
    None: 0,

    // 管理者相關權限
    EditAdmin: 1,

    // 查詢會員權限
    SelectMember: 2,

    // 修改會員權限
    EditMember: 3,

    // 查詢教練權限
    SelectCoach: 4,

    // 新增教練權限
    AddCoach: 5,

    // 修改教練權限
    EditCoach: 6,

    // 增刪修 展示團課 權限
    EditGroupClassShowcase: 7,

    // 查詢 展示團課 權限
    SelectGroupClassShowcase: 8,

    // 增刪修 團課表 權限
    EditGroupClassSchedule: 9,

    // 查詢 團課表 權限
    SelectGroupClassSchedule: 10,

    // 增刪修 團課表 權限
    EditPlan: 11,

    // 查詢 團課表 權限
    SelectPlan: 12,

    // 增修 會員預約課程
    EditMemberClass: 13,

    // 查詢 會員預約課程
    SelectMemberClass: 14,

    // 修改條款
    EditTerm: 15,

    // 查詢條款
    SelectTerm: 16,

    // 修改租約
    EditLeaseAgreement: 17,

    // 查詢租約
    SelectLeaseAgreement: 18,

    // 建立訂單(銷售員)
    AddOrder: 19,

    // 修改訂單 (EX:退款)
    EditOrder: 20,

    // 查詢訂單
    SelectOrder: 21,

    // 查詢付款紀錄
    SelectTransaction: 22,
}

export default function adminIdentityToText(identity) {
    let identityText;

    switch (identity) {
        case 0:
            identityText = "無";
            return identityText;
        case 1:
            identityText = "超級管理員";
            return identityText;
        case 2:
            identityText = "一般管理員";
            return identityText;
        case 3:
            identityText = "接待員";
            return identityText;
        case 4:
            identityText = "會計";
            return identityText;
        case 5:
            identityText = "課程管理員";
            return identityText;
        case 6:
            identityText = "教練管理員";
            return identityText;
        case 7:
            identityText = "業務";
            return identityText;
        default:
    }
}
