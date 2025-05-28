<template>
  <span class="inputSpan">
    <label class="lab">{{ inputTitle }}</label>
    <div class="radioContainer">
      <label class="labRadioBox" v-for="option in options" :key="option.value">
        <input
          type="radio"
          :name="inputType"
          :value="option.value"
          v-model="localValue"
          @change="handleChange(option.value)"
        />
        <span
          class="textRadio"
          :class="{
            checked: option.value == localValue,
            highlight: option.value == localValue && hightlightFlag,
          }"
          >{{ option.text }}</span
        >
      </label>
    </div>
  </span>
</template>
<script>
export default {
  name: "RadioInput",
  props: {
    value: {
      type: String,
      required: true,
    },
    options: {
      type: Array,
      required: true,
    },
    inputTitle: {
      type: String,
      default: "",
    },
    inputType: {
      type: String,
      default: "radioInput",
    },
    hightlightFlag: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      localValue: this.value,
    };
  },
  methods: {
    handleChange(newVal) {
      this.$emit("change", newVal); // 手動通知父元件
    },
  },
  watch: {
    localValue(newVal) {
      this.$emit("input", newVal);
    },
    value(newVal) {
      this.localValue = newVal;
    },
  },
};
</script>

<style scoped>
.inputSpan {
  display: flex;
  gap: 0.5rem;
  flex-direction: column;
}

.inputSpan .lab {
  font-weight: 500;
}

.radioContainer {
  display: flex;
  flex-wrap: wrap;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.2rem;
  min-width: 215px;
  font-size: 16px;
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

.textRadio.checked {
  background-color: #fff;
  font-weight: 600;
}

.textRadio.highlight {
  font-size: 24px;
}
</style>