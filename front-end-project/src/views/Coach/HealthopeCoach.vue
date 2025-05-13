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
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
// import TableNormal from "@/components/Table/TableNormal.vue";
import BtnNormal from "@/components/Btn/BtnNormal";
import SelectInput from "@/components/Input/SelectInput";
import SortSelector from "@/components/Selector/SortSelector";
import RecordSelector from "@/components/Selector/RecordSelector";
import StatusSelector from "@/components/Selector/StatusSelector";
// import PaginationComponent from "@/components/PaginationComponent";
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
      //   columns: [
      //     { label: "姓名", key: "Name" },
      //     { label: "手機", key: "Phone" },
      //     // { label: "當前會籍方案", key: "MembershipPlanName" },
      //     { label: "會籍到期日", key: "MembershipExpiry" },
      //     { label: "狀態", key: "Status" },
      //   ],
      //   coachList: [],
      //   resetDetailIndexFlag: false,
    };
  },
  methods: {
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
    getCoachData() {},
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