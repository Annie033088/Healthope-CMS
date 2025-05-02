<template>
  <div>
    <TitleCard text="管理者清單"></TitleCard>
    <SubTitleCard text="新增管理者"></SubTitleCard>
    <div class="addInputContainer">
      <div class="addInputLeft">
        <InputSpan labelText="請輸入帳號" v-model="account"></InputSpan>
        <InputSpan labelText="請輸入密碼" v-model="pwd" inputType="password"></InputSpan>
        <InputSpan labelText="請再輸入一次密碼" v-model="pwdAgain" inputType="password"></InputSpan>
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
      <BtnConfirm @click="addAdmin()" text="創建"></BtnConfirm>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import InputSpan from "@/components/Input/InputSpan";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "HealthopeAddAdmin",
  components: {
    TitleCard,
    SubTitleCard,
    InputSpan,
    BtnConfirm
  },
  props: {
    text: String,
    notificationBoxConfirmFlag: Boolean,
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
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = null;
                this.$emit("afterConfirmEvent", redirectRoute);
                this.unwatchFlag(); // 移除監聽
                this.unwatchFlag = null;
              }
            }
          );

          // 設定彈窗資料
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

.addInputRight .inputSpan  {
  margin-top: 5%;
}

.btnAddContainer {
  display: flex;
  justify-content: center;
  margin-top: 15px;
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
  gap: 15px;
}

.labRadioBox {
  flex: 1 1 auto;
  text-align: center;
  justify-content: center;
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