<template>
  <div class="tableContainer">
    <table>
      <thead>
        <tr>
          <th
            v-for="col in columns"
            :key="col.key"
            :style="{ width: avgWidth + '%' }"
          >
            {{ col.label }}
          </th>
          <th
            v-if="operationFlag"
            class="editHeadContainer"
            :style="{ width: '10%' }"
          >
            操作
          </th>
          <th v-if="checkDetailBtnFlag" :style="{ width: '10%' }">查看</th>
        </tr>
      </thead>
      <tbody>
        <template v-for="(row, index) in rows">
          <tr :key="'main-' + index" @click="toggleDetail(index)">
            <td v-for="col in columns" :key="col.key">
              <NormalSelector
                @click.stop=""
                v-if="col.type === 'dropDownSelector' && row[col.key]"
                :options="row[col.key].Options"
                :parentValue="row[col.key].Value"
                @change="
                  (value) => {
                    row[col.key].Value = value;
                    $emit(`change${col.key}`, row);
                  }
                "
                :disabled="!col.enableFlag"
              >
              </NormalSelector>
              <CheckboxInput
                v-else-if="
                  col.type === 'checkBoxInput' &&
                  (row[col.key] === true || row[col.key] === false)
                "
                :disabled="!col.enableFlag"
                v-model="row[col.key]"
                @change="
                  (value) => {
                    row[col.key] = value;
                    $emit(`change${col.key}`, row);
                  }
                "
              />
              <div v-else>
                {{ row[col.key] }}
              </div>
            </td>
            <td v-if="operationFlag" class="">
              <div class="editBtnContainer">
                <SvgEdit
                  v-if="editBtnFlag"
                  size="28"
                  @click.stop="$emit('goEdit', row)"
                ></SvgEdit>
                <SvgDelete
                  v-if="deleteBtnFlag"
                  size="30"
                  @click.stop="$emit('goDelete', row)"
                ></SvgDelete>
              </div>
            </td>
            <td v-if="checkDetailBtnFlag">
              <div class="editBtnContainer">
                <SvgCheckDetail
                  size="24"
                  @click.stop="$emit('goCheckDetail', row)"
                />
              </div>
            </td>
          </tr>
          <tr
            v-if="expandable && expandedIndex === index"
            :key="'detail-' + index"
            class="detailContainer"
          >
            <td :colspan="columns.length + 2">
              <slot name="detail" :row="row" />
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>

<script>
import SvgEdit from "@/components/Btn/SvgEdit";
import SvgDelete from "@/components/Btn/SvgDelete";
import SvgCheckDetail from "@/components/Btn/SvgCheckDetail";
import NormalSelector from "@/components/Selector/NormalSelector";
import CheckboxInput from "@/components/Input/CheckboxInput";

export default {
  name: "TableNormal",
  components: {
    SvgEdit,
    SvgDelete,
    SvgCheckDetail,
    NormalSelector,
    CheckboxInput,
  },
  props: {
    columns: {
      type: Array,
      required: true,
    },
    rows: {
      type: Array,
      required: true,
    },
    expandable: {
      type: Boolean,
      default: false,
    },
    editBtnFlag: {
      type: Boolean,
      default: false,
    },
    deleteBtnFlag: {
      type: Boolean,
      default: false,
    },
    checkDetailBtnFlag: {
      type: Boolean,
      default: false,
    },
    resetDetailIndexFlag: Boolean,
  },
  data() {
    return {
      expandedIndex: null,
    };
  },
  methods: {
    toggleDetail(index) {
      if (!this.expandable) return;
      this.expandedIndex = this.expandedIndex === index ? null : index;
    },
  },
  computed: {
    operationFlag() {
      return this.editBtnFlag || this.deleteBtnFlag;
    },
    avgWidth() {
      let column = this.columns.length;
      let width = 100;
      if (this.operationFlag) width -= 10;
      if (this.checkDetailBtnFlag) width -= 10;
      return width / column;
    },
  },
  watch: {
    resetDetailIndexFlag() {
      this.expandedIndex = null;
    },
  },
  created() {},
};
</script>

<style scoped>
.tableContainer {
  overflow-x: auto;
  max-width: 1000px;
  margin: 0 auto; /* 置中 */
}

table {
  width: 100%;
  border-collapse: collapse;
  border: 2px solid #e5e7eb;
  overflow: hidden;
  background-color: #ffff;
  font-family: sans-serif;
}

thead {
  text-align: left;
}

th,
td {
  padding: 12px 16px;
  white-space: nowrap;
  text-align: left;
}

th {
  font-weight: 500;
  color: #1f2937;
  border-bottom: 2px solid #e5e7eb;
}

tbody tr:hover {
  background-color: rgba(249, 248, 248, 0.668);
}

tbody tr {
  border-bottom: 1px solid #e5e7eb;
}

tbody td {
  color: #1f2937;
}

.detailContainer td {
  background-color: #f9f9f9;
  font-size: 14px;
  color: #444;
}

.editBtnContainer {
  display: flex;
  justify-content: start;
}
</style>