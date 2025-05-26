import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/plan',
        name: 'HealthopePlan',
        component: () => import('@/views/Plan/HealthopePlan'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }
                , { adminPermission: adminPermission.SelectPlan }]
        }
    },
    {
        path: '/plan/add',
        name: 'HealthopeAddPlan',
        component: () => import('@/views/Plan/HealthopeAddPlan'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }]
        }
    },
];