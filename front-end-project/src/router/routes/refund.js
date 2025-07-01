import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/refund',
        name: 'HealthopeRefund',
        component: () => import('@/views/Refund/HealthopeRefund'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.SelectTransaction }]
        }
    },
];