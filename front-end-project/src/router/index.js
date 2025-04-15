import VueRouter from "vue-router";
//import axios from 'axios';
//import { errorCodeDefine } from '../utils/globalSetting';

const routes = [
    /* {
        path: '/login',
        name: 'AppLogin',
        component: AppLogin,
        meta: { requiresAuth: false } // 登入頁面不需要驗證
    }, */
]

const router = new VueRouter({
    routes,
    mode: 'history'
});

/*router.beforeEach(async (to, from, next) => {
    if (document.cookie.trim() === "" && to.name !== 'AppLogin') {
        next({ name: 'AppLogin' })
    }

    const response = await axios.post("/api/Login/LoggedIn");

    if (to.matched.some(record => record.meta.requiresAuth)) {
        // 這個頁面需要登入
        if (response.data.ErrorCode === errorCodeDefine.Success) {
            next();
        } else {
            next({ name: 'AppLogin' });
        }
    }
    else if (to.name === 'AppLogin' && response.data.ErrorCode === errorCodeDefine.Success) {
        // 如果使用者已經登入，不讓他進入登入頁，直接導到首頁
        next({ name: 'AppHome' });
    }
    else {
        next(); // 其他頁面正常進入
    }
});*/

export default router