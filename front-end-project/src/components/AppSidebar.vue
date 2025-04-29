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
    <div class="sidebarRow" v-if="permissionMap.EditMember">
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
    <div class="sidebarRow">
      <button class="btnSidebar">
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
export default {
  name: "AppSidebar",
  props: {
    permissionList: [],
  },
  data() {
    return {
      // 用來儲存權限對照結果
      permissionMap: {
        EditAdmin: false, // 初始設置為 false 或你希望的初始狀態
        EditMember: false,
        None: true,
      },
    };
  },
  created() {
    this.initializePermissionMap(this.permissionList);
    this.setScrollWithWindow();
  },
  methods: {
    redirect(page) {
      if (this.$route.path === page) {
        this.$emit("refreshPage")
      }else{
        this.$router.push(page);
      }
    },
    // 檢查用戶擁有的權限
    initializePermissionMap(permissionList) {
      // 遍歷權限對照表並根據用戶權限設定對應結果
      for (let key in this.$adminPermission) {
        const permissionValue = this.$adminPermission[key];
        this.permissionMap[key] = permissionList.includes(permissionValue);
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
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("創建管理者時發生錯誤", error);
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
  mounted() {
    // 如果是 剛登入後，不需要請求權限 (因為登入時就一併帶過來了)
    if (!(this.permissionList.length > 0) && this.$loginFlag) {
      this.getPermission();
    }
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

.btnSidebar {
  background-color: #e0e0e0;
  border: none;
  padding: 5px;
  font-size: 1rem;
  width: 170px;
  border-radius: 1rem;
  cursor: pointer;
  display: flex;
  justify-content: left;
}

.btnSidebar:active {
  transform: translateY(0.2rem);
}

.btnSidebar:hover:not(:disabled) {
  background: #eee;
}
</style>