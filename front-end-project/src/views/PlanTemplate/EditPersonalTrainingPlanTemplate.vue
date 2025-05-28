<template>
  <div class="">
    <TitleCard text="方案" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="修改會籍方案"></SubTitleCard>
    <div class="sectionTitle">
      <h3>課程名稱：{{ currentPersonalTrainingPackageData.Name }}</h3>
    </div>
    <div class="inputBox">
      <InputSpan
        class="inputPlan"
        labelText="介紹"
        v-model="currentPersonalTrainingPackageData.Introduction"
      ></InputSpan>
      <RadioInput
        class="inputPlan"
        v-model="currentPersonalTrainingPackageData.Status"
        :options="[
          { value: 'true', text: '有效' },
          { value: 'false', text: '無效' },
        ]"
        inputTitle="狀態"
        inputType="radioStatus"
      />
      <RadioInput
        class="inputPlan"
        v-model="currentPersonalTrainingPackageData.Display"
        :options="[
          { value: 'true', text: '顯示' },
          { value: 'false', text: '不顯示' },
        ]"
        inputTitle="顯示在前台"
        inputType="radioDisplay"
      />
      <div class="imageUploadContainer">
        <label for="" class="labAvatar">請上傳展示圖片</label>
        <ImageUploader
          :previewUrl="currentPersonalTrainingPackageData.ImageUrl"
          class="imageUpload"
          @imageSelected="handleImage"
        />
      </div>
    </div>
    <div class="hintContainer">
      <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnContainer">
      <BtnConfirm @click="editPlan" text="修改方案"></BtnConfirm>
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
  name: "EditPersonalTrainingPlanTemplate",
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
      originalPersonalTrainingPackageData: {
        PersonalTrainingPackageId: 0,
        Name: "",
        Display: false,
        Introduction: "",
        Status: false,
        ImageUrl: "",
        UpdateTime: "",
      },
      currentPersonalTrainingPackageData: {
        PersonalTrainingPackageId: 0,
        Name: "",
        Display: "false",
        Introduction: "",
        Status: "false",
        ImageUrl: "",
        UpdateTime: "",
      },
      avatarFile: "",
    };
  },
  methods: {
    handleImage(file) {
      // 釋放前一個顯示的檔案
      this.revokePreviewUrl();

      // 用 ObjectURL 顯示預覽，不用 DataUR，效能較好
      this.currentPersonalTrainingPackageData.ImageUrl =
        URL.createObjectURL(file);

      // 設定上傳用檔案
      this.avatarFile = file;
    },
    revokePreviewUrl() {
      if (this.currentPersonalTrainingPackageData.ImageUrl) {
        URL.revokeObjectURL(this.currentPersonalTrainingPackageData.ImageUrl);
        this.currentPersonalTrainingPackageData.ImageUrl = null;
      }
    },
    async editPlan() {
      if (!this.isDataModified()) {
        this.verifyFail = true;
        this.hintText = "請修改資料或返回";
        return;
      }

      this.currentPersonalTrainingPackageData = this.cleanData(this.currentPersonalTrainingPackageData);

      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }

      try {
        let editPlanDto = {
          PersonalTrainingPackageId:
            this.currentPersonalTrainingPackageData.PersonalTrainingPackageId,
          UpdateTime: this.currentPersonalTrainingPackageData.UpdateTime,
          Display:
            this.currentPersonalTrainingPackageData.Display ===
            this.originalPersonalTrainingPackageData.Display
              ? null
              : this.currentPersonalTrainingPackageData.Display,
          Introduction:
            this.currentPersonalTrainingPackageData.Introduction ===
            this.originalPersonalTrainingPackageData.Introduction
              ? null
              : this.currentPersonalTrainingPackageData.Introduction,
          Status:
            this.currentPersonalTrainingPackageData.Status ===
            this.originalPersonalTrainingPackageData.Status
              ? null
              : this.currentPersonalTrainingPackageData.Status,
        };

        const formData = new FormData();
        formData.append("dataObject", JSON.stringify(editPlanDto));

        if (this.avatarFile) formData.append("avatarFile", this.avatarFile);

        const response = await this.$axios.post(
          "/api/PlanTemplate/EditPersonalTrainingPackage",
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          }
        );

        // post後回傳
        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.$router.push("/plan/personalTrainingPackage");
          return;
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改方案發生錯誤", error);
      }
    },
    validInput() {
      // 格式驗證
      if (
        this.currentPersonalTrainingPackageData.Status !== "false" &&
        this.currentPersonalTrainingPackageData.Status !== "true"
      ) {
        this.hintText = "有/無效選擇錯誤";
        return false;
      }

      if (
        this.currentPersonalTrainingPackageData.Introduction &&
        this.currentPersonalTrainingPackageData.Introduction.length > 200
      ) {
        this.hintText = "介紹需輸入 200 字內";
        return false;
      }

      if (
        this.currentPersonalTrainingPackageData.Display !== "false" &&
        this.currentPersonalTrainingPackageData.Display !== "true"
      ) {
        this.hintText = "是否顯示選擇錯誤";
        return false;
      }

      return true;
    },
    async getPersonalTrainingPackageById(id) {
      try {
        let getPersonalTrainingPackageByIdDto = {
          PersonalTrainingPackageId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/PlanTemplate/GetPersonalTrainingPackageEditDataById",
          getPersonalTrainingPackageByIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.originalPersonalTrainingPackageData =
            response.data.ApiDataObject;
          this.originalPersonalTrainingPackageData.PersonalTrainingPackageId = id;

          this.originalPersonalTrainingPackageData = this.cleanData(
            this.originalPersonalTrainingPackageData
          );
          this.currentPersonalTrainingPackageData = this.cleanData(
            this.originalPersonalTrainingPackageData
          );
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/plan/personalTrainingPackage";
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
        console.error("取得特定教練課方案時發生錯誤", error);
      }
    },
    cleanData(data) {
      return {
        ...data,
        Status: String(data.Status).trim(),
        Display: String(data.Display).trim(),
        Introduction: data.Introduction.trim(),
        ImageUrl: data.ImageUrl.trim(),
      };
    },
    isDataModified() {
      const cleanedCurrent = this.cleanData(
        this.currentPersonalTrainingPackageData
      );
      const cleanedOriginal = this.cleanData(
        this.originalPersonalTrainingPackageData
      );
      return JSON.stringify(cleanedCurrent) !== JSON.stringify(cleanedOriginal);
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/plan/personalTrainingPackage");
      return;
    }

    this.getPersonalTrainingPackageById(this.$route.query.id);
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
}

.inputBox {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.inputBox > div {
  min-width: 200px; /* 子元素最小寬度 */
  margin: 5px;
}

.inputPlan {
  max-width: 40%;
  width: 300px;
  margin-bottom: 1%;
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