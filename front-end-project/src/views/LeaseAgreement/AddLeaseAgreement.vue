<template>
  <div>
    <TitleCard text="租約" @refreshPage="$emit('refreshPage')"></TitleCard>
    <SubTitleCard text="新增租約"></SubTitleCard>
    <div class="sectionTitle"><p>新增租約</p></div>
    <div class="leaseInputContainer">
      <InputSpan
        class="inputSpanContainer date"
        labelText="租約開始日期"
        v-model="startTime"
        inputType="date"
        :required="true"
        @enter="addLeaseAgreement"
      ></InputSpan>
      <InputSpan
        class="inputSpanContainer date"
        labelText="租約結束日期"
        v-model="endTime"
        inputType="date"
        :required="true"
        @enter="addLeaseAgreement"
      ></InputSpan>
      <InputSpan
        class="inputSpanContainer"
        labelText="提醒前置天數"
        v-model="reminderLeadTime"
        :required="true"
        @enter="addLeaseAgreement"
      ></InputSpan>
    </div>
    <div class="hintContainer">
      <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnContainer">
      <BtnConfirm @click="addLeaseAgreement" text="新增租約草稿"></BtnConfirm>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import InputSpan from "@/components/Input/InputSpan";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "AddLeaseAgreement",
  components: {
    TitleCard,
    SubTitleCard,
    InputSpan,
    BtnConfirm,
  },
  data() {
    return {
      startTime: "",
      endTime: "",
      reminderLeadTime: "",
      verifyFail: false,
      hintText: "",
    };
  },
  methods: {
    async addLeaseAgreement() {
      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }

      const localStartTime = new Date(this.startTime + "T00:00:00");
      const isoStartTime = localStartTime.toISOString();
      const localEndTime = new Date(this.endTime + "T00:00:00");
      const isoEndTime = localEndTime.toISOString();

      let addLeaseAgreementDto = {
        StartTime: isoStartTime,
        EndTime: isoEndTime,
        ReminderLeadTime: this.reminderLeadTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/LeaseAgreement/AddLeaseAgreement",
          addLeaseAgreementDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$router.push("/leaseAgreement");
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增條款時發生錯誤", error);
      }
    },
    validInput() {
      // 格式驗證
      const IntMax = 2147483647;
      let reminderLeadTime = Number(this.reminderLeadTime);
      if (
        !Number.isInteger(reminderLeadTime) ||
        reminderLeadTime < 1 ||
        // 超出安全整數範圍
        reminderLeadTime > IntMax
      ) {
        this.hintText = "提醒前置天數輸入格式錯誤";
        return false;
      }

      // 只填合約開始日 或 只填合約結束日 或 結束日早於開始日 或 範圍超過 100 年
      const selectedStartDate = new Date(this.startTime);
      const selectedStartYear = selectedStartDate.getFullYear();
      const selectedEndDate = new Date(this.endTime);
      const selectedEndYear = selectedEndDate.getFullYear();
      const currentYear = new Date().getFullYear();
      const minYear = currentYear - 100;
      const maxYear = currentYear + 100;
      if (
        !this.startTime ||
        !this.endTime ||
        (!this.startTime && this.endTime) ||
        (this.startTime && !this.endTime) ||
        selectedEndDate < selectedStartDate ||
        selectedStartYear < minYear ||
        selectedStartYear > maxYear ||
        selectedEndYear < minYear ||
        selectedEndYear > maxYear
      ) {
        this.hintText = "合約日期錯誤";
        return false;
      }

      return true;
    },
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

.inputSpanContainer {
  max-width: 40%;
  width: 300px;
  margin-bottom: 1%;
}

.leaseInputContainer {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
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