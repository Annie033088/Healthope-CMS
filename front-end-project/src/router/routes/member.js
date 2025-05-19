import { adminPermission } from '@/utils/globalSetting';
export default [
    // 查詢會員
    {
        path: '/member',
        name: 'HealthopeMember',
        component: () => import('@/views/Member/HealthopeMember'),
        meta: { requireAuth: [{ adminPermission: adminPermission.EditMember }, { adminPermission: adminPermission.SelectMember }] }
    },
    // 修改會員
    {
        path: '/member/edit',
        name: 'HealthopeEditMember',
        component: () => import('@/views/Member/HealthopeEditMember'),
        meta: { requireAuth: [{ adminPermission: adminPermission.EditMember }] }
    },
    // 查詢會員細項資料
    {
        path: '/member/detail',
        name: 'HealthopeMemberDetail',
        component: () => import('@/views/Member/HealthopeMemberDetail'),
        meta: { requireAuth: [{ adminPermission: adminPermission.SelectMember }, { adminPermission: adminPermission.EditMember }] }
    },
];