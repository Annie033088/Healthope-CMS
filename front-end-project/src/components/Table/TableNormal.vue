<template>
  <div class="tableContainer">
    <table>
      <thead>
        <tr>
          <th v-for="col in columns" :key="col.key">
            {{ col.label }}
          </th>
          <th v-if="operationFlag">操作</th>
        </tr>
      </thead>
      <tbody>
        <template v-for="(row, index) in rows">
          <tr :key="'main-' + index" @click="toggleDetail(index)">
            <td v-for="col in columns" :key="col.key">
              {{ row[col.key] }}
            </td>
            <td v-if="operationFlag" class="editBtnContainer">
              <SvgEdit v-if="editBtnFlag" size="28" @click.stop="$emit('goEdit', row)"></SvgEdit>
              <SvgDelete
                v-if="deleteBtnFlag"
                size="32"
                @click.stop=""
              ></SvgDelete>
            </td>
          </tr>
          <tr
            v-if="expandedIndex === index"
            :key="'detail-' + index"
            class="detailContainer"
          >
            <td :colspan="columns.length + 1">
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

export default {
  name: "TableNormal",
  components: {
    SvgEdit,
    SvgDelete,
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
    onEdit(row) {
      console.log(row);
    },
  },
  computed: {
    operationFlag() {
      return this.editBtnFlag || this.deleteBtnFlag;
    },
  },
  watch: {
    resetDetailIndexFlag() {
      this.expandedIndex = null;
    },
  },
};
</script>

<style scoped>
.tableContainer {
  overflow-x: auto;
  max-width: 1000px; /* 限制表格最大寬度 */
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
  align-items: center;
}

/* RWD - 手機版卡片樣式 */
/* @media (max-width: 640px) {
    table,
    thead,
    tbody,
    th,
    td,
    tr {
    border: none 
    }

    thead {
      display: none;
    }

    tbody tr {
      margin-bottom: 1rem;
      border: 1px solid #e5e7eb;
      border-radius: 0.5rem;
      padding: 0.5rem;
      background-color: #fff;
    }

    tbody td {
      white-space: normal;
      padding: 0.5rem 0;
      position: relative;
      padding-left: 50%;
    }

    tbody td::before {
      position: absolute;
      top: 0.5rem;
      left: 0.75rem;
      width: 45%;
      padding-right: 0.5rem;
      white-space: nowrap;
      font-weight: 600;
      color: #6b7280;
    }

    tbody td:nth-child(1)::before {
      content: "Name";
    }

    tbody td:nth-child(2)::before {
      content: "DoB";
    }

    tbody td:nth-child(3)::before {
      content: "Role";
    }

    tbody td:nth-child(4)::before {
      content: "Salary";
    } 
  }*/
</style>