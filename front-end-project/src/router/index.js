import VueRouter from "vue-router";
import Vue from 'vue';
import homeRoutes from './routes/home';
import loginRoutes from './routes/login';
import otherRoutes from './routes/other';
import adminRoutes from './routes/admin';
import memberRoutes from './routes/member';
import coachRoutes from './routes/coach';
import groupClassRoutes from './routes/groupClass';
import planRoutes from './routes/planTemplate';
import memberClassRoutes from './routes/memberClass';
import termRoutes from './routes/term';
import leaseAgreementRoutes from './routes/leaseAgreement';
import orderRoutes from './routes/order';
import settingRoutes from './routes/setting';
import transactionRoutes from './routes/transaction';
import refundRoutes from './routes/refund';
import invoiceRoutes from './routes/invoice';

import axios from '../plugins/axios';
import { errorCodeDefine } from '../utils/globalSetting';

const routes = [
    // login
    ...loginRoutes,
    // 其他 route ( 包括修改自己密碼 )
    ...otherRoutes,
    // 管理者相關
    ...adminRoutes,
    // 會員相關
    ...memberRoutes,
    // 教練相關
    ...coachRoutes,
    // 團課相關
    ...groupClassRoutes,
    // home
    ...homeRoutes,
    // plan
    ...planRoutes,
    // memberClass
    ...memberClassRoutes,
    // 條款
    ...termRoutes,
    // 租約
    ...leaseAgreementRoutes,
    // 訂單
    ...orderRoutes,
    // 設定
    ...settingRoutes,
    // 付款紀錄
    ...transactionRoutes,
    // 退款與違約金紀錄
    ...refundRoutes,
    // 發票資訊
    ...invoiceRoutes,
]

const router = new VueRouter({
    routes,
    mode: 'history'
});

router.beforeEach(async (to, from, next) => {
    const requireAuth = to.meta.requireAuth;
    let havePermissionDto;

    if (requireAuth === "login" || null) {
        havePermissionDto = null
    } else {
        havePermissionDto = requireAuth
    }

    const response = await axios.post("/api/AccountAccess/HavePermission", havePermissionDto);

    // 如果使用者未登入
    if (response.data.ErrorCode === errorCodeDefine.UserNotLogin) {
        if (to.name === 'HealthopeLogin') {
            Vue.prototype.$loginFlag = false;
            return next();
        }
        Vue.prototype.$loginFlag = false;
        return next({ name: 'HealthopeLogin' });
    } else {
        Vue.prototype.$loginFlag = true;
    }

    // 如果使用者已經登入，不讓他進入登入頁，直接導到首頁
    if (to.name === 'HealthopeLogin' && response.data.ErrorCode !== errorCodeDefine.UserNotLogin) {
        Vue.prototype.$loginFlag = true;
        return next({ name: 'HealthopeHome' });
    }

    // 有權限且有登入
    if (response.data.ErrorCode === errorCodeDefine.Success) {
        Vue.prototype.$loginFlag = true;
        return next();
    }
    // 其他情況只剩沒權限, 轉倒到主頁
    else {
        return next({ name: 'HealthopeHome' });
    }
});

export default router