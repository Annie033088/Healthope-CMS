<template>
  <div class="app">
    <NotificationBox
      v-if="this.$notificationBox.notificationBoxFlag"
      class="notificationBox"
      @notificationBoxConfirm="notificationBoxConfirm"
    ></NotificationBox>
    <AppSidebar
      v-if="this.$loginFlag"
      class="sidebar"
      :permissionList="permissionList"
      :notificationBoxConfirmFlag="notificationBoxConfirmFlag"
      @afterConfirmEvent="afterConfirmEvent"
      @refreshPage="refreshRouterViewComponent"
    >
    </AppSidebar>
    <AppHeader
      :title="title"
      :notificationBoxConfirmFlag="notificationBoxConfirmFlag"
      @afterConfirmEvent="afterConfirmEvent"
    ></AppHeader>
    <router-view
      :key="routerViewKey"
      :class="{ contentContainer: this.$loginFlag }"
      :title="title"
      @sendPermission="setPermission"
      @refreshPage="refreshRouterViewComponent"
      :notificationBoxConfirmFlag="notificationBoxConfirmFlag"
      @afterConfirmEvent="afterConfirmEvent"
    ></router-view>
    <AppFooter />
  </div>
</template>

<script>
import AppFooter from "@/components/AppFooter.vue";
import NotificationBox from "@/components/NotificationBox.vue";
import AppHeader from "@/components/Header/AppHeader";
import AppSidebar from "@/components/AppSidebar.vue";

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
      permissionList: [],
      routerViewKey: 0,
      notificationBoxConfirmFlag: false,
    };
  },
  methods: {
    setPermission(permissionList) {
      this.permissionList = permissionList;
    },
    refreshRouterViewComponent() {
      this.routerViewKey += 1;
    },
    notificationBoxConfirm() {
      this.notificationBoxConfirmFlag = true;
    },
    afterConfirmEvent(redirectRoute) {
      this.notificationBoxConfirmFlag = false;
      
      if(redirectRoute === "stop") return;

      if (redirectRoute) this.$router.push(redirectRoute);
      else this.refreshRouterViewComponent();
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
