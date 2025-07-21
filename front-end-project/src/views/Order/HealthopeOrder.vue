<template>
  <div>
    <TitleCard text="訂單" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <BtnNormal
        text="新增訂單"
        @click="redirect('/order/add')"
        v-if="permissionMap.EditOrder || permissionMap.AddOrder"
      ></BtnNormal>
      <RadioSelector
        class="radioState"
        v-model="selectState"
        @change="getOrderByState"
        inputTitle="狀態："
        inputType="radioState"
        :options="orderStateOptions"
      />
      <RadioSelector
        class="radioMethod"
        v-model="selectMethod"
        @change="getOrderByMethod"
        inputTitle="付款方式："
        inputType="radioMethod"
        :options="paymentMethodOptions"
      />
      <SortSelector
        :options="[
          { value: 'amount', label: '金額' },
          { value: 'state', label: '狀態' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getOrder"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="setRecordPerPage"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="orderList"
      :checkDetailBtnFlag="true"
      :expandable="true"
      :resetDetailIndexFlag="resetDetailIndexFlag"
      @goCheckDetail="goDetail"
      @changeState="editState"
      @changeInvoiceStatus="editInvoiceStatus"
    >
      <template #detail="{ row }">
        <div class="detailRowContainer">
          <strong>操作：{{ row.Member }}</strong>
          <BtnNormal
            :disabled="!permissionMap.SelectMember && !permissionMap.EditMember"
            text="查看會員"
            @click="goMemberDetail(row)"
          />
          <BtnNormal
            :disabled="!(permissionMap.EditOrder || permissionMap.AddOrder)"
            v-if="Number(row.State.Value) === orderState.Pending"
            text="付款"
            @click="goCheckoutOrder(row)"
          />
          <BtnNormal
            :disabled="!permissionMap.EditOrder"
            v-if="row.State.Value === String(orderState.Paid)"
            text="7 日內無條件退款"
            @click="refundIn7Days(row)"
          />
          <BtnNormal
            :disabled="!permissionMap.EditOrder"
            v-if="
              row.State.Value === String(orderState.Paid) &&
              row.PlanType !== planType.Ticket
            "
            text="解約"
            @click="checkoutRefundQualifyAndTerminateOrder(row)"
          />
          <BtnNormal
            :disabled="!permissionMap.EditOrder"
            v-if="
              row.State.Value === String(orderState.Paid) &&
              row.PlanType !== planType.Ticket
            "
            text="違約"
            @click="checkoutRefundQualifyAndBreachOrder(row)"
          />
          <BtnNormal
            :disabled="!(permissionMap.EditOrder || permissionMap.AddOrder)"
            v-if="
              (row.InvoiceStatus.Value ===
                String(electronicInvoiceStatus.Fail) ||
                row.InvoiceStatus.Value === '') &&
              row.State.Value === String(orderState.Paid)
            "
            text="補印發票"
            @click="printInvoice(row)"
          />
        </div>
        <div class="detailRowContainer">
          <strong>備註：</strong> {{ row.Remark ? row.Remark : "無" }}
          <SvgEdit @click="editRemark(row)" v-if="permissionMap.EditOrder" />
        </div>
      </template>
    </TableNormal>
    <div>
      <PaginationComponent
        @searchPage="searchPage"
        :currentPage="currentPage"
        :totalPage="totalPage"
      ></PaginationComponent>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import {
  orderStateAndText,
  paymentMethodAndText,
  orderState,
  paymentMethod,
  orderCache,
  planType,
} from "@/utils/order";
import {
  electronicInvoiceStatusAndText,
  electronicInvoiceStatus,
  electronicInvoiceCategory,
} from "@/utils/electronicInvoice";
import BtnNormal from "@/components/Btn/BtnNormal";
import RadioSelector from "@/components/Selector/RadioSelector";
import TableNormal from "@/components/Table/TableNormal";
import SortSelector from "@/components/Selector/SortSelector";
import PaginationComponent from "@/components/PaginationComponent";
import RecordSelector from "@/components/Selector/RecordSelector";
import SvgReset from "@/components/Btn/SvgReset";
import SvgEdit from "@/components/Btn/SvgEdit";

export default {
  name: "HealthopeOrder",
  components: {
    TitleCard,
    BtnNormal,
    RadioSelector,
    TableNormal,
    SortSelector,
    PaginationComponent,
    RecordSelector,
    SvgReset,
    SvgEdit,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectState: "",
      selectMethod: "",
      selectSortOption: "",
      selectSortOrder: "descending",
      recordPerPage: "8",
      orderList: [],
      columns: [
        { label: "訂單編號", key: "OrderNumber" },
        { label: "會員", key: "Member" },
        { label: "方案", key: "PlanName" },
        {
          label: "訂單狀態",
          key: "State",
          type: "dropDownSelector",
          enableFlag:
            this.permissionMap.EditOrder || this.permissionMap.AddOrder,
        },
        { label: "金額", key: "Amount" },
        { label: "付款方式", key: "Method" },
        {
          label: "發票狀態",
          key: "InvoiceStatus",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditOrder,
        },
      ],
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      resetDetailIndexFlag: false,
    };
  },
  methods: {
    setRecordPerPage() {
      this.searchingPage = 1;
      this.getOrder();
    },
    getOrderByState() {
      this.searchingPage = 1;
      this.getOrder();
    },
    getOrderByMethod() {
      this.searchingPage = 1;
      this.getOrder();
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getOrder();
    },
    async editRemark(row) {
      let remarkInput = "";

      while (!remarkInput || remarkInput.length > 50) {
        remarkInput = prompt("請輸入備註（不能留空，50 字內）：");
        if (remarkInput === null) {
          return; // 取消的話就中斷 function
        }
      }

      // 沒修改的話就中斷 function
      if (remarkInput === row.Remark) return;

      let editOrderRemarkDto = {
        OrderId: row.OrderId,
        Remark: remarkInput,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/EditOrderRemark",
          editOrderRemarkDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async printInvoice(row) {
      try {
        const printInvoiceDto = {
          Category: electronicInvoiceCategory.Main,
          OrderId: row.OrderId,
        };
        // post
        const response = await this.$axios.post(
          "/api/Invoice/PrintInvoice",
          printInvoiceDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("補印發票時發生錯誤", error);
      }
    },
    goDetail(row) {
      if (row.OrderId < 1) return;
      this.$router.push({
        path: "/order/detail",
        query: { id: row.OrderId },
      });
    },
    redirect(path) {
      this.$router.push(path);
    },
    goCheckoutOrder(row) {
      // 付款方式是 現金:1 ; 信用卡:2
      // 若是教練方案還需選擇教練
      if (row.Method === "現金") row.Method = "1";
      if (row.Method === "信用卡") row.Method = "2";

      orderCache.tempOrder = {
        OrderId: row.OrderId,
        UpdateTime: row.UpdateTime,
        MemberName: row.MemberName,
        MemberPhone: row.MemberPhone,
        PlanName: row.PlanName,
        PaymentMethod: row.Method,
        PlanType: row.PlanType,
        Amount: row.Amount,
      };

      if (row.Method === "2" || row.PlanType === 2) {
        this.$router.push({
          name: "HealthopeBerforeCheckout",
        });
      } else {
        this.$router.push({
          name: "HealthopeCheckoutOrder",
        });
      }
    },
    goMemberDetail(row) {
      if (row.MemberId < 1) return;
      this.$router.push({
        path: "/member/detail",
        query: { id: row.MemberId },
      });
    },
    async getOrder() {
      if (!this.validInput()) return;
      // post 的 dto 變數
      let getOrderDto = {
        State: this.selectState || null,
        Method: this.selectMethod || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };
      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/GetOrder",
          getOrderDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          this.currentPage = this.searchingPage;
          this.orderList = response.data.ApiDataObject.OrderList;

          this.orderList.forEach((order) => {
            let stateOption = [];

            orderStateAndText.forEach((state) => {
              if (order.State === Number(state.value)) stateOption.push(state);

              if (
                Number(state.value) === orderState.Cancel &&
                order.State === orderState.Pending
              )
                stateOption.push(state);

              if (
                Number(state.value) === orderState.Terminate &&
                order.State === orderState.Paid &&
                order.PlanType !== 3 // 票劵方案
              )
                stateOption.push(state);

              if (
                Number(state.value) === orderState.Breach &&
                order.State === orderState.Paid &&
                order.PlanType !== 3 // 票劵方案
              )
                stateOption.push(state);

              if (
                Number(state.value) === orderState.RefundIn7Days &&
                order.State === orderState.Paid
              )
                stateOption.push(state);
            });

            order.State = {
              Value: String(order.State),
              Options: stateOption,
              OldValue: String(order.State),
            };

            let method = paymentMethodAndText.find(
              (item) => item.value === String(order.Method)
            );
            order.Method = method.text;
            order.Member = order.MemberName + `(0${order.MemberPhone})`;
            order.Amount = "$" + order.Amount;
            let invoiceStatusOption = [];

            if (!order.InvoiceStatus) {
              invoiceStatusOption.push({ value: "", text: "無" });
            }

            electronicInvoiceStatusAndText.forEach((status) => {
              if (order.InvoiceStatus === Number(status.value)) {
                invoiceStatusOption.push(status);
              }

              if (
                order.InvoiceStatus === electronicInvoiceStatus.PendingVoid &&
                Number(status.value) === electronicInvoiceStatus.Voided
              ) {
                invoiceStatusOption.push(status);
              }

              if (
                order.InvoiceStatus ===
                  electronicInvoiceStatus.PendingDiscount &&
                Number(status.value) === electronicInvoiceStatus.Discounted
              ) {
                invoiceStatusOption.push(status);
              }
            });

            order.InvoiceStatus = {
              Value: order.InvoiceStatus ? String(order.InvoiceStatus) : "",
              Options: invoiceStatusOption,
              OldValue: order.InvoiceStatus ? String(order.InvoiceStatus) : "",
            };
          });

          this.totalPage = response.data.ApiDataObject.TotalPage;
        } else {
          if (this.unwatchFlag) {
            this.unwatchFlag(); // 確保監聽被移除
            this.unwatchFlag = null;
          }

          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/";
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
        console.error("取得訂單列表時發生錯誤", error);
      }
    },
    validInput() {
      // 驗證參數
      if (
        this.selectState !== "" &&
        !Object.values(orderState).includes(Number(this.selectState))
      ) {
        return false;
      }

      if (
        this.selectMethod !== "" &&
        !Object.values(paymentMethod).includes(Number(this.selectMethod))
      )
        return false;

      if (
        !(
          this.selectSortOrder === "ascending" ||
          this.selectSortOrder === "descending"
        )
      )
        return false;
      if (
        !(
          this.selectSortOption === "amount" ||
          this.selectSortOption === "state" ||
          this.selectSortOption === ""
        )
      )
        return false;
      if (
        !(
          this.recordPerPage === "8" ||
          this.recordPerPage === "12" ||
          this.recordPerPage === "16"
        )
      )
        return false;

      const IntMax = 2147483647;
      let searchingPage = Number(this.searchingPage);
      if (
        !Number.isInteger(searchingPage) ||
        searchingPage < 1 ||
        // 超出安全整數範圍
        searchingPage > IntMax
      )
        return false;

      return true;
    },
    resetSearchingRecord() {
      this.selectMethod = "";
      this.selectState = "";
      this.selectSortOrder = "descending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getOrder();
    },
    editState(row) {
      // 待付款 => 取消
      if (
        row.State.OldValue === String(orderState.Pending) &&
        row.State.Value === String(orderState.Cancel)
      ) {
        let ediOrderStateDto = {
          OrderId: row.OrderId,
          UpdateTime: row.UpdateTime,
        };
        this.cancelPendingOrder(ediOrderStateDto);
        return;
      }

      // 設定彈窗資料
      this.$notificationBox.notificationBoxFlag = true;
      this.$notificationBox.notificationBoxTitle = "發生錯誤!無效的狀態轉換";
      this.$notificationBox.notificationBoxErrorCode = 0;
    },
    editInvoiceStatus(row) {
      // 確認轉換狀態 待作廢 => 作廢
      if (
        row.InvoiceStatus.Value === String(electronicInvoiceStatus.Voided) &&
        row.InvoiceStatus.OldValue ===
          String(electronicInvoiceStatus.PendingVoid)
      ) {
        let editInvoiceStatusDto = {
          Category: electronicInvoiceCategory.Main,
          UpdateTime: row.InvoiceUpdateTime,
          OrderId: row.OrderId,
        };
        this.voidInvoice(editInvoiceStatusDto);
      } else if (
        row.InvoiceStatus.Value ===
          String(electronicInvoiceStatus.Discounted) &&
        row.InvoiceStatus.OldValue ===
          String(electronicInvoiceStatus.PendingDiscount)
      ) {
        let editInvoiceStatusDto = {
          Category: electronicInvoiceCategory.Main,
          UpdateTime: row.InvoiceUpdateTime,
          OrderId: row.OrderId,
        };
        this.discountInvoice(editInvoiceStatusDto);
      }
    },
    async voidInvoice(editInvoiceStatusDto) {
      try {
        // post
        const response = await this.$axios.post(
          "/api/Invoice/VoidInvoice",
          editInvoiceStatusDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async discountInvoice(editInvoiceStatusDto) {
      try {
        // post
        const response = await this.$axios.post(
          "/api/Invoice/DiscountInvoice",
          editInvoiceStatusDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async cancelPendingOrder(ediOrderStateDto) {
      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/CancelPendingOrder",
          ediOrderStateDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async checkoutRefundQualifyAndTerminateOrder(row) {
      let ediOrderStateDto = {
        OrderId: row.OrderId,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/CheckoutRefundQualifyAndTerminateOrder",
          ediOrderStateDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
          let invoiceNumber = response.data.ApiDataObject.InvoiceNumber;

          if (invoiceNumber) {
            // 設定彈窗資料
            this.$notificationBox.notificationBoxFlag = true;
            this.$notificationBox.notificationBoxTitle =
              "字軌號碼為：" + invoiceNumber;
            this.$notificationBox.notificationBoxErrorCode = 0;
          }
        } else if (
          response.data.ErrorCode === this.$errorCodeDefine.ConfirmAgain
        ) {
          if (this.unwatchFlag) {
            this.unwatchFlag(); // 確保監聽被移除
            this.unwatchFlag = null;
          }

          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "stop";
                this.$emit("afterConfirmEvent", redirectRoute);
                try {
                  this.terminateOrder(ediOrderStateDto);
                } catch (error) {
                  console.error("修改訂單狀態時發生錯誤", error);
                } finally {
                  this.unwatchFlag(); // 確保監聽被移除
                  this.unwatchFlag = null;
                }
              }
            }
          );

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxCancelFlag = true;
          this.$notificationBox.notificationBoxTitle =
            "這筆訂單因尚未使用，且符合 7 日內條件，將全額退費。請確認是否要繼續執行解約？";
          this.$notificationBox.notificationBoxErrorCode = 0;
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async terminateOrder(ediOrderStateDto) {
      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/TerminateOrder",
          ediOrderStateDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
          let invoiceNumber = response.data.ApiDataObject.InvoiceNumber;

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle =
            "字軌號碼為：" + invoiceNumber;
          this.$notificationBox.notificationBoxErrorCode = 0;
        } else {
          this.getOrder();
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async checkoutRefundQualifyAndBreachOrder(row) {
      let ediOrderStateDto = {
        OrderId: row.OrderId,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/CheckoutRefundQualifyAndBreachOrder",
          ediOrderStateDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
          let invoiceNumber = response.data.ApiDataObject.InvoiceNumber;

          if (invoiceNumber) {
            // 設定彈窗資料
            this.$notificationBox.notificationBoxFlag = true;
            this.$notificationBox.notificationBoxTitle =
              "字軌號碼為：" + invoiceNumber;
            this.$notificationBox.notificationBoxErrorCode = 0;
          }
        } else if (
          response.data.ErrorCode === this.$errorCodeDefine.ConfirmAgain
        ) {
          if (this.unwatchFlag) {
            this.unwatchFlag(); // 確保監聽被移除
            this.unwatchFlag = null;
          }

          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "stop";
                this.$emit("afterConfirmEvent", redirectRoute);

                try {
                  this.breachOrder(ediOrderStateDto);
                } catch (error) {
                  console.error("修改訂單狀態時發生錯誤", error);
                } finally {
                  this.unwatchFlag(); // 確保監聽被移除
                  this.unwatchFlag = null;
                }
              }
            }
          );

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxCancelFlag = true;
          this.$notificationBox.notificationBoxTitle =
            "這筆訂單因尚未使用，且符合 7 日內條件，將全額退費。請確認是否要繼續執行設置違約？";
          this.$notificationBox.notificationBoxErrorCode = 0;
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async breachOrder(row) {
      let ediOrderStateDto = {
        OrderId: row.OrderId,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/BreachOrder",
          ediOrderStateDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
        } else {
          this.getOrder();

          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
    async refundIn7Days(row) {
      let ediOrderStateDto = {
        OrderId: row.OrderId,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/RefundIn7Days",
          ediOrderStateDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getOrder();
          let invoiceNumber = response.data.ApiDataObject.InvoiceNumber;

          if (invoiceNumber) {
            // 設定彈窗資料
            this.$notificationBox.notificationBoxFlag = true;
            this.$notificationBox.notificationBoxTitle =
              "字軌號碼為：" + invoiceNumber;
            this.$notificationBox.notificationBoxErrorCode = 0;
          }
        } else {
          this.getOrder();
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("修改備註時發生錯誤", error);
      }
    },
  },
  created() {
    this.getOrder();
  },
  computed: {
    orderStateOptions() {
      let options = [...orderStateAndText];
      options.push({ value: "", text: "全部" });
      return options;
    },
    paymentMethodOptions() {
      let options = [...paymentMethodAndText];
      options.push({ value: "", text: "全部" });
      return options;
    },
    orderState() {
      return orderState;
    },
    electronicInvoiceStatus() {
      return electronicInvoiceStatus;
    },
    planType() {
      return planType;
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

.radioState {
  width: 650px;
}

.radioMethod {
  width: 350px;
}

.detailRowContainer {
  display: flex;
  align-items: center;
  gap: 5px;
}
</style>