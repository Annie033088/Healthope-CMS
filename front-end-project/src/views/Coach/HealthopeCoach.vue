<template>
  <div>
    <TitleCard text="教練清單" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <BtnNormal text="新增教練" @click="redirect('/coach/add')"></BtnNormal>
      <SelectInput
        placeholder="Name..."
        v-model="searchName"
        @select="selectCoachByName"
      ></SelectInput>
      <SelectInput
        placeholder="手機末三碼..."
        v-model="searchPhone"
        @select="selectCoachByPhone"
      ></SelectInput>
      <StatusSelector v-model="selectStatus" @change="selectCoachByStatus" />
      <SortSelector
        :options="[
          { value: 'name', label: '姓名' },
          { value: 'contractEndTime', label: '合約到期' },
          { value: 'status', label: '狀態' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getCoachData"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="getCoachData"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="coachList"
      :expandable="true"
      :editBtnFlag="true"
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
import TableNormal from "@/components/Table/TableNormal.vue";
import BtnNormal from "@/components/Btn/BtnNormal";
import SelectInput from "@/components/Input/SelectInput";
import SortSelector from "@/components/Selector/SortSelector";
import RecordSelector from "@/components/Selector/RecordSelector";
import StatusSelector from "@/components/Selector/StatusSelector";
import PaginationComponent from "@/components/PaginationComponent";
import SvgReset from "@/components/Btn/SvgReset";

export default {
  name: "HealthopeCoach",
  components: {
    TitleCard,
    SelectInput,
    BtnNormal,
    SortSelector,
    RecordSelector,
    StatusSelector,
    SvgReset,
    TableNormal,
    PaginationComponent,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      searchName: "",
      searchPhone: "",
      selectStatus: "",
      selectSortOrder: "ascending",
      selectSortOption: "",
      recordPerPage: "8",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      columns: [
        { label: "姓名", key: "Name" },
        { label: "手機", key: "Phone" },
        { label: "教練分類", key: "Type" },
        { label: "合約開始日", key: "ContractStartTime" },
        { label: "合約到期日", key: "ContractEndTime" },
        { label: "狀態", key: "Status" },
      ],
      coachList: [],
      //   resetDetailIndexFlag: false,
    };
  },
  methods: {
    searchPage(page) {
      this.searchingPage = page;
      this.getCoachData();
    },
    redirect(path) {
      this.$router.push(path);
    },
    selectCoachByStatus() {
      this.searchingPage = 1;
      this.getCoachData();
    },
    selectCoachByPhone() {
      this.searchingPage = 1;
      this.searchPhone = this.searchPhone.trim();
      if (this.searchPhone === "") {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "搜尋不得為空";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      this.getCoachData();
    },
    selectCoachByName() {
      this.searchingPage = 1;
      this.searchName = this.searchName.trim();
      if (this.searchName === "") {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "搜尋不得為空";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      this.getCoachData();
    },
    resetSearchingRecord() {
      this.selectStatus = "";
      this.selectSortOrder = "ascending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.searchPhone = "";
      this.searchName = "";
      this.searchingPage = 1;
      this.getCoachData();
    },
   async getCoachData() {
      // 驗證參數
      if (isNaN(this.searchPhone)) {
        this.searchPhone = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入長度需為 3 位數字";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      if (!(this.searchPhone.length === 3 || this.searchPhone === "")) {
        this.searchPhone = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入長度需為 3 位數字";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      if (this.searchName.length > 15) {
        this.searchName = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入長度不得超過 15 位數";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      if (
        !(
          this.selectStatus === "true" ||
          this.selectStatus === "false" ||
          this.selectStatus === ""
        )
      )
        return;
      if (
        !(
          this.selectSortOrder === "ascending" ||
          this.selectSortOrder === "descending"
        )
      )
        return;
      if (
        !(
          this.selectSortOption === "name" ||
          this.selectSortOption === "status" ||
          this.selectSortOption === "contractEndTime" ||
          this.selectSortOption === ""
        )
      )
        return;
      if (
        !(
          this.recordPerPage === "8" ||
          this.recordPerPage === "12" ||
          this.recordPerPage === "16"
        )
      )
        return;
      if (this.searchingPage < 1) return;

      // post 的 dto 變數
      let getCoachDto = {
        Status: this.selectStatus || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        SearchName: this.searchName || null,
        SearchPhone: this.searchPhone || null,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Coach/GetCoach",
          getCoachDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          // 顯示資料
          this.coachList = [];

          response.data.ApiDataObject.CoachList.forEach((coach) => {
            if (coach.Status === true) coach.Status = "啟用中";
            else coach.Status = "停用";
            coach.Phone = ("0" + coach.Phone).replace(/^(\d{4})\d{3}(\d{3})$/, '$1-xxx-$2');
            this.coachList.push(coach);
          });

          this.totalPage = response.data.ApiDataObject.TotalPage;
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
        console.error("取得管理者列表時發生錯誤", error);
      }
    },
  },
  created() {
    this.getCoachData();
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
</style>