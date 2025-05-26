<template>
  <div>
    <TitleCard text="會員" @refreshPage="$emit('refreshPage')"></TitleCard>
    <div class="functionColumn">
      <SearchInput
        placeholder="Name..."
        v-model="searchName"
        @search="selectMemberByName"
      ></SearchInput>
      <SearchInput
        placeholder="手機末三碼..."
        v-model="searchPhone"
        @search="selectMemberByPhone"
      ></SearchInput>
       <RadioSelector
       class="statusSelector"
        v-model="selectStatus"
        @change="selectMemberByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="[
          { value: '', text: '無' },
          { value: 'true', text: '啟用' },
          { value: 'false', text: '停用' },
        ]"
      />
      <SortSelector
        :options="[
          { value: 'name', label: '姓名' },
          { value: 'membershipExpiry', label: '會籍到期' },
          { value: 'status', label: '狀態' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getMemberData"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="getMemberData"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="memberList"
      :expandable="true"
      :editBtnFlag="permissionMap.EditMember"
      :checkDetailBtnFlag="true"
      :resetDetailIndexFlag="resetDetailIndexFlag"
      @goEdit="goEditMember"
      @goCheckDetail="goDetail"
    >
      <template #detail="{ row }">
        <div class="detailRowContainer">
          <div class="detailRowLeft">
            <strong>信箱：</strong> {{ row.Email ? row.Email : "無" }} <br />
            <strong>手機審核：</strong>
            <span
              :class="{
                phoneVerifiedText: row.PhoneVerified,
                phoneUnverifiedText: !row.PhoneVerified,
              }"
              >{{ row.PhoneVerified ? "通過" : "未驗證" }} </span
            ><br />
            <div class="groupClassRecordContainer">
              <div><strong>未出席團課：</strong>{{ row.AbsenceTime }} 次</div>
              <div>
                <strong>可否預約團課：</strong>
                {{ row.AllowGroupClassFlag ? "是" : "否" }}
              </div>
              <div v-if="!row.AllowGroupClassFlag">
                <strong>允許開始日：</strong>{{ row.AllowGroupClass }}
              </div>
            </div>
          </div>
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
import TableNormal from "@/components/Table/TableNormal.vue";
import SearchInput from "@/components/Input/SearchInput";
import SortSelector from "@/components/Selector/SortSelector";
import RecordSelector from "@/components/Selector/RecordSelector";
import RadioSelector from "@/components/Selector/RadioSelector";
import PaginationComponent from "@/components/PaginationComponent";
import SvgReset from "@/components/Btn/SvgReset";

export default {
  name: "HealthopeMember",
  components: {
    TitleCard,
    TableNormal,
    SearchInput,
    SortSelector,
    RecordSelector,
    RadioSelector,
    PaginationComponent,
    SvgReset,
  },
  props: {
    permissionMap: {},
    text: String,
    notificationBoxConfirmFlag:Boolean
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
        // { label: "當前會籍方案", key: "MembershipPlanName" },
        { label: "會籍到期日", key: "MembershipExpiry" },
        { label: "狀態", key: "Status" },
      ],
      memberList: [],
      resetDetailIndexFlag: false,
    };
  },
  methods: {
    goEditMember(row) {
      if (row.MemberId < 1) return;
      this.$router.push({ path: "/member/edit", query: { id: row.MemberId } });
    },
    goDetail(row) {
      if (row.MemberId < 1) return;
      this.$router.push({ path: "/member/detail", query: { id: row.MemberId } });
    },
    selectMemberByStatus() {
      this.searchingPage = 1;
      this.getMemberData();
    },
    selectMemberByPhone() {
      this.searchingPage = 1;
      this.searchPhone = this.searchPhone.trim();
      if (this.searchPhone === "") {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "搜尋不得為空";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      this.getMemberData();
    },
    selectMemberByName() {
      this.searchingPage = 1;
      this.searchName = this.searchName.trim();
      if (this.searchName === "") {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "搜尋不得為空";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      this.getMemberData();
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getMemberData();
    },
    async getMemberData() {
      // 驗證參數
      if (isNaN(this.searchPhone)) {
        this.searchPhone = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入需為 3 位數字";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      if (!(this.searchPhone.length === 3 || this.searchPhone === "")) {
        this.searchPhone = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入需為 3 位數字";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      if (this.searchName.length > 50) {
        this.searchName = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入長度不得超過 50 位數";
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
          this.selectSortOption === "membershipExpiry" ||
          this.selectSortOption === "status" ||
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
      let getMemberDto = {
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
          "/api/Member/GetMember",
          getMemberDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          // 用來計算會員是否被禁止預約團課 及 會籍是否到期
          const today = new Date();

          // 顯示資料
          this.memberList = [];

          response.data.ApiDataObject.MemberList.forEach((member) => {
            if (member.Status === true) member.Status = "啟用中";
            else member.Status = "停用";
            member.Phone = ("0" + member.Phone).replace(/^(\d{4})\d{3}(\d{3})$/, '$1-xxx-$2');
            member.MembershipExpiry = member.MembershipExpiry.substring(0, 10);
            member.AllowGroupClass = member.AllowGroupClass.substring(0, 10);
            const membershipExpiryTargetDate = new Date(
              member.MembershipExpiry
            );

            if (membershipExpiryTargetDate < today)
              member.MembershipExpiry = "無會籍";

            const allowGroupClassTargetDate = new Date(member.AllowGroupClass);

            if (allowGroupClassTargetDate > today)
              member.AllowGroupClassFlag = false;
            else member.AllowGroupClassFlag = true;

            this.memberList.push(member);
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
        console.error("取得會員列表時發生錯誤", error);
      }
    },
    resetSearchingRecord() {
      this.selectStatus = "";
      this.selectSortOrder = "ascending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.searchPhone = "";
      this.searchName = "";
      this.searchingPage = 1;
      this.getMemberData();
    },
  },
  created() {
    this.getMemberData();
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

.statusSelector{
  width: 300px;
}

.detailRowContainer {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
}

.groupClassRecordContainer {
  display: flex;
  gap: 15px;
}

.phoneVerifiedText {
  color: #5dbe86;
}

.phoneUnverifiedText {
  color: #bc5858c0;
}
</style>