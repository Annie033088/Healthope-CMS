<template>
  <div class="">
    <TitleCard text="教練" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增教練"></SubTitleCard>
    <div class="sectionTitle"><p>教練基本資料區</p></div>
    <div class="basicInputContainer">
      <div class="leftInputBox">
        <div class="inputContainer">
          <InputSpan
            class="inputSpanContainer"
            labelText="姓名"
            v-model="name"
            :required="true"
            @enter="addCoach"
          ></InputSpan>
          <InputSpan
            class="inputSpanContainer"
            labelText="手機號碼"
            v-model="phone"
            :required="true"
            @enter="addCoach"
          ></InputSpan>
          <InputSpan
            class="inputSpanContainer"
            labelText="信箱"
            v-model="email"
            :required="false"
            @enter="addCoach"
          ></InputSpan>
          <RadioInput
            class="coachType"
            v-model="selectType"
            :options="[
              { value: '1', text: '私人教練' },
              { value: '2', text: '約聘教練' },
            ]"
            inputTitle="請選擇教練類別"
          />
          <div class="contractInputContainer">
            <InputSpan
              class="left"
              labelText="合約開始日"
              v-model="contractStartTime"
              inputType="date"
              @enter="addCoach"
            ></InputSpan>
            <InputSpan
              class="right"
              labelText="合約到期日"
              v-model="contractEndTime"
              inputType="date"
              @enter="addCoach"
            ></InputSpan>
          </div>
        </div>
      </div>
      <div class="rightInputBox">
        <div class="inputContainer">
          <InputSpan
            class="inputSpanContainer"
            labelText="帳號"
            v-model="account"
            :required="true"
            @enter="addCoach"
          ></InputSpan>
          <InputSpan
            class="inputSpanContainer"
            labelText="密碼"
            v-model="pwd"
            inputType="password"
            :required="true"
            @enter="addCoach"
          ></InputSpan>
          <InputSpan
            class="inputSpanContainer"
            labelText="再輸入一次密碼"
            v-model="pwdAgain"
            inputType="password"
            :required="true"
            @enter="addCoach"
          ></InputSpan>
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
        v-model="introduction"
      />
      <label for="specialty">特長</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="specialty"
        v-model="specialty"
      />
      <label for="certification">證照</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="certification"
        v-model="certification"
      />
    </div>
    <div class="sectionTitle"><p>圖片上傳區</p></div>
    <div class="imageUploadContainer">
      <label for="" class="labAvatar">請上傳頭像</label>
      <ImageUploader
        :previewUrl="previewUrl"
        class="imageUpload"
        @imageSelected="handleImage"
      />
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
import ImageUploader from "@/components/Input/ImageUploader";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "HealthopeAddCoach",
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
      name: "",
      hintText: "",
      account: "",
      pwd: "",
      pwdAgain: "",
      phone: "",
      email: "",
      selectType: "1",
      contractStartTime: "",
      contractEndTime: "",
      introduction: "",
      specialty: "",
      certification: "",
      avatarFile: "",
      previewUrl: "",
      verifyFail: false,
    };
  },
  methods: {
    async addCoach() {
      this.name = this.name.trim();
      this.account = this.account.trim();
      this.pwd = this.pwd.trim();
      this.pwdAgain = this.pwdAgain.trim();
      this.phone = this.phone.trim();
      this.email = this.email.trim();
      this.selectType = this.selectType.trim();
      this.contractStartTime = this.contractStartTime.trim();
      this.contractEndTime = this.contractEndTime.trim();
      this.introduction = this.introduction.trim();
      this.specialty = this.specialty.trim();
      this.certification = this.certification.trim();

      // 格式驗證
      if (!this.name || this.name.length > 15) {
        this.verifyFail = true;
        this.hintText = "名稱格式錯誤";
        return;
      }

      let phone = Number(this.phone);
      const phoneRegex = /^[9]\d{8}$/;
      if (Number.isNaN(phone) || !phoneRegex.test(phone)) {
        this.verifyFail = true;
        this.hintText = "手機格式錯誤";
        return;
      }

      if (!this.validEmail(this.email)) {
        this.hintText = "信箱格式錯誤";
        this.verifyFail = true;
        return;
      }

      if (!(this.selectType === "1" || this.selectType === "2")) {
        this.hintText = "教練類別錯誤";
        this.verifyFail = true;
        return;
      }

      // 只填合約開始日 或 只填合約結束日 或 結束日早於開始日 或 範圍超過 100 年
      const selectedStartDate = new Date(this.contractStartTime);
      const selectedStartYear = selectedStartDate.getFullYear();
      const selectedEndDate = new Date(this.contractEndTime);
      const selectedEndYear = selectedEndDate.getFullYear();
      const currentYear = new Date().getFullYear();
      const minYear = currentYear - 100;
      const maxYear = currentYear + 100;
      if (
        (!this.contractStartTime && this.contractEndTime) ||
        (this.contractStartTime && !this.contractEndTime) ||
        selectedEndDate < selectedStartDate ||
        selectedStartYear < minYear ||
        selectedStartYear > maxYear ||
        selectedEndYear < minYear ||
        selectedEndYear > maxYear
      ) {
        this.hintText = "合約日期錯誤";
        this.verifyFail = true;
        return;
      }

      // 帳號密碼驗證用的正規表達式
      const accountAndPwdRegex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;
      if (
        !(
          accountAndPwdRegex.test(this.pwd) &&
          accountAndPwdRegex.test(this.account)
        )
      ) {
        this.hintText = "請輸入 8~20 位英文數字";
        this.verifyFail = true;
        return;
      }

      if (this.pwd !== this.pwdAgain) {
        this.hintText = "兩次密碼輸入不一致";
        this.verifyFail = true;
        return;
      }

      if (this.pwd === this.account) {
        this.hintText = "帳號密碼不可相同";
        this.verifyFail = true;
        return;
      }

      if (this.introduction.length > 50) {
        this.hintText = "簡介輸入需在 50 字以內";
        this.verifyFail = true;
        return;
      }

      if (this.specialty.length > 200) {
        this.hintText = "特長輸入需在 200 字以內";
        this.verifyFail = true;
        return;
      }

      if (this.certification.length > 200) {
        this.hintText = "證照輸入需在 200 字以內";
        this.verifyFail = true;
        return;
      }

      try {
        // 傳輸新增資料
        const addCoachDto = {
          Name: this.name,
          Phone: phone,
          Email: this.email,
          Type: this.selectType,
          ContractStartTime: this.contractStartTime,
          ContractEndTime: this.contractEndTime,
          Account: this.account,
          Pwd: this.pwd,
          Introduction: this.introduction,
          Specialty: this.specialty,
          Certification: this.certification,
        };

        // 空就填入設值
        // 手動拼接字串轉成 iso time
        if (!addCoachDto.ContractStartTime)
          addCoachDto.ContractStartTime = "0001-01-01T00:00:00Z";
        else
          addCoachDto.ContractStartTime =
            addCoachDto.ContractStartTime + "T00:00:00Z";

        if (!addCoachDto.ContractEndTime)
          addCoachDto.ContractEndTime = "0001-01-01T00:00:00Z";
        else
          addCoachDto.ContractEndTime =
            addCoachDto.ContractEndTime + "T00:00:00Z";

        // 考量到效率, 採用 form data 型式傳輸資料/檔案
        const formData = new FormData();
        formData.append("dataObject", JSON.stringify(addCoachDto));

        if (this.avatarFile) formData.append("avatarFile", this.avatarFile);

        // post後回傳
        const response = await this.$axios.post(
          "/api/Coach/AddCoach",
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

.leftInputBox,
.rightInputBox {
  width: 80%;
  max-width: 350px;
}

.coachType {
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

.inputSpanContainer {
  margin-bottom: 2%;
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