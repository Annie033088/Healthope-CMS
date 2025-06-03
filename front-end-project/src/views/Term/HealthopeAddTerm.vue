<template>
  <div>
    <TitleCard text="條款" @refreshPage="$emit('refreshPage')"></TitleCard>
    <SubTitleCard text="新增條款"></SubTitleCard>
    <div class="StepOneContainer" v-if="step === 1">
      <h2 class="">新增條款</h2>
      <!-- 1. 條款類型 -->
      <div class="input">
        <RadioInput
          v-model="type"
          :options="[
            { value: '1', text: '服務條款' },
            { value: '2', text: '隱私權政策' },
          ]"
          inputTitle="條款類型"
          inputType="selectType"
          @change="typeGetOldTerm"
        />
      </div>

      <!-- 2. 適用對象 -->
      <div class="input">
        <RadioInput
          v-model="target"
          :options="[
            { value: '1', text: '會員' },
            { value: '2', text: '教練' },
          ]"
          inputTitle="適用對象"
          inputType="selectApplicableTarget"
          @change="targerGetOldTerm"
        />
      </div>

      <!-- 3. 是否參考舊條款 -->
      <div class="input" v-if="selectTypeFlag && selectTargetFlag">
        <RadioInput
          v-model="useOld"
          :options="[
            { value: 'true', text: '是' },
            { value: 'false', text: '否' },
          ]"
          inputTitle="是否參考舊條款"
          inputType="selectReferenceOld"
          @change="useOldGetOldTerm"
        />
      </div>

      <!-- 顯示選擇舊條款的下拉（若選擇「是」） -->
      <div class="input" v-if="useOld === 'true'">
        <SelectInput
          labelText="選擇參考條款"
          :parentValue.sync="referenceId"
          :options="oldTermOption"
        />
      </div>
      <div class="hintContainer">
        <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnContainer" v-if="selectTypeFlag && selectTargetFlag">
        <BtnConfirm @click="nextStep" text="下一步"></BtnConfirm>
      </div>
    </div>
    <div class="stepTwoContainer" v-if="step === 2">
      <h2 class="">新增條款</h2>
      <h3 class="">{{ name }}</h3>
      <label for="versionDescription">請描述更新內容</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="versionDescription"
        v-model="versionDescription"
        @keydown.enter="addTerm"
      />
      <label for="detailContent">內文</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="detailContent"
        v-model="detailContent"
        @keydown.enter="addTerm"
      />
      <div class="hintContainer">
        <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnContainer">
        <BtnConfirm @click="previousStep" text="上一步"></BtnConfirm>
        <BtnConfirm @click="addTerm" text="新增"></BtnConfirm>
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
      step: 1,
      type: "",
      target: "",
      useOld: "false",
      referenceId: "",
      oldTerms: [],
      oldTermOption: [],
      name: "",
      versionDescription: "",
      detailContent: "",
    };
  },
  methods: {
    nextStep() {
      let referenceTerm;
      if (this.useOld === "true" && this.referenceId === "") {
        this.hintText = "請選擇參考條款";
        this.verifyFail = true;
        return;
      } else if (this.useOld === "true") {
        referenceTerm = this.oldTerms.find(
          (term) => term.TermId === Number(this.referenceId)
        );
        this.detailContent = referenceTerm.DetailContent;
      }

      this.verifyFail = false;
      this.name = "";

      if (this.target === "1") this.name = "會員";
      else if (this.target === "2") this.name = "教練";

      if (this.type === "1") this.name = this.name + " - " + "服務條款";
      else if (this.type === "2") this.name = this.name + " - " + "隱私權政策";

      this.step = 2;
    },
    previousStep() {
      this.verifyFail = false;
      this.detailContent = "";
      this.versionDescription = "";
      this.referenceId = "";
      this.step = 1;
    },
    typeGetOldTerm(type) {
      this.referenceId = "";

      if (type !== "1" && type !== "2") {
        this.hintText = "錯誤的類型";
        return;
      }

      if (!this.selectTypeFlag) return;
      if (this.useOld !== "true") return;

      this.getOldTerm();
    },
    targerGetOldTerm(target) {
      this.referenceId = "";

      if (target !== "1" && target !== "2") {
        this.hintText = "錯誤的對象";
        return;
      }

      if (!this.selectTargetFlag) return;
      if (this.useOld !== "true") return;

      this.getOldTerm();
    },
    useOldGetOldTerm(useOld) {
      this.referenceId = "";
      if (useOld !== "true") return;
      if (!this.selectTypeFlag) return;
      if (!this.selectTargetFlag) return;

      this.getOldTerm();
    },
    async getOldTerm() {
      let getOldTermDto = {
        Type: this.type,
        ApplicableTarget: this.target,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Term/GetOldTerm",
          getOldTermDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.oldTerms = response.data.ApiDataObject;
          this.oldTermOption = [];
          this.oldTermOption.push({ value: "", text: "請選擇條款" });
          this.oldTerms.forEach((term) => {
            let oldTerm = {
              value: term.TermId,
              text: term.Name + " - " + term.Version,
            };
            this.oldTermOption.push(oldTerm);
          });
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/term";
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
        console.error("取得舊條款時發生錯誤", error);
      }
    },
    async addTerm() {
      if (!this.validInput()) this.verifyFail = true;
      
      this.detailContent = this.detailContent.trim();
      this.versionDescription = this.versionDescription.trim();

      let addTermDto = {
        Type: this.type,
        ApplicableTarget: this.target,
        DetailContent: this.detailContent,
        VersionDescription: this.versionDescription,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Term/AddTerm",
          addTermDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$router.push("/term");
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增條款時發生錯誤", error);
      }
    },
    validInput() {
      if (this.detailContent && this.detailContent.length > 7000) {
        this.hintText = "請輸入 7000 字內內文";
        return false;
      }

      if (this.versionDescription && this.versionDescription.length > 200) {
        this.hintText = "請輸入 200 字內描述更新內容";
        return false;
      }

      if (!this.selectTargetFlag) return false;
      if (!this.selectTypeFlag) return false;

      if (this.referenceId && this.referenceId < 1) {
        this.hintText = "舊條款選擇錯誤";
        return false;
      }

      return true;
    },
  },
  computed: {
    selectTypeFlag() {
      return this.type === "1" || this.type === "2";
    },
    selectTargetFlag() {
      return this.target === "1" || this.target === "2";
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
  gap: 5px;
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

<style scoped>
.stepTwoContainer {
  display: flex;
  flex-direction: column;
  gap: 2px;
  width: 80%;
  margin-left: 10%;
  align-items: center;
}

.stepTwoContainer label {
  font-weight: 500;
  font-size: 18px;
  margin-bottom: 5px;
}

.stepTwoContainer textarea {
  width: 100%;
  padding: 12px 16px;
  border-radius: 8px;
  resize: none;
  height: 96px;
  border: none;
  outline: 2px solid #efefef;
  font-family: inherit;
  font-weight: bold;
  font-size: 16px;
  color: #333;
  margin-bottom: 2%;
}

.stepTwoContainer textarea:focus {
  outline: 2px solid #707070;
}

.stepTwoContainer .inputSpanContainer {
  width: 300px;
  margin-bottom: 2%;
}
</style>