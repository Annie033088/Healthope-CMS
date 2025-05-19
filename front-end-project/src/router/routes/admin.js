import { adminPermission } from '@/utils/globalSetting';
export default [
    // 查詢管理者
    {
        path: '/admin',
        name: 'HealthopeAdmin',
        component: () => import('@/views/Admin/HealthopeAdmin'),
        meta: { requireAuth: [{ adminPermission: adminPermission.EditAdmin }] }
    },
    // 新增管理者
    {
        path: '/admin/add',
        name: 'HealthopeAddAdmin',
        component: () => import('@/views/Admin/HealthopeAddAdmin'),
        meta: { requireAuth: [{ adminPermission: adminPermission.EditAdmin }] }
    },
    // 修改管理者
    {
        path: '/admin/edit',
        name: 'HealthopeEditAdmin',
        component: () => import('@/views/Admin/HealthopeEditAdmin'),
        meta: { requireAuth: [{ adminPermission: adminPermission.EditAdmin }] }
    },
];