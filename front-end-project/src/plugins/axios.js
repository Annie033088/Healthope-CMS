import axios from 'axios';
import Vue from 'vue';

// 創建 axios 實例
const axiosInstance = axios.create({
 // timeout: 1000, // 設置請求超時
});

// 添加請求攔截器
axiosInstance.interceptors.request.use(
  config => {
    // 在請求發送前做一些處理
    console.log("post ");
    if (config.data) console.log(JSON.stringify(config.data))

    return config;
  },
  error => {
    // 處理請求錯誤
    console.error('Request error:', error);
    return Promise.reject(error);
  }
);

// 添加回應攔截器
axiosInstance.interceptors.response.use(
  response => {
    console.log("response", JSON.stringify(response.data));
    // 在回應回來後做一些處理
    return response;
  },
  error => {
    // 處理回應錯誤
    console.error('Response error:', error);
    // 確保錯誤被傳遞
    return Promise.reject(error);
  }
);


// 如果你要全域使用，可以掛在 Vue.prototype
Vue.prototype.$axios = axiosInstance;

export default axiosInstance;