<template>
  <div class="">
    <TitleCard text="條款" @refreshPage="$emit('refreshPage')"></TitleCard>
    <SubTitleCard text="修改條款"></SubTitleCard>
    <div class="titleContainer">
      <h3 class="">修改條款</h3>
      <BtnNormal
        class="btnPublish"
        text="發布條款"
        @click="publishTerm"
      ></BtnNormal>
    </div>
    <div class="editContainer">
      <h3 class="">{{ currentTermData.Name }}</h3>
      <label for="versionDescription">請描述更新內容</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="versionDescription"
        v-model="currentTermData.VersionDescription"
        @keydown.enter="editTerm"
      />
      <label for="detailContent">內文</label>
      <textarea
        required=""
        cols="50"
        rows="10"
        id="detailContent"
        v-model="currentTermData.DetailContent"
        @keydown.enter="editTerm"
      />
    </div>
    <div class="hintContainer">
      <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnContainer">
      <BtnConfirm @click="editTerm" text="修改"></BtnConfirm>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import BtnNormal from "@/components/Btn/BtnNormal";

export default {
  name: "HealthopeEditTerm",
  components: {
    TitleCard,
    SubTitleCard,
    BtnConfirm,
    BtnNormal,
  },
  data() {
    return {
      verifyFail: false,
      hintText: "",
      currentTermData: {
        TermId: 0,
        Name: "",
        VersionDescription: "",
        DetailContent: "",
        UpdateTime: "",
      },
      originalTermData: {
        TermId: 0,
        Name: "",
        VersionDescription: "",
        DetailContent: "",
        UpdateTime: "",
      },
    };
  },
  methods: {
    async editTerm() {
      if (!this.isDataModified()) {
        this.hintText = "請修改資料或返回";
        this.verifyFail = true;
        return;
      }

      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }

      try {
        let editTermDto = {
          TermId: this.currentTermData.TermId,
          UpdateTime: this.currentTermData.UpdateTime,
          DetailContent:
            this.currentTermData.DetailContent ===
            this.originalTermData.DetailContent
              ? null
              : this.currentTermData.DetailContent,
          VersionDescription:
            this.currentTermData.VersionDescription ===
            this.originalTermData.VersionDescription
              ? null
              : this.currentTermData.VersionDescription,
        };

        const response = await this.$axios.post(
          "/api/Term/EditTerm",
          editTermDto
        );

        // post後回傳
        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.$router.push("/term");
          return;
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }

        if (response.data.ErrorCode === this.$errorCodeDefine.HasBeenModified) {
          this.$emit("refreshPage");
        }
      } catch (error) {
        console.error("修改方案發生錯誤", error);
      }
    },
    publishTerm() {},
    async GetTermEditDataById(id) {
      try {
        let getTermByIdDto = {
          TermId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Term/GetTermEditDataById",
          getTermByIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.originalTermData = response.data.ApiDataObject;
          this.originalTermData.TermId = id;

          this.originalTermData = this.cleanData(this.originalTermData);
          this.currentTermData = this.cleanData(this.originalTermData);
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
        console.error("取得特定條款時發生錯誤", error);
      }
    },
    validInput() {
      if (
        this.currentTermData.DetailContent &&
        this.currentTermData.DetailContent.length > 7000
      ) {
        this.hintText = "請輸入 7000 字內內文";
        return false;
      }

      if (
        this.currentTermData.VersionDescription &&
        this.currentTermData.VersionDescription.length > 200
      ) {
        this.hintText = "請輸入 200 字內描述更新內容";
        return false;
      }

      return true;
    },
    cleanData(data) {
      return {
        ...data,
        Name: data.Name.trim(),
        VersionDescription: data.VersionDescription.trim(),
        DetailContent: data.DetailContent.trim(),
      };
    },
    isDataModified() {
      const cleanedCurrent = this.cleanData(this.currentTermData);
      const cleanedOriginal = this.cleanData(this.originalTermData);
      return JSON.stringify(cleanedCurrent) !== JSON.stringify(cleanedOriginal);
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/term");
      return;
    }

    this.GetTermEditDataById(this.$route.query.id);
  },
};
</script>

<style scoped>
.btnContainer,
.hintContainer {
  display: flex;
  justify-content: center;
  margin-bottom: 5px;
  gap: 5px;
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

.editContainer {
  display: flex;
  flex-direction: column;
  gap: 2px;
  width: 80%;
  margin-left: 10%;
  align-items: center;
}

.editContainer label {
  font-weight: 500;
  font-size: 18px;
  margin-bottom: 5px;
}

.editContainer textarea {
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

.editContainer textarea:focus {
  outline: 2px solid #707070;
}

.titleContainer {
  display: flex;
  justify-content: space-evenly;
}

.titleContainer h3 {
  margin-left: 28%;
}

.btnPublish {
  margin-top: 25px;
  width: 130px;
  height: 50px;
  max-width: 35%;
  font-size: 16px;
}
</style>