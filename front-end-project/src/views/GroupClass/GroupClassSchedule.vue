<template>
  <div>
    <TitleCard text="團體課程表" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <BtnNormal
        text="新增課程"
        @click="redirect('/groupClass/schedule/add')"
      ></BtnNormal>
      <DateSelector
        class="timeSelector"
        v-model="searchName"
        @change="selectClassByName"
        inputTitle="時間："
        inputType="radioTime"
        :options="[
          { value: 'pastCourse', text: '過去' },
          { value: 'futureCourse', text: '未來' },
          { value: '', text: '全部' },
        ]"
      />
      <RadioSelector
        class="statusSelector"
        v-model="selectCategory"
        @change="selectClassByCategory"
        inputTitle="狀態："
        inputType="radioCategory"
        :options="[
          { value: '1', text: '未開放' },
          { value: '2', text: '開放' },
          { value: '3', text: '結束' },
          { value: '4', text: '取消' },
        ]"
      />
      <SortSelector
        :options="[
          { value: 'time', label: '時間' },
          { value: 'reserveParticipant', label: '預約人數' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getClassData"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="getClassData"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      class="tableContainer"
      :columns="columns"
      :rows="classList"
      :editBtnFlag="true"
      :expandable="true"
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
import SvgReset from "@/components/Btn/SvgReset";
import SortSelector from "@/components/Selector/SortSelector";
import RecordSelector from "@/components/Selector/RecordSelector";
import RadioSelector from "@/components/Selector/RadioSelector";
import DateSelector from "@/components/Selector/DateSelector";
import {
  groupClassCategoryAndText,
  groupClassIcon,
  groupClassCategoryReverse,
} from "@/utils/groupClass";
import TableNormal from "@/components/Table/TableNormal";
import PaginationComponent from "@/components/PaginationComponent";

export default {
  name: "GroupClassSchedule",
  components: {
    BtnNormal,
    TitleCard,
    SvgReset,
    SortSelector,
    RecordSelector,
    RadioSelector,
    TableNormal,
    PaginationComponent,
    DateSelector
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectCategory: "",
      selectSortOrder: "ascending",
      selectSortOption: "",
      recordPerPage: "8",
      searchName: "",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      columns: [
        { label: "日期", key: "Date" },
        { label: "星期", key: "Week" },
        { label: "時間", key: "Time" },
        { label: "名稱", key: "Name" },
        { label: "分類", key: "Category" },
        { label: "教練", key: "Coach" },
        { label: "地點", key: "Place" },
        { label: "人數", key: "Participant" },
        { label: "狀態", key: "Status" },
      ],
      classList: [{Date:"2025-06-30"}],
    };
  },
  methods: {
    searchPage(page) {
      this.searchingPage = page;
      this.getClassData();
    },
    goDetail(row) {
      if (row.GroupClassShowcaseId < 1) return;
      this.$router.push({
        path: "/groupClass/showcase/detail",
        query: { id: row.GroupClassShowcaseId },
      });
    },
    goEdit(row) {
      if (row.GroupClassShowcaseId < 1) return;
      this.$router.push({
        path: "/groupClass/showcase/edit",
        query: { id: row.GroupClassShowcaseId },
      });
    },
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    selectClassByName() {
      this.searchingPage = 1;
      this.searchName = this.searchName.trim();

      if (this.searchName === "") {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "搜尋不得為空";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      this.getClassData();
    },
    selectClassByCategory() {
      this.searchingPage = 1;
      this.getClassData();
    },
    async getClassData() {
      if (!this.validInput()) return;

      // post 的 dto 變數
      let getClassDto = {
        Category: this.selectCategory || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        SearchName: this.searchName || null,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/GroupClassShowcase/GetShowcase",
          getClassDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.classList = response.data.ApiDataObject.ShowcaseList;

          this.classList.forEach((course) => {
            for (
              let index = 0;
              index < groupClassCategoryAndText.length;
              index++
            ) {
              if (course.Category === index + 1)
                course.Category = groupClassCategoryAndText[index].text;
            }

            groupClassIcon.forEach((icon) => {
              if (course.Icon.toString() === icon.value)
                course.Icon = icon.text;
            });
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
        console.error("取得展示用團課列表時發生錯誤", error);
      }
    },
    validInput() {
      // 驗證參數
      if (
        !(this.searchName.length > 1 || this.searchName === "") ||
        this.searchName.length > 20
      ) {
        this.searchName = "";
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入長度需 2~20 位數";
        this.$notificationBox.notificationBoxErrorCode = 5;
        return false;
      }
      if (
        this.selectCategory !== "" &&
        !(this.selectCategory in groupClassCategoryReverse)
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
          this.selectSortOption === "name" ||
          this.selectSortOption === "sort" ||
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
      if (this.searchingPage < 1) return false;

      return true;
    },
    resetSearchingRecord() {
      this.selectCategory = "";
      this.selectSortOrder = "ascending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.searchName = "";
      this.searchingPage = 1;
      this.getClassData();
    },
    deleteShowcase(row) {
      // 添加監聽器，查看彈窗是否被按確認鍵
      this.unwatchFlag = this.$watch("notificationBoxConfirmFlag", (newVal) => {
        if (newVal) {
          let redirectRoute = "stop";
          this.$emit("afterConfirmEvent", redirectRoute);

          try {
            this.submitDeleteShowcase(row.GroupClassShowcaseId);
          } catch (error) {
            console.error("刪除展示課程時發生錯誤", error);
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
    async submitDeleteShowcase(id) {
      if (id < 1) return;

      try {
        // post
        let showcaseIdDto = {
          GroupClassShowcaseId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/GroupClassShowcase/DeleteShowcase",
          showcaseIdDto
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
  },
  computed: {
    groupClassCategoryAndText() {
      let category = [...groupClassCategoryAndText];
      category.push({ value: "", text: "無" });
      return category;
    },
    groupClassIcon() {
      return groupClassIcon;
    },
  },
  created() {
    //this.getClassData();
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
  width: 440px;
}
.timeSelector{
  width: 450px;
}

.detailRowContainer {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
}

.detailRowRight {
  display: flex;
  align-items: center;
  gap: 2px;
  font-size: 15px;
  margin-left: 10%;
}
</style>