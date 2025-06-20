<template>
  <div>
    <TitleCard text="訂單" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="查看訂單詳情"></SubTitleCard>
    <div class="sectionTitle">
      <h3>訂單資訊</h3>
    </div>
    <div class="orderDetailContentBox">
      <div class="orderDetailContentContainer">
        <div class="detailContent">
          <div class="top">
            <div class="contentTextBox">
              <label class="lab">訂單編號</label><br />
              <span>{{ order.OrderNumber }}</span>
            </div>
            <div class="contentTextBox">
              <label class="lab">訂單狀態</label><br />
              <span>{{ order.State }}</span>
            </div>
            <div class="contentTextBox">
              <label class="lab">方案名稱</label><br />
              <span>{{ order.PlanName }}</span>
            </div>
          </div>
          <div class="middle">
            <div class="contentTextBox">
              <label class="lab">金額</label><br />
              <span>{{ order.Amount }}</span>
            </div>
            <div class="contentTextBox">
              <label class="lab">付款方式</label><br />
              <span>{{ order.Method }}</span>
            </div>
          </div>
          <div class="bottom">
            <div class="contentTextBox">
              <label class="lab">備註</label><br />
              <span>{{ order.Remark }}</span>
            </div>
            <div class="contentTextBox">
              <label class="lab">創建時間</label><br />
              <span>{{ order.CreateTime }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="sectionTitle">
      <h3>訂單狀態</h3>
    </div>
    <div class="orderStateContainer">
      <OrderStateCard
        v-for="state in orderStateList"
        :key="state.OrderStateId"
        :state="state"
        @saveNote="handleSaveNote"
      />
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import OrderStateCard from "@/components/Card/OrderStateCard";
import { orderStateAndText, paymentMethodAndText } from "@/utils/order";

export default {
  name: "HealthopeOrderDetail",
  components: {
    TitleCard,
    SubTitleCard,
    OrderStateCard,
  },
  data() {
    return {
      order: {},
      orderStateList: [],
    };
  },
  methods: {
    async handleSaveNote({ OrderStateId, Remark }) {
      try {
        const editOrderStateDto = {
          OrderStateId,
          Remark,
        };

        // post
        const response = await this.$axios.post(
          "/api/Order/EditOrderStateRemark",
          editOrderStateDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrderDetail(this.order.OrderId);
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/order";
                this.$emit("afterConfirmEvent", redirectRoute);
                this.unwatchFlag(); // 移除監聽
                this.unwatchFlag = null;
              }
            }
          );

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改訂單狀態時發生錯誤", error);
      }
    },
    async getOrderDetail(orderId) {
      try {
        const OrderIdDto = {
          OrderId: orderId,
        };
        // post
        const response = await this.$axios.post(
          "/api/Order/GetOrderById",
          OrderIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.order = response.data.ApiDataObject.Order;
          this.orderStateList = response.data.ApiDataObject.OrderStateList;

          this.order.OrderId = orderId;
          
          orderStateAndText.forEach((state) => {
            if (Number(state.value) === this.order.State) {
              this.order.State = state.text;
            }
          });

          paymentMethodAndText.forEach((method) => {
            if (Number(method.value) === this.order.Method) {
              this.order.Method = method.text;
            }
          });

          this.order.Amount = "$" + this.order.Amount;

          const localTime = new Date(this.order.CreateTime).toLocaleString();
          this.order.CreateTime = localTime;
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/order";
                this.$emit("afterConfirmEvent", redirectRoute);
                this.unwatchFlag(); // 移除監聽
                this.unwatchFlag = null;
              }
            }
          );

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("取得訂單詳情時發生錯誤", error);
      }
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/order");
      return;
    }

    this.getOrderDetail(this.$route.query.id);
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
}

.orderDetailContentBox {
  display: flex;
  justify-content: center;
  margin-bottom: 15px;
}

.orderDetailContentContainer {
  display: flex;
  align-items: center;
  padding: 9px;
  width: 1000px;
  max-width: 80%;
  background-color: white;
  border-radius: 35px;
  gap: 9px;
  box-shadow: rgba(10, 37, 64, 0.35) 0px -1px 5px 0px inset;
}

.detailContent {
  display: flex;
  justify-content: space-evenly;
  align-items: center;
  flex-wrap: wrap;
  overflow: hidden;
  width: 1000px;
  max-width: 100%;
  border-radius: 30px;
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
}

.contentTextBox {
  margin-left: 25px;
  margin-bottom: 5px;
  width: 150px;
}

.contentTextBox label {
  font-size: 20px;
  font-weight: 700;
  color: #6f6f6f;
  font-family: "Microsoft JhengHei";
}

.detailContent .contentTextBox {
  margin-left: 0;
}

.top,
.bottom,
.middle {
  display: flex;
  flex-wrap: wrap;
  width: 100%;
  height: 100%;
  padding: 5px;
  gap: 10px 10%;
  word-break: break-word;
  justify-content: space-evenly;
}

.top,
.middle {
  padding-bottom: 10px;
  border-bottom: solid #eee 1px;
}
</style>

<style scoped>
/* order state */

.orderStateContainer {
  padding: 16px;
  font-family: "Segoe UI", sans-serif;
  display: flex;
  justify-content: center;
}
</style>