<template>
  <div class="chartContainer">
    <!-- 圖表標題 -->
    <div class="chartHeader">
      <h3>{{ title }}</h3>
    </div>

    <svg :width="width" :height="height" v-if="processedData.length > 1">
      <!-- Y 軸刻度線與文字 -->
      <g v-for="(y, i) in ySteps" :key="'y-' + i">
        <line
          :x1="padding"
          :y1="getY(y)"
          :x2="width - padding"
          :y2="getY(y)"
          stroke="#ccc"
          stroke-dasharray="4"
        />
        <text
          :x="padding - 10"
          :y="getY(y) + 5"
          text-anchor="end"
          font-size="10"
        >
          {{ y }}
        </text>
      </g>

      <!-- X 軸日期文字 -->
      <g v-for="(label, i) in labels" :key="'x-' + i">
        <text
          :x="getX(i)"
          :y="height - padding + 15"
          text-anchor="middle"
          font-size="10"
        >
          {{ label }}
        </text>
      </g>

      <!-- 三條折線 -->
      <polyline
        v-for="(points, idx) in linePoints"
        :key="'line-' + idx"
        :points="points"
        :stroke="colors[idx]"
        fill="none"
        stroke-width="2"
      />
    </svg>
    <p v-else class="">無足夠資料繪圖</p>

    <!-- 圖例 -->
    <div v-if="processedData.length > 1" class="legend">
      <span
        v-for="(name, i) in ['會籍收入', '教練課收入', '票券收入']"
        :key="i"
        :style="{ color: colors[i] }"
      >
        ■ {{ name }}
      </span>
    </div>
  </div>
</template>

<script>
export default {
  name: "TrendChart",
  props: {
    trendMode: {
      type: String, // 'day' or 'month'
      default: "day",
    },
    dailyData: {
      type: Array,
      required: true,
    },
    title: {
      type: String,
      default: "趨勢圖",
    },
  },
  data() {
    return {
      width: 800,
      height: 400,
      padding: 40,
      colors: ["#007bff", "#28a745", "#ffc107"], // 三條線的顏色
    };
  },
  computed: {
    labels() {
      return this.processedData.map((d) => d.label || `${d.Month}/${d.Day}`);
    },
    maxY() {
      const allValues = this.processedData
        .flatMap((d) => [
          d.MembershipRevenue,
          d.PersonalTrainingRevenue,
          d.SingleEntryRevenue,
        ])
        .filter((n) => typeof n === "number" && !isNaN(n));

      const max = Math.max(...allValues);
      return max > 0 ? Math.ceil(max / 1000) * 1000 : 1000;
    },
    ySteps() {
      const steps = 5;
      const stepValue = this.maxY / steps;
      return Array.from(
        { length: steps + 1 },
        (_, i) => i * stepValue
      ).reverse();
    },
    linePoints() {
      const getY = (val) => {
        const usableHeight = this.height - this.padding * 2;
        return this.padding + (1 - val / this.maxY) * usableHeight;
      };
      const getX = (idx) => {
        const usableWidth = this.width - this.padding * 2;
        const step = usableWidth / (this.processedData.length - 1);
        return this.padding + idx * step;
      };

      const buildLine = (key) => {
        return this.processedData
          .map((d, i) => {
            const x = getX(i);
            const y = getY(d[key]);
            if (!isFinite(x) || !isFinite(y)) return null;
            return `${x},${y}`;
          })
          .filter((p) => p !== null)
          .join(" ");
      };

      return [
        buildLine("MembershipRevenue"),
        buildLine("PersonalTrainingRevenue"),
        buildLine("SingleEntryRevenue"),
      ];
    },
    getY() {
      return (value) => {
        const usableHeight = this.height - this.padding * 2;
        return this.padding + (1 - value / this.maxY) * usableHeight;
      };
    },
    getX() {
      return (index) => {
        const usableWidth = this.width - this.padding * 2;
        const dataLength = this.processedData.length;
        if (dataLength <= 1) return this.padding; // 回傳固定值避免 NaN

        const step = usableWidth / (dataLength - 1);
        return this.padding + index * step;
      };
    },
    cleanedData() {
      return this.dailyData.map((d) => ({
        Day: d.Day,
        Year: d.Year || new Date().getFullYear(), // 若沒提供年份，給預設
        Month: d.Month || new Date().getMonth() + 1,
        MembershipRevenue: isFinite(d.MembershipRevenue)
          ? d.MembershipRevenue
          : 0,
        PersonalTrainingRevenue: isFinite(d.PersonalTrainingRevenue)
          ? d.PersonalTrainingRevenue
          : 0,
        SingleEntryRevenue: isFinite(d.SingleEntryRevenue)
          ? d.SingleEntryRevenue
          : 0,
      }));
    },
    processedData() {
      if (this.trendMode === "day") return this.cleanedData;

      const map = new Map();
      this.cleanedData.forEach((d) => {
        const key = `${d.Year}-${("0" + d.Month).slice(-2)}`;
        if (!map.has(key)) {
          map.set(key, {
            label: key,
            MembershipRevenue: 0,
            PersonalTrainingRevenue: 0,
            SingleEntryRevenue: 0,
          });
        }
        const agg = map.get(key);
        agg.MembershipRevenue += d.MembershipRevenue;
        agg.PersonalTrainingRevenue += d.PersonalTrainingRevenue;
        agg.SingleEntryRevenue += d.SingleEntryRevenue;
      });

      return Array.from(map.values());
    },
  },
};
</script>

<style scoped>
.chartContainer {
  display: flex;
  flex-direction: column;
  align-items: center;
  max-width: 80%;
  min-width: 300px;
  margin: auto;
  overflow-x: auto;
}

.legend {
  margin-top: 8px;
  display: flex;
  gap: 16px;
  font-size: 14px;
}
</style>
