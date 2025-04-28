<template>
  <div>
    <div class="loginTitleContainer">
      <div>
        <h1>歡迎使用</h1>
        <h1>Healthope 後台管理網站</h1>
      </div>
    </div>
    <div class="box">
      <div class="card">
        <div class="loginTextContainer">
          <h2>請輸入帳號密碼</h2>
        </div>
        <div class="loginFormContainer">
          <div class="form">
            <span class="inputSpan">
              <label class="label">使用者帳號</label>
              <input v-model="account" @keydown.enter="login"
            /></span>
            <span class="inputSpan">
              <label class="label">使用者密碼</label>
              <input v-model="pwd" type="password" @keydown.enter="login"
            /></span>
            <span v-if="loginFail" class="hintSpan">{{ this.hintText }}</span>
            <button class="btnLogin" @click="login()">登入</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "HealthopeLogin",
  data() {
    return {
      beforeLoginFlag: true,
      loginFail: false,
      account: "",
      pwd: "",
      hintText: "",
    };
  },
  methods: {
    goToLogin() {
      this.beforeLoginFlag = !this.beforeLoginFlag;
    },
    async login() {
      // 帳號密碼驗證用的正規表達式
      const regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;

      if (!(regex.test(this.account) && regex.test(this.pwd))) {
        this.hintText = "請輸入 8~20 位英文數字";
        this.loginFail = true;
        return;
      }

      if (this.account === this.pwd) {
        this.hintText = "帳號密碼請勿一致";
        this.loginFail = true;
        return;
      }

      try {
        // 傳輸登入資料
        const loginDto = {
          Account: this.account,
          Pwd: this.pwd,
        };

        // post後回傳
        const response = await this.$axios.post(
          "/api/AccountAccess/VerifyAdminLogin",
          loginDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.loginFail = false;
          this.$emit("sendPermission", response.data.ApiDataObject);
          this.$router.push("/");
          return;
        } else {
          this.loginFail = true;
          this.hintText = "帳號密碼輸入錯誤";
        }
      } catch (error) {
        console.error("登入驗證時發生錯誤", error);
      }
    },
  },
};
</script>

<style scoped>
.loginTitleContainer h1 {
  display: flex;
  justify-content: center;
  color: white;
}

.loginTitleContainer {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  background-color: #707070;
}

.loginFormContainer {
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}

.loginTextContainer h2 {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  color: white;
}

.form {
  --clrLight: #efefef;
  --clrDark: #707070;
  --clrLabel: #58bc82;
  --clrGrey: #9c9c9c60;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
  width: 100%;
  max-width: 300px;
}

.form .inputSpan {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form input {
  border-radius: 0.5rem;
  padding: 1rem 0.75rem;
  width: 100%;
  border: none;
  background-color: var(--clrGrey);
  outline: 2px solid var(--clrDark);
  color: white;
  font-size: 15px;
}

.form input:focus {
  outline: 2px solid var(--clrLabel);
}

.label {
  align-self: flex-start;
  color: var(--clrLabel);
  font-weight: 600;
}

.form .btnLogin {
  width: 100%;
  height: 50px;
  border-radius: 3rem;
  background-color: var(--clrDark);
  color: var(--clrLight);
  cursor: pointer;
  transition: all 300ms;
  font-weight: 600;
  font-size: 0.9rem;
  margin-left: 15px;
}

.hintSpan {
  color: var(--clrLabel);
  animation: slideInTop 0.5s cubic-bezier(0.25, 0.46, 0.45, 0.94) both;
}

@keyframes slideInTop {
  0% {
    transform: translateY(-30px);
    opacity: 0;
  }
  100% {
    transform: translateY(0);
    opacity: 1;
  }
}

.form .btnLogin:hover {
  background-color: var(--clrLabel);
  color: var(--clrDark);
}

.card {
  padding: 15px;
  width: 400px;
  height: 380px;
  border-radius: 0.75rem;
  background-color: rgb(36, 44, 59);
}

.box {
  margin-top: 25px;
  width: 100%;
  display: flex;
  justify-content: center;
}
</style>

