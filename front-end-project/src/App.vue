<template>
  <div class="app">
    <NotificationBox v-if="this.$notificationBox.notificationBoxFlag" class="notificationBox"></NotificationBox>
      <AppHeader
        :title="title"
      ></AppHeader>
      <router-view
        :title="title"
      ></router-view>
      <AppFooter/>
  </div>
</template>

<script>
import AppFooter from '@/components/AppFooter.vue';
import NotificationBox from '@/components/NotificationBox.vue';
import AppHeader from "@/components/Header/AppHeader";

export default {
  name: 'App',
  components: {
    AppFooter,
    AppHeader,
    NotificationBox
  },
  data(){
    return{
      title:"Healthope 健望館後台管理網站"
    }
  }
}
</script>

<style>
.btn {
  cursor: pointer;
  opacity: 70%;
}
:hover {
  opacity: 100%;
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
