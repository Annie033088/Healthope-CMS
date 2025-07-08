import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/financialStatements',
        name: 'FinancialStatements',
        component: () => import('@/views/Report/FinancialStatements'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.SelectFinancialStatements }]
        }
    },
    {
        path: '/coachClassPerformanceReport',
        name: 'CoachClassPerformanceReport',
        component: () => import('@/views/Report/CoachClassPerformanceReport'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.SelectCoachReport }]
        }
    },
];