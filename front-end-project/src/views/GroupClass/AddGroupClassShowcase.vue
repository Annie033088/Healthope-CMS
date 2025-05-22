<template>
  <div class="">
    <TitleCard text="展示用團課" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增課程"></SubTitleCard>
    <div class="sectionTitle"><p>課程基本資料區</p></div>
    <div class="basicInputBox">
      <div class="basicInputContainer">
        <InputSpan
          class="inputSpann"
          labelText="課程名稱"
          v-model="name"
          :required="true"
          @enter="addCourse"
        ></InputSpan>
        <InputSpan
          class="inputSpann"
          labelText="課程順序"
          v-model="sort"
          :required="true"
          @enter="addCourse"
        ></InputSpan>
      </div>
      <div class="categoryInputContainer">
        <RadioInput
          v-model="selectCategory"
          :options="groupClassCategoryAndText"
          inputTitle="選擇課程類別"
          inputType="selectCategory"
        />
      </div>
      <div class="iconInputContainer">
        <RadioInput
          v-model="selectIcon"
          :options="groupClassIcon"
          inputTitle="選擇課程 Icon"
          inputType="selectIcon"
          :hightlightFlag="true"
        />
      </div>
      <div class="addGroupClassShowcaseContentContainer">
        <label for="summary">簡介</label>
        <textarea
          required=""
          cols="50"
          rows="10"
          id="summary"
          v-model="summary"
        />
        <label for="detailContent">內文</label>
        <textarea
          required=""
          cols="50"
          rows="10"
          id="detailContent"
          v-model="detailContent"
        />
      </div>
    </div>
    <div class="sectionTitle"><p>圖片上傳區</p></div>
    <div class="imageUploadContainer">
      <label for="" class="labAvatar">請上傳展示圖</label>
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
      <BtnConfirm @click="addCourse" text="新增課程"></BtnConfirm>
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
import { groupClassIcon, groupClassCategoryAndText, groupClassCategoryReverse } from "@/utils/groupClass";

export default {
  name: "AddGroupClassShowcase",
  components: {
    TitleCard,
    SubTitleCard,
    InputSpan,
    RadioInput,
    ImageUploader,
    BtnConfirm,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      verifyFail: false,
      hintText: "",
      selectCategory: "7",
      selectIcon: "1",
      sort: "",
      name: "",
      summary: "",
      detailContent: "",
      previewUrl: "",
      avatarFile: "",
    };
  },
  methods: {
    async addCourse() {
      this.name = this.name.trim();
      this.sort = this.sort.trim();
      this.selectCategory = this.selectCategory.trim();
      this.selectIcon = this.selectIcon.trim();
      this.summary = this.summary.trim();
      this.detailContent = this.detailContent.trim();

      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }

      try {
        // 傳輸新增資料
        const addCourseDto = {
          Name: this.name,
          Sort: this.sort,
          Category: this.selectCategory,
          Icon: this.selectIcon,
          Summary: this.summary,
          DetailContent: this.detailContent,
        };
        // 考量到效率, 採用 form data 型式傳輸資料/檔案
        const formData = new FormData();
        formData.append("dataObject", JSON.stringify(addCourseDto));

        if (this.avatarFile) formData.append("avatarFile", this.avatarFile);

        // post後回傳
        const response = await this.$axios.post(
          "/api/GroupClassShowcase/AddShowcase",
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          }
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.$router.push("/groupClass/showcase");
          return;
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增展示課時發生錯誤", error);
      }
    },
    validInput() {
      // 格式驗證
      if (!this.name || this.name.length > 20) {
        this.hintText = "名稱格式錯誤";
        return false;
      }

      const IntMax = 2147483647;
      let sort = Number(this.sort);
      if (
        !Number.isInteger(sort) ||
        sort < 1 ||
        // 超出安全整數範圍
        sort > IntMax
      ) {
        this.hintText = "順序格式錯誤";
        return false;
      }


      if (!(this.selectCategory in groupClassCategoryReverse)) {
        this.hintText = "分類格式錯誤";
        return false;
      }

      let icon = Number(this.selectIcon);
      if (
        !Number.isInteger(icon) || // 不是整數
        icon < 1 ||
        // 超出安全整數範圍
        icon > IntMax
      ) {
        this.hintText = "icon 格式錯誤";
        return false;
      }

      if (this.summary.length > 80) {
        this.hintText = "簡介需輸入 80 字以內";
        return false;
      }

      if (this.detailContent.length > 500) {
        this.hintText = "內文需輸入 500 字以內";
        return false;
      }

      return true;
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
  },
  beforeDestroy() {
    this.revokePreviewUrl();
  },
  computed: {
    groupClassIcon() {
      return groupClassIcon;
    },
    groupClassCategoryAndText() {
      return groupClassCategoryAndText;
    },
  },
};
</script>

<style scoped>
.addGroupClassShowcaseContainer {
  width: 100%;
}

.sectionTitle {
  display: flex;
  justify-content: center;
  margin-top: 7px;
}

.sectionTitle p {
  font-size: 20px;
  font-weight: 700;
}

.basicInputBox {
  display: flex;
  flex-wrap: wrap;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.basicInputContainer {
  width: 100%;
  display: flex;
  justify-content: center;
  gap: 10%;
}

.inputSpann {
  max-width: 40%;
  width: 300px;
}

.categoryInputContainer,
.iconInputContainer {
  max-width: 80%;
  width: 800px;
  margin-bottom: 15px;
}
</style>

<style scoped>
/** textarea */
.addGroupClassShowcaseContentContainer {
  display: flex;
  flex-direction: column;
  gap: 2px;
  width: 75%;
}

.addGroupClassShowcaseContentContainer label,
.labAvatar {
  font-weight: 500;
  font-size: 18px;
}

.addGroupClassShowcaseContentContainer textarea {
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

.addGroupClassShowcaseContentContainer textarea:focus {
  outline: 2px solid #707070;
}
</style>

<style scoped>
/** img & hint & btn */
.imageUploadContainer {
  margin-left: 12.5%;
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