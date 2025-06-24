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
                , { adminPermission: adminPermission.EditOrder },],
            disableBack: true,
            backPath: "/order"
        },
    },
    {
        path: '/order',
        name: 'HealthopeOrder',
        component: () => import('@/views/Order/HealthopeOrder'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.AddOrder }
                , { adminPermission: adminPermission.EditOrder }, { adminPermission: adminPermission.SelectOrder }]
        },
    },
    {
        path: '/order/beforeCheckout',
        name: 'HealthopeBerforeCheckout',
        component: () => import('@/views/Order/HealthopeBerforeCheckout'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.AddOrder }
                , { adminPermission: adminPermission.EditOrder }]
        },
    },
    {
        path: '/order/detail',
        name: 'HealthopeOrderDetail',
        component: () => import('@/views/Order/HealthopeOrderDetail'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.AddOrder }
                , { adminPermission: adminPermission.EditOrder }, { adminPermission: adminPermission.SelectOrder }]
        }
    },
];