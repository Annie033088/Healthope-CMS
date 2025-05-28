<template>
  <span class="inputSpan">
    <div class="labelRow">
      <svg
        width="12"
        height="12"
        viewBox="0 0 24 24"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M21 13H14.4L19.1 17.7L17.7 19.1L13 14.4V21H11V14.3L6.3 19L4.9 17.6L9.4 13H3V11H9.6L4.9 6.3L6.3 4.9L11 9.6V3H13V9.4L17.6 4.8L19 6.3L14.3 11H21V13Z"
          :fill="required ? '#F24822' : '#f7f6f6'"
        />
      </svg>
      <label class="label">{{ labelText }}</label>
    </div>
    <input
      :value="localInputText"
      @input="onInput"
      :type="inputType"
      @keydown.enter="$emit('enter')"
    />
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
    required: { type: Boolean, default: false },
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
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.inputSpan label{
  font-weight: bold;
}

.inputSpan input {
  border-radius: 0.5rem;
  padding: 0.7rem 0.75rem;
  border: none;
  background-color: white;
  outline: 2px solid #efefef;
  font-size: 15px;
  height: 14px;
}

.inputSpan input:focus {
  outline: 2px solid #707070;
}

.labelRow {
  margin-left: -15px;
  display: flex;
}
</style>