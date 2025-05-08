<template>
  <div class="selectRecordContainer">
    <label class="labRecord">顯示：</label>
    <select class="selectRecord" v-model="localValue" @change="handleChange">
      <option
        v-for="opt in options"
        :key="opt"
        :value="opt"
      >
        {{ opt }}
      </option>
    </select>
    <label class="">筆</label>
  </div>
</template>
<script>
export default {
  name: "RecordSelector",
  props: {
    parentValue: {
      type: [String, Number],
      required: true
    },
    options: {
      type: Array,
      default: () => ["8", "12", "16"]
    }
  },
  data() {
    return {
      localValue: this.parentValue
    };
  },
  watch: {
    parentValue(val) {
      this.localValue = val;
    }
  },
  methods: {
    handleChange() {
      this.$emit("update:parentValue", this.localValue);
      this.$emit("change");
    }
  }
};
</script>

<style scoped>
.selectRecordContainer{
  display: flex;
  align-items: center;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.25rem;
  font-size: 14px;
  width: 150px;
}

.labRecord {
  padding: 0.5rem 0.5rem;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
}

.selectRecord {
  border: none;
  border-radius: 0.5rem;
  padding: 0.5rem 0.5rem;
  margin-right: 5px;
  background-color: #fafbfc;
}

.selectRecord:hover {
  cursor: pointer;
}

</style>