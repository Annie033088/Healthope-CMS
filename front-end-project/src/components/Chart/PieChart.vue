<template>
  <div class="pieChartWrapper">
    <!-- 圖表標題與總金額 -->
    <div class="chartHeader">
      <h3>{{ title }}</h3>
      <p class="total">總金額：${{ formatNumber(total) }}</p>
    </div>

    <!-- 圓餅圖 -->
    <div class="pieChart" :style="{ background: pieGradient }"></div>

    <!-- 圖例 -->
    <div class="legend">
      <div v-for="(item, index) in pieItems" :key="index" class="legendItem">
        <span class="colorBox" :style="{ background: item.color }"></span>
        <span class="label">{{ item.label }}</span>
        <span class="amount">${{ formatNumber(item.amount) }}</span>
        <span class="percent">({{ item.percent.toFixed(1) }}%)</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "PieChart",
  props: {
    chartData: {
      type: Object,
      required: true,
    },
    title: {
      type: String,
      default: "圖表",
    },
  },
  data() {
    return {
      colors: [
        "#4caf50",
        "#2196f3",
        "#ff9800",
        "#f44336",
        "#9c27b0",
        "#00bcd4",
      ],
    };
  },
  computed: {
    total() {
      return Object.values(this.chartData).reduce((a, b) => a + b, 0);
    },
    pieItems() {
      let currentPercent = 0;
      let colorIndex = 0;

      return Object.entries(this.chartData).map(([label, amount]) => {
        const percent = (amount / this.total) * 100;
        const start = currentPercent;
        const end = currentPercent + percent;
        const color = this.colors[colorIndex++ % this.colors.length];
        currentPercent += percent;

        return {
          label,
          amount,
          color,
          percent,
          startPercent: start.toFixed(2),
          endPercent: end.toFixed(2),
        };
      });
    },
    pieGradient() {
      return `conic-gradient(${this.pieItems
        .map(
          (item) => `${item.color} ${item.startPercent}% ${item.endPercent}%`
        )
        .join(", ")})`;
    },
  },
  methods: {
    formatNumber(num) {
      return num.toLocaleString();
    },
  },
};
</script>

<style scoped>
.pieChartWrapper {
  text-align: center;
  font-family: sans-serif;
  max-width: 400px;
  margin: 0 auto;
}
.chartHeader h3 {
  margin-bottom: 0;
}
.chartHeader .total {
  font-weight: bold;
  margin-bottom: 16px;
}
.pieChart {
  width: 250px;
  height: 250px;
  border-radius: 50%;
  margin: 0 auto 20px;
}
.legend {
  display: flex;
  flex-direction: column;
  align-items: stretch;
  gap: 6px;
}
.legendItem {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 4px 12px;
  font-size: 15px;
  border-bottom: 1px solid #eee;
}
.colorBox {
  width: 12px;
  height: 12px;
  border-radius: 2px;
  margin-right: 8px;
}
.label {
  flex: 1;
  text-align: left;
}
.amount {
  width: 80px;
  text-align: right;
}
.percent {
  width: 60px;
  text-align: right;
  color: #666;
}
</style>
