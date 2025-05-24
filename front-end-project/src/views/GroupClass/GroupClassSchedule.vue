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
        v-model="selectTime"
        @change="selectClassByTime"
        inputTitle="時間："
        inputType="radioTime"
        :options="[
          { value: 'past', text: '過去' },
          { value: 'future', text: '未來' },
          { value: 'all', text: '全部' },
        ]"
      />
      <RadioSelector
        class="statusSelector"
        v-model="selectStatus"
        @change="selectClassByStatus"
        inputTitle="狀態："
        inputType="radioCategory"
        :options="groupClassScheduleStatus"
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
      <template #detail="{ row }">
        <div class="detailRowContainer">
          <div class="detailRowLeft">
            <strong>報到人數：</strong> {{ row.CheckInParticipant }}
            <br />
            <strong>標籤(Tag)：</strong>
            <span>{{ row.Tag === 2 ? "代課" : "無" }} </span><br />
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
import BtnNormal from "@/components/Btn/BtnNormal";
import SvgReset from "@/components/Btn/SvgReset";
import SortSelector from "@/components/Selector/SortSelector";
import RecordSelector from "@/components/Selector/RecordSelector";
import RadioSelector from "@/components/Selector/RadioSelector";
import DateSelector from "@/components/Selector/DateSelector";
import { groupClassCategoryAndText, groupClassIcon, groupClassScheduleStatus } from "@/utils/groupClass";
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
    DateSelector,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectStatus: "",
      selectSortOrder: "ascending",
      selectSortOption: "",
      recordPerPage: "8",
      selectTime: "all",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      columns: [
        { label: "日期", key: "Date" },
        { label: "星期", key: "Weekday" },
        { label: "時間", key: "TimePart" },
        { label: "名稱", key: "ClassName" },
        { label: "分類", key: "Category" },
        { label: "教練", key: "CoachName" },
        { label: "地點", key: "Place" },
        { label: "人數", key: "Participant" },
        { label: "狀態", key: "Status" },
      ],
      classList: [{ Date: "2025-06-30" }],
    };
  },
  methods: {
    searchPage(page) {
      this.searchingPage = page;
      this.getClassData();
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
    selectClassByTime() {
      this.searchingPage = 1;
      this.selectTime = this.selectTime.trim();
      this.getClassData();
    },
    selectClassByStatus() {
      this.searchingPage = 1;
      this.getClassData();
    },
    async getClassData() {
      if (!this.validInput()) return;

      let specificDate = "";
      let dateRangeFilter = "";

      // 有可能是字串或時間
      if (this.validDate(this.selectTime)) {
        specificDate = new Date(this.selectTime).toISOString();
      } else {
        dateRangeFilter = this.selectTime;
      }

      // post 的 dto 變數
      let getClassDto = {
        Status: this.selectStatus || null,
        SpecificDate: specificDate || null, // EX:"2025-05-24" or null
        DateRangeFilter: dateRangeFilter || null, // EX: "past", "future", "all", or null
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/GroupClassSchedule/GetSchedule",
          getClassDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.classList = response.data.ApiDataObject.ScheduleList;

          this.classList.forEach((course) => {
            for (
              let index = 0;
              index < groupClassCategoryAndText.length;
              index++
            ) {
              if (course.Category === Number(groupClassCategoryAndText[index].value))
                course.Category = groupClassCategoryAndText[index].text;
            }

            for (
              let index = 0;
              index < groupClassScheduleStatus.length;
              index++
            ) {
              if (course.Status === Number(groupClassScheduleStatus[index].value))
                course.Status = groupClassScheduleStatus[index].text;
            }

            const localDate = new Date(course.Time);
            course.Date = localDate.toLocaleDateString("sv-SE"); // 用瑞典格式會保留 yyyy-MM-dd
            course.Weekday = localDate.toLocaleDateString(undefined, { weekday: 'long' });
            course.TimePart = localDate.toLocaleTimeString(undefined, {
              hour: "2-digit",
              minute: "2-digit",
              hour12: false,
            });

            course.Participant = course.ReserveParticipant + "/" +course.MaximumParticipant
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
        console.error("取得團課列表時發生錯誤", error);
      }
    },
    validDate(dateStr) {
      if (!/^\d{4}-\d{2}-\d{2}$/.test(dateStr)) return false;

      const date = new Date(dateStr);
      if (Number.isNaN(date.getTime())) return false;

      // 檢查元件解析後的年月日是否跟原始輸入一致
      const [year, month, day] = dateStr.split("-").map(Number);
      return (
        date.getFullYear() === year &&
        date.getMonth() + 1 === month &&
        date.getDate() === day
      );
    },
    validInput() {
      // 驗證參數
      if (
        this.selectTime !== "all" &&
        this.selectTime !== "past" &&
        this.selectTime !== "future" &&
        !this.validDate(this.selectTime)
      )
        return false;
      if (
        this.selectStatus !== "" &&
        this.selectStatus === "1" &&
        this.selectStatus === "2" &&
        this.selectStatus === "3" &&
        this.selectStatus === "4"
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
          this.selectSortOption === "time" ||
          this.selectSortOption === "reserveParticipant" ||
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
      this.selectStatus = "";
      this.selectSortOrder = "ascending";
      this.selectSortOption = "";
      this.recordPerPage = "8";
      this.selectTime = "";
      this.searchingPage = 1;
      this.getClassData();
    },
  },
  computed: {
    groupClassCategoryAndText() {
      return groupClassCategoryAndText;
    },
    groupClassIcon() {
      return groupClassIcon;
    },
    groupClassScheduleStatus(){
      let category = [...groupClassScheduleStatus];
      category.push({ value: "", text: "全部" });
      return category;
    }
  },
  created() {
    this.getClassData();
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
.timeSelector {
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