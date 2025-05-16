<template>
  <div>
    <label class="fileUpload" for="" @click="triggerFileInput">
      <div class="icon" v-if="!previewUrl">
        <svg xmlns="http://www.w3.org/2000/svg" fill="" viewBox="0 0 24 24">
          <path
            fill=""
            d="M10 1C9.73478 1 9.48043 1.10536 9.29289 1.29289L3.29289 7.29289C3.10536 7.48043 3 7.73478 3 8V20C3 21.6569 4.34315 23 6 23H7C7.55228 23 8 22.5523 8 22C8 21.4477 7.55228 21 7 21H6C5.44772 21 5 20.5523 5 20V9H10C10.5523 9 11 8.55228 11 8V3H18C18.5523 3 19 3.44772 19 4V9C19 9.55228 19.4477 10 20 10C20.5523 10 21 9.55228 21 9V4C21 2.34315 19.6569 1 18 1H10ZM9 7H6.41421L9 4.41421V7ZM14 15.5C14 14.1193 15.1193 13 16.5 13C17.8807 13 19 14.1193 19 15.5V16V17H20C21.1046 17 22 17.8954 22 19C22 20.1046 21.1046 21 20 21H13C11.8954 21 11 20.1046 11 19C11 17.8954 11.8954 17 13 17H14V16V15.5ZM16.5 11C14.142 11 12.2076 12.8136 12.0156 15.122C10.2825 15.5606 9 17.1305 9 19C9 21.2091 10.7909 23 13 23H20C22.2091 23 24 21.2091 24 19C24 17.1305 22.7175 15.5606 20.9844 15.122C20.7924 12.8136 18.858 11 16.5 11Z"
            clip-rule="evenodd"
            fill-rule="evenodd"
          ></path>
        </svg>
      </div>
      <div class="text" v-if="!previewUrl">
        <span>點擊上傳</span>
        <span>(.jpg, .jpeg, .png, .webp)</span>
      </div>
      <img v-if="previewUrl" :src="previewUrl" class="previewImage" />
    </label>
    <input
      type="file"
      id="file"
      ref="fileInput"
      @change="onFileChange"
      accept=".jpg,.jpeg,.png,.webp"
    />
  </div>
</template>

<script>
export default {
  name: "ImageUploader",
  props: {
    previewUrl: { String, default: "" },
  },
  data() {
    return {};
  },
  methods: {
    triggerFileInput() {
      this.$refs.fileInput.click();
    },
    onFileChange(event) {
      const file = event.target.files[0];
      if (!file) return;

      // 設定最大檔案大小為 2MB
      const maxFileSize = 2 * 1024 * 1024; // 2MB

      if (file.size > maxFileSize) {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "檔案過大！最大允許檔案大小為 2MB";
        this.$notificationBox.notificationBoxErrorCode = 0;
        this.$refs.fileInput.value = ""; // 清除選取
        return;
      }

      // 允許的 mime 跟 副檔名
      const allowType = ["image/jpeg", "image/png", "image/webp"];
      const allowExtension = [".jpg", ".jpeg", ".png", ".webp"];

      const isValidType = allowType.includes(file.type);
      const isValidExtension = allowExtension.some((ext) =>
        file.name.toLowerCase().endsWith(ext)
      );

      if (!isValidType || !isValidExtension) {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "請上傳 .jpg、.jpeg、.png 或 .webp 圖片!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        this.$refs.fileInput.value = ""; // 清除選取
        return;
      }

      const reader = new FileReader();
      reader.onload = (e) => {
        const arrayBuffer = e.target.result;
        const bytes = new Uint8Array(arrayBuffer);

        if (!this.isValidMagicNumber(bytes)) {
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle =
            "請上傳 .jpg、.jpeg、.png 或 .webp 圖片!";
          this.$notificationBox.notificationBoxErrorCode = 0;
          this.$refs.fileInput.value = ""; // 清除選取
          return;
        }

        this.$emit("imageSelected", file);
      };
      reader.readAsArrayBuffer(file);
    },
    isValidMagicNumber(bytes) {
      // 定義 magic number 檢查邏輯
      const isJPEG =
        bytes[0] === 0xff && bytes[1] === 0xd8 && bytes[2] === 0xff;
      const isPNG =
        bytes[0] === 0x89 &&
        bytes[1] === 0x50 &&
        bytes[2] === 0x4e &&
        bytes[3] === 0x47 &&
        bytes[4] === 0x0d &&
        bytes[5] === 0x0a &&
        bytes[6] === 0x1a &&
        bytes[7] === 0x0a;
      const isWEBP =
        bytes[0] === 0x52 && // "RIFF"
        bytes[1] === 0x49 &&
        bytes[2] === 0x46 &&
        bytes[3] === 0x46 &&
        bytes[8] === 0x57 && // "WEBP"
        bytes[9] === 0x45 &&
        bytes[10] === 0x42 &&
        bytes[11] === 0x50;
      return isJPEG || isPNG || isWEBP;
    },
  },
  created(){
  }
};
</script>

<style scoped>
.fileUpload {
  width: 100%;
  height: 200px;
  display: flex;
  flex-direction: column;
  gap: 20px;
  cursor: pointer;
  justify-content: center;
  border: 2px dashed #cacaca;
  background-color: rgba(255, 255, 255, 1);
  padding: 1.5rem;
  border-radius: 10px;
  box-shadow: 0px 48px 35px -48px rgba(0, 0, 0, 0.1);
}

.fileUpload .icon {
  display: flex;
  align-items: center;
  justify-content: center;
}

.fileUpload .icon svg {
  height: 80px;
  fill: rgba(75, 85, 99, 1);
}

.fileUpload .text {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.fileUpload .text span {
  font-weight: 400;
  color: rgba(75, 85, 99, 1);
}

#file {
  display: none;
}

.previewImage {
  max-height: 100%;
  max-width: 100%;
  object-fit: contain;
  border-radius: 8px;
}
</style>
