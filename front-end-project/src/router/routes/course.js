import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/groupClass/showcase',
        name: 'GroupClassShowCase',
        component: ()=> import('@/views/Course/GroupClassShowcase'),
        meta: { requireAuth: [{ adminPermission: adminPermission.AddGroupClassShowcase }] }
    },
];