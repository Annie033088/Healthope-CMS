<template>
  <div>
    <TitleCard text="管理者清單"></TitleCard>
    <SubTitleCard text="修改管理者"></SubTitleCard>
    <div class="adminCardContainer">
      <div class="adminCard">
        <div class="adminText">管理者</div>
        <div class="admin">
          <div class="cardImage">
            <svg
              width="50"
              height="50"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M12 4C13.0609 4 14.0783 4.42143 14.8284 5.17157C15.5786 5.92172 16 6.93913 16 8C16 9.06087 15.5786 10.0783 14.8284 10.8284C14.0783 11.5786 13.0609 12 12 12C10.9391 12 9.92172 11.5786 9.17157 10.8284C8.42143 10.0783 8 9.06087 8 8C8 6.93913 8.42143 5.92172 9.17157 5.17157C9.92172 4.42143 10.9391 4 12 4ZM12 14C16.42 14 20 15.79 20 18V20H4V18C4 15.79 7.58 14 12 14Z"
                fill="white"
              />
            </svg>
          </div>
          <div class="adminName">
            <!-- <span>qweqwk90ek!</span> -->
            <span>{{ admin.Account }}</span>
          </div>
        </div>
      </div>
    </div>
    <div class="editInputContainer">
      <div class="editInputLeft">
        <span class="inputSpan">
          <label class="lab">請選擇狀態</label>
          <div class="radioContainer">
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioStatus"
                value="true"
                v-model="selectStatus"
              />
              <span class="textRadio">啟用</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioStatus"
                value="false"
                v-model="selectStatus"
              />
              <span class="textRadio">停用</span>
            </label>
          </div>
        </span>
      </div>
      <div class="editInputRight">
        <span class="inputSpan">
          <label class="lab">請選擇身份</label>
          <div class="radioContainer">
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioIdentity"
                value="None"
                v-model="selectIdentity"
                checked=""
              />
              <span class="textRadio">無</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioIdentity"
                value="Admin"
                v-model="selectIdentity"
              />
              <span class="textRadio">管理員</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioIdentity"
                value="Receptionist"
                v-model="selectIdentity"
              />
              <span class="textRadio">櫃檯人員</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioIdentity"
                value="Accountant"
                v-model="selectIdentity"
              />
              <span class="textRadio">會計</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioIdentity"
                value="CourseManager"
                v-model="selectIdentity"
              />
              <span class="textRadio">課程管理</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioIdentity"
                value="CoachManager"
                v-model="selectIdentity"
              />
              <span class="textRadio">教練管理</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioIdentity"
                value="SalesRepresentative"
                v-model="selectIdentity"
              />
              <span class="textRadio">業務</span>
            </label>
          </div>
        </span>
      </div>
    </div>
    <div class="hintContainer">
      <span v-if="addFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnAddContainer">
      <button class="btnAdd btn" @click="editAdmin()">確認修改</button>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";

export default {
  name: "HealthopeEditAdmin",
  components: {
    TitleCard,
    SubTitleCard,
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
          if (!(this.selectStatus === "true" || this.selectStatus === "false"))
            return;
          editAdminDto.Status = this.selectStatus;
          editFlag = true;
        } else {
          editAdminDto.Status = null;
        }

        if (this.selectIdentity !== this.identityToText(this.admin.Identity)) {
          let identityNum = this.identityTextToNumber(this.selectIdentity);

          // 轉換失敗的話代表格式錯誤
          if (typeof identityNum !== "number") return;

          editAdminDto.Identity = identityNum;
          editFlag = true;
        } else {
          editAdminDto.Identity = null;
        }

        if (!editFlag) return;

        // post
        const response = await this.$axios.post(
          "/api/Admin/EditAdmin",
          editAdminDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$router.push("/admin");
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
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
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/admin";
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
          return identityText;
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
.editInputContainer {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-evenly;
  margin-top: 25px;
}

.editInputLeft,
.editInputRight {
  width: 60%;
  max-width: 350px;
}

.editInputLeft .inputSpan {
  margin-top: 20px;
}
.editInputRight .inputSpan {
  margin-top: 10px;
}

.btnAddContainer {
  display: flex;
  justify-content: center;
  margin-top: 25px;
}

.btnAdd {
  width: 100%;
  max-width: 400px;
  height: 50px;
  border-radius: 3rem;
  background-color: #707070;
  color: #efefef;
  cursor: pointer;
  transition: all 300ms;
  font-weight: 600;
  font-size: 0.9rem;
}

.btnAdd:hover {
  background-color: #eee;
  color: #707070;
}

.inputSpan {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.radioContainer {
  display: flex;
  flex-wrap: wrap;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.35rem;
  width: 100%;
  max-width: 350px;
  font-size: 16px;
  margin-left: 20px;
  gap: 15px;
}

.labRadioBox {
  flex: 1 1 auto;
  text-align: center;
  justify-content: center;
  min-width: 50px;
}

.labRadioBox input {
  display: none;
}

.labRadioBox .textRadio {
  display: flex;
  cursor: pointer;
  justify-content: center;
  border-radius: 0.5rem;
  padding: 0.5rem 0;
  transition: all 0.15s ease-in-out;
}

.labRadioBox input:checked + .textRadio {
  background-color: #fff;
  font-weight: 600;
}
</style>


<style scoped>
/* 管理員 樣式*/
.adminCardContainer {
  width: 100%;
  display: flex;
  justify-content: center;
  margin-top: 15px;
}

.adminCard {
  width: 30%;
  min-width: 300px;
  background-color: white;
  box-shadow: rgba(216, 216, 216, 0.962) 0px 0px 3px 0px inset;
  border-radius: 5%;
}

.adminText {
  display: flex;
  justify-content: center;
  background-color: rgb(237, 232, 232);
  font-size: 25px;
  border-radius: 5% 5% 0% 0%;
}

.admin {
  padding: 12px 10px;
  margin: 5px;
  display: flex;
  justify-content: center;
}

.cardContent {
  display: flex;
}

.cardImage {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background: #727272e7;
}

.adminName {
  margin-left: 20px;
  font-size: 25px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.cardEditBtn {
  color: #aa7f7f;
}
</style>