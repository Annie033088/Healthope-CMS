import VueRouter from "vue-router";
import Vue from 'vue';
import HealthopeHome from '@/views/HealthopeHome';
import HealthopeLogin from '@/views/HealthopeLogin';
import HealthopeAdmin from '@/views/Admin/HealthopeAdmin';
import HealthopeAddAdmin from '@/views/Admin/HealthopeAddAdmin';
import HealthopeMember from '@/views/Member/HealthopeMember';
import axios from 'axios';
import { errorCodeDefine } from '../utils/globalSetting';

const routes = [
    {
        path: '/login',
        name: 'HealthopeLogin',
        component: HealthopeLogin,
        meta: { requiresAuth: false } // 登入頁面不需要驗證
    },
    {
        path: '/',
        name: 'HealthopeHome',
        component: HealthopeHome,
        meta: { requiresAuth: true }
    },
    {
        path: '/admin',
        name: 'HealthopeAdmin',
        component: HealthopeAdmin,
        meta: { requiresAuth: true }
    },
    {
        path: '/admin/add',
        name: 'HealthopeAddAdmin',
        component: HealthopeAddAdmin,
        meta: { requiresAuth: true }
    },
    {
        path: '/member',
        name: 'HealthopeMember',
        component: HealthopeMember,
        meta: { requiresAuth: true }
    },
    {
        path: '*',
        name: 'HealthopeDefault',
        component: HealthopeHome,
        meta: { requiresAuth: true }
    }
]

const router = new VueRouter({
    routes,
    mode: 'history'
});

router.beforeEach(async (to, from, next) => {
    const response = await axios.post("/api/AccountAccess/LoggedIn");

    // 這個頁面需要登入
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (response.data.ErrorCode === errorCodeDefine.Success) {
            Vue.prototype.$loginFlag = true;
            next();
        } else {
            Vue.prototype.$loginFlag = false;
            next({ name: 'HealthopeLogin' });
        }
    }
    // 如果使用者已經登入，不讓他進入登入頁，直接導到首頁
    else if (to.name === 'HealthopeLogin' && response.data.ErrorCode === errorCodeDefine.Success) {
        Vue.prototype.$loginFlag = true;
        next({ name: 'HealthopeHome' });
    }
    // 其他頁面正常進入
    else {
        Vue.prototype.$loginFlag = false;
        next();
    }
});

export default router