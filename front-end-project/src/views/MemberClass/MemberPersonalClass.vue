<template>
  <div>
    <TitleCard
      text="會員教練課程"
      @refreshPage="$emit('refreshPage')"
    ></TitleCard>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";

export default {
  name: "MemberPersonalClass",
  components: {
    TitleCard,
  },
};
</script>