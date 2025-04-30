<template>
  <div>
    <header>
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
            width="35"
            height="35"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M19.2502 8.5C19.9796 8.5 20.679 8.21027 21.1948 7.69454C21.7105 7.17882 22.0002 6.47935 22.0002 5.75C22.0002 5.02065 21.7105 4.32118 21.1948 3.80546C20.679 3.28973 19.9796 3 19.2502 3C18.5209 3 17.8214 3.28973 17.3057 3.80546C16.7899 4.32118 16.5002 5.02065 16.5002 5.75C16.5002 6.47935 16.7899 7.17882 17.3057 7.69454C17.8214 8.21027 18.5209 8.5 19.2502 8.5ZM15.5762 6.503C15.4759 6.0081 15.4752 5.49817 15.5742 5.003H2.75221L2.65021 5.01C2.46196 5.0359 2.29056 5.13229 2.17065 5.2797C2.05073 5.42711 1.99123 5.61454 2.00419 5.80412C2.01714 5.9937 2.10157 6.1713 2.24042 6.30103C2.37927 6.43076 2.56219 6.50294 2.75221 6.503H15.5762ZM21.2532 18H2.75321L2.65121 18.007C2.46296 18.0329 2.29156 18.1293 2.17165 18.2767C2.05173 18.4241 1.99223 18.6115 2.00519 18.8011C2.01814 18.9907 2.10257 19.1683 2.24142 19.298C2.38027 19.4278 2.56319 19.4999 2.75321 19.5H21.2532L21.3552 19.493C21.5435 19.4671 21.7149 19.3707 21.8348 19.2233C21.9547 19.0759 22.0142 18.8885 22.0012 18.6989C21.9883 18.5093 21.9039 18.3317 21.765 18.202C21.6262 18.0722 21.4432 18.0001 21.2532 18ZM2.75321 11.503H21.2532C21.4432 11.5031 21.6262 11.5752 21.765 11.705C21.9039 11.8347 21.9883 12.0123 22.0012 12.2019C22.0142 12.3915 21.9547 12.5789 21.8348 12.7263C21.7149 12.8737 21.5435 12.9701 21.3552 12.996L21.2532 13.003H2.75321C2.56319 13.0029 2.38027 12.9308 2.24142 12.801C2.10257 12.6713 2.01814 12.4937 2.00519 12.3041C1.99223 12.1145 2.05173 11.9271 2.17165 11.7797C2.29156 11.6323 2.46296 11.5359 2.65121 11.51L2.75321 11.503Z"
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
        :notificationBoxConfirmFlag="notificationBoxConfirmFlag"
      @afterConfirmEvent="$emit('afterConfirmEvent')"
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
