<template>
  <div>
    <TitleCard text="方案" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <BtnNormal text="新增方案" @click="redirect('/plan/add')" v-if="permissionMap.EditPlan"></BtnNormal>
      <RadioSelector
        class="radioStatus"
        v-model="selectStatus"
        @change="getPlan"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="[
          { value: 'true', text: '有效' },
          { value: 'false', text: '無效' },
          { value: '', text: '全部' },
        ]"
      />
      <SortSelector
        :options="[
          { value: 'price', label: '價格' },
          { value: 'status', label: '狀態' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getPlan"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="setRecordPerPage"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="planList"
      :editBtnFlag="permissionMap.EditPlan"
      @goEdit="goEdit"
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
import TableNormal from "@/components/Table/TableNormal";
import SortSelector from "@/components/Selector/SortSelector";
import PaginationComponent from "@/components/PaginationComponent";
import RecordSelector from "@/components/Selector/RecordSelector";
import SvgReset from "@/components/Btn/SvgReset";

export default {
  name: "MembershipPlanTemplate",
  components: {
    TitleCard,
    BtnNormal,
    RadioSelector,
    TableNormal,
    SortSelector,
    PaginationComponent,
    RecordSelector,
    SvgReset,
  },
  props: {
    permissionMap: [],
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectStatus: "",
      selectSortOption: "",
      selectSortOrder: "descending",
      recordPerPage: "8",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      planList: [],
      columns: [
        { label: "方案名稱", key: "Name" },
        { label: "價格", key: "Price" },
        { label: "狀態", key: "Status" },
        { label: "期限", key: "Duration" },
        { label: "顯示在前台", key: "Display" },
      ],
    };
  },
  methods: {
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    setRecordPerPage() {
      this.searchingPage = 1;
      this.getPlan();
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getPlan();
    },
    async getPlan() {
      if (!this.validInput()) return;

      // post 的 dto 變數
      let getPlanDto = {
        Status: this.selectStatus || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/PlanTemplate/GetMembershipPlan",
          getPlanDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.planList = [];
          response.data.ApiDataObject.MembershipPlanList.forEach((plan) => {
            let displayPlan = {
              ...plan,
              Status: plan.Status === true ? "有效" : "無效",
              Display: plan.Display === true ? "顯示" : "不顯示",
              Duration: plan.Duration + "月",
            };
            this.planList.push(displayPlan);
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
        console.error("取得會籍方案列表時發生錯誤", error);
      }
    },
    validInput() {
      // 驗證參數
      if (
        this.selectStatus !== "" &&
        this.selectStatus !== "true" &&
        this.selectStatus !== "false"
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
          this.selectSortOption === "price" ||
          this.selectSortOption === "status" ||
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
      this.selectStatus = "";
      this.selectSortOrder = "descending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getPlan();
    },
    goEdit(row) {
      if (row.MembershipPlanId < 1) return;
      this.$router.push({
        path: "/plan/membershipPlan/edit",
        query: { id: row.MembershipPlanId },
      });
    },
  },
  computed: {},
  created() {
    this.getPlan();
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
  width: 350px;
}
</style>