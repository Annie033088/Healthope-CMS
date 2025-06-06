import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/order/add',
        name: 'HealthopeAddOrder',
        component: () => import('@/views/Order/HealthopeAddOrder'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.AddOrder }
                , { adminPermission: adminPermission.EditOrder }]
        }
    },
    {
        path: '/order/checkout',
        name: 'HealthopeCheckoutOrder',
        component: () => import('@/views/Order/HealthopeCheckoutOrder'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.AddOrder }
                , { adminPermission: adminPermission.EditOrder }]
        },
        props: route => ({ order: route.params.order })
    },
];