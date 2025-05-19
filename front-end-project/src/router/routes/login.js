export default [
    {
        path: '/login',
        name: 'HealthopeLogin',
        component: () => import('@/views/HealthopeLogin'),
        meta: { requireAuth: null } // 登入頁面不需要權限
    }
];