<template>
  <div class="selectorContainer">
    <label class="lab" v-if="labelText">{{ labelText }}</label>
    <select
      class="normalSelector"
      v-model="localValue"
      @change="handleChange"
      @click="$emit('click', $event)"
      :disabled="disabled"
    >
      <option v-for="opt in options" :key="opt.value" :value="opt.value">
        {{ opt.text }}
      </option>
    </select>
  </div>
</template>
<script>
export default {
  name: "NormalSelector",
  props: {
    parentValue: {
      type: [String, Number],
      required: true,
    },
    options: {
      type: Array,
    },
    labelText: {
      type: String,
      default: "",
    },
    disabled:{
      type:Boolean,
      default:false
    }
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
      this.$emit("change", this.localValue);
    },
  },
};
</script>

<style scoped>
.selectorContainer {
  display: inline-block;
  border-radius: 0.5rem;
  background-color: #eee;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.2rem;
  font-size: 14px;
}

.lab {
  padding: 0.5rem 0.5rem;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
}

.normalSelector {
  border: none;
  border-radius: 0.5rem;
  padding: 0.4rem 0.4rem;
  background-color: #fafbfc;
}

.normalSelector:hover {
  cursor: pointer;
}
</style>