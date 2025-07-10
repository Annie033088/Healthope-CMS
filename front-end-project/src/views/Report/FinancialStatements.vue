<template>
  <div>
    <TitleCard text="財務報表" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <NormalSelector
        class=""
        labelText="區間："
        :parentValue.sync="selectInterval"
        :options="[
          { value: 'month', text: '月' },
          { value: 'year', text: '年' },
        ]"
        @change="changeInterval"
      />
      <NormalSelector
        class=""
        labelText="年份："
        :parentValue.sync="selectYear"
        :options="yearOptions"
        @change="getReport"
      />
      <NormalSelector
        v-if="selectInterval === 'month'"
        class=""
        labelText="月份："
        :parentValue.sync="selectMonth"
        :options="monthOptions"
        @change="getReport"
      />
    </div>
    <div class="incomeExpenditureChart">
      <PieChart :chartData="intervalIncome" title="總收入" />
      <PieChart :chartData="intervalExpenditure" title="總支出" />
      <div class="totalAmount">
        <div class="card">
          <h4>總收入</h4>
          <p class="positiveAmount">${{ formatNumber(totalIncome) }}</p>
        </div>
        <div class="card">
          <h4>總支出</h4>
          <p class="negativeAmount">${{ formatNumber(totalExpenditure) }}</p>
        </div>
        <div class="card">
          <h4>總損益</h4>
          <p
            :class="
              intervalFinantialStatement.NetRevenue > 0
                ? 'positiveAmount'
                : 'negativeAmount'
            "
          >
            ${{ formatNumber(intervalFinantialStatement.NetRevenue) }}
          </p>
        </div>
      </div>
    </div>
    <div class="trendChart">
      <TrendChart
        :dailyData="finantialStatements"
        :trendMode="selectInterval === 'month' ? 'day' : 'month'"
        title="商品收入趨勢圖"
      ></TrendChart>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import NormalSelector from "@/components/Selector/NormalSelector";
import PieChart from "@/components/Chart/PieChart";
import TrendChart from "@/components/Chart/TrendChart";

export default {
  name: "FinancialStatements",
  components: {
    TitleCard,
    NormalSelector,
    PieChart,
    TrendChart,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectInterval: "month",
      selectYear: "",
      selectMonth: "",
      finantialStatements: [],
      intervalIncome: {},
      intervalExpenditure: {},
      intervalFinantialStatement: {
        MembershipRevenue: 0,
        PersonalTrainingRevenue: 0,
        SingleEntryRevenue: 0,
        TotalRevenue: 0,
        RefundExpense: 0,
        PenaltyIncome: 0,
        NetRevenue: 0,
      },
    };
  },
  methods: {
    changeInterval() {
      this.getReport();
    },
    async getReport() {
      let getReportDto;

      if (this.selectInterval === "month") {
        getReportDto = {
          Year: this.selectYear,
          Month: this.selectMonth,
        };
      } else if (this.selectInterval === "year") {
        getReportDto = {
          Year: this.selectYear,
          Month: null,
        };
      }

      try {
        // post
        const response = await this.$axios.post(
          "/api/Report/GetRevenueExpenseReport",
          getReportDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.finantialStatements = response.data.ApiDataObject;
          this.intervalIncome = {};
          this.intervalExpenditure = {};

          this.intervalFinantialStatement = {
            MembershipRevenue: 0,
            PersonalTrainingRevenue: 0,
            SingleEntryRevenue: 0,
            TotalRevenue: 0,
            RefundExpense: 0,
            PenaltyIncome: 0,
            NetRevenue: 0,
          };

          this.finantialStatements.forEach((finantialStatement) => {
            this.intervalFinantialStatement.MembershipRevenue +=
              finantialStatement.MembershipRevenue;
            this.intervalFinantialStatement.PersonalTrainingRevenue +=
              finantialStatement.PersonalTrainingRevenue;
            this.intervalFinantialStatement.SingleEntryRevenue +=
              finantialStatement.SingleEntryRevenue;
            this.intervalFinantialStatement.PenaltyIncome +=
              finantialStatement.PenaltyIncome;
            this.intervalFinantialStatement.RefundExpense +=
              finantialStatement.RefundExpense;
          });

          this.intervalFinantialStatement.NetRevenue =
            this.intervalFinantialStatement.MembershipRevenue +
            this.intervalFinantialStatement.PersonalTrainingRevenue +
            this.intervalFinantialStatement.SingleEntryRevenue +
            this.intervalFinantialStatement.PenaltyIncome -
            this.intervalFinantialStatement.RefundExpense;

          this.intervalIncome["會籍收入"] =
            this.intervalFinantialStatement.MembershipRevenue;
          this.intervalIncome["教練課收入"] =
            this.intervalFinantialStatement.PersonalTrainingRevenue;
          this.intervalIncome["單次入場"] =
            this.intervalFinantialStatement.SingleEntryRevenue;
          this.intervalIncome["違約金"] =
            this.intervalFinantialStatement.PenaltyIncome;
          this.intervalExpenditure["退費支出"] =
            this.intervalFinantialStatement.RefundExpense;
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
        console.error("取得財務報表時發生錯誤", error);
      }
    },
    formatNumber(num) {
      return num.toLocaleString();
    },
  },
  computed: {
    yearOptions() {
      const startYear = 2000;
      const endYear = new Date().getFullYear(); // 當前年份，例如 2025
      let options = [];

      for (let year = endYear; year >= startYear; year--) {
        options.push({
          value: year,
          text: year,
        });
      }

      return options;
    },
    monthOptions() {
      let options = [];

      for (let month = 1; month < 13; month++) {
        options.push({
          value: month,
          text: month,
        });
      }

      return options;
    },
    totalIncome() {
      return (
        this.intervalFinantialStatement.MembershipRevenue +
        this.intervalFinantialStatement.PersonalTrainingRevenue +
        this.intervalFinantialStatement.SingleEntryRevenue +
        this.intervalFinantialStatement.PenaltyIncome
      );
    },
    totalExpenditure() {
      return this.intervalFinantialStatement.RefundExpense;
    },
  },
  created() {
    this.selectYear = new Date().getFullYear();
    this.selectMonth = new Date().getMonth() + 1;

    this.getReport();
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
</style>

<style scoped>
.incomeExpenditureChart {
  width: 90%;
  display: flex;
  flex-wrap: wrap;
  gap: 5px;
}

.totalAmount {
  padding: 15px;
}

.totalAmount p {
  font-size: 21px;
}

.totalAmount .card {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  padding: 10px 6px;
  width: 200px;
  text-align: center;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  margin-bottom: 10px;
}

.totalAmount .card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.totalAmount .card h4 {
  margin: 0;
  font-size: 16px;
  color: #666;
}

.totalAmount .card .positiveAmount {
  font-size: 24px;
  font-weight: bold;
  margin-top: 8px;
  color: #2e7d32;
}

.totalAmount .card .negativeAmount {
  font-size: 24px;
  font-weight: bold;
  margin-top: 8px;
  color: #f44336;
}

.trendChart {
  width: 100%;
}
</style>