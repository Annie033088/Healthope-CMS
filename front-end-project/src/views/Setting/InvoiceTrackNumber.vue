<template>
  <div>
    <TitleCard text="發票字軌" @refreshPage="$emit('refreshPage')"></TitleCard>
    <div class="functionColumn">
      <BtnNormal
        text="新增字軌"
        @click="redirect('/setting/invoiceTrackNumber/add')"
        v-if="permissionMap.EditOrder"
      ></BtnNormal>
      <RadioSelector
        class="timeSelector"
        v-model="selectTime"
        @change="selectInvoiceTrackNumberByTime"
        inputTitle="字軌是否過期："
        inputType="radioTime"
        :options="[
          { value: 'false', text: '過期' },
          { value: 'true', text: '未過期' },
          { value: '', text: '全部' },
        ]"
      />
      <RadioSelector
        class="statusSelector"
        v-model="selectStatus"
        @change="selectInvoiceTrackNumberByStatus"
        inputTitle="字軌狀態："
        inputType="radioStatus"
        :options="invoiceTrackNumberStatusAndTextOptions"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="getInvoiceTrackNumberData"
      />
      <SvgReset @click="setRecordPerPage"></SvgReset>
    </div>
    <TableNormal
      class="tableContainer"
      :columns="columns"
      :rows="invoiceTrackNumberList"
      :deleteBtnFlag="permissionMap.EditOrder"
      @goDelete="deleteInvoiceTrackNumber"
      @changeStatus="editStatus"
    >
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
import BtnNormal from "@/components/Btn/BtnNormal";
import RadioSelector from "@/components/Selector/RadioSelector";
import {
  invoiceTrackNumberStatus,
  invoiceTrackNumberStatusAndText,
  invoiceTrackNumberStatusTranslateTable,
} from "@/utils/invoiceTrackNumber";
import RecordSelector from "@/components/Selector/RecordSelector";
import SvgReset from "@/components/Btn/SvgReset";
import TableNormal from "@/components/Table/TableNormal";
import PaginationComponent from "@/components/PaginationComponent";

export default {
  name: "InvoiceTrackNumber",
  components: {
    TitleCard,
    BtnNormal,
    RadioSelector,
    RecordSelector,
    SvgReset,
    TableNormal,
    PaginationComponent,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      startTime: "",
      endTime: "",
      reminderLeadTime: "",
      verifyFail: false,
      hintText: "",
      selectTime: "",
      selectStatus: "",
      recordPerPage: "8",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      columns: [
        { label: "期數", key: "InvoicePeriodText" },
        { label: "字軌前二碼", key: "TrackPrefix" },
        { label: "起始碼", key: "StartNumber" },
        { label: "結束碼", key: "EndNumber" },
        { label: "已使用號碼", key: "CurrentNumber" },
        {
          label: "狀態",
          key: "Status",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditOrder,
        },
      ],
      invoiceTrackNumberList: [],
    };
  },
  methods: {
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    async getInvoiceTrackNumberData() {
      if (!this.validInput()) return;

      let time = this.selectTime || null;

      if (time) time === "true" ? true : false;

      // post 的 dto 變數
      let getInvoiceTrackNumberDto = {
        Status: this.selectStatus || null,
        Time: time,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };
      try {
        // post
        const response = await this.$axios.post(
          "/api/Invoice/GetInvoiceTrackNumber",
          getInvoiceTrackNumberDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.invoiceTrackNumberList =
            response.data.ApiDataObject.InvoiceTrackNumberList;

          this.invoiceTrackNumberList.forEach((invoiceTrackNumber) => {
            invoiceTrackNumber.InvoicePeriodText = this.formatInvoicePeriod(
              invoiceTrackNumber.InvoicePeriod
            );

            let statusOption = [];

            invoiceTrackNumberStatusAndText.forEach((status) => {
              if (invoiceTrackNumber.Status === Number(status.value))
                statusOption.push(status);

              if (
                Number(status.value) === invoiceTrackNumberStatus.Active &&
                invoiceTrackNumber.Status === invoiceTrackNumberStatus.Inactive
              )
                statusOption.push(status);

              if (
                Number(status.value) === invoiceTrackNumberStatus.Disabled &&
                invoiceTrackNumber.Status === invoiceTrackNumberStatus.Active
              )
                statusOption.push(status);
            });

            invoiceTrackNumber.Status = {
              Value: String(invoiceTrackNumber.Status),
              Options: statusOption,
              OldValue: String(invoiceTrackNumber.Status),
            };

            if (
              invoiceTrackNumber.CurrentNumber < invoiceTrackNumber.StartNumber
            ) {
              invoiceTrackNumber.CurrentNumber = "無";
            }
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
        console.error("取得字軌列表時發生錯誤", error);
      }
    },
    validInput() {
      if (this.selectTime)
        if (this.selectTime !== "false" && this.selectTime !== "true")
          return false;

      if (this.selectStatus)
        if (
          !Object.values(invoiceTrackNumberStatus).includes(
            Number(this.selectStatus)
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
    setRecordPerPage() {
      this.searchingPage = 1;
      this.getInvoiceTrackNumberData();
    },
    selectInvoiceTrackNumberByTime() {
      this.searchingPage = 1;
      this.getInvoiceTrackNumberData();
    },
    selectInvoiceTrackNumberByStatus() {
      this.searchingPage = 1;
      this.getInvoiceTrackNumberData();
    },
    resetSearchingRecord() {
      this.selectStatus = "";
      this.selectTime = "";
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getInvoiceTrackNumberData();
    },
    editStatus(row) {
      if (
        !invoiceTrackNumberStatusTranslateTable(
          row.Status.OldValue,
          row.Status.Value
        )
      ) {
        if (this.unwatchFlag) {
          this.unwatchFlag(); // 移除監聽
          this.unwatchFlag = null;
        }
        // 添加監聽器，查看彈窗是否被按確認鍵
        this.unwatchFlag = this.$watch(
          "notificationBoxConfirmFlag",
          (newVal) => {
            if (newVal) {
              this.getInvoiceTrackNumberData();
              let redirectRoute = "stop";
              this.$emit("afterConfirmEvent", redirectRoute);
              this.unwatchFlag(); // 移除監聽
              this.unwatchFlag = null;
            }
          }
        );

        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "修改了錯誤的狀態!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      if (
        this.compareTerm(row.InvoicePeriod) < 0 &&
        row.Status.Value === String(invoiceTrackNumberStatus.Active)
      ) {
        if (this.unwatchFlag) {
          this.unwatchFlag(); // 移除監聽
          this.unwatchFlag = null;
        }
        // 添加監聽器，查看彈窗是否被按確認鍵
        this.unwatchFlag = this.$watch(
          "notificationBoxConfirmFlag",
          (newVal) => {
            if (newVal) {
              this.getInvoiceTrackNumberData();
              let redirectRoute = "stop";
              this.$emit("afterConfirmEvent", redirectRoute);
              this.unwatchFlag(); // 移除監聽
              this.unwatchFlag = null;
            }
          }
        );

        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "請勿啟用過期字軌!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      let editInvoiceTrackNumberStauts = {
        InvoiceTrackNumberId: row.InvoiceTrackNumberId,
        UpdateTime: row.UpdateTime,
        Status: row.Status.Value,
      };
      this.submitEditStatus(editInvoiceTrackNumberStauts);
    },
    async submitEditStatus(editInvoiceTrackNumberStauts) {
      try {
        // post
        const response = await this.$axios.post(
          "/api/Invoice/EditInvoiceTrackNumberStatus",
          editInvoiceTrackNumberStauts
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getInvoiceTrackNumberData();
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
                this.getInvoiceTrackNumberData();
                let redirectRoute = "stop";
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
        console.error("修改狀態時發生錯誤", error);
      }
    },
    deleteInvoiceTrackNumber(row) {
      if (row.Status.Value !== String(invoiceTrackNumberStatus.Inactive)) {
        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "請勿刪除使用中字軌及歷史字軌!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      if (this.unwatchFlag) {
        this.unwatchFlag(); // 確保監聽被移除
        this.unwatchFlag = null;
      }

      // 添加監聽器，查看彈窗是否被按確認鍵
      this.unwatchFlag = this.$watch("notificationBoxConfirmFlag", (newVal) => {
        if (newVal) {
          try {
            this.submitDelInvoiceTrackNumber(row.InvoiceTrackNumberId);
          } catch (error) {
            console.error("刪除時發生錯誤", error);
          } finally {
            this.unwatchFlag(); // 確保監聽被移除
            this.unwatchFlag = null;
          }
        }
      });

      // 設定彈窗資料
      this.$notificationBox.notificationBoxFlag = true;
      this.$notificationBox.notificationBoxTitle = "此操作不可修改，確認刪除?";
      this.$notificationBox.notificationBoxCancelFlag = true;
      this.$notificationBox.notificationBoxErrorCode = 0;
    },
    async submitDelInvoiceTrackNumber(id) {
      if (id < 1) return;

      try {
        // post
        let invoiceTrackNumberIdDto = {
          InvoiceTrackNumberId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Invoice/DeleteInvoiceTrackNumber",
          invoiceTrackNumberIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$emit("refreshPage");
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
                this.getInvoiceTrackNumberData();
                let redirectRoute = "stop";
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
        console.error("刪除失敗", error);
      }
    },
    formatInvoicePeriod(periodNumber) {
      const period = periodNumber % 10;
      const startMonth = (period - 1) * 2 + 1;
      const endMonth = startMonth + 1;

      return `${Math.floor(periodNumber / 10)} 年 ${startMonth
        .toString()
        .padStart(2, "0")}-${endMonth.toString().padStart(2, "0")} 月`;
    },
    compareTerm(term) {
      const now = new Date();
      const rocYear = now.getFullYear() - 1911;
      const month = now.getMonth() + 1;

      // 發票期數從 1~6，每 2 個月為一個期
      const period = Math.ceil(month / 2);
      const nowTerm = rocYear * 10 + period;
      const termParse = this.parseTerm(term);
      const nowTermParse = this.parseTerm(nowTerm);

      if (termParse.year !== nowTermParse.year) {
        return termParse.year - nowTermParse.year;
      } else {
        return termParse.period - nowTermParse.period;
      }
    },
    parseTerm(term) {
      return {
        year: Math.floor(term / 10),
        period: term % 10,
      };
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getInvoiceTrackNumberData();
    },
  },
  computed: {
    invoiceTrackNumberStatusAndTextOptions() {
      let invoiceTrackNumberStatusAndTextOptions = [
        ...invoiceTrackNumberStatusAndText,
      ];
      invoiceTrackNumberStatusAndTextOptions.push({ value: "", text: "全部" });
      return invoiceTrackNumberStatusAndTextOptions;
    },
  },
  created() {
    this.getInvoiceTrackNumberData();
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

.statusSelector {
  width: 500px;
}

.timeSelector {
  width: 350px;
}
</style>