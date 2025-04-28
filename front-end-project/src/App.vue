<template>
  <div class="app">
    <NotificationBox
      v-if="this.$notificationBox.notificationBoxFlag"
      class="notificationBox"
    ></NotificationBox>
    <AppSidebar
      v-if="this.$loginFlag"
      class="sidebar"
      :permissionList="permissionList"
    >
    </AppSidebar>
    <AppHeader :title="title"></AppHeader>
    <router-view
      :class="{ contentContainer: this.$loginFlag }"
      :title="title"
      @sendPermission="setPermission"
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
    };
  },
  methods: {
    setPermission(permissionList) {
      this.permissionList = permissionList;
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

.app {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #f7f6f6;
}
/* .app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
} */
</style>
