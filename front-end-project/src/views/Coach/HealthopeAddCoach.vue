<template>
  <div class="addContainer">
    <TitleCard text="教練清單" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增教練"></SubTitleCard>
    <div class="sectionTitle"><p>基本資料區</p></div>
    <div class="basicInputContainer">
      <div class="leftInputBox">
        <div class="inputContainer">
          <InputSpan
            class="inputSpan"
            labelText="姓名"
            v-model="name"
            :required="true"
          ></InputSpan>
          <InputSpan
            class="inputSpan"
            labelText="手機號碼"
            v-model="phone"
            :required="true"
          ></InputSpan>
          <InputSpan
            class="inputSpan"
            labelText="信箱"
            v-model="email"
            :required="false"
          ></InputSpan>
          <RadioInput
            v-model="selectType"
            :options="[
              { value: '1', text: '私人教練' },
              { value: '2', text: '約聘教練' },
            ]"
            inputTitle="請選擇教練類別"
          />
        </div>
      </div>
      <div class="rightInputBox">
        <div class="inputContainer">
          <InputSpan
            class="inputSpan"
            labelText="帳號"
            v-model="account"
            :required="true"
          ></InputSpan>
          <InputSpan
            class="inputSpan"
            labelText="密碼"
            v-model="pwd"
            inputType="password"
            :required="true"
          ></InputSpan>
          <InputSpan
            class="inputSpan"
            labelText="再輸入一次密碼"
            v-model="pwdAgain"
            inputType="password"
            :required="true"
          ></InputSpan>
        </div>
      </div>
    </div>
    <div class="sectionTitle"><p> 專業資訊區</p></div>
    <div class="form-group">
          <label for="textarea">簡介</label>
          <textarea required="" cols="50" rows="10" id="textarea" name="textarea">          </textarea>
        </div>
    <div class="hintContainer">
      <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnContainer">
      <BtnConfirm @click="addCoach" text="加入教練"></BtnConfirm>
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
  name: "HealthopeAddCoach",
  components: {
    TitleCard,
    SubTitleCard,
    InputSpan,
    BtnConfirm,
    RadioInput,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      name: "",
      hintText: "",
      account: "",
      pwd: "",
      pwdAgain: "",
      phone: "",
      email: "",
      verifyFail: false,
      selectType: "1",
    };
  },
  methods: {
    addCoach() {},
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
  margin-top: 15px;
}

.sectionTitle p {
  font-size: 20px;
  font-weight: 700;
}

.basicInputContainer {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-evenly;
}

.leftInputBox,
.rightInputBox {
  width: 60%;
  max-width: 350px;
}

.inputSpan {
  margin-bottom: 3%;
}

.hintContainer,
.btnContainer {
  display: flex;
  margin-top: 15px;
  justify-content: center;
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
</style>

<style scoped>
.form-group {
  display: flex;
  flex-direction: column;
  gap: 2px;
  width: 80%;
  margin-left: 10%;
}

.form-group label{
  font-weight: 600;
}

.form-group textarea {
  width: 100%;
  padding: 12px 16px;
  border-radius: 8px;
  resize: none;
  height: 96px;
  border: 1px solid #414141;
  background-color: transparent;
  font-family: inherit;
}

.form-group textarea:focus {
  outline: none;
  border-color: #e81cff;
}
</style>