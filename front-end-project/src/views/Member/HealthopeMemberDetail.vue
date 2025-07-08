<template>
  <div>
    <TitleCard text="會員" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="會員資料" />
    <div class="sectionTitle"><h3>基本資訊</h3></div>
    <div class="memberBasicInformationBox">
      <div class="memberBasicInformationContainer">
        <div class="avatar">
          <div class="avatarInner">
            <img src="@/assets/mockImage/avatar1.png" />
          </div>
        </div>
        <div class="basicInformationContent">
          <div class="top">
            <div class="contentTextBox">
              <label for="">姓名</label><br />
              <span>{{ member.Name ? member.Name : "Ｘ" }}</span>
            </div>
            <div class="contentTextBox">
              <label for="">性別</label><br />
              <span>{{ member.Gender }}</span>
            </div>
            <div class="contentTextBox">
              <label for="">生日</label><br />
              <span>{{ member.BirthDay }}</span>
            </div>
          </div>
          <div class="middle">
            <div class="contentTextBox">
              <label for=""
                >手機
                <span
                  :class="{
                    phoneVerifiedText: member.PhoneVerified,
                    phoneUnverifiedText: !member.PhoneVerified,
                  }"
                  >{{ member.PhoneVerified ? "通過" : "未驗證" }}
                </span>
              </label>
              <br />
              <span>{{ member.Phone }}</span>
            </div>
            <div class="contentTextBox emailBox">
              <label for="">信箱</label><br />
              <span>{{ member.Email }}</span>
            </div>
          </div>
          <div class="middle">
            <div class="contentTextBox">
              <label for="">身高</label><br />
              <span>{{ member.Height }}cm</span>
            </div>
            <div class="contentTextBox">
              <label for="">體重</label><br />
              <span>{{ member.Weight }}kg</span>
            </div>
          </div>
          <div class="bottom">
            <div class="contentTextBox">
              <label for="">緊急聯絡人姓名</label><br />
              <span>{{ member.EmergencyContactName }}</span>
            </div>
            <div class="contentTextBox emergencyPhone">
              <label for="">緊急聯絡人手機</label><br />
              <span>{{ member.EmergencyContactPhone }}</span>
            </div>
            <div class="contentTextBox emergencyRelation">
              <label for="">緊急聯絡人關係</label><br />
              <span>{{ member.EmergencyContactRelation }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="sectionTitle"><h3>帳號資訊</h3></div>
    <div class="memberAccountBox">
      <div class="memberAccountContainer">
        <div class="memberAccountContent">
          <div class="top">
            <div class="contentTextBox">
              <label class="lab">狀態</label><br />
              <span>{{ member.Status ? "啟用中" : "停用" }}</span>
            </div>
            <div class="contentTextBox">
              <label class="lab">會籍到期日</label><br />
              <span>{{ member.MembershipExpiry }}</span>
            </div>
            <div class="contentTextBox">
              <label class="lab">創建時間</label><br />
              <span>{{ member.CreateTime }}</span>
            </div>
          </div>
          <div class="bottom">
            <div class="contentTextBox">
              <label class="lab">未出席團課</label><br />
              <span>{{ member.AbsenceTime }} 次</span>
            </div>
            <div class="contentTextBox">
              <label class="lab">可否預約團課</label><br />
              <span>{{ AllowGroupClassFlag ? "是" : "否" }}</span>
            </div>
            <div class="contentTextBox" v-if="!this.AllowGroupClassFlag">
              <label class="lab">允許開始日</label><br />
              <span>{{ member.AllowGroupClass }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="sectionTitle">
      <h3>會籍方案</h3>
    </div>
    <div v-if="memberMembershipPlanList.length === 0" class="emptyMessage">
      尚未有任何會籍方案。
    </div>
    <div v-else>
      <TableNormal
        :columns="memberMembershipPlanColumns"
        :rows="memberMembershipPlanList"
        @changeStatus="editMemberMembershipPlanStatus"
      >
      </TableNormal>
    </div>
    <div class="sectionTitle"><h3>教練課程</h3></div>
    <div
      v-if="memberPersonalTrainingPackageList.length === 0"
      class="emptyMessage"
    >
      尚未有任何教練課程方案。
    </div>
    <div v-else>
      <TableNormal
        :columns="memberPersonalTrainingPackageColumns"
        :rows="memberPersonalTrainingPackageList"
        @changeCoach="editMemberPersonalTrainingPackageCoach"
      >
      </TableNormal>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import TableNormal from "@/components/Table/TableNormal";
import {
  memberMembershipPlanStatusAndText,
  memberMembershipPlanStatus,
  memberPersonalTrainingPackageStatusAndText,
} from "@/utils/memberPlan";

export default {
  components: {
    TitleCard,
    SubTitleCard,
    TableNormal,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
    permissionMap: {},
  },
  data() {
    return {
      member: {},
      AllowGroupClassFlag: false,
      memberMembershipPlanList: [],
      memberPersonalTrainingPackageList: [],
      memberMembershipPlanColumns: [
        { label: "方案名稱", key: "PlanName" },
        { label: "期限(月)", key: "Duration" },
        {
          label: "狀態",
          key: "Status",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditMemberMembershipPlan,
        },
        { label: "最後修改日期", key: "LocalUpdateTime" },
        { label: "結束日期", key: "LocalEndDate" },
      ],
      memberPersonalTrainingPackageColumns: [
        { label: "方案名稱", key: "PlanName" },
        {
          label: "教練",
          key: "Coach",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditMemberPersonalPeckagePlan,
        },
        { label: "課堂數", key: "SessionCount" },
        { label: "狀態", key: "Status" },
        { label: "最後修改日期", key: "LocalUpdateTime" },
      ],
      coachList: [],
    };
  },
  methods: {
    async editMemberMembershipPlanStatus(row) {
      // 檢查是否轉換成功
      if (
        !this.memberMembershipPlanStatusTranslator(
          Number(row.Status.OldValue),
          Number(row.Status.Value)
        )
      )
        return;

      const editStatusDto = {
        MemberMembershipPlanId: row.MemberMembershipPlanId,
        Status: row.Status.Value,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/MemberPlan/EditMemberMembershipPlanStatus",
          editStatusDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getMemberDetail(this.member.MemberId);
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
        console.error("修改會員的會籍方案時發生錯誤", error);
      }
    },
    async editMemberPersonalTrainingPackageCoach(row) {
      if (row.Coach.Value < 1) return;

      const editCoachDto = {
        MemberPersonalTrainingPackageId: row.MemberPersonalTrainingPackageId,
        CoachId: row.Coach.Value,
        UpdateTime: row.UpdateTime,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/MemberPlan/EditMemberPersonalTrainingPackageCoach",
          editCoachDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getMemberDetail(this.member.MemberId);
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
        console.error("修改會員的教練課方案時發生錯誤", error);
      }
    },
    async getMemberDetail(memberId) {
      if (memberId < 1) return;

      try {
        let memberIdDto = {
          MemberId: memberId,
        };

        // post
        const response = await this.$axios.post(
          "/api/Member/GetMemberDetail",
          memberIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.member = response.data.ApiDataObject.Member;
          this.memberMembershipPlanList =
            response.data.ApiDataObject.MemberMembershipPlanList;
          this.memberPersonalTrainingPackageList =
            response.data.ApiDataObject.MemberPersonalTrainingPackageList;
          this.coachList = response.data.ApiDataObject.CoachList;

          this.member.MemberId = memberId;

          this.memberMembershipPlanList.forEach((plan) => {
            let membershipPlanOptions = [];

            memberMembershipPlanStatusAndText.forEach((status) => {
              if (plan.Status === Number(status.value))
                membershipPlanOptions.push(status);

              if (
                plan.Status === memberMembershipPlanStatus.Inactive &&
                Number(status.value) === memberMembershipPlanStatus.Active
              )
                membershipPlanOptions.push(status);

              if (
                plan.Status === memberMembershipPlanStatus.Active &&
                Number(status.value) === memberMembershipPlanStatus.Paused
              )
                membershipPlanOptions.push(status);

              if (
                plan.Status === memberMembershipPlanStatus.Paused &&
                Number(status.value) === memberMembershipPlanStatus.Active
              )
                membershipPlanOptions.push(status);

              if (
                plan.Status === memberMembershipPlanStatus.Active &&
                Number(status.value) === memberMembershipPlanStatus.Completed &&
                plan.EndDate.substring(0, 10) !== "0001-01-01"
              ) {
                const today = new Date();
                today.setHours(0, 0, 0, 0);
                const endDate = new Date(plan.EndDate);
                endDate.setHours(0, 0, 0, 0);

                if (endDate < today) membershipPlanOptions.push(status);
              }
            });

            plan.Status = {
              Value: String(plan.Status),
              Options: membershipPlanOptions,
              OldValue: String(plan.Status),
            };

            if (plan.EndDate.substring(0, 10) === "0001-01-01") {
              plan.LocalEndDate = "-";
            } else {
              const localEndTime = new Date(plan.EndDate + "Z");
              plan.LocalEndDate = localEndTime.toLocaleString();
            }

            const localUpdateTime = new Date(plan.UpdateTime + "Z");
            plan.LocalUpdateTime = localUpdateTime.toLocaleString();
          });

          this.memberPersonalTrainingPackageList.forEach((plan) => {
            memberPersonalTrainingPackageStatusAndText.forEach((status) => {
              if (Number(status.value) === plan.Status)
                plan.Status = status.text;
            });

            let coachOption = [];
            this.coachList.forEach((coach) => {
              coachOption.push({
                value: coach.CoachId,
                text: coach.Name + `(0${coach.Phone})`,
              });
            });

            plan.Coach = {
              Value: String(plan.CoachId),
              Options: coachOption,
              OldValue: String(plan.CoachId),
            };

            const localUpdateTime = new Date(plan.UpdateTime + "Z");
            plan.LocalUpdateTime = localUpdateTime.toLocaleString();
          });
          // 調整顯示格式
          this.member.Gender = this.genderToText(this.member.Gender);
          this.member.Phone = ("0" + this.member.Phone).replace(
            /^(\d{4})\d{3}(\d{3})$/,
            "$1-xxx-$2"
          );
          if (!this.member.Email) this.member.Email = "Ｘ";
          if (this.member.BirthDay === "0001-01-01")
            this.member.BirthDay = "Ｘ";
          if (this.member.Height === 0) this.member.Height = "Ｘ";
          if (this.member.Weight === 0) this.member.Weight = "Ｘ";

          if (this.member.EmergencyContactName) {
            this.member.EmergencyContactPhone = (
              "0" + this.member.EmergencyContactPhone
            ).replace(/^(\d{4})\d{3}(\d{3})$/, "$1-xxx-$2");
          } else {
            this.member.EmergencyContactName = "Ｘ";
            this.member.EmergencyContactPhone = "Ｘ";
            this.member.EmergencyContactRelation = "Ｘ";
          }

          this.member.MembershipExpiry = this.member.MembershipExpiry.substring(
            0,
            10
          );
          this.member.AllowGroupClass = this.member.AllowGroupClass.substring(
            0,
            10
          );

          this.member.CreateTime = this.member.CreateTime.replace(
            "T",
            " "
          ).split(".")[0];

          const today = new Date();
          const membershipExpiryTargetDate = new Date(
            this.member.MembershipExpiry
          );

          if (membershipExpiryTargetDate < today)
            this.member.MembershipExpiry = "無會籍";

          const allowGroupClassTargetDate = new Date(
            this.member.AllowGroupClass
          );

          if (allowGroupClassTargetDate > today)
            this.AllowGroupClassFlag = false;
          else this.AllowGroupClassFlag = true;
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/member";
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
        console.error("取得特定管理者時發生錯誤", error);
      }
    },
    genderToText(genderNum) {
      if (genderNum === 1) return "男";
      if (genderNum === 2) return "女";
      if (genderNum === 3) return "其他";
      return "無";
    },
    memberMembershipPlanStatusTranslator(oldStatus, newStatus) {
      if (
        oldStatus === memberMembershipPlanStatus.Inactive &&
        newStatus === memberMembershipPlanStatus.Active
      )
        return true;

      if (
        oldStatus === memberMembershipPlanStatus.Active &&
        newStatus === memberMembershipPlanStatus.Paused
      )
        return true;

      if (
        oldStatus === memberMembershipPlanStatus.Paused &&
        newStatus === memberMembershipPlanStatus.Active
      )
        return true;

      if (
        oldStatus === memberMembershipPlanStatus.Active &&
        newStatus === memberMembershipPlanStatus.Completed
      )
        return true;

      return false;
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/member");
      return;
    }
    this.getMemberDetail(this.$route.query.id);
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
}

.avatar {
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
  border-radius: 30px;
  width: 200px;
  max-width: 50%;
  height: 100%;
  display: flex;
  align-items: center;
}

.avatarInner {
  width: 12rem;
  border-radius: 0.25rem;
  overflow: hidden;
}

.avatarInner img {
  width: 100%;
  height: auto;
  display: flex;
}

.memberBasicInformationBox,
.memberAccountBox {
  display: flex;
  justify-content: center;
}

.memberBasicInformationContainer,
.memberAccountContainer {
  display: flex;
  align-items: center;
  padding: 9px;
  width: 1000px;
  max-width: 80%;
  background-color: white;
  border-radius: 35px;
  gap: 9px;
  box-shadow: rgba(10, 37, 64, 0.35) 0px -1px 5px 0px inset;
}

.basicInformationContent,
.memberAccountContent {
  display: flex;
  justify-content: space-evenly;
  align-items: center;
  flex-wrap: wrap;
  overflow: hidden;
  width: 1000px;
  max-width: 100%;
  border-radius: 30px;
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
}

.basicInformationContent {
  min-height: 220px;
}

.memberAccountContent {
  min-height: 150px;
}

.top,
.bottom,
.middle {
  display: flex;
  flex-wrap: wrap;
  width: 100%;
  height: 100%;
  padding: 5px;
  gap: 10px 10%;
  word-break: break-word;
}

.top,
.middle {
  padding-bottom: 10px;
  border-bottom: solid #eee 1px;
}

.memberAccountContent .top,
.memberAccountContent .bottom {
  justify-content: space-evenly;
}

.contentTextBox {
  margin-left: 25px;
  margin-bottom: 5px;
  width: 150px;
}

.memberAccountContent .contentTextBox {
  margin-left: 0;
}

.contentTextBox label {
  font-size: 20px;
  font-weight: 700;
  color: #6f6f6f;
  font-family: "Microsoft JhengHei";
}

.phoneVerifiedText {
  border: #5dbe86 solid 1px;
  border-radius: 15%;
  font-size: 18px;
  color: #5dbe86;
}

.phoneUnverifiedText {
  font-size: 16px;
  border: #bc5858c0 solid 1px;
  border-radius: 15%;
  color: #bc5858c0;
}

.emptyMessage {
  padding: 16px;
  text-align: center;
  color: #777;
  border: 1px dashed #ccc;
  background-color: #fafafa;
  border-radius: 6px;
  margin-top: 12px;
}
</style>