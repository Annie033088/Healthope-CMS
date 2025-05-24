<template>
  <div>
    <TitleCard text="團體課程表" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增課程"></SubTitleCard>
    <div class="scheduleCreator">
      <!-- 左側：課程展示清單 -->
      <div class="panel left">
        <div class="panelTitle">
          <h3>📋 選擇展示課程</h3>
          <NormalSelector
            class="categorySelector"
            labelText="分類："
            :parentValue.sync="selectCategory"
            :options="selectCategoryAndText"
            @change="getShowcaseDataAndCoachData"
          />
        </div>
        <div class="courseCard" v-for="course in showcaseList" :key="course.id">
          <div class="cardFirstRow">
            <h4>{{ course.Icon.text }}</h4>
            <h4>{{ course.Name }}</h4>
            <span class="cardCategory">{{ course.Category.text }}</span>
          </div>
          <BtnNormal
            text="➕ 使用此課程建立排程"
            @click="selectCourse(course)"
          />
        </div>
      </div>

      <!-- 右側：建立排程 -->
      <div class="panel right">
        <h3>
          📅 新增團體課程排程
          <svg
            width="12"
            height="12"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M21 13H14.4L19.1 17.7L17.7 19.1L13 14.4V21H11V14.3L6.3 19L4.9 17.6L9.4 13H3V11H9.6L4.9 6.3L6.3 4.9L11 9.6V3H13V9.4L17.6 4.8L19 6.3L14.3 11H21V13Z"
              fill="#F24822"
            />
          </svg>
        </h3>
        <InputSpan
          class="inputSpanContainer"
          labelText="課程名稱"
          v-model="schedule.ClassName"
        ></InputSpan>
        <SelectInput
          labelText="分類"
          :parentValue.sync="schedule.Category"
          :options="groupClassCategoryAndText"
        />
        <SelectInput
          labelText="Icon"
          :parentValue.sync="schedule.Icon"
          :options="groupClassIcon"
        />
        <SelectInput
          labelText="教練名稱"
          :parentValue.sync="schedule.CoachId"
          :options="displayCoachList"
        />
        <div class="timeContainer">
          <InputSpan
            class="inputSpanContainer date"
            labelText="課程日期"
            v-model="inputDate"
            inputType="date"
          ></InputSpan>
          <SelectInput
            class="time"
            labelText="時間"
            :parentValue.sync="inputTime"
            :options="times"
          />
        </div>
        <InputSpan
          class="inputSpanContainer"
          labelText="地點"
          v-model="schedule.Place"
        ></InputSpan>

        <InputSpan
          class="inputSpanContainer"
          labelText="人數上限"
          v-model="schedule.MaximumParticipant"
        ></InputSpan>
        <div class="hintContainer">
          <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
        </div>
        <div class="btnContainer">
          <BtnConfirm
            class="btnConfirm"
            @click="addSchedule"
            text="✔ 儲存排程"
          ></BtnConfirm>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import InputSpan from "@/components/Input/InputSpan";
import SelectInput from "@/components/Input/SelectInput";
import BtnNormal from "@/components/Btn/BtnNormal";
import NormalSelector from "@/components/Selector/NormalSelector";
import {
  groupClassCategoryAndText,
  groupClassCategoryReverse,
  groupClassIcon,
} from "@/utils/groupClass";

export default {
  components: {
    TitleCard,
    SubTitleCard,
    BtnConfirm,
    InputSpan,
    SelectInput,
    BtnNormal,
    NormalSelector,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      verifyFail: false,
      hintText: "",
      selectCategory: "0",
      inputTime: "",
      inputDate: "",
      times: [
        { value: "8:30", text: "8:30-9:30" },
        { value: "9:40", text: "9:40-10:40" },
        { value: "10:50", text: "10:50-11:50" },
        { value: "14:00", text: "14:00-15:00" },
        { value: "15:10", text: "15:10-16:10" },
        { value: "16:20", text: "16:20-17:20" },
        { value: "17:30", text: "17:30-18:30" },
        { value: "18:40", text: "18:40-19:40" },
        { value: "19:50", text: "19:50-20:50" },
        { value: "21:00", text: "21:00-22:00" },
      ],
      showcaseList: [],
      displayCoachList: [],
      coachList: [],
      schedule: {
        ClassName: "",
        Category: "",
        Icon: "",
        CoachId: "",
        Time: "",
        Place: "",
        MaximumParticipant: "35",
      },
    };
  },
  computed: {
    selectCategoryAndText() {
      let selectCategoryAndText = [...groupClassCategoryAndText];
      selectCategoryAndText.push({ value: "0", text: "全部" });
      return selectCategoryAndText;
    },
    groupClassCategoryAndText() {
      return groupClassCategoryAndText;
    },
    groupClassIcon() {
      return groupClassIcon;
    },
  },
  methods: {
    selectCourse(course) {
      this.schedule.ClassName = course.Name;
      this.schedule.Category = course.Category.value;
      this.schedule.Icon = course.Icon.value;
    },
    async getShowcaseDataAndCoachData() {
      let category;

      if (this.selectCategory === "0") category = ""; // 0 代表搜尋全部
      else category = this.selectCategory;

      if (
        !this.selectCategory &&
        !groupClassCategoryReverse[this.selectCategory]
      )
        return;

      // post 的 dto 變數
      let getShowcaseAndCoachDto = {
        Category: category || null,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/GroupClassSchedule/GetShowcaseAndCoach",
          getShowcaseAndCoachDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.showcaseList = response.data.ApiDataObject.ShowcaseList;
          this.coachList = response.data.ApiDataObject.CoachList;
          this.displayCoachList = [];
          this.coachList.forEach((coach) => {
            let displayCoach = {
              ...coach,
              value: coach.CoachId,
              text: coach.Name,
            };
            this.displayCoachList.push(displayCoach);
          });

          this.showcaseList.forEach((course) => {
            for (
              let index = 0;
              index < groupClassCategoryAndText.length;
              index++
            ) {
              if (
                course.Category ===
                Number(groupClassCategoryAndText[index].value)
              )
                course.Category = groupClassCategoryAndText[index];
            }

            groupClassIcon.forEach((icon) => {
              if (course.Icon.toString() === icon.value) course.Icon = icon;
            });
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
        console.error("新增團課排程，取資料時發生錯誤", error);
      }
    },
    validInput() {
      // 格式驗證
      if (!this.schedule.ClassName || this.schedule.ClassName.length > 20) {
        this.hintText = "名稱需輸入 20 字以內";
        return false;
      }

      if (!(this.schedule.Category in groupClassCategoryReverse)) {
        this.hintText = "分類格式錯誤";
        return false;
      }

      const IntMax = 2147483647;
      let icon = Number(this.schedule.Icon);
      if (
        !Number.isInteger(icon) || // 不是整數
        icon < 1 ||
        // 超出安全整數範圍
        icon > IntMax
      ) {
        this.hintText = "icon 格式錯誤";
        return false;
      }

      let coachId = Number(this.schedule.CoachId);
      if (
        !Number.isInteger(coachId) || // 不是整數
        coachId < 1 ||
        // 超出安全整數範圍
        coachId > IntMax
      ) {
        this.hintText = "教練格式錯誤";
        return false;
      }

      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      this.minDate = tomorrow.toISOString().slice(0, 10);

      if (this.inputDate < this.minDate) {
        this.selectedDate = "";
        this.hintText = "日期格式錯誤";
        return false;
      }

      let validTime = false;
      this.times.forEach((time) => {
        if (time.value === this.inputTime) validTime = true;
      });

      if (!validTime) {
        this.inputTime = "";
        this.hintText = "時間格式錯誤";
        return false;
      }

      if (!this.schedule.Place || this.schedule.Place.length > 10) {
        this.hintText = "地點需輸入 10 字以內";
        return false;
      }

      let maximumParticipant = Number(this.schedule.MaximumParticipant);
      // 不是整數
      if (
        !Number.isInteger(maximumParticipant) ||
        maximumParticipant > 255 ||
        maximumParticipant < 1
      ) {
        this.hintText = "最大上限人數輸入錯誤 (最高 255 人)";
        return false;
      }

      return true;
    },
    async addSchedule() {
      this.schedule.ClassName = this.schedule.ClassName.trim();
      this.schedule.Category = this.schedule.Category.trim();
      this.schedule.Icon = this.schedule.Icon.trim();
      this.schedule.Place = this.schedule.Place.trim();
      this.schedule.MaximumParticipant = this.schedule.MaximumParticipant.trim();

      if (this.inputTime.split(":")[0].length < 2)
        this.schedule.Time = (this.inputDate + "T0" + this.inputTime).trim();
      else this.schedule.Time = (this.inputDate + "T" + this.inputTime).trim();

      if (!this.validInput()) {
        this.verifyFail = true;
        return;
      }

      const coach = this.coachList.find(
        (coach) => coach.CoachId === Number(this.schedule.CoachId)
      );

      try {
        // 傳輸新增資料
        let addScheduleDto = {
          ClassName: this.schedule.ClassName,
          Category: this.schedule.Category,
          Icon: this.schedule.Icon,
          Coach: coach,
          Time: this.schedule.Time,
          Place: this.schedule.Place,
          MaximumParticipant: this.schedule.MaximumParticipant,
        };

        // post後回傳
        const response = await this.$axios.post(
          "/api/GroupClassSchedule/AddSchedule",
          addScheduleDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.verifyFail = false;
          this.$router.push("/groupClass/schedule");
          return;
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增展示課時發生錯誤", error);
      }
    },
  },
  created() {
    this.getShowcaseDataAndCoachData();
  },
};
</script>

<style scoped>
.scheduleCreator {
  display: flex;
  gap: 24px;
  padding: 24px;
  font-family: sans-serif;
  flex-wrap: wrap;
}

.panel {
  flex: 1;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 16px;
}

.left {
  background-color: #fafafa;
  overflow-y: auto;
  max-height: 80vh;
  min-width: 210px;
}

.right {
  background-color: #fafafa;
  min-width: 210px;
}

.courseCard {
  border: 1px solid #ccc;
  border-radius: 6px;
  padding: 12px;
  margin-bottom: 12px;
  background: white;
}

.courseCard h4 {
  margin: 0 0 4px;
}

.panelTitle {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
}

.panelTitle .categorySelector {
  height: 34px;
}

.btnContainer,
.hintContainer {
  display: flex;
  justify-content: center;
  margin-bottom: 5px;
}

.btnConfirm {
  width: 200px;
  max-width: 75%;
  height: 43px;
}

.timeContainer {
  display: flex;
  gap: 5px;
}

.timeContainer .date,
.timeContainer .time {
  width: 49.5%;
}

.cardFirstRow {
  display: flex;
  margin-bottom: 8px;
}

.cardCategory {
  border: solid #f4ddddce;
  border-radius: 5px;
  font-size: 15px;
  padding: 1px;
  margin-left: 25px;
  background: #f4ddddce;
}

.hintSpan {
  color: #c07878;
  animation: slideInTop 0.5s cubic-bezier(0.25, 0.46, 0.45, 0.94) both;
}

@keyframes slideInTop {
  0% {
    transform: translateY(-30px);
    opacity: 0;
  }
  100% {
    transform: translateY(0);
    opacity: 1;
  }
}
</style>
