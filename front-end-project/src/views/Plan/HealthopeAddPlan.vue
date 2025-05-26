<template>
  <div class="">
    <TitleCard text="方案" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增課程"></SubTitleCard>
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
      />
    </div>
    <div class="inputBox">
      <div v-if="type" class="inputContaner">
        <InputSpan
          v-if="membershipFlag || personalTrainingFlag"
          class="inputPlan"
          labelText="名稱"
          v-model="name"
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
            labelText="方案期限"
            v-model="duration"
          ></InputSpan>
        </div>
        <div v-else-if="personalTrainingFlag" class="inputPlan">
          <InputSpan
            class="sessionCountInput"
            labelText="課程堂數"
            v-model="duration"
          ></InputSpan>
        </div>
        <InputSpan
          class="inputPlan"
          labelText="價格"
          v-model="price"
        ></InputSpan>
        <RadioInput
          class="inputPlan"
          v-model="status"
          :options="[
            { value: '1', text: '有效' },
            { value: '0', text: '無效' },
          ]"
          inputTitle="狀態"
          inputType="radioStatus"
        />
        <RadioInput
          v-if="membershipFlag || personalTrainingFlag"
          class="inputPlan"
          v-model="display"
          :options="[
            { value: '1', text: '顯示' },
            { value: '0', text: '不顯示' },
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
          <label for="" class="labAvatar">請上傳頭像</label>
          <ImageUploader
            :previewUrl="previewUrl"
            class="imageUpload"
            @imageSelected="handleImage"
          />
        </div>
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

export default {
  name: "HealthopeAddPlan",
  components: {
    TitleCard,
    SubTitleCard,
    RadioInput,
    InputSpan,
    ImageUploader,
  },
  data() {
    return {
      type: "",
      price: "",
      display: "0",
      duration: "",
      name: "",
      introduction: "",
      status: "0",
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
  },
  computed: {
    membershipFlag() {
      return this.type === "membershipPlan";
    },
    personalTrainingFlag() {
      return this.type === "personalTrainingPackage";
    },
  },
};
</script>

<style scoped>
.typeContainer {
  display: flex;
  justify-content: center;
  margin-top: 3%;
  margin-bottom: 2%;
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
  width: 49%;
}

.inputPlan {
  width: 350px;
  max-width: 60%;
  min-width: 220px;
  margin-bottom: 1%;
}

.imageUploadBox {
  display: flex;
  justify-content: center;
  width: 49%;
}

.imageUploadContainer {
  display: flex;
  flex-direction: column;
  max-width: 70%;
}

.imageUpload {
  max-width: 100%;
  width: 225px;
}

.labAvatar {
  font-weight: 500;
  font-size: 18px;
}
</style>