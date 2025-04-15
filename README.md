# Healthope-CMS #

## 目錄 ##

1. 簡介
2. 還原專案
3. 登入 sa
4. 其他備註

## 簡介 ##

Healthope-CMS 包含前端及後端
分別是 front-end-project (Vue2) 及 HealthopeCMS (.Net Framework MVC)
上線時，會將前後端統一打包並部署到 IIS

HealthopeCMS 方案包含四個專案：

1. ApiLayer (主要處理前後端的接口)
2. DomainLayer (主要處理業務邏輯或複雜邏輯)
3. PersistentLayer (主要處理資料庫溝通)
4. UnitTest

## 還原專案 ##

### 開發方式 ###

1. 複製 Healthope-CMS Repo `https://github.com/Annie033088/Healthope-CMS.git`
2. 進入 HealthopeCMS 資料夾運行方案 HealthopeCMS.sln 即可進行後端開發
3. 使用 VSCode 打開 front-end-project 資料夾即可進行前端開發

### 打包方式 ###

1. 在前端專案執行 npm run build 將前端專案打包至後端專案 HealthopeCMS/ApiLayer 資料夾底下
2. 將 asp.net 裡舊的 wwwroot 資料夾從專案移除，並將新的複製進專案 (ApiLayer)
3. 右鍵 ApiLayer 專案並選擇發布
4. 將發布檔案部署到 IIS

## 登入 sa ##

## 其他備註 ##
