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
    DeleteFailed: 12
};

//設定errorCode對應資料
export function errorCodeToMessage(errorCode) {
    console.log("!")
    let message;

    switch (errorCode) {
        case 1:
            message = "請求成功";
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
        default:
    }
}

export const adminPermission = {
    // 無
    None:0,

    // 管理者相關權限
    EditAdmin:1,

    // 
    EditMember:2
}

export default function adminIdentityToText (identity) {
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
