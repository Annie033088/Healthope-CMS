<template>
  <div>
    <TitleCard text="管理員"></TitleCard>
    <SubTitleCard text="新增管理員"></SubTitleCard>
    <div class="addInputContainer">
      <div class="addInputLeft">
        <InputSpan
          class="inputSpan"
          labelText="帳號"
          v-model="account"
          @enter="addAdmin"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="密碼"
          v-model="pwd"
          inputType="password"
          @enter="addAdmin"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="再輸入一次密碼"
          v-model="pwdAgain"
          inputType="password"
          @enter="addAdmin"
        ></InputSpan>
      </div>
      <div class="addInputRight">
        <RadioInput
          v-model="selectIdentity"
          :options="[
            { value: 'None', text: '無' },
            { value: 'Admin', text: '管理員' },
            { value: 'Receptionist', text: '櫃檯人員' },
            { value: 'Accountant', text: '會計' },
            { value: 'CourseManager', text: '課程管理' },
            { value: 'CoachManager', text: '教練管理' },
            { value: 'SalesRepresentative', text: '業務' },
          ]"
          inputTitle="請選擇身份"
        />
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
import RadioInput from "@/components/Input/RadioInput";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "HealthopeAddAdmin",
  components: {
    TitleCard,
    SubTitleCard,
    InputSpan,
    BtnConfirm,
    RadioInput,
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
      this.account = this.account.trim();
      this.pwd = this.pwd.trim();
      this.pwdAgain = this.pwdAgain.trim();
      this.selectIdentity = this.selectIdentity.trim();
      // 帳號密碼驗證用的正規表達式 ( 8~20 位英數字)
      const regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;

      if (!(regex.test(this.account) && regex.test(this.pwd))) {
        this.hintText = "請輸入 8~20 位英文數字";
        this.addFail = true;
        return;
      }

      if (this.account === this.pwd) {
        this.hintText = "帳號密碼不可相同";
        this.addFail = true;
        return;
      }

      if (this.pwd !== this.pwdAgain) {
        this.hintText = "兩次密碼輸入不一致";
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

.inputSpan {
  margin-top: 5%;
}

.btnAddContainer,
.hintContainer {
  display: flex;
  justify-content: center;
  margin-top: 15px;
}

.hintSpan {
  color: #c07878;
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
  padding: 0.2rem;
  width: 100%;
  max-width: 350px;
  font-size: 16px;
  margin-top: 5px;
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