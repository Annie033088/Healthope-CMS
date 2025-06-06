<template>
  <div>
    <TitleCard text="訂單" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="新增訂單"></SubTitleCard>
    <div class="createOrder">
      <h2>新增訂單</h2>

      <!-- 1. 選擇方案類型 -->
      <section>
        <h3>1. 選擇方案類型</h3>
        <RadioInput
          class="timeSelector"
          v-model="addOrderDto.productType"
          inputType="radioTime"
          :options="[
            { value: 'membership', text: '會籍方案' },
            { value: 'training', text: '教練課方案' },
            { value: 'ticket', text: '票券方案' },
          ]"
        />
      </section>

      <!-- 2. 選擇商品 -->
      <section v-if="addOrderDto.productType">
        <h3>2. 選擇商品</h3>
        <SelectInput
          class="selectProduct"
          :parentValue.sync="addOrderDto.productId"
          :options="productOptions"
        />
      </section>

      <!-- 3. 搜尋或新增會員 -->
      <section>
        <h3>3. 選擇會員</h3>
        <SearchInput
          @search="searchMember"
          placeholder="搜尋會員（姓名或電話）"
          v-model="memberKeyword"
          @enter="searchMember"
        ></SearchInput>
        <div v-if="filteredMembers.length > 0">
          <h3>搜尋結果：</h3>
          <div class="member-list">
            <div
              v-for="member in filteredMembers"
              :key="member.id"
              class="member-card"
              @click="selectMember(member)"
            >
              <p>
                <strong>{{ member.name }}</strong>
              </p>
              <p>電話：{{ member.phone }}</p>
            </div>
          </div>
        </div>
        <div v-else-if="memberKeyword">
          <p>查無會員</p>
        </div>
        <div v-if="selectedMember">
          <h3>已選擇會員</h3>
          <div class="member-selected">
            <p>
              <strong>{{ selectedMember.name }}</strong>
            </p>
            <p>電話：{{ selectedMember.phone }}</p>
            <button @click="clearSelected">取消選擇</button>
          </div>
        </div>
      </section>

      <!-- 4. 選擇付款方式與發票 -->
      <section class="">
        <h3>4. 結帳資訊</h3>
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
            { value: '1', text: 'Pax A920' },
            { value: '2', text: 'USB Reader' },
          ]"
        />
        <div v-if="addOrderDto.paymentMethod === '1'">
          <InputSpan
            class="inputSpanContainer"
            labelText="付款金額"
            inputType="number"
            v-model="paymentAmount"
          ></InputSpan>
          <p>找零：{{ paymentAmount ? paymentAmount - 100 : "X" }}</p>
        </div>
      </section>

      <!-- 5. 確認下單 -->
      <section>
        <h3>5. 完成訂單</h3>
        <BtnNormal text="➕ 建立訂單並結帳" @click="submitOrder" />
      </section>
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
        productType: "",
        productId: "",
        memberId: null,
        paymentMethod: "cash",
      },
      memberKeyword: "",
      paymentAmount: "",
      selectedReader: "",
      filteredMembers: [],
      selectedMember: null,
      products: {
        membership: [
          { Id: "m1", Name: "1個月會籍", Price: 1200 },
          { Id: "m3", Name: "3個月會籍", Price: 3000 },
        ],
        training: [{ Id: "t5", Name: "5堂教練課", Price: 5000 }],
        ticket: [{ Id: "tk10", Name: "10次入場券", Price: 1000 }],
      },
    };
  },
  computed: {
    filteredProducts() {
      return this.products[this.addOrderDto.productType] || [];
    },
    productOptions() {
      let options = [];
      this.filteredProducts.forEach((product) => {
        const option = {
          value: product.id,
          text: `${product.Name} - $${product.Price}`,
        };
        options.push(option);
      });
      return options;
    },
  },
  methods: {
    async searchMember() {
      const keyword = this.keyword.trim();
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
          this.filteredMembers = response.data.ApiDataObject.MemberList;
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
      this.selectedMember = member;
    },
    clearSelected() {
      this.selectedMember = null;
    },
    submitOrder() {
      if (!this.addOrderDto.productId || !this.addOrderDto.memberId) {
        alert("請選擇商品與會員");
        return;
      }

      const order = {
        memberId: this.addOrderDto.memberId,
        productType: this.addOrderDto.productType,
        productId: this.addOrderDto.productId,
        paymentMethod: this.addOrderDto.paymentMethod,
      };

      // 前往結帳頁並傳入 order
      this.$router.push({ name: "HealthopeCheckoutOrder", params: { order } });
    },
    async getAllTypePlan() {
      try {
        // post
        const response = await this.$axios.post(
          "/api/PlanTemplate/GetAllTypePlan"
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.products = { membership: [], training: [], ticket: [] };

          response.data.ApiDataObject.MembershipPlanList.forEach((plan) => {
            this.products.membership.push({
              ...plan,
              Id: plan.MembershipPlanId,
            });
          });

          response.data.ApiDataObject.PersonalTrainingPackageList.forEach(
            (plan) => {
              this.products.training.push({
                ...plan,
                Id: plan.PersonalTrainingPackageId,
              });
            }
          );

          response.data.ApiDataObject.TicketPlanList.forEach((plan) => {
            this.products.ticket.push({
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

.selectProduct {
  width: 230px;
  max-width: 80%;
}

.paymentMethodSeletor {
  margin-right: 15px;
}

.inputSpanContainer {
  margin-top: 5px;
}
</style>
