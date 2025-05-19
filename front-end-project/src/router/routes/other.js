export default [
    {
        path: '/editSelfPwd',
        name: 'HealthopeEditSelfPwd',
        component:  () => import( '@/views/Other/HealthopeEditSelfPwd'),
        meta: { requireAuth: 'login' } // 主頁只要有登入就好
    }
];