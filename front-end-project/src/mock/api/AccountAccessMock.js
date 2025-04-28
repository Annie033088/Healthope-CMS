import Vue from 'vue';

export default function (mock) {
    mock.onPost("/api/AccountAccess/VerifyAdminLogin").reply(config => {
        const loginDto = JSON.parse(config.data);

        // 模擬超級管理員登入
        if (loginDto.Account === "superAdmin123" && loginDto.Pwd === "superAdmin456") {
            Vue.prototype.$loginFlag = true;
            return [200, { ErrorCode: 1, ApiDataObject: [1, 2] }];
        }

        // 模擬一般管理員登入
        if (loginDto.Account === "generalAdmin123" && loginDto.Pwd === "generalAdmin456") {
            Vue.prototype.$loginFlag = true;
            return [200, { ErrorCode: 1, ApiDataObject: [2] }];
        }

        return [200, { ErrorCode: 9 }];
    })

    mock.onPost("/api/AccountAccess/AdminLogout").reply(() => {
        Vue.prototype.$loginFlag = false;
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/AccountAccess/LoggedIn").reply(() => {
        if (Vue.prototype.$loginFlag) {
            return [200, { ErrorCode: 1 }]
        }
        return [200, { ErrorCode: 6 }]
    })
}