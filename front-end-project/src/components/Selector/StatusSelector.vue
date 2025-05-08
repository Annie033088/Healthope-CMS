<template>
  <div class="statusContainer">
    <label class="labStatus">狀態：</label>
    <label class="labRadioBox">
      <input
        type="radio"
        name="radioStatus"
        value=""
        :checked="value === ''"
        @change="updateValue('')"
      />
      <span class="textRadio">無</span>
    </label>
    <label class="labRadioBox">
      <input
        type="radio"
        name="radioStatus"
        value="true"
        :checked="value === 'true'"
        @change="updateValue('true')"
      />
      <span class="textRadio">啟用</span>
    </label>
    <label class="labRadioBox">
      <input
        type="radio"
        name="radioStatus"
        value="false"
        :checked="value === 'false'"
        @change="updateValue('false')"
      />
      <span class="textRadio">停用</span>
    </label>
  </div>
</template>

<script>
export default {
  name: "StatusSelector",
  props: {
    value: {
      type: String,
      required: true,
    },
  },
  methods: {
    updateValue(val) {
      this.$emit("input", val); // 使用 input 事件來配合 v-model
      this.$emit("change");
    },
  },
};
</script>

<style scoped>
.statusContainer {
  display: flex;
  align-items: center;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.25rem;
  width: 300px;
  font-size: 14px;
}

.labStatus {
  padding: 0.5rem 0.5rem;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
}

.labRadioBox {
  flex: 1 1 auto;
  text-align: center;
  justify-content: center;
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
