<template>
  <div>
    <TitleCard text="訂單" @refreshPage="$emit('refreshPage')" />
    <div class="beforeCheckout">
      <section>
        <div v-if="order.PaymentMethod === '2'">
          <h3>選擇讀卡機</h3>
          <NormalSelector
            class="selector"
            labelText="請選擇讀卡機："
            :parentValue.sync="selectedReader"
            :options="[
              { value: 'Reader01', text: 'Pax A920' },
              { value: 'Reader02', text: 'USB Reader' },
            ]"
          />
        </div>
        <div v-if="order.PlanType === 2">
          <h3>選擇教練</h3>
          <SelectInput
            class="selectPlan"
            :parentValue.sync="coachId"
            :options="coachOptions"
          />
        </div>
        <h3>結帳</h3>
        <BtnNormal text="➕ 進行結帳" @click="goCheckoutOrder" />
      </section>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import BtnNormal from "@/components/Btn/BtnNormal";
import NormalSelector from "@/components/Selector/NormalSelector";
import SelectInput from "@/components/Input/SelectInput";

export default {
  name: "HealthopeBerforeCheckout",
  components: {
    TitleCard,
    BtnNormal,
    NormalSelector,
    SelectInput,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectedReader: "",
      order: {},
      coachId: "",
      coaches: [],
    };
  },
  methods: {
    goCheckoutOrder() {
      this.order.CoachId = null;
      this.order.CoachName = null;
      this.order.CardReaderId = null;

      if (this.order.PaymentMethod === "2") {
        if (!this.selectedReader) {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "請選擇讀卡機!";
          this.$notificationBox.notificationBoxErrorCode = 0;
          return;
        }

        this.order.CardReaderId = this.selectedReader;
      }

      if (this.order.PlanType === 2) {
        const IntMax = 2147483647;
        let coachId = Number(this.coachId);

        if (!Number.isInteger(coachId) || coachId < 1 || coachId > IntMax) {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "請選擇教練!";
          this.$notificationBox.notificationBoxErrorCode = 0;
          return;
        }

        let selectCoach = null;

        if (this.coachId) {
          selectCoach = this.coaches.find((coach) => coach.CoachId === coachId);
        }

        if (!selectCoach) return;

        this.order.CoachId = coachId;
        this.order.CoachName = selectCoach.Name;
      }

      this.$router.push({
        name: "HealthopeCheckoutOrder",
        query: {
          order: JSON.stringify(this.order),
        },
      });
    },
    async getPersonalCoach() {
      // 已載入或非教練方案則跳過
      if (this.coaches.length > 0 || this.order.PlanType !== 2) return;
      try {
        // post
        const response = await this.$axios.post("/api/Coach/GetPersonalCoach");

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.coaches = [];
          response.data.ApiDataObject.forEach((coach) => {
            this.coaches.push(coach);
          });
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("取得教練時發生錯誤", error);
      }
    },
  },
  computed: {
    coachOptions() {
      if (this.order.PlanType !== 2) return [];

      return this.coaches.map((coach) => ({
        value: coach.CoachId,
        text: coach.Name + ` (0${coach.Phone})`,
      }));
    },
  },
  created() {
    const orderStr = this.$route.query.order;
    this.order = orderStr ? JSON.parse(orderStr) : null;

    if (this.order.PlanType === 2) {
      this.getPersonalCoach();
    }
  },
};
</script>

<style scoped>
section {
  width: 600px;
  max-width: 80%;
  margin: 1em 0;
  padding: 1em;
  border: 1px solid #ddd;
  border-radius: 8px;
}

.beforeCheckout {
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>