<template>
  <div class="checkout-page">
    <h2>訂單結帳</h2>

    <!-- 1. 訂單資訊 -->
    <section>
      <h3>訂單摘要</h3>
      <p>會員：{{ order.member.name }}（{{ order.member.phone }}）</p>
      <p>商品：{{ order.product.name }}</p>
      <p>價格：NT$ {{ order.product.price }}</p>
    </section>

    <!-- 2. 付款方式選擇 -->
    <section>
      <h3>付款方式</h3>
      <p>{{ paymentMethodLabel }}</p>
    </section>

    <!-- 放在 checkout 畫面中，條件為付款方式為現金 -->
    <section v-if="order.paymentMethod === 'cash'">
      <h3>💵 現金付款</h3>
      <p>應收金額：NT$ {{ totalAmount }}</p>

      <label for="received">實收金額：</label>
      <input
        id="received"
        type="number"
        v-model.number="receivedAmount"
        min="0"
        placeholder="請輸入客人給的錢"
      />

      <p>找零金額：NT$ {{ changeAmount }}</p>

      <p v-if="receivedAmount < totalAmount" style="color: red">
        ⚠️ 實收金額不足！
      </p>
    </section>

    <!-- 3. 收款按鈕 -->
    <section>
      <button @click="confirmPayment">✅ 收款並完成訂單</button>
    </section>

    <!-- 4. 成功提示 -->
    <div v-if="paid">
      <h3>✅ 收款成功！</h3>
      <p>訂單已完成</p>
      <button @click="$emit('goHome')">返回首頁</button>
    </div>
  </div>
</template>

<script>
export default {
  name: "HealthopeCheckoutOrder",
  props: ["order"],
  data() {
    return {
      receivedAmount: 0,
      paymentMethod: this.order.paymentMethod || "cash",
      paid: false,
    };
  },
  methods: {
    confirmPayment() {
      if (
        this.order.paymentMethod === "cash" &&
        this.receivedAmount < this.totalAmount
      ) {
        alert("⚠️ 實收金額不足，無法完成收款");
        return;
      }
      // 進行訂單完成 API 呼叫
      this.paid = true;
    },
    totalAmount() {
      return this.order.product.price;
    },
    changeAmount() {
      return Math.max(0, this.receivedAmount - this.totalAmount);
    },
  },
  computed: {
    paymentMethodLabel() {
      switch (this.order.paymentMethod) {
        case "cash":
          return "現金";
        case "card":
          return "信用卡";
        case "mobile":
          return "行動支付";
        case "bank":
          return "銀行轉帳";
        default:
          return "未知";
      }
    },
  },
};
</script>

<style scoped>
.checkout-page {
  max-width: 600px;
  margin: auto;
}
section {
  margin-bottom: 1.5em;
  padding: 1em;
  border: 1px solid #ccc;
  border-radius: 8px;
}
</style>