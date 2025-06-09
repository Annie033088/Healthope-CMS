<template>
  <div v-if="visible" class="modalBackdrop">
    <div class="modal">
      <h2>💳 刷卡付款</h2>
      <p v-if="!done">{{ loadingMessage }}</p>
      <p v-if="done">{{ resultMessage }}</p>

      <div class="spinner" v-if="isLoading"></div>

      <div class="actions" v-if="done">
        <button @click="retry">重新刷卡</button>
        <button @click="close">關閉</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "CardPaymentBox",
  props: {
    orderId: {
      type: Number,
      required: true,
    },
    visible: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isLoading: false,
      done: false,
      resultMessage: "",
      loadingMessage: "請將卡片放在讀卡機上...",
    };
  },
  methods: {
    async startPayment() {
      this.isLoading = true;
      this.done = false;
      this.resultMessage = "";

      let orderIdDto = {
        OrderId: this.orderId,
      };
      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/PayByCard",
          orderIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.resultMessage = "✅ 付款成功！";
        } else {
          this.resultMessage = `❌ 付款失敗：${response.data.message}`;
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("取得會員時發生錯誤", error);
      } finally {
        this.isLoading = false;
        this.done = true;
      }
    },
    retry() {
      this.startPayment();
    },
    close() {
      this.$emit("close");
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

button {
  margin: 0 5px;
  padding: 6px 12px;
  border: none;
  background: #007bff;
  color: white;
  border-radius: 6px;
  cursor: pointer;
}

button:hover {
  background: #0056b3;
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
