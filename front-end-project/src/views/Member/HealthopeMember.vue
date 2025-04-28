<template>
  <div>
    <TitleCard text="會員清單"></TitleCard>
  </div>
</template>


<script>
import TitleCard from "@/components/Card/TitleCard";

export default {
  name: "HealthopeMember",
  components: {
    TitleCard,
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