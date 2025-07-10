<template>
  <div>
    <TitleCard
      text="會員教練課程"
      @refreshPage="$emit('refreshPage')"
    ></TitleCard>
    <div class="functionColumn">
      <BtnNormal
        text="新增課程"
        @click="redirect('/memberPersonalClass/add')"
      ></BtnNormal>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import BtnNormal from "@/components/Btn/BtnNormal";

export default {
  name: "MemberPersonalClass",
  components: {
    TitleCard,
    BtnNormal,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  methods:{
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
  }
};
</script>

<style scoped>
.functionColumn {
  margin: 15px;
  display: flex;
  flex-wrap: wrap;
  gap: 10px 20px;
}
</style>