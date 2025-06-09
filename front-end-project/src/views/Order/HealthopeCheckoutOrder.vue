<template>
  <div class="">
    <TitleCard text="訂單" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="訂單結帳"></SubTitleCard>
    <div class="checkoutPage">
      <h2>訂單結帳</h2>

      <!-- 1. 訂單資訊 -->
      <section>
        <h3>訂單摘要</h3>
        <p>會員：{{ order.MemberName }}（0{{ order.MemberPhone }}）</p>
        <p>商品：{{ order.ProductName }}</p>
        <p>價格：NT$ {{ order.Amount }}元</p>
        <p>付款方式： {{ paymentMethodLabel }}</p>
      </section>

      <!-- 2. 成功提示 -->
      <div v-if="paid">
        <h3>✅ 收款成功！</h3>
        <p>訂單已完成</p>
        <BtnNormal @click="redirect('/')" text="返回首頁"></BtnNormal>
      </div>
    </div>

    <CardPaymentBox
      v-if="showPaymentBox"
      :orderId="order.OrderId"
      @close="showPaymentBox = false"
    />
  </div>
</template>

<script>
import BtnNormal from "@/components/Btn/BtnNormal";
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import CardPaymentBox from "@/components/Box/CardPaymentBox";
export default {
  name: "HealthopeCheckoutOrder",
  components: {
    BtnNormal,
    TitleCard,
    SubTitleCard,
    CardPaymentBox,
  },
  data() {
    return {
      paid: false,
      order: {},
      showPaymentBox: false,
    };
  },
  methods: {
    redirect(path) {
      this.$router.push(path);
    },
  },
  computed: {
    paymentMethodLabel() {
      switch (this.order.PaymentMethod) {
        case "1":
          return "現金";
        case "2":
          return "信用卡";
        default:
          return "未知";
      }
    },
  },
  mounted() {
    if (this.order.PaymentMethod === "1") this.paid = true;
    if (this.order.PaymentMethod === "2") this.showPaymentBox = true;
  },
  created() {
    const orderStr = this.$route.query.order;
    this.order = orderStr ? JSON.parse(orderStr) : null;
  },
};
</script>

<style scoped>
.checkoutPage {
  width: 600px;
  max-width: 80%;
  margin: auto;
}

section {
  margin-bottom: 1.5em;
  padding: 1em;
  border: 1px solid #ccc;
  border-radius: 8px;
}
</style>