<template>
  <div>
    <TitleCard
      text="會員教練課程"
      @refreshPage="$emit('refreshPage')"
    ></TitleCard>
    <SubTitleCard text="新增課程"></SubTitleCard>
    <div class="createMemberPersonalClass">
      <!-- 搜尋或新增會員 -->
      <section>
        <h3>選擇會員</h3>
        <SearchInput
          @search="searchMember"
          placeholder="搜尋（姓名或電話）"
          v-model="memberKeyword"
          @enter="searchMember"
        ></SearchInput>
        <div v-if="filteredMembers.length != 0">
          <h3>搜尋結果：</h3>
          <div class="">
            <div
              v-for="member in filteredMembers"
              :key="member.MemberId"
              :class="
                selectedMember.MemberId === member.MemberId
                  ? 'selectedCard'
                  : 'filterCard'
              "
              @click="selectMember(member)"
            >
              <p class="cardRow">
                <strong>{{ member.Name }}</strong
                ><strong
                  class="selectedLab"
                  v-if="selectedMember.MemberId === member.MemberId"
                  >已選擇</strong
                >
              </p>
              <p>電話：{{ "0" + member.Phone }}</p>
            </div>
          </div>
        </div>
        <div v-else-if="!selectDefaultFlag">
          <p>查無會員</p>
        </div>
      </section>

      <!-- 根據會員有的教練課程, 選擇教練 -->
      <section>
        <h3>選擇教練課程</h3>
        <div v-if="memberPersonalTrainingPackages.length != 0">
          <div class="">
            <div
              v-for="pTCourse in memberPersonalTrainingPackages"
              :key="pTCourse.MemberPersonalTrainingPackageId"
              :class="
                selectedPTCourse.MemberPersonalTrainingPackageId ===
                pTCourse.MemberPersonalTrainingPackageId
                  ? 'selectedCard'
                  : 'filterCard'
              "
              @click="selectMemberPersonalTrainingPackage(pTCourse)"
            >
              <p class="cardRow">
                <strong>{{ pTCourse.PlanName }}</strong
                ><strong
                  class="selectedLab"
                  v-if="
                    selectedPTCourse.MemberPersonalTrainingPackageId ===
                    pTCourse.MemberPersonalTrainingPackageId
                  "
                  >已選擇</strong
                >
              </p>
              <p>
                <span
                  >教練：{{ pTCourse.CoachName }}(0{{
                    pTCourse.CoachPhone
                  }})</span
                >
                <span class="pTCourseCount"
                  >課堂數：{{ pTCourse.UsedSession }}/{{
                    pTCourse.SessionCount
                  }}</span
                >
              </p>
            </div>
          </div>
        </div>
        <div v-else>
          <p>無課程</p>
        </div>
      </section>

      <!-- 選擇時間 -->
      <section>
        <h3>選擇課程時間</h3>

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
      </section>

      <div class="hintContainer">
        <span v-if="verifyFail" class="hintSpan">{{ this.hintText }}</span>
      </div>
      <div class="btnContainer">
        <BtnConfirm
          class="btnConfirm"
          @click="addMemberPersonalClass"
          text="✔ 儲存課程"
        ></BtnConfirm>
      </div>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import SearchInput from "@/components/Input/SearchInput";
import SelectInput from "@/components/Input/SelectInput";
import BtnConfirm from "@/components/Btn/BtnConfirm";
import InputSpan from "@/components/Input/InputSpan";

export default {
  name: "MemberPersonalClass",
  components: {
    TitleCard,
    SubTitleCard,
    SearchInput,
    SelectInput,
    BtnConfirm,
    InputSpan,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectedMember: {},
      filteredMembers: [],
      memberPersonalTrainingPackages: [],
      selectedPTCourse: {},
      selectDefaultFlag: true,
      inputDate: "",
      inputTime: "",
      memberKeyword: "",
      verifyFail: false,
      hintText: "",
    };
  },
  methods: {
    async addMemberPersonalClass() {
      if (!this.addValidInput()) {
        this.verifyFail = true;
        return;
      }

      const localDate = new Date(this.inputDate + "T" + this.inputTime);
      const isoUtcString = localDate.toISOString();

      let addMemberPersonalClassDto = {
        MemberPersonalTrainingPackageId:
          this.selectedPTCourse.MemberPersonalTrainingPackageId,
        MemberId: this.selectedMember.MemberId,
        CoachId: this.selectedPTCourse.CoachId,
        Time: isoUtcString,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/MemberClass/AddMemberPersonalClass",
          addMemberPersonalClassDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$router.push("/memberPersonalClass");
        } else {
          // 設定彈窗資料
          this.$notificationBox.notificationBoxFlag = true;
          this.$notificationBox.notificationBoxTitle = "發生錯誤!";
          this.$notificationBox.notificationBoxErrorCode =
            response.data.ErrorCode;
        }
      } catch (error) {
        console.error("新增會員私人教練時發生錯誤", error);
      }
    },
    selectMemberPersonalTrainingPackage(course) {
      this.selectedPTCourse = course;
    },
    async selectMember(member) {
      this.selectedMember = member;

      if (member.MemberId < 1) return;

      let memberIdDto = {
        MemberId: member.MemberId,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/MemberClass/GetPersonalTrainingPackageAndCoach",
          memberIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.selectedPTCourse = {};
          this.memberPersonalTrainingPackages = response.data.ApiDataObject;
        } else {
          if (this.unwatchFlag) {
            this.unwatchFlag(); // 移除監聽
            this.unwatchFlag = null;
          }

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
        console.error("取得教練時發生錯誤", error);
      }
    },
    async searchMember() {
      const keyword = this.memberKeyword.trim();
      if (!keyword) {
        this.filteredMembers = [];
        return;
      }

      const phoneRegex = /^0?9\d{8}$/;

      let getMemberDto = {
        Phone: phoneRegex.test(keyword) ? keyword : null,
        Name: phoneRegex.test(keyword) ? null : keyword,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Member/GetMemberByNameOrPhone",
          getMemberDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.selectedMember = {};
          this.memberPersonalTrainingPackages = [];
          this.selectedPTCourse = {};
          this.filteredMembers = response.data.ApiDataObject;

          if (response.data.ApiDataObject.length === 0)
            this.selectDefaultFlag = false;
          else this.selectDefaultFlag = true;
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
        console.error("取得會員時發生錯誤", error);
      }
    },
    addValidInput() {
      if (Object.keys(this.selectedMember).length === 0) {
        this.hintText = "請選擇會員";
        return false;
      }

      if (Object.keys(this.selectedPTCourse).length === 0) {
        this.hintText = "請選擇教練課";
        return false;
      }

      if (
        this.selectedPTCourse.MemberPersonalTrainingPackageId < 1 ||
        this.selectedMember.MemberId < 1 ||
        this.selectedPTCourse.CoachId < 1
      ) {
        this.hintText = "格式錯誤";
        return false;
      }

      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      this.minDate = tomorrow.toISOString().slice(0, 10);

      if (this.inputDate < this.minDate) {
        this.selectedDate = "";
        this.hintText = "日期格式錯誤(請選擇明日之後的日期)";
        return false;
      }

      if (!this.inputTime) {
        this.hintText = "請選擇時間";
        return false;
      } else {
        let matchTime = false;

        this.times.forEach((time) => {
          if (this.inputTime === time.value) {
            matchTime = true;
          }
        });

        if (!matchTime) {
          this.hintText = "請選擇時間";
          return false;
        }
      }

      return true;
    },
  },
  computed: {
    times() {
      const times = [];
      const startHour = 8;
      const endHour = 21;

      for (let hour = startHour; hour <= endHour; hour++) {
        for (let minute of [0, 30]) {
          if (hour === endHour && minute === 30) break;

          const hh = hour.toString().padStart(2, "0");
          const mm = minute.toString().padStart(2, "0");
          const timeStr = `${hh}:${mm}`;
          times.push({ value: timeStr, text: timeStr });
        }
      }
      return times;
    },
  },
};
</script>

<style scoped>
.createMemberPersonalClass {
  display: flex;
  flex-direction: column;
  align-items: center;
}

section {
  width: 600px;
  max-width: 80%;
  margin: 1em 0;
  padding: 1em;
  border: 1px solid #ddd;
  border-radius: 8px;
}

.filterCard {
  background-color: white;
  margin: 1em 0;
  padding: 1em;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  cursor: pointer;
}

.filterCard:hover {
  background: rgba(255, 255, 255, 0.668);
}

.selectedCard {
  background-color: #edeff2;
  margin: 1em 0;
  padding: 1em;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  cursor: pointer;
}

.cardRow {
  display: flex;
  justify-content: space-between;
}

.cardRow .selectedLab {
  color: cadetblue;
}

.pTCourseCount {
  margin-left: 20px;
}

.timeContainer {
  display: flex;
  gap: 5px;
}

.timeContainer .date,
.timeContainer .time {
  width: 49.5%;
}

.btnContainer,
.hintContainer {
  display: flex;
  justify-content: center;
  margin-bottom: 5px;
}

.btnConfirm {
  width: 350px;
  max-width: 75%;
  height: 43px;
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