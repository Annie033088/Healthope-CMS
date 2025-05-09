import VueRouter from "vue-router";
import Vue from 'vue';
import HealthopeHome from '@/views/HealthopeHome';
import HealthopeLogin from '@/views/HealthopeLogin';
import HealthopeAdmin from '@/views/Admin/HealthopeAdmin';
import HealthopeAddAdmin from '@/views/Admin/HealthopeAddAdmin';
import HealthopeEditAdmin from '@/views/Admin/HealthopeEditAdmin';
import HealthopeMember from '@/views/Member/HealthopeMember';
import HealthopeEditMember from '@/views/Member/HealthopeEditMember';
import HealthopeEditSelfPwd from '@/views/Other/HealthopeEditSelfPwd';
import axios from '../plugins/axios';
import { errorCodeDefine, adminPermission } from '../utils/globalSetting';

const routes = [
    {
        path: '/login',
        name: 'HealthopeLogin',
        component: HealthopeLogin,
        meta: { requireAuth: null } // 登入頁面不需要權限
    },
    {
        path: '/',
        name: 'HealthopeHome',
        component: HealthopeHome,
        meta: { requireAuth: 'login' } // 主頁只要有登入就好
    },
    {
        path: '/editSelfPwd',
        name: 'HealthopeEditSelfPwd',
        component: HealthopeEditSelfPwd,
        meta: { requireAuth: 'login' } // 主頁只要有登入就好
    },
    {
        path: '/admin',
        name: 'HealthopeAdmin',
        component: HealthopeAdmin,
        meta: { requireAuth: [{ adminPermission: adminPermission.EditAdmin }] }
    },
    {
        path: '/admin/add',
        name: 'HealthopeAddAdmin',
        component: HealthopeAddAdmin,
        meta: { requireAuth: [{ adminPermission: adminPermission.EditAdmin }] }
    },
    {
        path: '/admin/edit',
        name: 'HealthopeEditAdmin',
        component: HealthopeEditAdmin,
        meta: { requireAuth: [{ adminPermission: adminPermission.EditAdmin }] }
    },
    {
        path: '/member',
        name: 'HealthopeMember',
        component: HealthopeMember,
        meta: { requireAuth: [{ adminPermission: adminPermission.EditMember }] }
    },
    {
        path: '/member/edit',
        name: 'HealthopeEditMember',
        component: HealthopeEditMember,
        meta: { requireAuth: [{ adminPermission: adminPermission.EditMember }] }
    },
    {
        path: '*',
        name: 'HealthopeDefault',
        component: HealthopeHome,
        meta: { requireAuth: 'login' }
    }
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