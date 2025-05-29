<template>
  <button class="btnCancel btn" @click="$emit('click', $event)">
    {{ text }}
  </button>
</template>

<script>
export default {
  name: "BtnCancel",
  props: {
    text: [String, Number],
  },
};
</script>

<style scoped>
.btnCancel {
  width: 100%;
  max-width: 400px;
  height: 50px;
  border-radius: 3rem;
  background-color: #828282;
  color: #efefef;
  cursor: pointer;
  transition: all 300ms;
  font-weight: 600;
  border: none;
  font-size: 16px;
}

.btnCancel:hover {
  background-color: #666666f2;
}
</style>
