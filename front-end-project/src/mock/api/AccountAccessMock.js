export default function (mock) {
    mock.onPost("/api/AccountAccess/VerifyAdminLogin").reply(config => {
        const loginDto = JSON.parse(config.data);

        // 模擬超級管理員登入
        if (loginDto.Account === "superAdmin123" && loginDto.Pwd === "superAdmin456") {
            return [200, { ErrorCode: 1 }];
        } else {
            return [200, { ErrorCode: 9 }];
        }
    })

    mock.onPost("/api/AccountAccess/AdminLogout").reply(() => {
        return [200, {  ErrorCode: 6 }]
    })

    mock.onPost("/api/AccountAccess/LoggedIn").reply(() => {
    return [200, {  ErrorCode: 1 }]
    })
}