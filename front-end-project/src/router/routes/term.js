import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/term',
        name: 'HealthopeTerm',
        component: () => import('@/views/Term/HealthopeTerm'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditTerm }
                , { adminPermission: adminPermission.SelectTerm }]
        }
    },
    {
        path: '/term/add',
        name: 'HealthopeAddTerm',
        component: () => import('@/views/Term/HealthopeAddTerm'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditTerm }]
        }
    },
];