<template>
  <div>
    <TitleCard text="付款紀錄" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <RadioSelector
        class="radioStatus"
        v-model="selectStatus"
        @change="getTransactionByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="transactionStatusOptions"
      />
      <RadioSelector
        class="radioMethod"
        v-model="selectMethod"
        @change="getTransactionByMethod"
        inputTitle="付款方式："
        inputType="radioMethod"
        :options="transactionMethodOptions"
      />
      <SortSelector
        :options="[
          { value: 'amount', label: '金額' },
          { value: 'time', label: '交易時間' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getTransaction"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="setRecordPerPage"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="transactionList"
      :expandable="true"
      :resetDetailIndexFlag="resetDetailIndexFlag"
    >
      <template #detail="{ row }">
        <div class="detailRowContainer">
          <strong>操作：</strong>
          <BtnNormal text="查看訂單" @click="goOrderDetail(row)" />
          <BtnNormal
            v-if="row.Method === '信用卡' && row.Status === '成功'"
            text="查看金流資料"
            @click="checkoutCashFlowData(row)"
          />
          <BtnNormal text="查看會員" @click="goMemberDetail(row)" />
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
import RadioSelector from "@/components/Selector/RadioSelector";
import SvgReset from "@/components/Btn/SvgReset";
import RecordSelector from "@/components/Selector/RecordSelector";
import SortSelector from "@/components/Selector/SortSelector";
import TableNormal from "@/components/Table/TableNormal";
import PaginationComponent from "@/components/PaginationComponent";
import BtnNormal from "@/components/Btn/BtnNormal";
import {
  transactionStatusAndText,
  transactionStatus,
  transactionMethodAndText,
  transactionMethod,
} from "@/utils/transaction";
export default {
  name: "HealthopeTransaction",
  components: {
    TitleCard,
    RadioSelector,
    SvgReset,
    SortSelector,
    RecordSelector,
    TableNormal,
    PaginationComponent,
    BtnNormal,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectStatus: "",
      selectMethod: "",
      selectSortOption: "",
      selectSortOrder: "descending",
      recordPerPage: "8",
      transactionList: [],
      columns: [
        { label: "付款會員", key: "Member" },
        { label: "付款方式", key: "Method" },
        { label: "狀態", key: "Status" },
        { label: "金額", key: "Amount" },
        { label: "交易時間(UTC)", key: "Time" },
      ],
      resetDetailIndexFlag: false,
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
    };
  },
  methods: {
    goMemberDetail(row) {
      if (row.MemberId < 1) return;
      this.$router.push({
        path: "/member/detail",
        query: { id: row.MemberId },
      });
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getTransaction();
    },
    setRecordPerPage() {
      this.searchingPage = 1;
      this.getTransaction();
    },
    getTransactionByStatus() {
      this.searchingPage = 1;
      this.getTransaction();
    },
    getTransactionByMethod() {
      this.searchingPage = 1;
      this.getTransaction();
    },
    resetSearchingRecord() {
      this.selectMethod = "";
      this.selectStatus = "";
      this.selectSortOrder = "descending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getTransaction();
    },
    async checkoutCashFlowData(row) {
      try {
        const transactionIdDto = {
          TransactionId: row.TransactionId,
        };
        // post
        const response = await this.$axios.post(
          "/api/Transaction/GetCreditCardCashFlowData",
          transactionIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle =
            "授權碼：" +
            response.data.ApiDataObject.AuthCode +
            "\n金流平台交易編號：" +
            response.data.ApiDataObject.GatewayTransactionId;
          this.$notificationBox.notificationBoxErrorCode = 0;
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
        console.error("取得金流資料時發生錯誤", error);
      }
    },
    async getTransaction() {
      if (!this.validInput()) return;
      // post 的 dto 變數
      let getTransactionDto = {
        Status: this.selectStatus || null,
        Method: this.selectMethod || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Transaction/GetTransaction",
          getTransactionDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          this.currentPage = this.searchingPage;
          this.transactionList = response.data.ApiDataObject.TransactionList;
          this.totalPage = response.data.ApiDataObject.TotalPage;

          this.transactionList.forEach((transaction) => {
            transaction.Amount = "$" + transaction.Amount;
            transaction.Member = `${transaction.MemberName} (0${transaction.MemberPhone})`;

            transactionStatusAndText.forEach((status) => {
              if (Number(status.value) === transaction.Status) {
                transaction.Status = status.text;
              }
            });

            transactionMethodAndText.forEach((method) => {
              if (Number(method.value) === transaction.Method) {
                transaction.Method = method.text;
              }
            });

            const displayTime = new Date(transaction.Time)
              .toISOString()
              .replace("T", " ")
              .substring(0, 19);

            transaction.Time = displayTime;
          });
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
        console.error("取得付款紀錄列表時發生錯誤", error);
      }
    },
    goOrderDetail(row) {
      if (row.OrderId < 1) return;
      this.$router.push({
        path: "/order/detail",
        query: { id: row.OrderId },
      });
    },
    validInput() {
      // 驗證參數
      if (
        this.selectStatus !== "" &&
        !Object.values(transactionStatus).includes(Number(this.selectStatus))
      ) {
        return false;
      }
      if (
        this.selectMethod !== "" &&
        !Object.values(transactionMethod).includes(Number(this.selectMethod))
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
          this.selectSortOption === "time" ||
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
  },
  created() {
    this.getTransaction();
  },
  computed: {
    transactionStatusOptions() {
      let options = [...transactionStatusAndText];
      options.push({ value: "", text: "全部" });
      return options;
    },
    transactionMethodOptions() {
      let options = [...transactionMethodAndText];
      options.push({ value: "", text: "全部" });
      return options;
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

.radioStatus {
  width: 370px;
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