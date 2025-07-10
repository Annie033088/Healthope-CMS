<template>
  <div>
    <TitleCard text="退款與違約金" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <RadioSelector
        class="radioStatus"
        v-model="selectStatus"
        @change="getRefundByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="refundStatusOptions"
      />
      <RadioSelector
        class="radioRefundType"
        v-model="selectRefundType"
        @change="getRefundByRefundType"
        inputTitle="付款方式："
        inputType="radioRefundType"
        :options="refundTypeOptions"
      />
      <SortSelector
        :options="[
          { value: 'status', label: '狀態' },
          { value: 'createTime', label: '生成時間' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getRefund"
      />
      <RecordSelector :parentValue.sync="recordPerPage" @change="getRefund" />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="refundList"
      :expandable="true"
      :resetDetailIndexFlag="resetDetailIndexFlag"
    >
      <template #detail="{ row }">
        <div class="detailRowContainer">
          <strong>操作：</strong>
          <BtnNormal text="查看訂單" @click="goOrderDetail(row)" />
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
  refundStatusAndText,
  refundStatus,
  refundTypeAndText,
  refundType,
} from "@/utils/refund";
export default {
  name: "HealthopeRefund",
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
      selectRefundType: "",
      selectSortOption: "",
      selectSortOrder: "descending",
      recordPerPage: "8",
      refundList: [],
      columns: [
        { label: "退款類型", key: "RefundType" },
        { label: "狀態", key: "Status" },
        { label: "原始應退", key: "RefundAmount" },
        { label: "違約金", key: "PenaltyAmount" },
        { label: "淨退額", key: "NetRefund" },
        { label: "退費時間", key: "LocalCreateTime" },
      ],
      resetDetailIndexFlag: false,
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
    };
  },
  methods: {
    searchPage(page) {
      this.searchingPage = page;
      this.getRefund();
    },
    getRefundByStatus() {
      this.searchingPage = 1;
      this.getRefund();
    },
    getRefundByRefundType() {
      this.searchingPage = 1;
      this.getRefund();
    },
    goOrderDetail(row) {
      if (row.OrderId < 1) return;
      this.$router.push({
        path: "/order/detail",
        query: { id: row.OrderId },
      });
    },
    resetSearchingRecord() {
      this.selectRefundType = "";
      this.selectStatus = "";
      this.selectSortOrder = "ascending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getRefund();
    },
    async getRefund() {
      if (!this.validInput()) return;
      // post 的 dto 變數
      let getRefundDto = {
        Status: this.selectStatus || null,
        RefundType: this.selectRefundType || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Refund/GetRefund",
          getRefundDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          this.currentPage = this.searchingPage;
          this.refundList = response.data.ApiDataObject.RefundList;
          this.totalPage = response.data.ApiDataObject.TotalPage;

          this.refundList.forEach((refund) => {
            refundStatusAndText.forEach((status) => {
              if (Number(status.value) === refund.Status) {
                refund.Status = status.text;
              }
            });

            refundTypeAndText.forEach((type) => {
              if (Number(type.value) === refund.RefundType) {
                refund.RefundType = type.text;
              }
            });

            refund.NetRefund = refund.RefundAmount - refund.PenaltyAmount;
            refund.NetRefund = "$" + refund.NetRefund;
            refund.RefundAmount = "$" + refund.RefundAmount;
            refund.PenaltyAmount =
              refund.PenaltyAmount === 0 ? "-" : "$" + refund.PenaltyAmount;

            const localCreateTime = new Date(
              refund.CreateTime + "Z"
            ).toLocaleString();

            refund.LocalCreateTime = localCreateTime;
          });
        } else {
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
        console.error("取得退款與違約金紀錄列表時發生錯誤", error);
      }
    },
    validInput() {
      // 驗證參數
      if (
        this.selectStatus !== "" &&
        !Object.values(refundStatus).includes(Number(this.selectStatus))
      ) {
        return false;
      }
      if (
        this.selectRefundType !== "" &&
        !Object.values(refundType).includes(Number(this.selectRefundType))
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
          this.selectSortOption === "status" ||
          this.selectSortOption === "createTime" ||
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
    this.getRefund();
  },
  computed: {
    refundStatusOptions() {
      let options = [...refundStatusAndText];
      options.push({ value: "", text: "全部" });
      return options;
    },
    refundTypeOptions() {
      let options = [...refundTypeAndText];
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
  width: 400px;
}

.radioRefundType {
  width: 400px;
}
</style>