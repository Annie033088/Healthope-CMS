<template>
  <div>
    <TitleCard text="發票字軌" @refreshPage="$emit('refreshPage')"></TitleCard>
    <SubTitleCard text="新增發票字軌" />
    <div class="sectionTitle"><p>新增發票字軌</p></div>
    <div class="addInputContainer">
      <InputSpan
        class="inputSpanContainer"
        labelText="字軌前二碼"
        v-model="trackPrefix"
        :required="true"
        @enter="addInvoiceTrackNumber"
      ></InputSpan>
      <div class="numberContainer">
        <InputSpan
          class="inputSpanContainer halfWidthInput"
          labelText="起始號碼(上線 8 碼)"
          v-model="startNumber"
          :required="true"
          @enter="addInvoiceTrackNumber"
        ></InputSpan
        ><InputSpan
          class="inputSpanContainer halfWidthInput"
          labelText="結束號碼(上線 8 碼)"
          v-model="endNumber"
          :required="true"
          @enter="addInvoiceTrackNumber"
        ></InputSpan>
      </div>
      <div class="periodContainer">
        <InputSpan
          class="inputSpanContainer halfWidthInput"
          labelText="民國年份"
          v-model="invoiceYear"
          :required="true"
          @enter="addInvoiceTrackNumber"
        ></InputSpan
        ><InputSpan
          class="inputSpanContainer halfWidthInput"
          labelText="期數(1~6)"
          v-model="invoiceCycle"
          :required="true"
          @enter="addInvoiceTrackNumber"
        ></InputSpan>
      </div>
    </div>
    <div class="hintContainer">
      <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
    </div>
    <div class="btnContainer">
      <BtnConfirm
        class="btnConfirm"
        @click="addInvoiceTrackNumber"
        text="新增字軌"
      ></BtnConfirm>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import InputSpan from "@/components/Input/InputSpan";
import BtnConfirm from "@/components/Btn/BtnConfirm";

export default {
  name: "AddInvoiceTrackNumber",
  components: {
    TitleCard,
    SubTitleCard,
    InputSpan,
    BtnConfirm,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      verifyFail: false,
      hintText: "",
      trackPrefix: "",
      startNumber: "",
      endNumber: "",
      invoiceYear: "",
      invoiceCycle: "",
    };
  },
  methods: {
    async addInvoiceTrackNumber() {
      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }

      const now = new Date();
      let taiwanYear = now.getFullYear() - 1911; // 西元轉民國
      let period = Math.floor((now.getMonth() + 1 + 1) / 2); // 兩個月為一期，1~6期
      let nowInvoicePeriod = taiwanYear * 10 + period;

      if (nowInvoicePeriod > Number(this.invoiceYear + this.invoiceCycle)) {
        // 添加監聽器，查看彈窗是否被按確認鍵
        this.unwatchFlag = this.$watch(
          "notificationBoxConfirmFlag",
          (newVal) => {
            if (newVal) {
              try {
                this.submitAddInvoiceTrackNumber();
              } catch (error) {
                console.error("刪除條款時發生錯誤", error);
              } finally {
                this.unwatchFlag(); // 確保監聽被移除
                this.unwatchFlag = null;
              }
            }
          }
        );

        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "設定的期數已過期，是否確認新增?";
        this.$notificationBox.notificationBoxCancelFlag = true;
        this.$notificationBox.notificationBoxErrorCode = 0;
      } else {
        this.submitAddInvoiceTrackNumber();
      }
    },
    async submitAddInvoiceTrackNumber() {
      this.trackPrefix = this.trackPrefix.toUpperCase();

      let addInvoiceTrackNumberDto = {
        TrackPrefix: this.trackPrefix,
        StartNumber: this.startNumber,
        EndNumber: this.endNumber,
        InvoicePeriod: this.invoiceYear + this.invoiceCycle,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Invoice/AddInvoiceTrackNumber",
          addInvoiceTrackNumberDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.$router.push("/setting/invoiceTrackNumber");
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增租約時發生錯誤", error);
      }
    },
    validInput() {
      // 字軌驗證 正則表達
      let prefixRegex = /^[A-Za-z]{2}$/;
      if (!prefixRegex.test(this.trackPrefix)) {
        this.hintText = "字軌請輸入兩碼英文";
        return false;
      }

      let startNumber = Number(this.startNumber);
      if (
        !Number.isInteger(startNumber) ||
        startNumber < 1 ||
        // 超出 8 碼
        startNumber > 99999999
      ) {
        this.hintText = "起始碼請輸入 8 位內數字";
        return false;
      }

      let endNumber = Number(this.endNumber);
      if (
        !Number.isInteger(endNumber) ||
        endNumber < 1 ||
        // 超出 8 碼
        endNumber > 99999999
      ) {
        this.hintText = "結束碼請輸入 8 位內數字";
        return false;
      }

      if (startNumber >= endNumber) {
        this.hintText = "起始碼不可大等於結束碼";
        return false;
      }

      let invoiceYear = Number(this.invoiceYear);
      if (
        !Number.isInteger(invoiceYear) ||
        // 目前僅能 3 碼
        invoiceYear < 100 ||
        invoiceYear > 999
      ) {
        this.hintText = "民國年份輸入錯誤 (僅 3 碼)";
        return false;
      }

      let invoiceCycle = Number(this.invoiceCycle);
      if (
        !Number.isInteger(invoiceCycle) ||
        // 目前僅能 3 碼
        invoiceCycle < 1 ||
        invoiceCycle > 6
      ) {
        this.hintText = "期數輸入錯誤 (1~6)";
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

.addInputContainer {
  display: flex;
  flex-direction: column;
  gap: 24px;
  padding: 24px;
  align-items: center;
}

.inputSpanContainer {
  width: 300px;
  max-width: 80%;
}

.numberContainer,
.periodContainer {
  display: flex;
  justify-content: center;
  gap: 5px;
  flex-wrap: wrap;
}

.halfWidthInput {
  width: 48.5%;
  min-width: 200px;
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