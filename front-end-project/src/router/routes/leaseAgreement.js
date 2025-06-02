import { adminPermission } from '@/utils/globalSetting';
export default [
  {
    path: '/leaseAgreement',
    name: 'LeaseAgreement',
    component: () => import('@/views/LeaseAgreement/LeaseAgreement'),
     meta: { requireAuth: [{ adminPermission: adminPermission.EditLeaseAgreement }, 
        { adminPermission: adminPermission.SelectLeaseAgreement }] }
  },
  {
    path: '/leaseAgreement/add',
    name: 'AddLeaseAgreement',
    component: () => import('@/views/LeaseAgreement/AddLeaseAgreement'),
     meta: { requireAuth: [{ adminPermission: adminPermission.EditLeaseAgreement }] }
  }
];