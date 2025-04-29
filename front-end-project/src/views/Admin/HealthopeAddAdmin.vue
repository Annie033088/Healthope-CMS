<template>
  <div>
    <TitleCard text="管理者清單"></TitleCard>
    <SubTitleCard text="新增管理者"></SubTitleCard>
    <div class="card">
      <div class="addInputContainer">
        <div class="addInputLeft">
          <span class="inputSpan">
            <label class="label">請輸入帳號</label>
            <input v-model="account"
          /></span>
          <span class="inputSpan">
            <label class="label">請輸入密碼</label>
            <input type="password" v-model="pwd"
          /></span>
          <span class="inputSpan">
            <label class="label">請再輸入一次密碼</label>
            <input type="password" v-model="pwdAgain"
          /></span>
        </div>
        <div class="addInputRight">
          <span class="inputSpan">
            <label class="label">請選擇身份</label>
            <div class="radioContainer">
              <label class="labRadioBox">
                <input
                  type="radio"
                  name="radioIdentity"
                  value="None"
                  v-model="selectIdentity"
                  checked=""
                />
                <span class="textRadio">無</span>
              </label>
              <label class="labRadioBox">
                <input
                  type="radio"
                  name="radioIdentity"
                  value="Admin"
                  v-model="selectIdentity"
                />
                <span class="textRadio">管理員</span>
              </label>
              <label class="labRadioBox">
                <input
                  type="radio"
                  name="radioIdentity"
                  value="Receptionist"
                  v-model="selectIdentity"
                />
                <span class="textRadio">櫃檯人員</span>
              </label>
              <label class="labRadioBox">
                <input
                  type="radio"
                  name="radioIdentity"
                  value="Accountant"
                  v-model="selectIdentity"
                />
                <span class="textRadio">會計</span>
              </label>
              <label class="labRadioBox">
                <input
                  type="radio"
                  name="radioIdentity"
                  value="CourseManager"
                  v-model="selectIdentity"
                />
                <span class="textRadio">課程管理</span>
              </label>
              <label class="labRadioBox">
                <input
                  type="radio"
                  name="radioIdentity"
                  value="CoachManager"
                  v-model="selectIdentity"
                />
                <span class="textRadio">教練管理</span>
              </label>
              <label class="labRadioBox">
                <input
                  type="radio"
                  name="radioIdentity"
                  value="SalesRepresentative"
                  v-model="selectIdentity"
                />
                <span class="textRadio">業務</span>
              </label>
            </div>
          </span>
        </div>
      </div>
      <div class="hintContainer">
        <span v-if="addFail" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnAddContainer">
        <button class="btnAdd btn" @click="addAdmin()">創建</button>
      </div>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";

export default {
  name: "HealthopeAddAdmin",
  components: {
    TitleCard,
    SubTitleCard,
  },
  props: {
    text: String,
  },
  data() {
    return {
      hintText: "",
      account: "",
      pwd: "",
      pwdAgain: "",
      addFail: false,
      selectIdentity: "None",
    };
  },
  methods: {
    async addAdmin() {
      // 帳號密碼驗證用的正規表達式
      const regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;

      if (!(regex.test(this.account) && regex.test(this.pwd))) {
        this.hintText = "請輸入 8~20 位英文數字";
        this.addFail = true;
        return;
      }

      if (this.pwd !== this.pwdAgain) {
        this.hintText = "兩次密碼輸入不一致";
        this.addFail = true;
        return;
      }

      if (this.account === this.pwd) {
        this.hintText = "帳號密碼不可相同";
        this.addFail = true;
        return;
      }

      try {
        // 傳輸登入資料
        const addAdminDto = {
          Account: this.account,
          Pwd: this.pwd,
          Identity: this.selectIdentity,
        };

        // post後回傳
        const response = await this.$axios.post(
          "/api/Admin/AddAdmin",
          addAdminDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.addFail = false;
          this.$router.push("/admin");
          return;
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
  },
};
</script>

<style scoped>
.addInputContainer {
  --clrLight: #efefef;
  --clrDark: #707070;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-evenly;
  margin-top: 25px;
}

.addInputLeft,
.addInputRight {
  width: 60%;
  max-width: 350px;
}

.addInputLeft .inputSpan {
  margin-top: 20px;
}
.addInputRight .inputSpan {
  margin-top: 10px;
}

.addInputContainer .inputSpan {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.addInputContainer input {
  border-radius: 0.5rem;
  padding: 0.7rem 0.75rem;
  width: 100%;
  border: none;
  background-color: white;
  outline: 2px solid var(--clrLight);
  font-size: 15px;
  margin-left: 15px;
}

.addInputContainer input:focus {
  outline: 2px solid var(--clrDark);
}

.btnAddContainer {
  display: flex;
  justify-content: center;
  margin-top: 15px;
}

.btnAdd {
  width: 100%;
  max-width: 400px;
  height: 50px;
  border-radius: 3rem;
  background-color: #707070;
  color: #efefef;
  cursor: pointer;
  transition: all 300ms;
  font-weight: 600;
  font-size: 0.9rem;
}

.btnAdd:hover {
  background-color: #eee;
  color: #707070;
}

.hintContainer {
  margin-top: 15px;
  display: flex;
  justify-content: center;
}

.hintSpan {
  color: #707070;
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

.radioContainer {
  display: flex;
  flex-wrap: wrap;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.35rem;
  width: 100%;
  max-width: 350px;
  font-size: 16px;
  margin-left: 20px;
}

.labRadioBox {
  flex: 1 1 auto;
  text-align: center;
  justify-content: center;
  margin-left: 15px;
  min-width: 50px;
}

.labRadioBox input {
  display: none;
}

.labRadioBox .textRadio {
  display: flex;
  cursor: pointer;
  justify-content: center;
  border-radius: 0.5rem;
  padding: 0.5rem 0;
  transition: all 0.15s ease-in-out;
}

.labRadioBox input:checked + .textRadio {
  background-color: #fff;
  font-weight: 600;
}
</style>