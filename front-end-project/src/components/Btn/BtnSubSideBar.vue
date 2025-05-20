<template>
  <div class="subSidebarRow">
    <button class="btnSidebar" @click="$emit('click', $event)">
      <b class="subSidebarText">{{ text }}</b>
    </button>
  </div>
</template>

<script>
export default {
  name: "BtnSubSideBar",
  props: {
    text: String,
  },
};
</script>

<style scoped>
.subSidebarRow {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  margin-top: 15px;
}

.sidebarIcon {
  margin-right: 8px;
}

.subSidebarText {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: rgb(100, 42, 42);
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