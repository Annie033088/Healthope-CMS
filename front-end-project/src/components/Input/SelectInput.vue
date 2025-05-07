<template>
  <div class="selectInputGroup">
    <input
      type="text"
      autocomplete="off"
      :placeholder="placeholder"
      @keydown.enter="$emit('select')"
      :value="localInputText"
      @input="onInput"
    />
    <svg
      @click="$emit('select')"
      class="btnSearch"
      width="30"
      height="30"
      viewBox="0 0 24 24"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M16.893 16.92L19.973 20M19 11.5C19 13.4891 18.2098 15.3968 16.8033 16.8033C15.3968 18.2098 13.4891 19 11.5 19C9.51088 19 7.60322 18.2098 6.1967 16.8033C4.79018 15.3968 4 13.4891 4 11.5C4 9.51088 4.79018 7.60322 6.1967 6.1967C7.60322 4.79018 9.51088 4 11.5 4C13.4891 4 15.3968 4.79018 16.8033 6.1967C18.2098 7.60322 19 9.51088 19 11.5Z"
        stroke="#B4B4B4"
        stroke-width="1.5"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </svg>
  </div>
</template>
<script>
export default {
  name: "SelectInput",
  props: {
    placeholder:String,
    value: String, // 這是 v-model 的來源
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
.selectInputGroup {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  max-width: 200px;
  border: 1px solid rgb(200, 200, 200);
  background-color: transparent;
  border-radius: 1rem;
}

.selectInputGroup input {
  font-size: 100%;
  padding: 0.6em;
  outline: none;
  border: none;
  background-color: transparent;
  border-radius: 1rem;
  width: 100%;
}

.selectInputGroup input:hover {
  background-color: #fafbfc;
}

.btnSearch {
  border-end-end-radius: 20px;
  border-top-right-radius: 20px;
  border-left: 1.5px solid rgb(200, 200, 200);
  margin-right: 4px;
}

.btnSearch:hover {
  cursor: pointer;
  background-color: #fafbfc;
}

.btnSearch:active {
  background-color: #edeff2;
  transition: none 0.1s;
}
</style>