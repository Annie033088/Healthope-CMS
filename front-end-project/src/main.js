import Vue from 'vue'
import VueRouter from "vue-router";
import router from './router';
import App from './App.vue'
import axios from 'axios';
import {errorCodeDefine, errorCodeToMessage, adminPermission} from './utils/globalSetting';

if (process.env.NODE_ENV === 'development') {
  await import('./mock/mock.js')
}

Vue.prototype.$errorCodeDefine = errorCodeDefine;
Vue.prototype.$errorCodeToMessage = errorCodeToMessage;
Vue.prototype.$adminPermission = adminPermission;
Vue.prototype.$loginFlag = false;
Vue.prototype.$axios = axios;
Vue.prototype.$notificationBox = Vue.observable({
  notificationBoxFlag: false,
  notificationBoxTitle: "",
  notificationBoxErrorCode: 0,
});
Vue.config.productionTip = false
Vue.use(VueRouter);


new Vue({
  router,
  render: h => h(App),
}).$mount('#app')
