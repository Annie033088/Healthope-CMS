import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/coach',
        name: 'HealthopeCoach',
        component: ()=> import('@/views/Coach/HealthopeCoach'),
        meta: { requireAuth: [{ adminPermission: adminPermission.SelectCoach },
             {adminPermission: adminPermission.AddCoach}, { adminPermission: adminPermission.EditCoach }] }
    },
    {
        path: '/coach/add',
        name: 'HealthopeAddCoach',
        component: ()=> import('@/views/Coach/HealthopeAddCoach'),
        meta: { requireAuth: [{ adminPermission: adminPermission.AddCoach }] }
    },
    {
        path: '/coach/edit',
        name: 'HealthopeEditCoach',
        component: ()=> import('@/views/Coach/HealthopeEditCoach'),
        meta: { requireAuth: [{ adminPermission: adminPermission.EditCoach }] }
    },
];