const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  //代理伺服器
 /* devServer:{
    proxy:{
      '^/api':{
        target: 'https://localhost:44380',
        changeOrgin:true,
        logLevel:'debug',
        pathRewrite:{'^/api':'/api'},
      }
    },
  },*/
  // build 出的檔案位置
  outputDir: '../HealthopeCMS/ApiLayer/wwwroot',
  publicPath: '/wwwroot', 
})
