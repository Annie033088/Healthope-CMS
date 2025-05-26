<template>
  <div>
    <TitleCard text="方案" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <BtnNormal text="新增方案" @click="redirect('/plan/add')"></BtnNormal>
      <RadioSelector
        v-model="selectStatus"
        @change="selectPlanByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="[
          { value: '1', text: '有效' },
          { value: '0', text: '無效' },
        ]"
      />
      <RadioSelector
        class="radioType"
        v-model="selectType"
        @change="selectPlanByType"
        inputTitle="方案類別："
        inputType="radioType"
        :options="[
          { value: 'membershipPlan', text: '會籍' },
          { value: 'personalTrainingPackage', text: '教練課' },
          { value: 'ticketPlan', text: '入場劵' },
          { value: '', text: '全部' },
        ]"
      />
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import BtnNormal from "@/components/Btn/BtnNormal";
import RadioSelector from "@/components/Selector/RadioSelector";

export default {
  name: "HealthopePlan",
  components: {
    TitleCard,
    BtnNormal,
    RadioSelector,
  },
  data() {
    return {
      selectStatus: "0",
      selectType: "",
    };
  },
  methods: {
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    selectPlanByStatus() {},
    selectPlanByType() {},
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
.radioType {
  width: 350px;
}
</style>