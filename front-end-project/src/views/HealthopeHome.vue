<template>
  <div>
    <AppSidebar class="sidebar"> </AppSidebar>
    <h1 class="contentContainer">weqiopweiqweqwioep</h1>
  </div>
</template>

<script>
import AppSidebar from "@/components/AppSidebar.vue";

export default {
  name: "HealthopeHome",
  components: {
    AppSidebar,
  },
  data() {
    return {
      tasks: [],
    };
  },
};
</script>

<style scoped>
.contentContainer {
  margin-top: 0;
  margin-left: 200px;
}
</style>