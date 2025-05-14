<template>
  <div>
    <TitleCard text="會員清單"></TitleCard>
    <SubTitleCard text="修改會員"></SubTitleCard>
    <div class="editMemberBox">
      <div class="editMemberContainer">
        <div class="editMemberContent">
          <div class="top">
            <div class="contentTextBox">
              <label class="lab">會員</label><br />
              <span>{{ member.Name }}</span>
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
            />
            <InputSpan
              class="editInput editInputPhone"
              labelText="手機號碼"
              v-model="phone"
            ></InputSpan>
          </div>
        </div>
      </div>
    </div>
    <div class="hintContainer">
      <span v-if="addFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnEditContainer">
      <BtnConfirm @click="editMember()" text="確認修改"></BtnConfirm>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import RadioInput from "@/components/Input/RadioInput";
import InputSpan from "@/components/Input/InputSpan";

export default {
  name: "HealthopeEditMember",
  components: {
    TitleCard,
    SubTitleCard,
    BtnConfirm,
    RadioInput,
    InputSpan,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      addFail: false,
      phone: "0987654321",
      selectStatus: "true",
      member: {
        MemberId: 0,
        Name: "無",
        Status: true,
        Phone: 987654321,
        UpdateTime: "",
      },
    };
  },
  methods: {
    async editMember() {
      try {
        this.phone = this.phone.trim();
        // 若無修改則返回 // 若格式錯誤則返回
        let originalStatus = this.member.Status ? "true" : "false";
        let editFlag = false;
        let editMemberDto = {
          MemberId: this.member.MemberId,
          UpdateTime: this.member.UpdateTime,
        };

        if (this.selectStatus !== originalStatus) {
          // 只允許 true / false
          if (!(this.selectStatus === "true" || this.selectStatus === "false"))
            return;
          editMemberDto.Status = this.selectStatus;
          editFlag = true;
        } else {
          editMemberDto.Status = null;
        }

        // 手機格式驗證
        let phone = Number(this.phone);
        let regex = /^[9]\d{8}$/;
        if (Number.isNaN(phone) || !regex.test(phone)) return;
        if (phone !== this.member.Phone) {
          editMemberDto.Phone = phone;
          editFlag = true;
        } else {
          editMemberDto.Phone = null;
        }

        // 沒修改過 或 格式錯誤就不觸發 post
        if (!editFlag) return;

        // post
        const response = await this.$axios.post(
          "/api/Member/EditMember",
          editMemberDto
        );
        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$router.push("/member");
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
    async getMemberEditDataById(id) {
      try {
        let memberByIdDto = {
          MemberId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Member/GetMemberEditDataById",
          memberByIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.member = response.data.ApiDataObject;
          this.member.MemberId = id;
          this.selectStatus = this.member.Status ? "true" : "false";
          this.phone = "0" + this.member.Phone;
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/member";
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
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/member");
      return;
    }
    this.getMemberEditDataById(this.$route.query.id);
  },
};
</script>

<style scoped>
.btnEditContainer {
  display: flex;
  justify-content: center;
  margin-top: 25px;
}

.editMemberBox {
  display: flex;
  justify-content: center;
  margin-top: 5%;
}

.editMemberContainer {
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

.editMemberContent {
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

.editMemberContent .top,
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

.editMemberContent .top {
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

.editInput {
  max-width: 60%;
  width: 150px;
}
</style>