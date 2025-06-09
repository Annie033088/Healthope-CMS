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
          @change="changePlanType"
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
      selectDefaultFlag: true,
      products: {
        membership: [
          { Id: "m1", Name: "1個月會籍", Price: 1200 },
          { Id: "m3", Name: "3個月會籍", Price: 3000 },
        ],
        training: [{ Id: "t5", Name: "5堂教練課", Price: 5000 }],
        ticket: [{ Id: "tk10", Name: "10次入場券", Price: 1000 }],
      },
      hintText: "",
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
          value: product.Id,
          text: `${product.Name} - $${product.Price}`,
        };
        options.push(option);
      });
      return options;
    },
    change() {
      if (this.addOrderDto.productId && this.paymentAmount) {
        const product = this.filteredProducts.find(
          (product) => product.Id === this.addOrderDto.productId
        );

        if (product) return this.paymentAmount - product.Price;
      }
      return null;
    },
  },
  methods: {
    changePlanType() {
      this.addOrderDto.productId = "";
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

      let amount = null;
      let productName = null;

      this.filteredProducts.forEach((product) => {
        if (product.Id === this.addOrderDto.productId) {
          amount = product.Price;
          productName = product.Name;
        }
      });

      if (!amount) return;

      const addOrderDto = {
        MemberId: this.selectedMember.MemberId,
        ProductType: this.addOrderDto.productType,
        ProductId: this.addOrderDto.productId,
        PaymentMethod: this.addOrderDto.paymentMethod,
        CardReaderId: this.selectedReader,
        Amount: amount,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Order/AddOrder",
          addOrderDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          const order = {
            OrderId:response.data.ApiDataObject.OrderId,
            MemberName: this.selectedMember.Name,
            MemberPhone: this.selectedMember.Phone,
            ProductName: productName,
            PaymentMethod: this.addOrderDto.paymentMethod,
            Amount: amount,
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
    validInput() {
      const IntMax = 2147483647;
      let productId = Number(this.addOrderDto.productId);
      if (
        !Number.isInteger(productId) ||
        productId < 1 ||
        // 超出安全整數範圍
        productId > IntMax
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
        this.addOrderDto.productType !== "membership" &&
        this.addOrderDto.productType !== "training" &&
        this.addOrderDto.productType !== "ticket"
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
