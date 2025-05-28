<template>
  <div class="">
    <TitleCard text="展示用團課" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增課程"></SubTitleCard>
    <div class="sectionTitle"><p>課程基本資料區</p></div>
    <div class="basicInputBox">
      <div class="basicInputContainer">
        <InputSpan
          class="inputSpanContainer"
          labelText="課程名稱"
          v-model="currentShowcaseData.Name"
          :required="true"
          @enter="editShowcase"
        ></InputSpan>
        <InputSpan
          class="inputSpanContainer"
          labelText="課程順序"
          v-model="currentShowcaseData.Sort"
          :required="true"
          @enter="editShowcase"
        ></InputSpan>
      </div>
      <div class="categoryInputContainer">
        <RadioInput
          v-model="currentShowcaseData.Category"
          :options="groupClassCategoryAndText"
          inputTitle="選擇課程類別"
          inputType="selectCategory"
        />
      </div>
      <div class="iconInputContainer">
        <RadioInput
          v-model="currentShowcaseData.Icon"
          :options="groupClassIcon"
          inputTitle="選擇課程 Icon"
          inputType="selectIcon"
          :hightlightFlag="true"
        />
      </div>
      <div class="editGroupClassShowcaseContentContainer">
        <label for="summary">簡介</label>
        <textarea
          required=""
          cols="50"
          rows="10"
          id="summary"
          v-model="currentShowcaseData.Summary"
        />
        <label for="detailContent">內文</label>
        <textarea
          required=""
          cols="50"
          rows="10"
          id="detailContent"
          v-model="currentShowcaseData.DetailContent"
        />
      </div>
    </div>
    <div class="sectionTitle"><p>圖片上傳區</p></div>
    <div class="imageUploadContainer">
      <label for="" class="labImage">請上傳展示圖</label>
      <ImageUploader
        :previewUrl="currentShowcaseData.ImageUrl"
        class="imageUpload"
        @imageSelected="handleImage"
      />
    </div>
    <div class="hintContainer">
      <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnContainer">
      <BtnConfirm @click="editShowcase" text="修改課程"></BtnConfirm>
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
import {
  groupClassIcon,
  groupClassCategoryAndText,
  groupClassCategoryReverse,
} from "@/utils/groupClass";

export default {
  name: "EditGroupClassShowcase",
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
      currentShowcaseData: {
        GroupClassShowcaseId: 0,
        Name: "",
        Category: "7",
        Icon: "1",
        Sort: "1",
        Summary: "",
        DetailContent: "",
        ImageUrl: "",
        UpdateTime: "",
      },
      originalShowcaseData: {
        GroupClassShowcaseId: 0,
        Name: "",
        Category: 7,
        Icon: 1,
        Sort: 1,
        Summary: "",
        DetailContent: "",
        ImageUrl: "",
        UpdateTime: "",
      },
      verifyFail: false,
      hintText: "",
      avatarFile: "",
    };
  },
  methods: {
    async editShowcase() {
      if (!this.isDataModified()) {
        this.verifyFail = true;
        this.hintText = "請修改資料或返回";
        return;
      }

      this.currentShowcaseData = this.cleanData(this.currentShowcaseData);

      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }

      try {
        // 傳輸修改資料
        const editShowcaseDto = {
          GroupClassShowcaseId: this.currentShowcaseData.GroupClassShowcaseId,
          UpdateTime: this.currentShowcaseData.UpdateTime,
          Name:
            this.currentShowcaseData.Name === this.originalShowcaseData.Name
              ? null
              : this.currentShowcaseData.Name,
          Category:
            this.currentShowcaseData.Category ===
            this.originalShowcaseData.Category
              ? null
              : this.currentShowcaseData.Category,
          Icon:
            this.currentShowcaseData.Icon === this.originalShowcaseData.Icon
              ? null
              : this.currentShowcaseData.Icon,
          Sort:
            this.currentShowcaseData.Sort === this.originalShowcaseData.Sort
              ? null
              : this.currentShowcaseData.Sort,
          Summary:
            this.currentShowcaseData.Summary ===
            this.originalShowcaseData.Summary
              ? null
              : this.currentShowcaseData.Summary,
          DetailContent:
            this.currentShowcaseData.DetailContent ===
            this.originalShowcaseData.DetailContent
              ? null
              : this.currentShowcaseData.DetailContent,
        };

        // 考量到效率, 採用 form data 型式傳輸資料/檔案
        const formData = new FormData();
        formData.append("dataObject", JSON.stringify(editShowcaseDto));

        if (this.avatarFile) formData.append("avatarFile", this.avatarFile);

        // post後回傳
        const response = await this.$axios.post(
          "/api/GroupClassShowcase/EditShowcase",
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
        console.error("新增教練時發生錯誤", error);
      }
    },
    async getShowcaseById(id) {
      try {
        let getShowcaseByIdDto = {
          GroupClassShowcaseId: id,
        };
        // post
        const response = await this.$axios.post(
          "/api/GroupClassShowcase/GetShowcaseEditDataById",
          getShowcaseByIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.originalShowcaseData = response.data.ApiDataObject;
          this.originalShowcaseData.GroupClassShowcaseId = id;

          this.originalShowcaseData = this.cleanData(this.originalShowcaseData);
          this.currentShowcaseData = this.cleanData(this.originalShowcaseData);
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/groupClass/showcase";
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
        console.error("取得特定展示用團課時發生錯誤", error);
      }
    },
    validInput() {
      // 格式驗證
      if (
        !this.currentShowcaseData.Name ||
        this.currentShowcaseData.Name.length > 20
      ) {
        this.hintText = "名稱格式錯誤";
        return false;
      }

      const IntMax = 2147483647;
      let sort = Number(this.currentShowcaseData.Sort);
      if (
        !Number.isInteger(sort) ||
        sort < 1 ||
        // 超出安全整數範圍
        sort > IntMax
      ) {
        this.hintText = "順序格式錯誤";
        return false;
      }

      if (
        !this.currentShowcaseData.Category ||
        !!(this.selectCategory in groupClassCategoryReverse)
      ) {
        this.hintText = "分類格式錯誤";
        return false;
      }

      let icon = Number(this.currentShowcaseData.Icon);
      if (
        !Number.isInteger(icon) || // 不是整數
        icon < 1 ||
        // 超出安全整數範圍
        icon > IntMax
      ) {
        this.hintText = "icon 格式錯誤";
        return false;
      }

      if (this.currentShowcaseData.Summary.length > 80) {
        this.hintText = "簡介需輸入 80 字以內";
        return false;
      }

      if (this.currentShowcaseData.DetailContent.length > 500) {
        this.hintText = "內文需輸入 500 字以內";
        return false;
      }

      return true;
    },
    handleImage(file) {
      // 釋放前一個顯示的檔案
      this.revokePreviewUrl();

      // 用 ObjectURL 顯示預覽，不用 DataUR，效能較好
      this.currentShowcaseData.ImageUrl = URL.createObjectURL(file);

      // 設定上傳用檔案
      this.avatarFile = file;
    },
    revokePreviewUrl() {
      if (this.currentShowcaseData.ImageUrl) {
        URL.revokeObjectURL(this.currentShowcaseData.ImageUrl);
        this.currentShowcaseData.ImageUrl = null;
      }
    },
    isDataModified() {
      const cleanedCurrent = this.cleanData(this.currentShowcaseData);
      const cleanedOriginal = this.cleanData(this.originalShowcaseData);
      return JSON.stringify(cleanedCurrent) !== JSON.stringify(cleanedOriginal);
    },
    cleanData(data) {
      return {
        ...data,
        Name: data.Name.trim(),
        Sort: String(data.Sort).trim(),
        Icon: String(data.Icon).trim(),
        Category: String(data.Category).trim(),
        Summary: data.Summary.trim(),
        DetailContent: data.DetailContent.trim(),
        ImageUrl: data.ImageUrl.trim(),
      };
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
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/groupClass/showcase");
      return;
    }

    this.getShowcaseById(this.$route.query.id);
  },
};
</script>

<style scoped>
.editGroupClassShowcaseContainer {
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

.inputSpanContainer {
  max-width: 40%;
  width: 300px;
  margin-bottom: 1%;
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
.editGroupClassShowcaseContentContainer {
  display: flex;
  flex-direction: column;
  gap: 2px;
  width: 75%;
}

.editGroupClassShowcaseContentContainer label,
.labImage {
  font-weight: 500;
  font-size: 18px;
}

.editGroupClassShowcaseContentContainer textarea {
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

.editGroupClassShowcaseContentContainer textarea:focus {
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