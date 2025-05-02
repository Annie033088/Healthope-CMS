<template>
  <div>
    <SubTitleCard text="修改密碼"></SubTitleCard>
    <div class="inputBox">
      <div class="inputContainer">
        <InputSpan
          class="inputSpan"
          labelText="請輸入舊密碼"
          v-model="oldPwd"
          @enter="editSelfPwd"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="請輸入新密碼"
          v-model="newPwd"
          inputType="password"
          @enter="editSelfPwd"
        ></InputSpan>
        <InputSpan
          class="inputSpan"
          labelText="請再輸入一次新密碼"
          v-model="newAgainPwd"
          inputType="password"
          @enter="editSelfPwd"
        ></InputSpan>
      </div>
    </div>
    <div class="hintContainer">
      <span v-if="editFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnAddContainer">
      <BtnConfirm @click="editSelfPwd" text="確認"></BtnConfirm>
    </div>
  </div>
</template>


<script>
import SubTitleCard from "@/components/Card/SubTitleCard";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import InputSpan from "@/components/Input/InputSpan";

export default {
  name: "HealthopeEditSelfPwd",
  components: {
    SubTitleCard,
    InputSpan,
    BtnConfirm,
  },
  props:{
    notificationBoxConfirmFlag:Boolean
  },
  data() {
    return {
      hintText: "",
      oldPwd: "",
      newPwd: "",
      newAgainPwd: "",
      editFail: false,
    };
  },
  methods: {
    async editSelfPwd() {
      // 帳號密碼驗證用的正規表達式
      const regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;

      if (!(regex.test(this.oldPwd) && regex.test(this.newPwd))) {
        this.hintText = "請輸入 8~20 位英文數字";
        this.editFail = true;
        return;
      }

      if (this.newPwd !== this.newAgainPwd) {
        this.hintText = "兩次密碼輸入不一致";
        this.editFail = true;
        return;
      }

      if (this.oldPwd === this.newPwd) {
        this.hintText = "新舊密碼不可相同";
        this.editFail = true;
        return;
      }

      try {
        // 傳輸登入資料
        const editAdminPwd = {
          OldPwd : this.oldPwd,
          NewPwd: this.newPwd
        };

        // post後回傳
        const response = await this.$axios.post(
          "/api/AccountAccess/EditSelfPwd",
          editAdminPwd
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.editFail = false;
          this.$router.go(0);
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
        console.error("修改密碼時發生錯誤", error);
      }
    },
  },
};
</script>

<style scoped>
.inputBox {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  margin-top: 25px;
  justify-content: center;
}

.inputContainer {
  width: 60%;
  max-width: 350px;
}

.btnAddContainer {
  display: flex;
  justify-content: center;
  margin-top: 15px;
  margin-left: 55px;
}

.hintContainer {
  margin-top: 15px;
  margin-left: 55px;
  display: flex;
  justify-content: center;
}

.hintSpan {
  color: #707070;
  animation: slideInTop 0.5s cubic-bezier(0.25, 0.46, 0.45, 0.94) both;
}
</style>