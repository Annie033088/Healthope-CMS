<template>
  <div>
    <header>
      <link rel="icon" href="@/assets/logo/HealthopeLogo-circle-removebg-preview">
      <div class="headerContainer">
        <div class="logoContainer">
          <router-link to="/" class="">
            <img class="btn" src="@/assets/logo/HealthopeCMSLogo.png" />
          </router-link>
        </div>
        <div class="dropDownImg" v-if="!loginPage">
          <svg
            @click="openPopUpWindow"
            class="btn"
            width="30"
            height="30"
            viewBox="0 0 16 16"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M2 3.5C2 3.36739 2.05268 3.24021 2.14645 3.14645C2.24021 3.05268 2.36739 3 2.5 3H10.5C10.6326 3 10.7598 3.05268 10.8536 3.14645C10.9473 3.24021 11 3.36739 11 3.5C11 3.63261 10.9473 3.75979 10.8536 3.85355C10.7598 3.94732 10.6326 4 10.5 4H2.5C2.36739 4 2.24021 3.94732 2.14645 3.85355C2.05268 3.75979 2 3.63261 2 3.5ZM2 11.5C2 11.3674 2.05268 11.2402 2.14645 11.1464C2.24021 11.0527 2.36739 11 2.5 11H9.5C9.63261 11 9.75979 11.0527 9.85355 11.1464C9.94732 11.2402 10 11.3674 10 11.5C10 11.6326 9.94732 11.7598 9.85355 11.8536C9.75979 11.9473 9.63261 12 9.5 12H2.5C2.36739 12 2.24021 11.9473 2.14645 11.8536C2.05268 11.7598 2 11.6326 2 11.5ZM2.5 7C2.36739 7 2.24021 7.05268 2.14645 7.14645C2.05268 7.24021 2 7.36739 2 7.5C2 7.63261 2.05268 7.75979 2.14645 7.85355C2.24021 7.94732 2.36739 8 2.5 8H13.5C13.6326 8 13.7598 7.94732 13.8536 7.85355C13.9473 7.75979 14 7.63261 14 7.5C14 7.36739 13.9473 7.24021 13.8536 7.14645C13.7598 7.05268 13.6326 7 13.5 7H2.5Z"
              fill="#757575"
            />
          </svg>
        </div>
      </div>
    </header>
    <div v-if="popUpFlag" class="popUpWindowContainer">
      <HeaderPopUp
        class="popUpWindow"
        ref="popUpWindow"
        @closePopUpWindow="closePopUpWindow()"
        :notificationBoxConfirmFlag="this.notificationBoxConfirmFlag"
        @afterConfirmEvent="afterConfirmEvent"
      ></HeaderPopUp>
    </div>
  </div>
</template>

<script>
import HeaderPopUp from "@/components/Header/HeaderPopUp";

export default {
  name: "AppHeader",
  components: {
    HeaderPopUp,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
    title: String,
  },
  data() {
    return {
      popUpFlag: false,
    };
  },
  computed: {
    loginPage() {
      if (this.$route.path == "/login") {
        return true;
      } else {
        return false;
      }
    },
  },
  methods: {
    openPopUpWindow() {
      if (this.popUpFlag == true) {
        this.popUpFlag == false;
        return;
      }

      this.popUpFlag = true;
      this.$nextTick(() => {
        setTimeout(() => {
          document.addEventListener("click", this.clickOutside);
        }, 0);
      });
    },
    closePopUpWindow() {
      if (this.popUpFlag == false) return;

      this.popUpFlag = false;
      document.removeEventListener("click", this.clickOutside);
    },
    clickOutside(event) {
      const popUpWindow = this.$refs.popUpWindow?.$el;
      if (popUpWindow && !popUpWindow.contains(event.target)) {
        this.closePopUpWindow();
      }
    },
    afterConfirmEvent(redirectRoute) {
      this.$emit("afterConfirmEvent", redirectRoute);
    },
  },
};
</script>

<style scoped>
header {
  width: 100%;
  border-bottom: 1px solid rgba(190, 190, 190, 0.619);
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
  background-color: white;
}

.headerContainer {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  z-index: 10;
}

.logoContainer {
  margin-left: 15px;
  max-width: 160px;
}

header img {
  width: 100%;
}

header .dropDownImg {
  margin-right: 20px;
}

.popUpWindow {
  position: fixed;
  top: 50px;
  right: 1%;
  animation: fadeIn 0.7s cubic-bezier(0.39, 0.575, 0.565, 1) both;
}

@keyframes fadeIn {
  0% {
    opacity: 0;
  }
  100% {
    opacity: 1;
  }
}
</style>
