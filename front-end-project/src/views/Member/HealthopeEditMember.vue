<template>
  <div>
    <TitleCard text="會員清單"></TitleCard>
    <SubTitleCard text="修改會員"></SubTitleCard>
    <div class="memberCardContainer">
      <EditTitleCard title="會員" :content="member.Name"></EditTitleCard>
    </div>
    <div class="editInputContainer">
      <div class="editInputTop">
        <EditStatusInput v-model="selectStatus" />
      </div>
      <div class="editInputBotton">
        <InputSpan labelText="手機號碼" v-model="phone"></InputSpan>
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
import EditStatusInput from "@/components/Input/EditStatusInput";
import EditTitleCard from "@/components/Card/EditTitleCard";
import InputSpan from "@/components/Input/InputSpan";

export default {
  name: "HealthopeEditMember",
  components: {
    TitleCard,
    SubTitleCard,
    BtnConfirm,
    EditStatusInput,
    EditTitleCard,
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

        // 沒修改過的話就不觸發 post
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
.editInputContainer {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  flex-direction: column;
  margin-top: 25px;
}

.editInputBotton,
.editInputTop {
  max-width: 60%;
  width: 150px;
  margin-right: 50px;
}

.btnEditContainer {
  display: flex;
  justify-content: center;
  margin-top: 25px;
}
</style>


<style scoped>
/* 會員 樣式*/
.memberCardContainer {
  width: 100%;
  display: flex;
  justify-content: center;
  margin-top: 15px;
}
</style>