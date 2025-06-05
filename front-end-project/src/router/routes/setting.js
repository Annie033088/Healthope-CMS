import { adminPermission } from '@/utils/globalSetting';
export default [
    {
        path: '/setting/invoiceTrackNumber',
        name: 'InvoiceTrackNumber',
        component: () => import('@/views/Setting/InvoiceTrackNumber'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditOrder }]
        }
    },
    {
        path: '/setting/invoiceTrackNumber/add',
        name: 'AddInvoiceTrackNumber',
        component: () => import('@/views/Setting/AddInvoiceTrackNumber'),
        meta: {
            requireAuth: [{ adminPermission: adminPermission.EditOrder }]
        }
    },
];