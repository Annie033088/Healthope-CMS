<template>
  <div>
    <TitleCard text="財務報表" @refreshPage="$emit('refreshPage')" />
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";

export default {
  name: "FinancialStatements",
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