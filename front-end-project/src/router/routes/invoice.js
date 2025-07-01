import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/invoice',
        name: 'HealthopeInvoice',
        component: () => import('@/views/Invoice/HealthopeInvoice'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.SelectTransaction }, { adminPermission: adminPermission.AddOrder }
                , { adminPermission: adminPermission.EditOrder }, { adminPermission: adminPermission.SelectOrder }]
        }
    },
];