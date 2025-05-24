<template>
  <div class="radioSelectorContainer">
    <label class="lab">{{ inputTitle }}</label>
    <label class="labRadioBox" v-for="option in options" :key="option.value">
      <input
        type="radio"
        :name="inputType"
        :value="option.value"
        :checked="value === option.value"
        @change="updateValue(option.value)"
      />
      <span class="textRadio">{{ option.text }}</span>
    </label>
    <input
      id="date"
      type="date"
      class="txbDate"
      v-model="localDate"
      @change="updateValue(localDate)"
    />
  </div>
</template>

<script>
export default {
  name: "DateSelector",
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
  },
  data() {
    return {
      localDate: this.value,
    };
  },
  methods: {
    updateValue(val) {
      this.$emit("input", val); // 使用 input 事件來配合 v-model
      this.$emit("change");
    },
  },
  watch: {
    value: {
      immediate: true,
      handler(val) {
        let checked = false;
        this.options.forEach((option) => {
          if (val === option.value) {
            checked = true;
          }
        });
        
        if (checked) this.localDate = "";
      },
    },
  },
};
</script>

<style scoped>
.radioSelectorContainer {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.25rem;
  font-size: 14px;
}

.lab {
  padding: 0.5rem 0.5rem;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
  white-space: nowrap;
}

.labRadioBox {
  flex: 1 1 auto;
  text-align: center;
  justify-content: center;
  flex-wrap: wrap;
  min-width: 50px;
  margin-right: 5px;
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

.selectDate {
  border: none;
  border-radius: 0.5rem;
  padding: 0.5rem 0.5rem;
  margin-right: 5px;
  background-color: #fafbfc;
  max-width: 65px;
  cursor: pointer;
}

.txbDate {
  border: 1px solid #ccc;
  border-radius: 0.5rem;
  background-color: #fff;

  padding: 0.5rem 0.5rem;
  font-size: 15px;
  color: #333;

  max-width: 150px;
  box-sizing: border-box;
  height: 36px;
}
</style>
