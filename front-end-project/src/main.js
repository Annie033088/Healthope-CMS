import Vue from 'vue'
//import VueRouter from "vue-router";
//import router from './router';
import App from './App.vue'
import axios from 'axios';
import {errorCodeDefine, errorCodeToMessage} from './utils/globalSetting';
import setupMock from './mock/mock.js'

setupMock();
Vue.prototype.$errorCodeDefine = errorCodeDefine;
Vue.prototype.$errorCodeToMessage = errorCodeToMessage;
Vue.prototype.$axios = axios;
Vue.config.productionTip = false
//Vue.use(VueRouter);


new Vue({
  //router,
  render: h => h(App),
}).$mount('#app')
