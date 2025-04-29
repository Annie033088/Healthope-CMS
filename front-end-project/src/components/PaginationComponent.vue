<template>
  <div class="paginationContainer">
    <div class="paginationBtnContainer">
      <BtnNormal
        text="＜"
        class="btnNormal"
        @click="searchPage(currentPage - 1)"
      ></BtnNormal>
      <BtnNormal
        text="1"
        class="btnNormal"
        :class="{ currentPage: isCurrentPage('1') }"
        @click="searchPage(1)"
      ></BtnNormal>
      <BtnNormal
        v-for="(btnText, index) in visibleBtnTextList"
        :key="index"
        :text="btnText"
        class="btnNormal"
        :class="{ currentPage: isCurrentPage(btnText) }"
        @click="searchPage(btnText)"
      ></BtnNormal>
      <BtnNormal
        v-if="totalPage > 5"
        :text="totalPage"
        class="btnNormal"
        :class="{ currentPage: isCurrentPage(totalPage) }"
        @click="searchPage(totalPage)"
      ></BtnNormal>
      <BtnNormal
        text="＞"
        class="btnNormal"
        @click="searchPage(currentPage + 1)"
      ></BtnNormal>
    </div>

    <div class="searchPageContainer">
      <label class="">第</label>
      <input
        class="searchPage"
        v-model="txbSearchPage"
        @keydown.enter="searchPage(txbSearchPage)"
      />
      <label class="">頁</label>
    </div>
  </div>
</template>
<script>
import BtnNormal from "@/components/Btn/BtnNormal";

export default {
  name: "PaginationComponent",
  props: {
    currentPage: {
      type: Number,
    },
    totalPage: {
      type: Number,
    },
  },
  data() {
    return {
      txbSearchPage: "",
    };
  },
  components: {
    BtnNormal,
  },
  methods: {
    isCurrentPage(text) {
      const num = Number(text);
      return !isNaN(num) && num === this.currentPage;
    },
    searchPage(page) {
      const pageNum = Number(page);

      if (
        pageNum < 1 ||
        isNaN(pageNum) ||
        pageNum === this.currentPage ||
        pageNum > this.totalPage
      )
        return;

      this.$emit("searchPage", pageNum);
    },
  },
  computed: {
    visibleBtnTextList() {
      return this.btnTextList.filter((_, index) => this.totalPage > index + 1);
    },
    btnTextList() {
      if (this.currentPage < 5) {
        return ["2", "3", "4", "5", "‧‧‧"];
      }

      if (this.currentPage > this.totalPage - 4) {
        return [
          "‧‧‧",
          this.totalPage - 4,
          this.totalPage - 3,
          this.totalPage - 2,
          this.totalPage - 1,
        ];
      }

      return [
        "‧‧‧",
        this.currentPage - 1,
        this.currentPage,
        this.currentPage + 1,
        "‧‧‧",
      ];
    },
  },
};
</script>

<style scoped>
.btnNormal {
  color: #5c5c5cc6;
  max-width: 38px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.paginationContainer {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
  margin-top: 10px;
}

.paginationBtnContainer {
  display: flex;
  justify-content: center;
  gap: 2px;
}

.currentPage {
  background: #b2dbb594;
}

.currentPage:hover {
  background: #a6cdaab5;
}

.searchPageContainer {
  display: flex;
  align-items: center;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.25rem;
  font-size: 14px;
  gap: 5px;
}

.searchPage {
  border-color: #fafbfc;
  border-radius: 0.5rem;
  padding: 0.2rem 0.2rem;
  max-width: 25px;
  background-color: #fafbfc;
}
</style>