import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/groupClass/showcase',
        name: 'GroupClassShowcase',
        component: ()=> import('@/views/Course/GroupClassShowcase'),
        meta: { requireAuth: [{ adminPermission: adminPermission.AddGroupClassShowcase }] }
    },
    {
        path: '/groupClass/showcase/add',
        name: 'AddGroupClassShowcase',
        component: ()=> import('@/views/Course/AddGroupClassShowcase'),
        meta: { requireAuth: [{ adminPermission: adminPermission.AddGroupClassShowcase }] }
    },
];