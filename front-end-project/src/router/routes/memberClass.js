import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/memberPersonalClass',
        name: 'MemberPersonalClass',
        component: () => import('@/views/MemberClass/MemberPersonalClass'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditMemberClass }
                , { adminPermission: adminPermission.SelectMemberClass }]
        }
    },
];