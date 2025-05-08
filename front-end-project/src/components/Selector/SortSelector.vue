<template>
  <div class="sortContainer">
    <label class="labSort">排序選項：</label>
    <select
      class="selectSortOrder"
      v-model="localSortOption"
      @change="emitChange"
    >
      <option value="">無</option>
      <option
        v-for="option in options"
        :key="option.value"
        :value="option.value"
      >
        {{ option.label }}
      </option>
    </select>

    <label class="labRadioBox">
      <input
        type="radio"
        name="radioSort"
        value="ascending"
        v-model="localSortOrder"
        @change="emitChange"
      />
      <span class="textRadio">升序</span>
    </label>

    <label class="labRadioBox">
      <input
        type="radio"
        name="radioSort"
        value="descending"
        v-model="localSortOrder"
        @change="emitChange"
      />
      <span class="textRadio">降序</span>
    </label>
  </div>
</template>

<script>
export default {
  name: "SortSelector",
  props: {
    options: {
      type: Array,
      required: true, // 例如: [{ value: 'account', label: '帳號' }]
    },
    sortOption: {
      type: String,
      default: '',
    },
    sortOrder: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      localSortOption: this.sortOption,
      localSortOrder: this.sortOrder,
    };
  },
  watch: {
    sortOption(val) {
      this.localSortOption = val;
    },
    sortOrder(val) {
      this.localSortOrder = val;
    },
  },
  methods: {
    emitChange() {
      this.$emit("update:sortOption", this.localSortOption);
      this.$emit("update:sortOrder", this.localSortOrder);
      this.$emit("change"); // 觸發父層如 getAdminData
    },
  },
};
</script>

<style scoped>
.sortContainer{
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

.labSort{
  padding: 0.5rem 0.5rem;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
}

.selectSortOrder {
  border: none;
  border-radius: 0.5rem;
  padding: 0.5rem 0.5rem;
  margin-right: 5px;
  background-color: #fafbfc;
  max-width: 65px;
  cursor: pointer;
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