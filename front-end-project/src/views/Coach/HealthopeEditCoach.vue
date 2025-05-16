<template>
  <div class="addContainer">
    <TitleCard text="教練清單" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增教練"></SubTitleCard>
    <div class="sectionTitle"><p>基本資料區</p></div>
    <div class="basicInputContainer">
      <div class="leftInputBox">
        <div class="inputContainer">
          <InputSpan
            labelText="姓名"
            v-model="currentCoachData.Name"
            :required="true"
            @enter="editCoach"
          ></InputSpan>
          <InputSpan
            labelText="手機號碼"
            v-model="currentCoachData.Phone"
            :required="true"
            @enter="editCoach"
          ></InputSpan>
          <InputSpan
            labelText="信箱"
            v-model="currentCoachData.Email"
            :required="false"
            @enter="editCoach"
          ></InputSpan>
          <RadioInput
            class="coachStatus"
            v-model="currentCoachData.Status"
            :options="[
              { value: 'true', text: '啟用' },
              { value: 'false', text: '停用' },
            ]"
            inputTitle="狀態"
          />
          <div class="contractInputContainer">
            <InputSpan
              class="left"
              labelText="合約開始日"
              v-model="currentCoachData.ContractStartTime"
              inputType="date"
              @enter="editCoach"
            ></InputSpan>
            <InputSpan
              class="right"
              labelText="合約到期日"
              v-model="currentCoachData.ContractEndTime"
              inputType="date"
              @enter="editCoach"
            ></InputSpan>
          </div>
        </div>
      </div>
    </div>
    <div class="sectionTitle"><p>專業資訊區</p></div>
    <div class="professionalContainer">
      <label for="introduction">簡介</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="introduction"
        v-model="currentCoachData.Introduction"
      />
      <label for="specialty">特長</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="specialty"
        v-model="currentCoachData.Specialty"
      />
      <label for="certification">證照</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="certification"
        v-model="currentCoachData.Certification"
      />
    </div>
    <div class="sectionTitle"><p>圖片上傳區</p></div>
    <div class="imageUploadContainer">
      <label for="" class="labAvatar">請上傳頭像</label>
      <ImageUploader
        :previewUrl="currentCoachData.PhotoUrl"
        class="imageUpload"
        @imageSelected="handleImage"
      />
    </div>
    <div class="hintContainer">
      <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnContainer">
      <BtnConfirm @click="editCoach" text="確認修改"></BtnConfirm>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import InputSpan from "@/components/Input/InputSpan";
import RadioInput from "@/components/Input/RadioInput";
import ImageUploader from "@/components/Input/ImageUploader";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "HealthopeEditCoach",
  components: {
    TitleCard,
    SubTitleCard,
    InputSpan,
    BtnConfirm,
    RadioInput,
    ImageUploader,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      hintText: "",
      avatarFile: "",
      verifyFail: false,
      currentCoachData: {
        CoachId: 0,
        Name: "",
        Status: "false",
        Phone: "",
        Email: "",
        ContractStartTime: "",
        ContractEndTime: "",
        Introduction: "",
        Specialty: "",
        Certification: "",
        PhotoUrl: "",
        UpdateTime: "",
      },
      originalCoachData: {
        CoachId: 0,
        Name: "",
        Status: true,
        Phone: 999999999,
        Email: "",
        ContractStartTime: "",
        ContractEndTime: "",
        Introduction: "",
        Specialty: "",
        Certification: "",
        PhotoUrl: "",
        UpdateTime: "",
      },
    };
  },
  methods: {
    async editCoach() {
      if (!this.isDataModified()) {
        this.verifyFail = true;
        this.hintText = "請修改資料或返回";
        return;
      }

      if (!this.validInput()) return;

      try {
        // 傳輸新增資料
        const editCoachDto = {
          CoachId: this.currentCoachData.CoachId,
          UpdateTime: this.currentCoachData.UpdateTime,
          Name:
            this.currentCoachData.Name === this.originalCoachData.Name
              ? null
              : this.currentCoachData.Name,
          Phone:
            this.currentCoachData.Phone === this.originalCoachData.Phone
              ? null
              : this.currentCoachData.Phone,
          Email:
            this.currentCoachData.Email === this.originalCoachData.Email
              ? null
              : this.currentCoachData.Email,
          Status:
            this.currentCoachData.Status === this.originalCoachData.Status
              ? null
              : this.currentCoachData.Status,
          ContractStartTime:
            this.currentCoachData.ContractStartTime ===
            this.originalCoachData.ContractStartTime
              ? null
              : this.currentCoachData.ContractStartTime,
          ContractEndTime:
            this.currentCoachData.ContractEndTime ===
            this.originalCoachData.ContractEndTime
              ? null
              : this.currentCoachData.ContractEndTime,
          Introduction:
            this.currentCoachData.Introduction ===
            this.originalCoachData.Introduction
              ? null
              : this.currentCoachData.Introduction,
          Specialty:
            this.currentCoachData.Specialty === this.originalCoachData.Specialty
              ? null
              : this.currentCoachData.Specialty,
          Certification:
            this.currentCoachData.Certification ===
            this.originalCoachData.Certification
              ? null
              : this.currentCoachData.Certification,
        };

        // 考量到效率, 採用 form data 型式傳輸資料/檔案
        const formData = new FormData();
        formData.append("dataObject", JSON.stringify(editCoachDto));

        if (this.avatarFile) formData.append("avatarFile", this.avatarFile);

        // post後回傳
        const response = await this.$axios.post(
          "/api/Coach/EditCoach",
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          }
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.$router.push("/coach");
          return;
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增教練時發生錯誤", error);
      }
    },
    handleImage(file) {
      // 釋放前一個顯示的檔案
      this.revokePreviewUrl();

      // 用 ObjectURL 顯示預覽，不用 DataUR，效能較好
      this.currentCoachData.PhotoUrl = URL.createObjectURL(file);

      // 設定上傳用檔案
      this.avatarFile = file;
    },
    revokePreviewUrl() {
      if (this.currentCoachData.PhotoUrl) {
        URL.revokeObjectURL(this.currentCoachData.PhotoUrl);
        this.currentCoachData.PhotoUrl = null;
      }
    },
    validEmail(email) {
      // 可空
      if (!email) return true;

      // [^\s@] 代表至少一個不是空白或 @ 的字元
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // EX: abc@ewq.ee
      if (email.length > 254) return false; // 規定總長最長 254

      const parts = email.split("@");
      if (parts.length !== 2) return false;

      const [localPart, domain] = parts;

      if (
        localPart.length < 3 || // 建議最少 3 字元
        localPart.length > 64 || // 規定 @以前 最長 64
        domain.length > 251 // 不得超過 254 - 3
      ) {
        return false;
      }

      return emailRegex.test(email);
    },
    async getCoachEditDataById(id) {
      try {
        let coachIdDto = {
          CoachId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Coach/GetCoachEditDataById",
          coachIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.originalCoachData = response.data.ApiDataObject;
          this.originalCoachData.Phone = "0" + this.originalCoachData.Phone;
          this.currentCoachData = this.cleanData(this.originalCoachData);
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/coach";
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
        console.error("取得特定管理者時發生錯誤", error);
      }
    },
    isDataModified() {
      const cleanedCurrent = this.cleanData(this.currentCoachData);
      const cleanedOriginal = this.cleanData(this.originalCoachData);
      return JSON.stringify(cleanedCurrent) !== JSON.stringify(cleanedOriginal);
    },
    validInput() {
      // 格式驗證
      if (
        !this.currentCoachData.Name ||
        this.currentCoachData.Name.length > 15
      ) {
        this.verifyFail = true;
        this.hintText = "名稱格式錯誤";
        return false;
      }

      let phone = Number(this.currentCoachData.Phone);
      const phoneRegex = /^[9]\d{8}$/;
      if (Number.isNaN(phone) || !phoneRegex.test(phone)) {
        this.verifyFail = true;
        this.hintText = "手機格式錯誤";
        return false;
      }

      if (!this.validEmail(this.currentCoachData.Email)) {
        this.hintText = "信箱格式錯誤";
        this.verifyFail = true;
        return false;
      }

      if (
        this.currentCoachData.Status !== "true" &&
        this.currentCoachData.Status !== "false"
      ) {
        this.hintText = "狀態格式錯誤";
        this.verifyFail = true;
        return false;
      }

      // 只填合約開始日 或 只填合約結束日 或 結束日早於開始日
      if (
        (!this.currentCoachData.ContractStartTime &&
          this.currentCoachData.ContractStartTime) ||
        (this.currentCoachData.ContractStartTime &&
          !this.currentCoachData.ContractEndTime) ||
        new Date(this.currentCoachData.ContractEndTime) <
          new Date(this.currentCoachData.ContractStartTime)
      ) {
        this.hintText = "合約日期錯誤";
        this.verifyFail = true;
        return false;
      }

      if (this.currentCoachData.Introduction.length > 50) {
        this.hintText = "簡介輸入需在 50 字以內";
        this.verifyFail = true;
        return false;
      }

      if (this.currentCoachData.Specialty.length > 200) {
        this.hintText = "特長輸入需在 200 字以內";
        this.verifyFail = true;
        return false;
      }

      if (this.currentCoachData.Certification.length > 200) {
        this.hintText = "證照輸入需在 200 字以內";
        this.verifyFail = true;
        return false;
      }

      return true;
    },
    cleanData(data) {
      return {
        ...data,
        Name: data.Name.trim(),
        Status: String(data.Status).trim(),
        Phone: String(data.Phone).trim(),
        Email: data.Email.trim(),
        ContractStartTime: data.ContractStartTime.trim().substring(0, 10),
        ContractEndTime: data.ContractEndTime.trim().substring(0, 10),
        Introduction: data.Introduction.trim(),
        Specialty: data.Specialty.trim(),
        Certification: data.Certification.trim(),
        PhotoUrl: data.PhotoUrl.trim(),
      };
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/coach");
      return;
    }
    this.getCoachEditDataById(this.$route.query.id);
  },
  beforeDestroy() {
    this.revokePreviewUrl();
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
  margin-top: 7px;
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

.leftInputBox {
  width: 80%;
  max-width: 350px;
}

.coachStatus {
  margin-bottom: 3%;
}

.contractInputContainer {
  display: flex;
  gap: 5px;
  width: 100%;
}

.contractInputContainer .left,
.contractInputContainer .right {
  display: flex;
  width: 50%;
}
</style>

<style scoped>
.professionalContainer {
  display: flex;
  flex-direction: column;
  gap: 2px;
  width: 75%;
  margin-left: 10%;
}

.professionalContainer label,
.labAvatar {
  font-weight: 500;
  font-size: 18px;
}

.professionalContainer textarea {
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
}

.professionalContainer textarea:focus {
  outline: 2px solid #707070;
}

.imageUploadContainer {
  margin-left: 10%;
  max-width: 70%;
  display: flex;
  flex-direction: column;
}

.imageUpload {
  max-width: 100%;
  width: 225px;
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