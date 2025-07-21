<template>
  <div>
    <TitleCard
      text="會員教練課程"
      @refreshPage="$emit('refreshPage')"
    ></TitleCard>
    <div class="functionColumn">
      <BtnNormal
        text="新增課程"
        @click="redirect('/memberPersonalClass/add')"
        v-if="permissionMap.EditMemberClass"
      ></BtnNormal>
      <SearchInput
        placeholder="會員手機末三碼..."
        v-model="searchPhone"
        @search="selectByPhone"
      ></SearchInput>
      <RadioSelector
        class="statusSelector"
        v-model="selectStatus"
        @change="selectByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="memberPersonalClassStatusAndText"
      />
      <SortSelector
        :options="[
          { value: 'time', label: '上課時間' },
          { value: 'coachId', label: '教練' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getMemberPersonalClassData"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="setRecordPerPage"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      :columns="columns"
      :rows="memberPersonalClassList"
      :expandable="true"
      :resetDetailIndexFlag="resetDetailIndexFlag"
      @changeStatus="checkEditStatus"
    >
      <template #detail="{ row }">
        <div class="detailRowContainer">
          <strong>操作：</strong>
          <BtnNormal text="查看會員" @click="goMemberDetail(row)" />
        </div>
        <div class="detailRowContainer">
          <strong>備註：</strong> {{ row.Remark ? row.Remark : "無" }}
          <SvgEdit @click="editRemark(row)" />
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
import BtnNormal from "@/components/Btn/BtnNormal";
import SearchInput from "@/components/Input/SearchInput";
import SortSelector from "@/components/Selector/SortSelector";
import RecordSelector from "@/components/Selector/RecordSelector";
import RadioSelector from "@/components/Selector/RadioSelector";
import SvgReset from "@/components/Btn/SvgReset";
import {
  memberPersonalClassStatusAndText,
  memberPersonalClassStatus,
  memberPersonalClassCategoryAndText,
} from "@/utils/memberPersonalClass";
import TableNormal from "@/components/Table/TableNormal.vue";
import PaginationComponent from "@/components/PaginationComponent";
import SvgEdit from "@/components/Btn/SvgEdit";

export default {
  name: "MemberPersonalClass",
  components: {
    TitleCard,
    BtnNormal,
    SearchInput,
    SortSelector,
    RecordSelector,
    RadioSelector,
    SvgReset,
    TableNormal,
    PaginationComponent,
    SvgEdit,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectSortOption: "",
      selectSortOrder: "descending",
      recordPerPage: "8",
      searchPhone: "",
      selectStatus: "",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      columns: [
        { label: "會員", key: "Member" },
        { label: "教練", key: "Coach" },
        { label: "課程時間", key: "LocalTime" },
        {
          label: "狀態",
          key: "Status",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditMemberClass,
        },
        { label: "分類", key: "Category" },
      ],
      memberPersonalClassList: [],
      resetDetailIndexFlag: false,
    };
  },
  methods: {
    searchPage(page) {
      this.searchingPage = page;
      this.getMemberPersonalClassData();
    },
    async editRemark(row) {
      let remarkInput = "";

      while (!remarkInput || remarkInput.length > 20) {
        remarkInput = prompt("請輸入備註（不能留空，20 字內）：");
        if (remarkInput === null) {
          return; // 取消的話就中斷 function
        }
      }

      // 沒修改的話就中斷 function
      if (remarkInput === row.Remark) return;

      let editMemberPersonalClassRemarkDto = {
        MemberPersonalClassId: row.MemberPersonalClassId,
        Remark: remarkInput,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/MemberClass/EditMemberPersonalClassRemark",
          editMemberPersonalClassRemarkDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getMemberPersonalClassData();
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
                this.getMemberPersonalClassData();
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
        console.error("修改備註時發生錯誤", error);
      }
    },
    setRecordPerPage() {
      this.searchingPage = 1;
      this.getMemberPersonalClassData();
    },
    selectByStatus() {
      this.searchingPage = 1;
      this.getMemberPersonalClassData();
    },
    selectByPhone() {
      this.searchingPage = 1;
      this.searchPhone = this.searchPhone.trim();

      if (this.searchPhone === "") {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "搜尋不得為空";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      this.getMemberPersonalClassData();
    },
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    async getMemberPersonalClassData() {
      if (!this.validInput()) return;
      // post 的 dto 變數
      let getMemberPersonalClassDto = {
        Status: this.selectStatus || null,
        SearchPhone: this.searchPhone || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/MemberClass/GetMemberPersonalClass",
          getMemberPersonalClassDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.resetDetailIndexFlag = !this.resetDetailIndexFlag;
          this.currentPage = this.searchingPage;
          this.memberPersonalClassList =
            response.data.ApiDataObject.MemberPersonalClassList;

          this.memberPersonalClassList.forEach((course) => {
            course.Member = `${course.MemberName} (0${course.MemberPhone})`;
            course.Coach = `${course.CoachName}`;

            const localTime = new Date(course.Time + "Z").toLocaleString();
            course.LocalTime = localTime;

            memberPersonalClassCategoryAndText.forEach((category) => {
              if (Boolean(course.Category) === Boolean(category.value)) {
                course.Category = category.text;
              }
            });
            let statusOption = [];

            memberPersonalClassStatusAndText.forEach((status) => {
              if (course.Status === Number(status.value)) {
                statusOption.push(status);
              }

              if (
                Number(status.value) === memberPersonalClassStatus.Cancelled &&
                (course.Status ===
                  memberPersonalClassStatus.BookingInProgress ||
                  course.Status ===
                    memberPersonalClassStatus.BookedSuccessfully ||
                  course.Status === memberPersonalClassStatus.DidNotAttend)
              ) {
                statusOption.push(status);
              }
            });

            course.Status = {
              OldValue: String(course.Status),
              Value: String(course.Status),
              Options: statusOption,
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
        console.error("取得會員的預約教練課列表時發生錯誤", error);
      }
    },
    validInput() {
      // 驗證參數
      if (
        this.selectStatus !== "" &&
        !Object.values(memberPersonalClassStatus).includes(
          Number(this.selectStatus)
        )
      ) {
        return false;
      }

      if (isNaN(this.searchPhone)) {
        this.searchPhone = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入需為 3 位數字";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return false;
      }

      if (!(this.searchPhone.length === 3 || this.searchPhone === "")) {
        this.searchPhone = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入需為 3 位數字";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return false;
      }

      if (
        !(
          this.selectSortOrder === "ascending" ||
          this.selectSortOrder === "descending"
        )
      )
        return false;
      if (
        !(
          this.selectSortOption === "time" ||
          this.selectSortOption === "caochId" ||
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
      this.searchPhone = "";
      this.searchingPage = 1;
      this.getMemberPersonalClassData();
    },
    goMemberDetail(row) {
      if (row.MemberId < 1) return;
      this.$router.push({
        path: "/member/detail",
        query: { id: row.MemberId },
      });
    },
    checkEditStatus(row) {
      if (this.unwatchFlag) {
        this.unwatchFlag(); // 確保監聽被移除
        this.unwatchFlag = null;
      }

      // 添加監聽器，查看彈窗是否被按確認鍵
      this.unwatchFlag = this.$watch("notificationBoxConfirmFlag", (newVal) => {
        if (newVal) {
          let redirectRoute = "stop";
          this.$emit("afterConfirmEvent", redirectRoute);

          try {
            this.editStatus(row);
          } catch (error) {
            console.error("修改會員預約的教練課狀態時發生錯誤", error);
          } finally {
            this.unwatchFlag(); // 確保監聽被移除
            this.unwatchFlag = null;
          }
        }
      });

      this.getMemberPersonalClassData();

      // 設定彈窗資料
      this.$notificationBox.notificationBoxFlag = true;
      this.$notificationBox.notificationBoxTitle = "此操作不可修改，確認修改?";
      this.$notificationBox.notificationBoxCancelFlag = true;
      this.$notificationBox.notificationBoxErrorCode = 0;
    },
    async editStatus(row) {
      // 檢查是否轉換成功
      if (
        !this.statusTranslator(
          Number(row.Status.OldValue),
          Number(row.Status.Value)
        )
      )
        return;

      const editStatusDto = {
        MemberPersonalClassId: row.MemberPersonalClassId,
        Status: row.Status.Value,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/MemberClass/EditMemberPersonalClassStatus",
          editStatusDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getMemberPersonalClassData();
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          if (this.unwatchFlag) {
            this.unwatchFlag(); // 移除監聽
            this.unwatchFlag = null;
          }

          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                this.getMemberPersonalClassData();
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
        console.error("修改會員的教練課成時發生錯誤", error);
      }
    },
    statusTranslator(oldStatus, newStatus) {
      if (Number(newStatus) === memberPersonalClassStatus.Cancelled) {
        if (
          Number(oldStatus) === memberPersonalClassStatus.BookingInProgress ||
          Number(oldStatus) === memberPersonalClassStatus.BookedSuccessfully ||
          Number(oldStatus) === memberPersonalClassStatus.DidNotAttend
        ) {
          return true;
        }
      }
      return false;
    },
  },
  computed: {
    memberPersonalClassStatusAndText() {
      let options = [...memberPersonalClassStatusAndText];
      options.push({ value: "", text: "全部" });
      return options;
    },
  },
  created() {
    this.getMemberPersonalClassData();
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
  width: 580px;
}
.detailRowContainer {
  display: flex;
  align-items: center;
  gap: 5px;
}
</style>