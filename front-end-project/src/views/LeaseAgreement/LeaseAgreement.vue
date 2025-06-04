<template>
  <div>
    <TitleCard text="場館租約" @refreshPage="$emit('refreshPage')"></TitleCard>
    <div class="functionColumn">
      <BtnNormal
        text="新增租約"
        @click="redirect('/leaseAgreement/add')"
      ></BtnNormal>
      <RadioSelector
        class="statusSelector"
        v-model="selectStatus"
        @change="selectLeaseAgreementByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="leaseAgreementStatusAndTextOptions"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="getLeaseAgreementData"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      class="tableContainer"
      :columns="columns"
      :rows="leaseAgreementList"
      :deleteBtnFlag="permissionMap.EditLeaseAgreement"
      @goDelete="deleteLeaseAgreement"
      @changeStatus="editStatus"
      @changeRemind="editRemind"
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
import RecordSelector from "@/components/Selector/RecordSelector";
import SvgReset from "@/components/Btn/SvgReset";
import TableNormal from "@/components/Table/TableNormal";
import PaginationComponent from "@/components/PaginationComponent";
import {
  leaseAgreementStatusAndText,
  leaseAgreementStatus,
  leaseAgreementStatusTranslateTable,
} from "@/utils/leaseAgreement";

export default {
  name: "LeaseAgreement",
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
      selectStatus: "",
      recordPerPage: "8",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      leaseAgreementList: [{ Remark: "!!" }],
      columns: [
        { label: "租約開始日", key: "StartTime" },
        { label: "租約結束日", key: "EndTime" },
        { label: "提醒前置天數", key: "ReminderLeadTime" },
        {
          label: "狀態",
          key: "Status",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditLeaseAgreement,
        },
        {
          label: "提醒",
          key: "Remind",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditLeaseAgreement,
        },
        { label: "備註", key: "Remark" },
      ],
    };
  },
  methods: {
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    selectLeaseAgreementByStatus() {
      this.searchingPage = 1;
      this.getLeaseAgreementData();
    },
    async getLeaseAgreementData() {
      if (!this.validInput()) return;

      // post 的 dto 變數
      let getLeaseAgreementDto = {
        Status: this.selectStatus || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };
      try {
        // post
        const response = await this.$axios.post(
          "/api/LeaseAgreement/GetLeaseAgreement",
          getLeaseAgreementDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.leaseAgreementList =
            response.data.ApiDataObject.LeaseAgreementList;

          this.leaseAgreementList.forEach((leaseAgreement) => {
            let statusOption = [];

            leaseAgreementStatusAndText.forEach((status) => {
              if (leaseAgreement.Status === Number(status.value))
                statusOption.push(status);

              // 未啟用 => 可以選啟用 (結束日需 > 今日)
              if (
                Number(status.value) === leaseAgreementStatus.Active &&
                leaseAgreement.Status === leaseAgreementStatus.Inactive
              ) {
                if (new Date(leaseAgreement.EndTime) > new Date())
                  statusOption.push(status);
              }

              if (leaseAgreement.Status === leaseAgreementStatus.Active) {
                if (Number(status.value) === leaseAgreementStatus.Completed)
                  statusOption.push(status);

                if (Number(status.value) === leaseAgreementStatus.Cancel)
                  statusOption.push(status);
              }
            });

            leaseAgreement.Status = {
              Value: String(leaseAgreement.Status),
              Options: statusOption,
              OldValue: String(leaseAgreement.Status),
            };

            let remindOption;

            if (leaseAgreement.Remind)
              remindOption = [
                { value: true, text: "開啟" },
                { value: false, text: "關閉" },
              ];
            else remindOption = [{ value: false, text: "關閉" }];

            leaseAgreement.Remind = {
              Value: leaseAgreement.Remind,
              Options: remindOption,
              OldValue: leaseAgreement.Remind,
            };

            leaseAgreement.StartTime = leaseAgreement.StartTime.substring(
              0,
              10
            );
            leaseAgreement.EndTime = leaseAgreement.EndTime.substring(0, 10);

            if (leaseAgreement.Remind.Value)
              leaseAgreement.EndTime = "🔔" + leaseAgreement.EndTime;

            leaseAgreement.ReminderLeadTime += " 天";
          });

          this.totalPage = response.data.ApiDataObject.TotalPage;
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
        console.error("取得租約列表時發生錯誤", error);
      }
    },
    validInput() {
      if (this.selectStatus)
        if (
          !Object.values(leaseAgreementStatus).includes(
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
    deleteLeaseAgreement(row) {
      if (row.Status.Value !== String(leaseAgreementStatus.Inactive)) {
        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "請勿刪除使用中租約及歷史租約";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      // 添加監聽器，查看彈窗是否被按確認鍵
      this.unwatchFlag = this.$watch("notificationBoxConfirmFlag", (newVal) => {
        if (newVal) {
          let redirectRoute = "stop";
          this.$emit("afterConfirmEvent", redirectRoute);

          try {
            this.submitDelLeaseAgreement(row.LeaseAgreementId);
          } catch (error) {
            console.error("刪除租約時發生錯誤", error);
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
    async submitDelLeaseAgreement(id) {
      if (id < 1) return;

      try {
        // post
        let leaseAgreementIdIdDto = {
          LeaseAgreementId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/LeaseAgreement/DeleteLeaseAgreement",
          leaseAgreementIdIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$emit("refreshPage");
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = null;
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
    editStatus(row) {
      if (
        !leaseAgreementStatusTranslateTable(
          row.Status.OldValue,
          row.Status.Value
        )
      ) {
        // 添加監聽器，查看彈窗是否被按確認鍵
        this.unwatchFlag = this.$watch(
          "notificationBoxConfirmFlag",
          (newVal) => {
            if (newVal) {
              let redirectRoute = null;
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
      }

      let editLeaseAgreementStauts = {
        LeaseAgreementId: row.LeaseAgreementId,
        UpdateTime: row.UpdateTime,
        Status: row.Status.Value,
        Remark: null,
      };

      if (Number(row.Status.Value) === leaseAgreementStatus.Cancel) {
        let remarkInput = "";

        while (!remarkInput || remarkInput.length > 50) {
          remarkInput = prompt("請輸入取消備註（不能留空，50 字內）：");
          if (remarkInput === null) {
            this.$emit("refreshPage");
            return; // 取消的話就中斷 function
          }
        }

        editLeaseAgreementStauts.Remark = remarkInput;
        this.submitEditStatus(editLeaseAgreementStauts);
      } else {
        this.submitEditStatus(editLeaseAgreementStauts);
      }
    },
    async submitEditStatus(editLeaseAgreementStauts) {
      try {
        // post
        const response = await this.$axios.post(
          "/api/LeaseAgreement/EditLeaseAgreementStatus",
          editLeaseAgreementStauts
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getLeaseAgreementData();
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = null;
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
    async editRemind(row) {
      console.log(row.Remind.OldValue);
      if (Boolean(row.Remind.OldValue) === false) {
        // 添加監聽器，查看彈窗是否被按確認鍵
        this.unwatchFlag = this.$watch(
          "notificationBoxConfirmFlag",
          (newVal) => {
            if (newVal) {
              let redirectRoute = null;
              this.$emit("afterConfirmEvent", redirectRoute);
              this.unwatchFlag(); // 移除監聽
              this.unwatchFlag = null;
            }
          }
        );

        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "修改了錯誤的提醒狀態!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      let editLeaseAgreementRemind = {
        LeaseAgreementId: row.LeaseAgreementId,
        UpdateTime: row.UpdateTime,
        Remind: row.Remind.Value,
      };
      try {
        // post
        const response = await this.$axios.post(
          "/api/LeaseAgreement/EditLeaseAgreementRemind",
          editLeaseAgreementRemind
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getLeaseAgreementData();
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = null;
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
        console.error("修改提醒狀態時發生錯誤", error);
      }
    },
    resetSearchingRecord() {
      this.selectStatus = "";
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getLeaseAgreementData();
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getLeaseAgreementData();
    },
  },
  computed: {
    leaseAgreementStatusAndTextOptions() {
      let leaseAgreementStatusAndTextOptions = [...leaseAgreementStatusAndText];
      leaseAgreementStatusAndTextOptions.push({ value: "", text: "全部" });
      return leaseAgreementStatusAndTextOptions;
    },
  },
  created() {
    this.getLeaseAgreementData();
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
  width: 450px;
}
</style>