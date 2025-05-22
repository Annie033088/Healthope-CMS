import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/groupClass/showcase',
        name: 'GroupClassShowcase',
        component: () => import('@/views/GroupClass/GroupClassShowcase'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditGroupClassShowcase }
                , { adminPermission: adminPermission.SelectGroupClassShowcase }]
        }
    },
    {
        path: '/groupClass/showcase/add',
        name: 'AddGroupClassShowcase',
        component: () => import('@/views/GroupClass/AddGroupClassShowcase'),
        meta: { requireAuth: [{ adminPermission: adminPermission.EditGroupClassShowcase }] }
    },
    {
        path: '/groupClass/showcase/detail',
        name: 'GroupClassShowcaseDetail',
        component: () => import('@/views/GroupClass/GroupClassShowcaseDetail'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditGroupClassShowcase }
                , { adminPermission: adminPermission.SelectGroupClassShowcase }
            ]
        }
    },
    {
        path: '/groupClass/showcase/edit',
        name: 'EditGroupClassShowcase',
        component: () => import('@/views/GroupClass/EditGroupClassShowcase'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditGroupClassShowcase }]
        }
    },
    {
        path: '/groupClass/schedule',
        name: 'GroupClassSchedule',
        component: () => import('@/views/GroupClass/GroupClassSchedule'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditGroupClassSchedule },
            { adminPermission: adminPermission.SelectGroupClassSchedule }]
        }
    },

];