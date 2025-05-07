<template>
  <div>
    <TitleCard text="會員清單"></TitleCard>
    <TableNormal></TableNormal>
  </div>
</template>


<script>
import TitleCard from "@/components/Card/TitleCard";
import TableNormal from"@/components/Table/TableNormal.vue";

export default {
  name: "HealthopeMember",
  components: {
    TitleCard,
    TableNormal
  },
  props: {
    text: String,
  },
  data() {
    return {
      hintText: "",
    };
  },
  methods: {},
};
</script>