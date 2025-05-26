<template>
  <div class="sidebarCard">
    <div class="sidebarRow" v-if="permissionMap.EditAdmin">
      <button class="btnSidebar" @click="redirect('/admin')">
        <svg
          class="sidebarIcon"
          width="26"
          height="26"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            fill-rule="evenodd"
            clip-rule="evenodd"
            d="M12 2C17.523 2 22 6.477 22 12C22.0034 14.3079 21.2053 16.5453 19.742 18.33L19.762 18.352L19.63 18.464C18.6921 19.5731 17.5235 20.4642 16.2056 21.0749C14.8878 21.6856 13.4525 22.0014 12 22C9.05 22 6.4 20.723 4.57 18.693L4.37 18.463L4.238 18.353L4.258 18.329C2.79491 16.5446 1.99679 14.3075 2 12C2 6.477 6.477 2 12 2ZM12 17C10.14 17 8.459 17.592 7.207 18.406C8.58959 19.4429 10.2718 20.0024 12 20C13.7282 20.0024 15.4104 19.4429 16.793 18.406C15.3623 17.4894 13.6991 17.0015 12 17ZM12 4C10.4945 3.99996 9.01959 4.42471 7.74472 5.22545C6.46986 6.02619 5.44681 7.1704 4.79316 8.52657C4.13952 9.88274 3.88181 11.3958 4.04965 12.8919C4.2175 14.388 4.80409 15.8064 5.742 16.984C7.363 15.821 9.575 15 12 15C14.425 15 16.637 15.821 18.258 16.984C19.1959 15.8064 19.7825 14.388 19.9504 12.8919C20.1182 11.3958 19.8605 9.88274 19.2068 8.52657C18.5532 7.1704 17.5301 6.02619 16.2553 5.22545C14.9804 4.42471 13.5055 3.99996 12 4ZM12 6C13.0609 6 14.0783 6.42143 14.8284 7.17157C15.5786 7.92172 16 8.93913 16 10C16 11.0609 15.5786 12.0783 14.8284 12.8284C14.0783 13.5786 13.0609 14 12 14C10.9391 14 9.92172 13.5786 9.17157 12.8284C8.42143 12.0783 8 11.0609 8 10C8 8.93913 8.42143 7.92172 9.17157 7.17157C9.92172 6.42143 10.9391 6 12 6ZM12 8C11.4696 8 10.9609 8.21071 10.5858 8.58579C10.2107 8.96086 10 9.46957 10 10C10 10.5304 10.2107 11.0391 10.5858 11.4142C10.9609 11.7893 11.4696 12 12 12C12.5304 12 13.0391 11.7893 13.4142 11.4142C13.7893 11.0391 14 10.5304 14 10C14 9.46957 13.7893 8.96086 13.4142 8.58579C13.0391 8.21071 12.5304 8 12 8Z"
            fill="#757575"
          />
        </svg>
        <b>管理員</b>
      </button>
    </div>
    <div
      class="sidebarRow"
      v-if="permissionMap.EditMember || permissionMap.SelectMember"
    >
      <button class="btnSidebar" @click="redirect('/member')">
        <svg
          class="sidebarIcon"
          width="26"
          height="26"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            fill-rule="evenodd"
            clip-rule="evenodd"
            d="M12 2C17.523 2 22 6.477 22 12C22.0034 14.3079 21.2053 16.5453 19.742 18.33L19.762 18.352L19.63 18.464C18.6921 19.5731 17.5235 20.4642 16.2056 21.0749C14.8878 21.6856 13.4525 22.0014 12 22C9.05 22 6.4 20.723 4.57 18.693L4.37 18.463L4.238 18.353L4.258 18.329C2.79491 16.5446 1.99679 14.3075 2 12C2 6.477 6.477 2 12 2ZM12 17C10.14 17 8.459 17.592 7.207 18.406C8.58959 19.4429 10.2718 20.0024 12 20C13.7282 20.0024 15.4104 19.4429 16.793 18.406C15.3623 17.4894 13.6991 17.0015 12 17ZM12 4C10.4945 3.99996 9.01959 4.42471 7.74472 5.22545C6.46986 6.02619 5.44681 7.1704 4.79316 8.52657C4.13952 9.88274 3.88181 11.3958 4.04965 12.8919C4.2175 14.388 4.80409 15.8064 5.742 16.984C7.363 15.821 9.575 15 12 15C14.425 15 16.637 15.821 18.258 16.984C19.1959 15.8064 19.7825 14.388 19.9504 12.8919C20.1182 11.3958 19.8605 9.88274 19.2068 8.52657C18.5532 7.1704 17.5301 6.02619 16.2553 5.22545C14.9804 4.42471 13.5055 3.99996 12 4ZM12 6C13.0609 6 14.0783 6.42143 14.8284 7.17157C15.5786 7.92172 16 8.93913 16 10C16 11.0609 15.5786 12.0783 14.8284 12.8284C14.0783 13.5786 13.0609 14 12 14C10.9391 14 9.92172 13.5786 9.17157 12.8284C8.42143 12.0783 8 11.0609 8 10C8 8.93913 8.42143 7.92172 9.17157 7.17157C9.92172 6.42143 10.9391 6 12 6ZM12 8C11.4696 8 10.9609 8.21071 10.5858 8.58579C10.2107 8.96086 10 9.46957 10 10C10 10.5304 10.2107 11.0391 10.5858 11.4142C10.9609 11.7893 11.4696 12 12 12C12.5304 12 13.0391 11.7893 13.4142 11.4142C13.7893 11.0391 14 10.5304 14 10C14 9.46957 13.7893 8.96086 13.4142 8.58579C13.0391 8.21071 12.5304 8 12 8Z"
            fill="#757575"
          />
        </svg>
        <b>會員</b>
      </button>
    </div>
    <div
      class="sidebarRow"
      v-if="
        permissionMap.AddCoach ||
        permissionMap.SelectCoach ||
        permissionMap.EditCoach
      "
    >
      <button class="btnSidebar" @click="redirect('/coach')">
        <svg
          class="sidebarIcon"
          width="26"
          height="26"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            fill-rule="evenodd"
            clip-rule="evenodd"
            d="M12 2C17.523 2 22 6.477 22 12C22.0034 14.3079 21.2053 16.5453 19.742 18.33L19.762 18.352L19.63 18.464C18.6921 19.5731 17.5235 20.4642 16.2056 21.0749C14.8878 21.6856 13.4525 22.0014 12 22C9.05 22 6.4 20.723 4.57 18.693L4.37 18.463L4.238 18.353L4.258 18.329C2.79491 16.5446 1.99679 14.3075 2 12C2 6.477 6.477 2 12 2ZM12 17C10.14 17 8.459 17.592 7.207 18.406C8.58959 19.4429 10.2718 20.0024 12 20C13.7282 20.0024 15.4104 19.4429 16.793 18.406C15.3623 17.4894 13.6991 17.0015 12 17ZM12 4C10.4945 3.99996 9.01959 4.42471 7.74472 5.22545C6.46986 6.02619 5.44681 7.1704 4.79316 8.52657C4.13952 9.88274 3.88181 11.3958 4.04965 12.8919C4.2175 14.388 4.80409 15.8064 5.742 16.984C7.363 15.821 9.575 15 12 15C14.425 15 16.637 15.821 18.258 16.984C19.1959 15.8064 19.7825 14.388 19.9504 12.8919C20.1182 11.3958 19.8605 9.88274 19.2068 8.52657C18.5532 7.1704 17.5301 6.02619 16.2553 5.22545C14.9804 4.42471 13.5055 3.99996 12 4ZM12 6C13.0609 6 14.0783 6.42143 14.8284 7.17157C15.5786 7.92172 16 8.93913 16 10C16 11.0609 15.5786 12.0783 14.8284 12.8284C14.0783 13.5786 13.0609 14 12 14C10.9391 14 9.92172 13.5786 9.17157 12.8284C8.42143 12.0783 8 11.0609 8 10C8 8.93913 8.42143 7.92172 9.17157 7.17157C9.92172 6.42143 10.9391 6 12 6ZM12 8C11.4696 8 10.9609 8.21071 10.5858 8.58579C10.2107 8.96086 10 9.46957 10 10C10 10.5304 10.2107 11.0391 10.5858 11.4142C10.9609 11.7893 11.4696 12 12 12C12.5304 12 13.0391 11.7893 13.4142 11.4142C13.7893 11.0391 14 10.5304 14 10C14 9.46957 13.7893 8.96086 13.4142 8.58579C13.0391 8.21071 12.5304 8 12 8Z"
            fill="#757575"
          />
        </svg>
        <b>教練</b>
      </button>
    </div>
    <div>
      <div class="sidebarRow" v-if="groupClassFlag">
        <button
          class="btnDropDownSidebar"
          @click="dropDownFlag.Course ? closeDropdown() : openDropdown()"
        >
          <svg
            class="sidebarIcon"
            width="25"
            height="25"
            viewBox="0 0 25 25"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <g clip-path="url(#clip0_85_33)">
              <path
                d="M8.981 22.437C9.07094 22.5345 9.11876 22.6636 9.11408 22.7961C9.1094 22.9287 9.05259 23.0541 8.956 23.145L7.87 24.1605C7.77297 24.2505 7.64422 24.2983 7.51198 24.2935C7.37974 24.2888 7.25479 24.2318 7.1645 24.135L0.840001 17.319C0.750126 17.2214 0.702388 17.0923 0.707163 16.9598C0.711938 16.8272 0.768839 16.7019 0.865501 16.611L1.953 15.597C2.05004 15.507 2.17878 15.4592 2.31102 15.464C2.44326 15.4688 2.56822 15.5258 2.6585 15.6225L8.981 22.437ZM17.051 9.61201C17.1409 9.70958 17.1886 9.83868 17.1838 9.97125C17.1791 10.1038 17.1222 10.2292 17.0255 10.32L10.1905 16.705C10.0935 16.795 9.96472 16.8428 9.83248 16.838C9.70024 16.8333 9.57529 16.7763 9.485 16.6795L7.8535 14.919C7.76363 14.8214 7.71589 14.6923 7.72066 14.5598C7.72544 14.4272 7.78234 14.3019 7.879 14.211L14.7125 7.82601C14.7604 7.78135 14.8167 7.74658 14.8781 7.72369C14.9395 7.7008 15.0048 7.69024 15.0703 7.69261C15.1358 7.69498 15.2001 7.71023 15.2597 7.7375C15.3193 7.76476 15.3729 7.8035 15.4175 7.85151L17.051 9.61201ZM11.3065 20.2635C11.4935 20.465 11.482 20.7835 11.281 20.9715L10.1935 21.9865C10.0965 22.0765 9.96772 22.1243 9.83548 22.1195C9.70324 22.1148 9.57829 22.0578 9.488 21.961L3.167 15.145C3.07706 15.0475 3.02924 14.9184 3.03392 14.7859C3.0386 14.6533 3.09541 14.5279 3.192 14.437L4.2775 13.4225C4.37454 13.3325 4.50328 13.2847 4.63552 13.2895C4.76776 13.2943 4.89271 13.3513 4.983 13.448L11.3065 20.2635ZM21.838 9.85651C21.9278 9.95415 21.9755 10.0833 21.9706 10.2159C21.9657 10.3484 21.9087 10.4737 21.812 10.5645L20.725 11.5795C20.628 11.6697 20.4992 11.7177 20.3668 11.713C20.2345 11.7083 20.1094 11.6513 20.019 11.5545L13.697 4.74001C13.6069 4.64256 13.559 4.51347 13.5637 4.38086C13.5684 4.24825 13.6253 4.12286 13.722 4.03201L14.812 3.01451C14.8599 2.96985 14.9162 2.93508 14.9776 2.91219C15.039 2.8893 15.1043 2.87874 15.1698 2.88111C15.2353 2.88348 15.2996 2.89873 15.3592 2.926C15.4188 2.95326 15.4724 2.992 15.517 3.04001L21.838 9.85651ZM24.16 7.68651C24.2497 7.78428 24.2972 7.91341 24.2925 8.04599C24.2877 8.17856 24.231 8.30393 24.1345 8.39501L23.0495 9.40951C23.0016 9.4542 22.9453 9.489 22.884 9.51193C22.8226 9.53487 22.7573 9.54548 22.6918 9.54315C22.6263 9.54083 22.562 9.52562 22.5024 9.4984C22.4428 9.47118 22.3891 9.43248 22.3445 9.38451L16.0235 2.56701C15.9338 2.4693 15.8861 2.34021 15.8908 2.20763C15.8955 2.07506 15.9521 1.94965 16.0485 1.85851L17.1345 0.842008C17.1823 0.797274 17.2385 0.762426 17.2999 0.739463C17.3612 0.716501 17.4265 0.705876 17.4919 0.708199C17.5574 0.710521 17.6217 0.725746 17.6813 0.752998C17.7409 0.780251 17.7945 0.818995 17.839 0.867008L24.16 7.68651Z"
                fill="#F24822"
              />
            </g>
            <defs>
              <clipPath id="clip0_85_33">
                <rect width="25" height="25" fill="white" />
              </clipPath>
            </defs>
          </svg>
          <b>課程</b>
          <svg
            v-if="!dropDownFlag.Course"
            class="sidebarSymbol"
            width="20"
            height="20"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M19 12.998H13V18.998H11V12.998H5V10.998H11V4.99799H13V10.998H19V12.998Z"
              fill="black"
            />
          </svg>
          <svg
            v-else
            class="sidebarSymbol"
            width="20"
            height="20"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path d="M19 12.998H5V10.998H19V12.998Z" fill="black" />
          </svg>
        </button>
      </div>
      <div
        class="btnSubContainer"
        :class="dropDownFlag.Course ? 'showDropdown' : 'hideDropdown'"
        @animationend="handleAnimationEnd"
        v-if="showElement.Course"
      >
        <BtnSubSideBar
          v-if="
            permissionMap.EditGroupClassShowcase ||
            permissionMap.SelectGroupClassShowcase
          "
          text="展示用團課"
          @click="redirect('/groupClass/showcase')"
        ></BtnSubSideBar>
        <BtnSubSideBar
          v-if="
            permissionMap.EditGroupClassSchedule ||
            permissionMap.SelectGroupClassSchedule
          "
          text="團體課程表"
          @click="redirect('/groupClass/schedule')"
        ></BtnSubSideBar>
      </div>
    </div>
    <div
      class="sidebarRow"
      v-if="permissionMap.EditPlan || permissionMap.SelectPlan"
    >
      <button class="btnSidebar" @click="redirect('/plan')">
        <svg
          class="sidebarIcon"
          width="24"
          height="24"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M10 9.75L12 8.75L14 9.75V5H10V9.75ZM7 17V15H12V17H7ZM5 21C4.45 21 3.97933 20.8043 3.588 20.413C3.19667 20.0217 3.00067 19.5507 3 19V5C3 4.45 3.196 3.97933 3.588 3.588C3.98 3.19667 4.45067 3.00067 5 3H19C19.55 3 20.021 3.196 20.413 3.588C20.805 3.98 21.0007 4.45067 21 5V19C21 19.55 20.8043 20.021 20.413 20.413C20.0217 20.805 19.5507 21.0007 19 21H5ZM5 19H19V5H16V13L12 11L8 13V5H5V19Z"
            fill="#AA7F7F"
          />
        </svg>
        <b>方案</b>
      </button>
    </div>
    <div class="sidebarRow">
      <button class="btnSidebar">
        <svg
          class="sidebarIcon"
          width="24"
          height="24"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M4 19V20H22V22H2V2H4V17C7 17 10 15 12.1 11.4C15.1 6.4 18.4 4 22 4V6C19.2 6 16.5 8.1 13.9 12.5C11.3 16.6 7.7 19 4 19Z"
            fill="black"
          />
        </svg>
        <b>報表</b>
      </button>
    </div>
  </div>
</template>

<script>
import BtnSubSideBar from "@/components/Btn/BtnSubSideBar";

export default {
  name: "AppSidebar",
  components: {
    BtnSubSideBar,
  },
  props: {
    // 用來儲存權限對照結果
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
    },
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      dropDownFlag: {
        Course: false,
      },
      showElement: {
        Course: false,
      },
    };
  },
  created() {
    this.setScrollWithWindow();
  },
  methods: {
    openDropdown() {
      this.showElement.Course = true;
      this.dropDownFlag.Course = true;
    },
    closeDropdown() {
      this.dropDownFlag.Course = false; // 播放消失動畫
    },
    handleAnimationEnd() {
      if (!this.dropDownFlag.Course) {
        // 消失動畫播完後，才真正隱藏元素
        this.showElement.Course = false;
      }
    },
    redirect(page) {
      if (this.$route.path === page) {
        this.$emit("refreshPage");
      } else {
        this.$router.push(page);
      }
    },
    setScrollWithWindow() {
      // 讓側邊欄跟著視窗移動
      window.addEventListener("scroll", () => {
        const header = document.querySelector(".headerContainer");
        const sidebar = document.querySelector(".sidebarCard");
        const headerBottom = header.getBoundingClientRect().bottom;

        if (headerBottom <= 80) {
          if (headerBottom <= 0) sidebar.style.top = 0;
          else sidebar.style.top = headerBottom + "px";
        } else {
          sidebar.style.top = "auto";
        }
      });
    },
  },
  computed: {
    groupClassFlag() {
      return (
        this.permissionMap.EditGroupClassShowcase ||
        this.permissionMap.SelectGroupClassShowcase ||
        this.permissionMap.EditGroupClassSchedule ||
        this.permissionMap.SelectGroupClassSchedule
      );
    },
  },
};
</script>


<style scoped>
.sidebarCard {
  position: fixed;
  top: 58px;
  width: 200px;
  height: 100vh;
  border-radius: 3px;
  background: #e0e0e0;
}

.sidebarRow {
  display: flex;
  align-items: center;
  justify-content: start;
  margin-left: 15px;
  margin-top: 15px;
}

.sidebarIcon {
  margin-right: 8px;
}

.btnSidebar,
.btnDropDownSidebar {
  background-color: #e0e0e0;
  border: none;
  padding: 5px;
  font-size: 1rem;
  width: 170px;
  border-radius: 1rem;
  cursor: pointer;
  display: flex;
  justify-content: left;
  align-items: center;
}

.btnSidebar:active {
  transform: translateY(0.2rem);
}

.btnSidebar:hover,
.btnDropDownSidebar:hover {
  background: #eee;
}

.sidebarSymbol {
  margin-left: 70px;
}
</style>

<style scoped>
.showDropdown {
  animation: flip-in-hor-bottom 0.5s cubic-bezier(0.25, 0.46, 0.45, 0.94) both;
}

.hideDropdown {
  animation: flip-out-hor-top 0.45s cubic-bezier(0.55, 0.085, 0.68, 0.53) both;
}

@keyframes flip-in-hor-bottom {
  0% {
    transform: rotateX(80deg);
    opacity: 0;
  }
  100% {
    transform: rotateX(0);
    opacity: 1;
  }
}

@keyframes flip-out-hor-top {
  0% {
    transform: rotateX(0);
    opacity: 1;
  }
  100% {
    transform: rotateX(70deg);
    opacity: 0;
  }
}
</style>