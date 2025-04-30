<template>
  <div id="" class="notificationBox">
    <div class="notificationBoxContainer">
      <span class="close" @click="notificationCancel">&times;</span>
      <div class="notificationBoxTitle">
        <h2>{{ this.$notificationBox.notificationBoxTitle }}</h2>
      </div>
      <div class="notificationBoxContent">
        <p>
          {{
            this.$errorCodeToMessage(
              this.$notificationBox.notificationBoxErrorCode
            )
          }}
        </p>
      </div>
      <div class="notificationBoxBtn">
        <button class="btnConfirm" @click="notificationBoxConfirm">OK</button>
        <button v-if="this.$notificationBox.notificationBoxCancelFlag" class="btnCancel" @click="notificationCancel">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "NotificationBox",
  methods: {
    notificationBoxConfirm(){
      this.$notificationBox.notificationBoxFlag = false;
      this.$notificationBox.notificationBoxCancelFlag = false;
this.$emit('notificationBoxConfirm');
    },
    notificationCancel() {
      this.$notificationBox.notificationBoxFlag = false;
      this.$notificationBox.notificationBoxCancelFlag = false;
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
  gap: 5px;
}

.notificationBoxBtn button {
  border: none;
  padding: 1rem;
  font-size: 1rem;
  width: 10em;
  border-radius: 1rem;
  box-shadow: 0 0.4rem #dfd9d9;
  cursor: pointer;
}

.notificationBoxBtn button:active {
  color: white;
  box-shadow: 0 0.2rem #dfd9d9;
  transform: translateY(0.2rem);
}

.notificationBoxBtn button:hover {
  color: white;
  text-shadow: 0 0.1rem #bcb4b4;
}

.btnConfirm {
  color: #58bc82;
}

.btnCancel {
  color: #bc5858c0;
}

.btnConfirm:hover {
  background: #58bc82;
}

.btnCancel:hover {
  background: #bc5858c0;
}
</style>