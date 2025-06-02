<template>
  <div>
    <TitleCard text="租約" @refreshPage="$emit('refreshPage')"></TitleCard>
    <div class="functionColumn">
      <BtnNormal
        text="新增租約"
        @click="redirect('/leaseAgreement/add')"
      ></BtnNormal>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import BtnNormal from "@/components/Btn/BtnNormal";
export default {
  name: "LeaseAgreement",
  components: {
    TitleCard,
    BtnNormal,
  },
  methods: {
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
  },
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