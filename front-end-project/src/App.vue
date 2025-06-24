<template>
  <div class="app">
    <NotificationBox
      v-if="this.$notificationBox.notificationBoxFlag"
      class="notificationBox"
      @notificationBoxConfirm="notificationBoxConfirm"
      @created="notificationBoxCreated"
    ></NotificationBox>
    <AppSidebar
      v-if="this.$loginFlag"
      class="sidebar"
      :permissionMap="permissionMap"
      :notificationBoxConfirmFlag="notificationBoxConfirmFlag"
      @afterConfirmEvent="afterConfirmEvent"
      @refreshPage="refreshRouterViewComponent"
      @getPermission="getPermission"
    >
    </AppSidebar>
    <AppHeader
      :title="title"
      :notificationBoxConfirmFlag="notificationBoxConfirmFlag"
      @afterConfirmEvent="afterConfirmEvent"
    ></AppHeader>
    <router-view
      v-if="permissionMapReady || !this.$loginFlag"
      :key="routerViewKey"
      :class="{ contentContainer: this.$loginFlag }"
      :title="title"
      @sendPermission="setPermission"
      @refreshPage="refreshRouterViewComponent"
      :notificationBoxConfirmFlag="notificationBoxConfirmFlag"
      @afterConfirmEvent="afterConfirmEvent"
      :permissionMap="permissionMap"
    ></router-view>
    <AppFooter />
  </div>
</template>

<script>
import AppFooter from "@/components/AppFooter.vue";
import NotificationBox from "@/components/NotificationBox.vue";
import AppHeader from "@/components/Header/AppHeader";
import AppSidebar from "@/components/AppSidebar.vue";
import { adminPermission } from "@/utils/globalSetting";

export default {
  name: "App",
  components: {
    AppFooter,
    AppHeader,
    NotificationBox,
    AppSidebar,
  },
  data() {
    return {
      title: "Healthope 健望館後台管理網站",
      routerViewKey: 0,
      notificationBoxConfirmFlag: false,
      permissionMapReady: false,
      permissionMap: {
        None: false,
        EditAdmin: false,
        SelectMember: false,
        EditMember: false,
        SelectCoach: false,
        AddCoach: false,
        EditCoach: false,
        EditGroupClassShowcase: false,
        SelectGroupClassShowcase: false,
        EditGroupClassSchedule: false,
        SelectGroupClassSchedule: false,
        EditPlan: false,
        SelectPlan: false,
        AddOrder: false,
        EditOrder: false,
        SelectOrder: false,
      },
    };
  },
  methods: {
    setPermission(permissionList) {
      this.initializePermissionMap(permissionList);
    },
    refreshRouterViewComponent() {
      this.routerViewKey += 1;
    },
    notificationBoxConfirm() {
      this.notificationBoxConfirmFlag = true;
    },
    afterConfirmEvent(redirectRoute) {
      this.notificationBoxConfirmFlag = false;

      if (redirectRoute === "stop") return;

      if (redirectRoute) this.$router.push(redirectRoute);
      else this.refreshRouterViewComponent();
    },
    notificationBoxCreated() {
      this.notificationBoxConfirmFlag = false;
    },
    // 檢查用戶擁有的權限
    initializePermissionMap(permissionList) {
      // 遍歷權限對照表並根據用戶權限設定對應結果
      for (let key in adminPermission) {
        const permissionValue = adminPermission[key];
        this.$set(
          this.permissionMap,
          key,
          permissionList.includes(permissionValue)
        );
        this.permissionMapReady = true;
      }
    },
    // 發送請求取得權限
    async getPermission() {
      try {
        // post
        const response = await this.$axios.post("/api/Admin/GetPermission");

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.initializePermissionMap(response.data.ApiDataObject);
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                this.$emit("afterConfirmEvent");
                this.unwatchFlag(); // 移除監聽
                this.unwatchFlag = null;
              }
            }
          );

          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("創建管理者時發生錯誤", error);
      }
    },
  },
};
</script>

<style>
.btn {
  cursor: pointer;
}

.btn:active {
  transform: translateY(0.2rem);
}

.contentContainer {
  margin-left: 200px;
}

html,
body {
  background-color: #f7f6f6;
  min-height: 100vh;
  margin: 0;
  overflow-x: hidden; /* 防止橫向捲軸 */
}
</style>
