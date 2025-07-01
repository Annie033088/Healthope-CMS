<template>
  <div>
    <TitleCard text="發票資訊" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <RadioSelector
        class="radioStatus"
        v-model="selectStatus"
        @change="getInvoiceByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="invoiceStatusOptions"
      />
      <RadioSelector
        class="radioCategory"
        v-model="selectCategory"
        @change="getInvoiceByCategory"
        inputTitle="發票分類："
        inputType="radioRefundType"
        :options="invoiceCategoryOptions"
      />
      <RecordSelector :parentValue.sync="recordPerPage" @change="getInvoice" />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="invoiceList"
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
import TableNormal from "@/components/Table/TableNormal";
import PaginationComponent from "@/components/PaginationComponent";
import BtnNormal from "@/components/Btn/BtnNormal";
import {
  electronicInvoiceStatusAndText,
  electronicInvoiceStatus,
  electronicInvoiceCategoryAndText,
  electronicInvoiceCategory,
} from "@/utils/electronicInvoice";
export default {
  name: "HealthopeInvoice",
  components: {
    TitleCard,
    RadioSelector,
    SvgReset,
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
      selectCategory: "",
      recordPerPage: "8",
      invoiceList: [],
      columns: [
        { label: "發票號碼", key: "InvoiceNumber" },
        { label: "隨機碼", key: "RandomNumber" },
        { label: "開立時間", key: "InvoiceTime" },
        { label: "狀態", key: "Status" },
        { label: "總計額", key: "TotalAmount" },
        { label: "分類", key: "Category" },
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
      this.getInvoice();
    },
    getInvoiceByStatus() {
      this.searchingPage = 1;
      this.getInvoice();
    },
    getInvoiceByCategory() {
      this.searchingPage = 1;
      this.getInvoice();
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
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getInvoice();
    },
    async getInvoice() {
      if (!this.validInput()) return;
      // post 的 dto 變數
      let getInvoiceDto = {
        Status: this.selectStatus || null,
        Category: this.selectCategory || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Invoice/GetInvoice",
          getInvoiceDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          this.currentPage = this.searchingPage;
          this.invoiceList = response.data.ApiDataObject.InvoiceList;
          this.totalPage = response.data.ApiDataObject.TotalPage;

          this.invoiceList.forEach((invoice) => {
            electronicInvoiceStatusAndText.forEach((status) => {
              if (Number(status.value) === invoice.Status) {
                invoice.Status = status.text;
              }
            });

            electronicInvoiceCategoryAndText.forEach((category) => {
              if (Number(category.value) === invoice.Category) {
                invoice.Category = category.text;
              }
            });

            invoice.TotalAmount = "$" + invoice.TotalAmount;

            let invoiceTime = "";

            if (invoice.InvoiceTime.split("T")[0] === "1900-01-01") {
              invoiceTime = "-";
            } else {
              invoiceTime = new Date(invoice.InvoiceTime).toLocaleString();
            }

            invoice.InvoiceTime = invoiceTime;
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
        console.error("取得發票列表時發生錯誤", error);
      }
    },
    validInput() {
      // 驗證參數
      if (
        this.selectStatus !== "" &&
        !Object.values(electronicInvoiceStatus).includes(
          Number(this.selectStatus)
        )
      ) {
        return false;
      }
      if (
        this.selectCategory !== "" &&
        !Object.values(electronicInvoiceCategory).includes(
          Number(this.selectCategory)
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
    this.getInvoice();
  },
  computed: {
    invoiceStatusOptions() {
      let options = [...electronicInvoiceStatusAndText];
      options.push({ value: "", text: "全部" });
      return options;
    },
    invoiceCategoryOptions() {
      let options = [...electronicInvoiceCategoryAndText];
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
  width: 700px;
}

.radioCategory {
  width: 400px;
}
</style>