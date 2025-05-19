export default [
  {
    path: '*',
    name: 'HealthopeHome',
    component: () => import('@/views/HealthopeHome'),
    meta: { requireAuth: 'login' }
  }
];