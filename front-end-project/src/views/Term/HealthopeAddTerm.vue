<template>
  <div>
    <TitleCard text="條款" @refreshPage="$emit('refreshPage')"></TitleCard>
    <SubTitleCard text="新增條款"></SubTitleCard>
    <div class="StepOneContainer">
      <h2 class="">新增條款</h2>
      <!-- 1. 條款類型 -->
      <div class="input">
        <RadioInput
          v-model="form.type"
          :options="[
            { value: '1', text: '服務條款' },
            { value: '2', text: '隱私權政策' },
          ]"
          inputTitle="條款類型"
          inputType="selectType"
        />
      </div>

      <!-- 2. 適用對象 -->
      <div class="input">
        <RadioInput
          v-model="form.target"
          :options="[
            { value: '1', text: '會員' },
            { value: '2', text: '教練' },
          ]"
          inputTitle="適用對象"
          inputType="selectApplicableTarget"
        />
      </div>

      <!-- 3. 是否參考舊條款 -->
      <div class="input">
        <RadioInput
          v-model="form.useOld"
          :options="[
            { value: 'true', text: '是' },
            { value: 'false', text: '否' },
          ]"
          inputTitle="是否參考舊條款"
          inputType="selectReferenceOld"
          @change="changeReferenceOld"
        />
      </div>

      <!-- 顯示選擇舊條款的下拉（若選擇「是」） -->
      <div class="input" v-if="form.useOld === 'true'">
        <SelectInput
          labelText="選擇參考條款"
          :parentValue.sync="form.referenceId"
          :options="oldTerms"
        />
      </div>

      <div class="hintContainer">
        <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnContainer">
        <BtnConfirm
          class="btnConfirm"
          @click="submit"
          text="下一步"
        ></BtnConfirm>
      </div>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import RadioInput from "@/components/Input/RadioInput";
import SelectInput from "@/components/Input/SelectInput";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "HealthopeAddTerm",
  components: {
    TitleCard,
    SubTitleCard,
    RadioInput,
    SelectInput,
    BtnConfirm,
  },
  data() {
    return {
      verifyFail: false,
      hintText: "",
      form: {
        type: "",
        target: "",
        useOld: "false",
        referenceId: "",
      },
      oldTerms: [
        { value: 1, text: "2023 會員條款 v1.0" },
        { value: 2, text: "課程條款 2024-01" },
      ],
    };
  },
  methods: {
    submit() {},
    changeReferenceOld(value) {
      console.log(value)
      if (this.form.useOld === "true")
        if (!this.selectTypeFlag || !this.selectTargetFlag) {
          this.form.useOld = "false"
        }
    },
  },
  computed: {
    selectTypeFlag() {
      return this.form.type === "true" || this.form.type === "false";
    },
    selectTargetFlag() {
      return this.form.target === "true" || this.form.target === "false";
    },
  },
};
</script>

<style scoped>
.StepOneContainer {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  gap: 15px;
}

.input {
  width: 350px;
  max-width: 60%;
}

.btnContainer,
.hintContainer {
  display: flex;
  justify-content: center;
  margin-bottom: 5px;
}

.btnConfirm {
  width: 200px;
  max-width: 75%;
  height: 43px;
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