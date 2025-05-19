<template>
  <div class="sidebarRow">
    <button class="btnSidebar" @click="$emit('click', $event)">
      <b class="sidebarIcon">●</b>
      <b>{{ text }}</b>
    </button>
  </div>
</template>

<script>
export default {
  name: "BtnSubSideBar",
  props:{
    text:String,
  }
};
</script>

<style scoped>
.sidebarRow {
  display: flex;
  align-items: center;
  justify-content: start;
}

.sidebarIcon {
  margin-right: 8px;
}

.btnSidebar {
  background-color: #e0e0e0;
  border: none;
  padding: 5px;
  font-size: 1rem;
  width: 170px;
  border-radius: 1rem;
  cursor: pointer;
  display: flex;
  justify-content: left;
}

.btnSidebar:active {
  transform: translateY(0.2rem);
}

.btnSidebar:hover:not(:disabled) {
  background: #eee;
}
</style>