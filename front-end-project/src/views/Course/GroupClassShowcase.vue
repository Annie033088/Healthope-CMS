<template>
  <div>
    <TitleCard text="團課展示" @refreshPage="$emit('refreshPage')" />
    <div class="functionColumn">
      <BtnNormal
        text="新增課程"
        @click="redirect('/groupClass/showcase/add')"
      ></BtnNormal>
      <SelectInput
        @select="selectClassByName"
        placeholder="Name..."
        v-model="searchName"
      ></SelectInput>
      <RadioSelector
        class="categorySelector"
        v-model="selectCategory"
        @change="selectClassByCategory"
        inputTitle="分類："
        inputType="radioCategory"
        :options="groupClassCategory"
      />
      <SortSelector
        :options="[
          { value: 'sort', label: '順序' },
          { value: 'name', label: '名稱' },
        ]"
        :sortOption.sync="selectSortOption"
        :sortOrder.sync="selectSortOrder"
        @change="getClassData"
      />
      <RecordSelector
        :parentValue.sync="recordPerPage"
        @change="getClassData"
      />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import BtnNormal from "@/components/Btn/BtnNormal";
import SvgReset from "@/components/Btn/SvgReset";
import SortSelector from "@/components/Selector/SortSelector";
import RecordSelector from "@/components/Selector/RecordSelector";
import RadioSelector from "@/components/Selector/RadioSelector";
import SelectInput from "@/components/Input/SelectInput";
import { groupClassCategory } from "@/utils/groupClass";

export default {
  name: "GroupClassShowcase",
  components: {
    BtnNormal,
    TitleCard,
    SelectInput,
    SvgReset,
    SortSelector,
    RecordSelector,
    RadioSelector,
  },
  data() {
    return {
      selectCategory: "",
      selectSortOrder: "ascending",
      selectSortOption: "",
      recordPerPage: "8",
      searchName: "",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
    };
  },
  methods: {
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    selectClassByName() {},
    selectClassByCategory() {},
    getClassData() {},
    resetSearchingRecord() {},
  },
  computed: {
    groupClassCategory() {
      let category = [...groupClassCategory] ;
      category.push({ value: "", text: "無" });
      return category;
    },
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

.categorySelector {
  width: 600px;
}
</style>