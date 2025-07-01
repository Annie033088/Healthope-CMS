import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/transaction',
        name: 'HealthopeTransaction',
        component: () => import('@/views/Transaction/HealthopeTransaction'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.SelectTransaction }]
        }
    },
];