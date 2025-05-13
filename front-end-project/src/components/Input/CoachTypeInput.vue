<template>
      <span class="typeInputSpan">
          <label class="lab">請選擇類別</label>
          <div class="typeRadioContainer">
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioType"
                value="1"
                v-model="localType"
              />
              <span class="textRadio">私人教練</span>
            </label>
            <label class="labRadioBox">
              <input
                type="radio"
                name="radioType"
                value="2"
                v-model="localType"
              />
              <span class="textRadio">約聘教練</span>
            </label>
          </div>
        </span>
</template>
<script>
export default {
  name: "CoachTypeInput",
  props: {
    value: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      localType: this.value
    };
  },
  watch: {
    localType(newVal) {
      this.$emit("input", newVal);  
    },
    value(newVal) {
      this.localType = newVal;
    }
  }
};
</script>

<style scoped>
.typeInputSpan {
  display: flex;
  gap: 0.5rem;
  flex-direction: column;
}

.typeRadioContainer {
  display: flex;
  flex-wrap: wrap;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.2rem;
  width: 250px;
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

.labRadioBox input:checked + .textRadio {
  background-color: #fff;
  font-weight: 600;
}
</style>