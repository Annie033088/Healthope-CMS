<template>
  <div id="" class="notificationBox">
    <div class="notificationBoxContainer">
      <span class="close" @click="colseNotificationBox">&times;</span>
      <div class="notificationBoxTitle">
        <h2>{{ this.$notificationBox.notificationBoxTitle }}</h2>
      </div>
      <div class="notificationBoxContent">
        <p>{{ this.$errorCodeToMessage( this.$notificationBox.notificationBoxErrorCode) }}</p>
      </div>
      <div class="notificationBoxBtn">
        <button class="btnConfirm" @click="colseNotificationBox">OK</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "NotificationBox",
  methods: {
    colseNotificationBox() {
      this.$notificationBox.notificationBoxFlag = false;
    },
  },
  created() {},
};
</script>


<style scoped>
.notificationBox {
  position: fixed;
  z-index: 1000;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgba(0, 0, 0, 0.4); /* 背景半透明 */
}

/* 彈窗內容 */
.notificationBoxContainer {
  background-color: #fff;
  margin: 15% auto;
  padding: 20px;
  border-radius: 8px;
  width: 80%;
  max-width: 500px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
  animation: fadeIn 0.3s ease-out;
}

/* 關閉按鈕 */
.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
  cursor: pointer;
}

.close:hover {
  color: #333;
}

/* 動畫效果 */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.9);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}

.notificationBoxTitle,
.notificationBoxContent,
.notificationBoxBtn {
  display: flex;
  justify-content: center;
}

.btnConfirm {
  background-color: #eee;
  border: none;
  padding: 1rem;
  font-size: 1rem;
  width: 10em;
  border-radius: 1rem;
  color: #58bc82;
  box-shadow: 0 0.4rem #dfd9d9;
  cursor: pointer;
}

.btnConfirm:active {
  color: white;
  box-shadow: 0 0.2rem #dfd9d9;
  transform: translateY(0.2rem);
}

.btnConfirm:hover:not(:disabled) {
  background: #58bc82;
  color: white;
  text-shadow: 0 0.1rem #bcb4b4;
}

.btnConfirm:disabled {
  cursor: auto;
  color: grey;
}
</style>