<template>
      <button class="btnNormal" @click="$emit('click', $event)">{{text}}</button>
</template>

<script>
export default {
  name: "BtnNormal",
  props: {
    text: String,
  },
}
</script>

<style scoped>
.btnNormal {
  background-color: #fafbfc;
  border: 1px solid rgba(27, 31, 35, 0.15);
  border-radius: 6px;
  color: #24292e;
  cursor: pointer;
  padding: 6px 16px;
}

.btnNormal:hover {
  background-color: #f3f4f6;
  transition-duration: 0.1s;
}

.btnNormal:active {
  background-color: #edeff2;
  transition: none 0.1s;
}
</style>