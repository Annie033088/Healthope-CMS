<template>
  <div>
    <TitleCard text="管理員"></TitleCard>
    <SubTitleCard text="修改管理員"></SubTitleCard>
    <div class="editAdminBox">
      <div class="editAdminContainer">
        <div class="editAdminContent">
          <div class="top">
            <div class="contentTextBox">
              <label class="lab">管理者</label><br />
              <span>{{ admin.Account }}</span>
            </div>
          </div>
          <div class="bottom">
            <RadioInput
              v-model="selectStatus"
              :options="[
                { value: 'true', text: '啟用' },
                { value: 'false', text: '停用' },
              ]"
              inputTitle="請選擇狀態"
              inputType="statusInput"
            /><RadioInput
              v-model="selectIdentity"
              :options="[
                { value: 'None', text: '無' },
                { value: 'Admin', text: '管理員' },
                { value: 'Receptionist', text: '櫃檯人員' },
                { value: 'Accountant', text: '會計' },
                { value: 'CourseManager', text: '課程管理' },
                { value: 'CoachManager', text: '教練管理' },
                { value: 'SalesRepresentative', text: '業務' },
              ]"
              inputTitle="請選擇身份"
              inputType="identityInput"
            />
          </div>
        </div>
      </div>
    </div>
    <div class="editInputContainer">
      <div class="editInputTop"></div>
      <div class="editInputBotton"></div>
    </div>
    <div class="hintContainer">
      <span v-if="addFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnEditContainer">
      <BtnConfirm @click="editAdmin()" text="確認修改"></BtnConfirm>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import RadioInput from "@/components/Input/RadioInput";

export default {
  name: "HealthopeEditAdmin",
  components: {
    TitleCard,
    SubTitleCard,
    BtnConfirm,
    RadioInput,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      addFail: false,
      selectIdentity: "None",
      selectStatus: "true",
      admin: {
        AdminId: 0,
        Account: "",
        Status: true,
        Identity: 1,
        UpdateTime: "",
      },
    };
  },
  methods: {
    async editAdmin() {
      try {
        // 若無修改則返回 // 若格式錯誤則返回
        let originalStatus = this.admin.Status ? "true" : "false";
        let editFlag = false;
        let editAdminDto = {
          AdminId: this.admin.AdminId,
          UpdateTime: this.admin.UpdateTime,
        };

        if (this.selectStatus !== originalStatus) {
          // 只允許 true / false
          if (
            !(this.selectStatus === "true" || this.selectStatus === "false")
          ) {
            this.addFail = true;
            this.hintText = "狀態格式錯誤";
            return;
          }
          editAdminDto.Status = this.selectStatus;
          editFlag = true;
        } else {
          editAdminDto.Status = null;
        }

        if (this.selectIdentity !== this.identityToText(this.admin.Identity)) {
          let identityNum = this.identityTextToNumber(this.selectIdentity);

          // 轉換失敗的話代表格式錯誤
          if (identityNum === -1) {
            this.addFail = true;
            this.hintText = "身份格式錯誤";
            return;
          }

          editAdminDto.Identity = identityNum;
          editFlag = true;
        } else {
          editAdminDto.Identity = null;
        }

        if (!editFlag) {
          this.addFail = true;
          this.hintText = "請修改資料或返回";
          return;
        }

        // post
        const response = await this.$axios.post(
          "/api/Admin/EditAdmin",
          editAdminDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$router.push("/admin");
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          if (this.unwatchFlag) {
            this.unwatchFlag(); // 移除監聽
            this.unwatchFlag = null;
          }
          
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = null;
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
    async getAdminById(id) {
      try {
        let getAdminByIdDto = {
          AdminId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Admin/GetAdminById",
          getAdminByIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.admin = response.data.ApiDataObject;
          this.admin.AdminId = id;
          this.selectStatus = this.admin.Status ? "true" : "false";
          this.selectIdentity = this.identityToText(this.admin.Identity);
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
        console.error("取得特定管理者時發生錯誤", error);
      }
    },
    identityToText(identity) {
      let identityText;

      switch (identity) {
        case 2:
          identityText = "Admin";
          return identityText;
        case 3:
          identityText = "Receptionist";
          return identityText;
        case 4:
          identityText = "Accountant";
          return identityText;
        case 5:
          identityText = "CourseManager";
          return identityText;
        case 6:
          identityText = "CoachManager";
          return identityText;
        case 7:
          identityText = "SalesRepresentative";
          return identityText;
        default:
          identityText = "None";
          return identityText;
      }
    },
    identityTextToNumber(identityText) {
      let identity;

      switch (identityText) {
        case "None":
          identity = 0;
          return identity;
        case "Admin":
          identity = 2;
          return identity;
        case "Receptionist":
          identity = 3;
          return identity;
        case "Accountant":
          identity = 4;
          return identity;
        case "CourseManager":
          identity = 5;
          return identity;
        case "CoachManager":
          identity = 6;
          return identity;
        case "SalesRepresentative":
          identity = 7;
          return identity;
        default:
          return -1;
      }
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/admin");
      return;
    }
    this.getAdminById(this.$route.query.id);
  },
};
</script>

<style scoped>
.btnEditContainer,
.hintContainer {
  display: flex;
  justify-content: center;
  margin-top: 25px;
}

.editAdminBox {
  display: flex;
  justify-content: center;
  margin-top: 5%;
}

.editAdminContainer {
  display: flex;
  position: relative;
  align-items: center;
  padding: 9px;
  width: 1000px;
  max-width: 80%;
  background-color: #fcfcfc;
  border-radius: 35px;
  box-shadow: rgba(10, 37, 64, 0.35) 0px -1px 5px 0px inset;
}

.editAdminContent {
  display: flex;
  justify-content: space-evenly;
  flex-direction: column;
  align-items: center;
  flex-wrap: wrap;
  overflow: hidden;
  width: 1000px;
  max-width: 100%;
  min-height: 220px;
  border-radius: 30px;
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
}

.editAdminContent .top,
.bottom {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  width: 100%;
  height: 100%;
  padding: 5px;
  gap: 10px 20%;
  word-break: break-word;
}

.editAdminContent .top {
  padding-bottom: 10px;
  border-bottom: solid #c5c5c5 1px;
}

.contentTextBox label {
  font-size: 24px;
  font-weight: 700;
  color: #1c1c1c;
  font-family: "Microsoft JhengHei";
}

.contentTextBox span {
  font-size: 18px;
  font-family: "Microsoft JhengHei";
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
