import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/plan/membershipPlan',
        name: 'MembershipPlanTemplate',
        component: () => import('@/views/PlanTemplate/MembershipPlanTemplate'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }
                , { adminPermission: adminPermission.SelectPlan }]
        }
    },
    {
        path: '/plan/membershipPlan/edit',
        name: 'EditMembershipPlanTemplate',
        component: () => import('@/views/PlanTemplate/EditMembershipPlanTemplate'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }]
        }
    },
    {
        path: '/plan/personalTrainingPackage',
        name: 'PersonalTrainingPlanTemplate',
        component: () => import('@/views/PlanTemplate/PersonalTrainingPlanTemplate'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }
                , { adminPermission: adminPermission.SelectPlan }]
        }
    },
    {
        path: '/plan/personalTrainingPackage/edit',
        name: 'EditPersonalTrainingPlanTemplate',
        component: () => import('@/views/PlanTemplate/EditPersonalTrainingPlanTemplate'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }]
        }
    },
    {
        path: '/plan/ticket',
        name: 'TicketPlanTemplate',
        component: () => import('@/views/PlanTemplate/TicketPlanTemplate'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }
                , { adminPermission: adminPermission.SelectPlan }]
        }
    },
    {
        path: '/plan/add',
        name: 'AddPlanTemplate',
        component: () => import('@/views/PlanTemplate/AddPlanTemplate'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditPlan }]
        }
    },
];