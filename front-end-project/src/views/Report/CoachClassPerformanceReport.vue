<template>
  <div>
    <TitleCard text="教練課報表" @refreshPage="$emit('refreshPage')" />
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";

export default {
  name: "CoachClassPerformanceReport",
  components: {
    TitleCard,
  },
  data() {
    return {};
  },
};
</script>

<style scoped>
</style>