<template>
  <div>
    <TitleCard text="訂單" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增訂單"></SubTitleCard>
    <div class="createOrder">
      <div v-if="step === 1">
        <h2>新增訂單</h2>

        <!-- 1. 選擇方案類型 -->
        <section>
          <h3>選擇方案類型</h3>
          <RadioInput
            class="timeSelector"
            v-model="addOrderDto.planType"
            inputType="radioTime"
            :options="[
              { value: '1', text: '會籍方案' },
              { value: '2', text: '教練課方案' },
              { value: '3', text: '票券方案' },
            ]"
            @change="changePlanType"
          />
        </section>

        <!-- 2. 選擇方案 -->
        <section v-if="addOrderDto.planType">
          <h3>選擇方案</h3>
          <SelectInput
            class="selectPlan"
            :parentValue.sync="addOrderDto.planId"
            :options="planOptions"
          />
        </section>

        <!-- 3. 搜尋或新增會員 -->
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
                class="filterMemberCard"
                @click="selectMember(member)"
              >
                <p>
                  <strong>{{ member.Name }}</strong>
                </p>
                <p>電話：{{ "0" + member.Phone }}</p>
              </div>
            </div>
          </div>
          <div v-else-if="!selectDefaultFlag">
            <p>查無會員</p>
          </div>
          <div v-if="selectedMember">
            <h3>已選擇會員</h3>
            <div class="filterMemberCard">
              <p class="">
                <strong>{{ selectedMember.Name }}</strong>
              </p>
              <p>
                電話：{{ "0" + selectedMember.Phone }}
                <BtnNormal @click="clearSelected" text="取消選擇"></BtnNormal>
              </p>
            </div>
          </div>
        </section>

        <!-- 4. 選擇付款方式與發票 -->
        <section class="">
          <h3>結帳資訊</h3>
          <NormalSelector
            class="paymentMethodSeletor selector"
            labelText="付款方式："
            :parentValue.sync="addOrderDto.paymentMethod"
            :options="[
              { value: '1', text: '現金' },
              { value: '2', text: '信用卡' },
            ]"
          />

          <NormalSelector
            class="selector"
            v-if="addOrderDto.paymentMethod === '2'"
            labelText="請選擇讀卡機："
            :parentValue.sync="selectedReader"
            :options="[
              { value: 'Reader01', text: 'Pax A920' },
              { value: 'Reader02', text: 'USB Reader' },
            ]"
          />
          <div v-if="addOrderDto.paymentMethod === '1'">
            <InputSpan
              class="inputSpanContainer"
              labelText="付款金額"
              inputType="number"
              v-model="paymentAmount"
            ></InputSpan>
            <p>找零：{{ change ? change + "元" : "X" }}</p>
          </div>
        </section>

        <!-- 5. 確認下單 -->
        <section>
          <h3>完成訂單</h3>
          <BtnNormal text="➕ 建立訂單並結帳" @click="submitOrder" />
        </section>
      </div>
      <div v-else-if="step === 2">
        <section>
          <h3>選擇教練</h3>
          <SelectInput
            class="selectPlan"
            :parentValue.sync="addOrderDto.coachId"
            :options="coachOptions"
          />
          <h3>結帳</h3>
          <BtnNormal text="➕ 進行結帳" @click="goCheckoutOrder" />
        </section>
      </div>
    </div>
  </div>
</template>

<script>
import RadioInput from "@/components/Input/RadioInput";
import SelectInput from "@/components/Input/SelectInput";
import SearchInput from "@/components/Input/SearchInput";
import NormalSelector from "@/components/Selector/NormalSelector";
import BtnNormal from "@/components/Btn/BtnNormal";
import InputSpan from "@/components/Input/InputSpan";
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
export default {
  name: "HealthopeAddOrder",
  components: {
    RadioInput,
    SelectInput,
    SearchInput,
    NormalSelector,
    BtnNormal,
    InputSpan,
    TitleCard,
    SubTitleCard,
  },
  data() {
    return {
      addOrderDto: {
        planType: "",
        planId: "",
        memberId: null,
        paymentMethod: "cash",
        coachId: "",
      },
      memberKeyword: "",
      paymentAmount: "",
      selectedReader: "",
      filteredMembers: [],
      selectedMember: null,
      selectDefaultFlag: true,
      plans: {
        membership: [
          { Id: "m1", Name: "1個月會籍", Price: 1200 },
          { Id: "m3", Name: "3個月會籍", Price: 3000 },
        ],
        training: [{ Id: "t5", Name: "5堂教練課", Price: 5000 }],
        ticket: [{ Id: "tk10", Name: "10次入場券", Price: 1000 }],
      },
      coaches: [],
      hintText: "",
      step: 1,
      orderId: Number,
      updateTime: String,
      selectPlan: {},
    };
  },
  computed: {
    filteredPlans() {
      if (this.addOrderDto.planType === "1") return this.plans["membership"];
      else if (this.addOrderDto.planType === "2") return this.plans["training"];
      else if (this.addOrderDto.planType === "3") return this.plans["ticket"];

      return [];
    },
    planOptions() {
      let options = [];
      this.filteredPlans.forEach((plan) => {
        const option = {
          value: plan.Id,
          text: `${plan.Name} - $${plan.Price}`,
        };
        options.push(option);
      });
      return options;
    },
    change() {
      if (this.addOrderDto.planId && this.paymentAmount) {
        const plan = this.filteredPlans.find(
          (plan) => plan.Id === this.addOrderDto.planId
        );

        if (plan) return this.paymentAmount - plan.Price;
      }
      return null;
    },
    coachOptions() {
      if (this.addOrderDto.planType !== "2" || this.step !== 2) return [];

      return this.coaches.map((coach) => ({
        value: coach.CoachId,
        text: coach.Name + ` (0${coach.Phone})`,
      }));
    },
  },
  methods: {
    changePlanType() {
      this.addOrderDto.planId = "";
      this.addOrderDto.coachId = "";
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
          this.filteredMembers = response.data.ApiDataObject;

          if (response.data.ApiDataObject.length === 0)
            this.selectDefaultFlag = false;
          else this.selectDefaultFlag = true;
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
        console.error("取得會員時發生錯誤", error);
      }
    },
    selectMember(member) {
      if (!member.PhoneVerified) {
        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "尚未驗證手機的會員不得購買方案!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      this.selectedMember = member;
    },
    clearSelected() {
      this.selectedMember = null;
    },
    async submitOrder() {
      if (!this.validInput()) {
        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "發生錯誤!" + this.hintText;
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      this.selectPlan = this.filteredPlans.find(
        (plan) => plan.Id === this.addOrderDto.planId
      );

      if (!this.selectPlan) return;

      const addOrderDto = {
        MemberId: this.selectedMember.MemberId,
        PlanId: this.addOrderDto.planId,
        Method: this.addOrderDto.paymentMethod,
        PlanType: this.addOrderDto.planType,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/AddOrderWithTicket",
          addOrderDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          if (this.addOrderDto.planType === "2") {
            this.step = 2;
            this.orderId = response.data.ApiDataObject.OrderId;
            this.updateTime = response.data.ApiDataObject.UpdateTime;
            return;
          }

          const order = {
            OrderId: response.data.ApiDataObject.OrderId,
            UpdateTime: response.data.ApiDataObject.UpdateTime,
            MemberName: this.selectedMember.Name,
            MemberPhone: this.selectedMember.Phone,
            PlanName: this.selectPlan.Name,
            PaymentMethod: this.addOrderDto.paymentMethod,
            CardReaderId: this.selectedReader || null,
            Amount: this.selectPlan.Price,
          };

          this.$router.push({
            name: "HealthopeCheckoutOrder",
            query: {
              order: JSON.stringify(order),
            },
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
        console.error("新增訂單時發生錯誤", error);
      }
    },
    goCheckoutOrder() {
      const IntMax = 2147483647;
      let coachId = Number(this.addOrderDto.coachId);

      if (
        this.addOrderDto.planType === "2" &&
        (!Number.isInteger(coachId) || coachId < 1 || coachId > IntMax)
      ) {
        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "請選擇教練!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      let selectCoach = null;

      if (this.addOrderDto.coachId) {
        selectCoach = this.coaches.find((coach) => coach.CoachId === coachId);
      }

      if (!selectCoach) return;

      const order = {
        OrderId: this.orderId,
        UpdateTime: this.updateTimem,
        MemberName: this.selectedMember.Name,
        MemberPhone: this.selectedMember.Phone,
        PlanName: this.selectPlan.Name,
        PaymentMethod: this.addOrderDto.paymentMethod,
        CardReaderId: this.selectedReader || null,
        Amount: this.selectPlan.Price,
        CoachName: selectCoach.Name,
      };

      this.$router.push({
        name: "HealthopeCheckoutOrder",
        query: {
          order: JSON.stringify(order),
        },
      });
    },
    async getAllTypePlan() {
      try {
        // post
        const response = await this.$axios.post(
          "/api/PlanTemplate/GetAllTypePlan"
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.plans = { membership: [], training: [], ticket: [] };

          response.data.ApiDataObject.MembershipPlanList.forEach((plan) => {
            this.plans.membership.push({
              ...plan,
              Id: plan.MembershipPlanId,
            });
          });

          response.data.ApiDataObject.PersonalTrainingPackageList.forEach(
            (plan) => {
              this.plans.training.push({
                ...plan,
                Id: plan.PersonalTrainingPackageId,
              });
            }
          );

          response.data.ApiDataObject.TicketPlanList.forEach((plan) => {
            this.plans.ticket.push({
              ...plan,
              Id: plan.TicketPlanId,
              Name: "一次性票劵",
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
        console.error("取得方案時發生錯誤", error);
      }
    },
    validInput() {
      const IntMax = 2147483647;
      let planId = Number(this.addOrderDto.planId);
      if (
        !Number.isInteger(planId) ||
        planId < 1 ||
        // 超出安全整數範圍
        planId > IntMax
      ) {
        this.hintText = "方案錯誤";
        return false;
      }

      if (!this.selectedMember) {
        this.hintText = "請選擇會員";
        return false;
      }

      let memberId = Number(this.selectedMember.MemberId);
      if (
        !Number.isInteger(memberId) ||
        memberId < 1 ||
        // 超出安全整數範圍
        memberId > IntMax
      ) {
        this.hintText = "會員錯誤";
        return false;
      }

      if (
        this.addOrderDto.planType !== "1" &&
        this.addOrderDto.planType !== "2" &&
        this.addOrderDto.planType !== "3"
      ) {
        this.hintText = "方案類別錯誤";
        return false;
      }

      if (
        this.addOrderDto.paymentMethod !== "1" &&
        this.addOrderDto.paymentMethod !== "2"
      ) {
        this.hintText = "付款方式錯誤";
        return false;
      }

      if (this.addOrderDto.paymentMethod === "2" && !this.selectedReader) {
        this.hintText = "請選擇讀卡機";
        return false;
      }

      return true;
    },
    async getPersonalCoach() {
      // 已載入或非教練方案則跳過
      if (this.coaches.length > 0 || this.addOrderDto.planType !== "2") return;
      try {
        // post
        const response = await this.$axios.post("/api/Coach/GetPersonalCoach");

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.coaches = [];
          response.data.ApiDataObject.forEach((coach) => {
            this.coaches.push(coach);
          });
        } else {
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
  },
  watch: {
    step(newVal) {
      if (newVal === 2) this.getPersonalCoach();
    },
  },
  created() {
    this.getAllTypePlan();
  },
};
</script>

<style scoped>
.createOrder {
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

.selectPlan {
  width: 230px;
  max-width: 80%;
}

.paymentMethodSeletor {
  margin-right: 15px;
}

.inputSpanContainer {
  margin-top: 5px;
}

.filterMemberCard {
  background-color: white;
  margin: 1em 0;
  padding: 1em;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  cursor: pointer;
}

.filterMemberCard:hover {
  background: rgba(255, 255, 255, 0.668);
}
</style>
