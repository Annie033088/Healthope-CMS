<template>
  <span class="inputSpan">
    <label class="label">{{ labelText }}</label>
    <input :value="localInputText" @input="onInput" :type="inputType" @keydown.enter="$emit('enter')" />
  </span>
</template>

<script>
export default {
  name: "InputSpan",
  props: {
    labelText: String,
    value: String, // 這是 v-model 的來源
    inputType: {
      type: String,
      default: "text", // 預設為 text
    },
  },
  data() {
    return {
      localInputText: this.value,
    };
  },
  watch: {
    value(newValue) {
      this.localInputText = newValue;
    },
  },
  methods: {
    onInput(event) {
      const newValue = event.target.value;
      this.localInputText = newValue;
      this.$emit("input", newValue); // 發出給父層更新 v-model
    },
  },
};
</script>

<style scoped>
.inputSpan {
  margin-top: 5%;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.inputSpan input {
  border-radius: 0.5rem;
  padding: 0.7rem 0.75rem;
  width: 100%;
  border: none;
  background-color: white;
  outline: 2px solid #efefef;
  font-size: 15px;
  margin-left: 15px;
}

.inputSpan input:focus {
  outline: 2px solid #707070;
}
</style>