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
        <p>商品：{{ order.PlanName }}</p>
        <p v-if="order.CoachName">教練：{{ order.CoachName }}</p>
        <p>價格：NT$ {{ order.Amount }}元</p>
        <p>付款方式： {{ paymentMethodLabel }}</p>
      </section>

      <!-- 2. 成功提示 -->
      <section v-if="paid && paidSuccess">
        <h3>✅ 收款成功！</h3>
        <p>訂單已完成</p>
        <BtnNormal @click="redirect('/')" text="返回首頁"></BtnNormal>
      </section>

      <!-- 2. 失敗提示 -->
      <section v-if="paid && !paidSuccess">
        <h3>❌ 付款失敗：請再試一次</h3>
        <BtnNormal
          v-if="order.paymentMethod == '1'"
          @click="retryCardPay"
          text="重新刷卡"
        ></BtnNormal>
        <BtnNormal
          class="btnFailToHome"
          @click="redirect('/')"
          text="返回首頁"
        ></BtnNormal>
      </section>

      <div v-show="qrCodeString && paid && paidSuccess">
        <canvas ref="canvas" @click="copyQrCodeString"></canvas>
        <p v-if="copied" class="qrCodeCopyHint">已複製到剪貼簿！</p>
      </div>
    </div>

    <CardPaymentBox
      v-if="showPaymentBox"
      :order="order"
      @close="showPaymentBox = false"
      @cardPaySuccess="cardPaySuccess"
      @cardPayFail="cardPayFail"
    />
  </div>
</template>

<script>
import BtnNormal from "@/components/Btn/BtnNormal";
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import CardPaymentBox from "@/components/Box/CardPaymentBox";
import QRCode from "qrcode";

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
      paidSuccess: false,
      qrCodeString: "",
      copied: false,
    };
  },
  methods: {
    async copyQrCodeString() {
      try {
        await navigator.clipboard.writeText(this.qrCodeString);
        this.copied = true;
        setTimeout(() => (this.copied = false), 2000);
      } catch (err) {
        console.error("複製失敗:", err);
      }
    },
    redirect(path) {
      this.$router.push(path);
    },
    cardPaySuccess(qrCodeString) {
      this.showPaymentBox = false;
      this.paidSuccess = true;
      this.paid = true;
      if (qrCodeString) {
        this.qrCodeString = qrCodeString;
      }
    },
    cardPayFail() {
      this.showPaymentBox = false;
      this.paidSuccess = false;
      this.paid = true;
    },
    retryCardPay() {
      this.showPaymentBox = true;
    },
    async payByCash() {
      let payByCashDto = {
        OrderId: this.order.OrderId,
        UpdateTime: this.order.UpdateTime,
        CoachId: this.order.CoachId ?? null,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/PayByCash",
          payByCashDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.paid = true;
          this.paidSuccess = true;
          if (response.data.ApiDataObject) {
            this.qrCodeString = response.data.ApiDataObject.QrCodeString;
          }
        } else {
          this.paid = true;
          this.paidSuccess = false;
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "現金付款發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("現金付款時發生錯誤", error);
      } finally {
        this.isLoading = false;
      }
    },
    drawQrCode(text) {
      QRCode.toCanvas(
        this.$refs.canvas,
        text,
        { errorCorrectionLevel: "H" },
        (error) => {
          if (error) console.error(error);
        }
      );
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
    if (this.order.PaymentMethod === "1") {
      this.payByCash();
    }
    if (this.order.PaymentMethod === "2") this.showPaymentBox = true;
  },
  created() {
    const orderStr = this.$route.query.order;
    this.order = orderStr ? JSON.parse(orderStr) : null;
  },
  watch: {
    qrCodeString(val) {
      if (val) {
        this.drawQrCode(val);
      }
    },
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

.btnFailToHome {
  margin-left: 5px;
}

canvas {
  cursor: pointer;
  border: 1px solid #ccc;
}

.qrCodeCopyHint {
  color: green;
}
</style>