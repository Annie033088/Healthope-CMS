<template>
  <div class="">
    <TitleCard text="方案" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增方案"></SubTitleCard>
    <div class="typeContainer">
      <RadioInput
        v-model="type"
        :options="[
          { value: 'membershipPlan', text: '會籍' },
          { value: 'personalTrainingPackage', text: '教練課' },
          { value: 'ticketPlan', text: '入場劵' },
        ]"
        inputTitle="請選擇方案類型"
        inputType="radioType"
        @change="changeType"
      />
    </div>
    <div v-if="ticketFlag || membershipFlag || personalTrainingFlag">
      <div class="inputBox">
        <div :class="ticketFlag ? 'ticketInputContainer' : 'inputContaner'">
          <InputSpan
            v-if="membershipFlag || personalTrainingFlag"
            class="inputPlan"
            labelText="名稱"
            v-model="name"
            :required="true"
          ></InputSpan>
          <InputSpan
            v-if="membershipFlag || personalTrainingFlag"
            class="inputPlan"
            labelText="介紹"
            v-model="introduction"
          ></InputSpan>
          <div v-if="membershipFlag" class="inputPlan">
            <InputSpan
              class="membershipExpiryInput"
              labelText="方案期限 ( 月 )"
              v-model="duration"
              :required="true"
            ></InputSpan>
          </div>
          <div v-else-if="personalTrainingFlag" class="inputPlan">
            <InputSpan
              class="sessionCountInput"
              labelText="課程堂數"
              v-model="sessionCount"
              :required="true"
            ></InputSpan>
          </div>
          <InputSpan
            class="inputPlan"
            labelText="價格"
            v-model="price"
            :required="true"
          ></InputSpan>
          <RadioInput
            class="inputPlan"
            v-model="status"
            :options="[
              { value: 'true', text: '有效' },
              { value: 'false', text: '無效' },
            ]"
            inputTitle="狀態"
            inputType="radioStatus"
          />
          <RadioInput
            v-if="membershipFlag || personalTrainingFlag"
            class="inputPlan"
            v-model="display"
            :options="[
              { value: 'true', text: '顯示' },
              { value: 'false', text: '不顯示' },
            ]"
            inputTitle="顯示在前台"
            inputType="radioDisplay"
          />
        </div>
        <div class="imageUploadBox">
          <div
            class="imageUploadContainer"
            v-if="membershipFlag || personalTrainingFlag"
          >
            <label for="" class="labAvatar">請上傳展示圖片</label>
            <ImageUploader
              :previewUrl="previewUrl"
              class="imageUpload"
              @imageSelected="handleImage"
            />
          </div>
        </div>
      </div>
      <div class="hintContainer">
        <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnContainer">
        <BtnConfirm @click="addPlan" text="新增方案"></BtnConfirm>
      </div>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import RadioInput from "@/components/Input/RadioInput";
import InputSpan from "@/components/Input/InputSpan";
import ImageUploader from "@/components/Input/ImageUploader";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "AddPlanTemplate",
  components: {
    TitleCard,
    SubTitleCard,
    RadioInput,
    InputSpan,
    ImageUploader,
    BtnConfirm,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      hintText: "",
      verifyFail: false,
      type: "",
      price: "",
      display: "false",
      duration: "",
      sessionCount: "",
      name: "",
      introduction: "",
      status: "false",
      avatarFile: "",
      previewUrl: "",
    };
  },
  methods: {
    handleImage(file) {
      // 釋放前一個顯示的檔案
      this.revokePreviewUrl();

      // 用 ObjectURL 顯示預覽，不用 DataUR，效能較好
      this.previewUrl = URL.createObjectURL(file);

      // 設定上傳用檔案
      this.avatarFile = file;
    },
    revokePreviewUrl() {
      if (this.previewUrl) {
        URL.revokeObjectURL(this.previewUrl);
        this.previewUrl = null;
      }
    },
    async addPlan() {
      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }
      try {
        let { addPlanDto, postPath } = this.GetAddPlanDtoAndPostPath();
        let response;

        if (this.ticketFlag) {
          response = await this.$axios.post(postPath, addPlanDto);
        } else if (this.membershipFlag || this.personalTrainingFlag) {
          const formData = new FormData();
          formData.append("dataObject", JSON.stringify(addPlanDto));

          if (this.avatarFile) formData.append("avatarFile", this.avatarFile);

          response = await this.$axios.post(postPath, formData, {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          });
        } else return;

        // post後回傳
        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.$router.push("/plan");
          return;
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增方案發生錯誤", error);
      }
    },
    validInput() {
      // 格式驗證
      const IntMax = 2147483647;
      let price = Number(this.price);
      if (
        !Number.isInteger(price) ||
        price < 1 ||
        // 超出安全整數範圍
        price > IntMax
      ) {
        this.hintText = "價格錯誤";
        return false;
      }

      if (this.status !== "false" && this.status !== "true") {
        this.hintText = "有/無效選擇錯誤";
        return false;
      }

      if (this.membershipFlag || this.personalTrainingFlag)
        if (!this.name || this.name.length > 20) {
          this.hintText = "名稱需輸入 20 字";
          return false;
        }

      if (this.membershipFlag || this.personalTrainingFlag)
        if (this.introduction && this.introduction.length > 200) {
          this.hintText = "介紹需輸入 200 字內";
          return false;
        }

      if (this.membershipFlag || this.personalTrainingFlag)
        if (this.display !== "false" && this.display !== "true") {
          this.hintText = "是否顯示選擇錯誤";
          return false;
        }

      const tinyIntMax = 255;
      let duration = Number(this.duration);
      if (this.membershipFlag)
        if (
          !Number.isInteger(duration) ||
          duration < 1 ||
          // 超出安全整數範圍
          duration > tinyIntMax
        ) {
          this.hintText = "會員期限錯誤(需在 255 個月內)";
          return false;
        }

      let sessionCount = Number(this.sessionCount);
      if (this.personalTrainingFlag)
        if (
          !Number.isInteger(sessionCount) ||
          sessionCount < 1 ||
          // 超出安全整數範圍
          sessionCount > IntMax
        ) {
          this.hintText = "價格錯誤";
          return false;
        }

      return true;
    },
    changeType() {
      this.name = "";
      this.introduction = "";
      this.duration = "";
      this.sessionCount = "";
      this.price = "";
      this.status = "false";
      this.display = "fasle";
      this.avatarFile = "";
      this.revokePreviewUrl();
    },
    GetAddPlanDtoAndPostPath() {
      let postPath = "";
      let addPlanDto = {
        Price: this.price,
        Status: this.status,
      };

      if (this.ticketFlag) {
        postPath = "/api/PlanTemplate/AddTicketPlan";
        return { addPlanDto, postPath };
      } else if (this.membershipFlag) {
        postPath = "/api/PlanTemplate/AddMembershipPlan";
        addPlanDto.Name = this.name;
        addPlanDto.Introduction = this.introduction;
        addPlanDto.Duration = this.duration;
        addPlanDto.Display = this.display;
        return { addPlanDto, postPath };
      } else if (this.personalTrainingFlag) {
        postPath = "/api/PlanTemplate/AddPersonalTrainingPackage";
        addPlanDto.Name = this.name;
        addPlanDto.Introduction = this.introduction;
        addPlanDto.SessionCount = this.sessionCount;
        addPlanDto.Display = this.display;
        return { addPlanDto, postPath };
      } else {
        return { addPlanDto: {}, postPath: "" };
      }
    },
  },
  computed: {
    membershipFlag() {
      return this.type === "membershipPlan";
    },
    personalTrainingFlag() {
      return this.type === "personalTrainingPackage";
    },
    ticketFlag() {
      return this.type === "ticketPlan";
    },
  },
};
</script>

<style scoped>
.typeContainer {
  display: flex;
  justify-content: center;
  margin-top: 2%;
  margin-bottom: 1%;
}

.inputBox {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  width: 100%;
}
.inputBox > div {
  min-width: 200px; /* 子元素最小寬度 */
  margin: 5px;
}

.inputContaner {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.ticketInputContainer {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.inputPlan {
  width: 350px;
  max-width: 80%;
  min-width: 220px;
  margin-bottom: 1%;
}

.imageUploadBox {
  display: flex;
  justify-content: center;
}

.imageUploadContainer {
  display: flex;
  flex-direction: column;
  max-width: 70%;
}

.imageUpload {
  max-width: 90%;
  width: 350px;
}

.labAvatar {
  font-weight: 500;
  font-size: 18px;
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