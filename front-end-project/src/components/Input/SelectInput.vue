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
    <select class="selectInput" v-model="localValue" @change="handleChange">
      <option v-for="opt in options" :key="opt.value" :value="opt.value">
        {{ opt.text }}
      </option>
    </select>
  </span>
</template>
<script>
export default {
  name: "SelectInput",
  props: {
    labelText: {
      type: String,
    },
    parentValue: {
      type: [String, Number],
      required: true,
      default: "",
    },
    options: {
      type: Array,
    },
    required: {
      default: false,
    },
  },
  data() {
    return {
      localValue: this.parentValue,
    };
  },
  watch: {
    parentValue(val) {
      this.localValue = val;
    },
  },
  methods: {
    handleChange() {
      this.$emit("update:parentValue", this.localValue);
      this.$emit("change");
    },
  },
};
</script>

<style scoped>
.selectInput {
  border-radius: 0.5rem;
  padding: 0rem 0.75rem;
  border: none;
  outline: 2px solid #efefef;
  font-size: 15px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.selectInput:hover {
  cursor: pointer;
}

.inputSpan {
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 0.5rem;
  margin-bottom: 3%;
}

.inputSpan label {
  font-weight: bold;
}

.inputSpan input:focus {
  outline: 2px solid #707070;
}

.labelRow {
  margin-left: -15px;
  display: flex;
}
</style>