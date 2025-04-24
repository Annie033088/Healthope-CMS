<template>
  <div class="card">
    <div class="closeBtnContainer">
      <svg
        class="closeBtn"
        @click="$emit('closePopUpWindow')"
        width="26"
        height="26"
        viewBox="0 0 24 24"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M6.4 19L5 17.6L10.6 12L5 6.4L6.4 5L12 10.6L17.6 5L19 6.4L13.4 12L19 17.6L17.6 19L12 13.4L6.4 19Z"
          fill="#757575"
        />
      </svg>
    </div>
    <h3>管理員，你好！</h3>
    <div class="btnContainer">
      <button>
        <span class="btnTop">修改密碼</span>
      </button>
    </div>
    <div class="btnContainer">
      <button>
        <span class="btnTop" @click="logout()">登出</span>
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: "HeaderPopUp",
  methods: {
    async logout() {
      const response = await this.$axios.post("/api/AccountAccess/AdminLogout");

      if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
        this.$emit("closePopUpWindow");
        this.$router.push("/login");
        return;
      }else{
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "發生錯誤!";
        this.$notificationBox.notificationBoxErrorCode = response.data.ErrorCode;
      }
    },
  },
};
</script>


<style scoped>
.card {
  width: 190px;
  height: 254px;
  background: white;
  border-radius: 15px;
  transition: border-radius 0.5s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  box-shadow: inset 0 -3em 3em rgba(0, 0, 0, 0.1), 0 0 0 2px rgb(190, 190, 190),
    0.3em 0.3em 1em rgba(0, 0, 0, 0.3);
}

h3 {
  display: flex;
  justify-content: center;
}

.btnContainer {
  margin-top: 10px;
  width: 100%;
  display: flex;
  justify-content: center;
}

button {
  width: 165px;
  /* 變數 */
  --buttonRadius: 0.75em;
  --buttonColor: #e8e8e8;
  font-size: 15px;
  font-weight: bold;
  border: none;
  cursor: pointer;
  background: none;
}

.btnTop {
  display: block;
  box-sizing: border-box;
  border-radius: var(--buttonRadius);
  padding: 0.75em 1.5em;
  background: var(--buttonColor);
  transform: translateY(-0.2em);
  transition: transform 0.1s ease;
}

button:hover .btnTop {
  transform: translateY(-0.33em);
}

button:active .btnTop {
  transform: translateY(0);
}

.closeBtnContainer {
  display: flex;
  align-items: center;
  justify-content: end;
  width: 100%;
  height: 8%;
}

.closeBtn {
  margin-top: 25px;
  margin-right: 10px;
  border-radius: 15px;
  cursor: pointer;
}
.closeBtn:hover {
  background: #e8e8e8;
  transition: 0.5s;
}
</style>