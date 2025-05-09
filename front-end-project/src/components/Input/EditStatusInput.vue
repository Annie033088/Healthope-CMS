<template>
      <span class="statusInputSpan">
          <label class="lab">請修改狀態</label>
          <div class="statusRadioContainer">
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioStatus"
                value="true"
                v-model="localStatus"
              />
              <span class="textRadio">啟用</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioStatus"
                value="false"
                v-model="localStatus"
              />
              <span class="textRadio">停用</span>
            </label>
          </div>
        </span>
</template>
<script>
export default {
  name: "EditStatusInput",
  props: {
    value: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      localStatus: this.value
    };
  },
  watch: {
    localStatus(newVal) {
      this.$emit("input", newVal);  
    },
    value(newVal) {
      this.localStatus = newVal;
    }
  }
};
</script>

<style scoped>
.statusInputSpan {
  display: flex;
  gap: 0.5rem;
  flex-direction: column;
  margin-bottom: 20px;
}

.statusRadioContainer {
  display: flex;
  flex-wrap: wrap;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.35rem;
  width: 150px;
  font-size: 16px;
  margin-left: 20px;
  gap: 15px;
}

.labRadioBox {
  flex: 1 1 auto;
  text-align: center;
  justify-content: center;
  flex-wrap: wrap;
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