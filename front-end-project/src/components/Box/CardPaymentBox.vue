<template>
  <div class="modalBackdrop">
    <div class="modal">
      <h2>💳 刷卡付款</h2>
      <p>{{ loadingMessage }}</p>

      <div class="spinner" v-if="isLoading"></div>
    </div>
  </div>
</template>

<script>
export default {
  name: "CardPaymentBox",
  props: {
    order: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      isLoading: false,
      loadingMessage: "請將卡片放在讀卡機上...",
    };
  },
  methods: {
    async startPayment() {
      this.isLoading = true;

      let payByCardDto = {
        OrderId: this.order.OrderId,
        CardReaderId: this.order.CardReaderId,
        UpdateTime: this.order.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/PayByCard",
          payByCardDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$emit("cardPaySuccess");
        } else {
          this.$emit("cardPayFail");
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "刷卡發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("刷卡付款時發生錯誤", error);
      } finally {
        this.isLoading = false;
      }
    },
  },
  mounted() {
    this.startPayment();
  },
};
</script>

<style scoped>
.modalBackdrop {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.modal {
  background: white;
  border-radius: 12px;
  padding: 20px 30px;
  width: 300px;
  text-align: center;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
}

.spinner {
  margin: 20px auto;
  border: 5px solid #f3f3f3;
  border-top: 5px solid #007bff;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
}

.actions {
  margin-top: 20px;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
