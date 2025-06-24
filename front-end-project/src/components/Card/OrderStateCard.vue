<template>
  <div class="card">
    <div class="stateName">{{ this.stateText }}</div>
    <div class="stateTime">創建：{{ formatTime(state.CreateTime) }}</div>
    <div class="stateTime">更改：{{ formatTime(state.UpdateTime) }}</div>
    <div class="stateNote">
      <div v-if="!editing">
        備註：{{ state.Remark || "無" }}
        <button @click="enableEdit" class="editBtn">✏️ 編輯</button>
      </div>
      <div v-else>
        <textarea v-model="editRemark" class="noteInput" />
        <button @click="save" class="sabeBtn">💾 儲存</button>
        <button @click="cancel" class="cancelBtn">取消</button>
      </div>
    </div>
  </div>
</template>

<script>
import { orderStateAndText } from "@/utils/order";

export default {
  name: "OrderStateCard",
  props: ["state"],
  data() {
    return {
      editing: false,
      editRemark: this.state.Remark || "",
    };
  },
  methods: {
    formatTime(utc) {
      return new Date(utc).toLocaleString();
    },
    enableEdit() {
      this.editing = true;
    },
    cancel() {
      this.editing = false;
      this.editRemark = this.state.Remark || "";
    },
    save() {
      this.$emit("saveNote", {
        OrderStateId: this.state.OrderStateId,
        Remark: this.editRemark,
        UpdateTime: this.state.UpdateTime,
      });
      this.editing = false;
    },
  },
  computed: {
    stateText() {
      let text = "";
      orderStateAndText.forEach((state) => {
        if (Number(state.value) === this.state.State) {
          text = state.text;
        }
      });
      return text;
    },
  },
};
</script>

<style scoped>
.card {
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 12px;
  padding: 16px;
  width: calc(50% - 16px);
  box-shadow: 2px 2px 6px rgba(0, 0, 0, 0.1);
}

.stateName {
  font-weight: bold;
  font-size: 16px;
  margin-bottom: 8px;
}

.stateTime {
  font-size: 14px;
  color: #555;
  margin-bottom: 8px;
}

.stateNote {
  font-size: 14px;
  color: #333;
}

.editBtn {
  margin-left: 8px;
  font-size: 12px;
  background: none;
  color: #007bff;
  border: none;
  cursor: pointer;
}

.editBtn:hover {
  text-decoration: underline;
}

.noteInput {
  width: 100%;
  font-size: 14px;
  padding: 6px;
  margin-top: 4px;
  border: 1px solid #aaa;
  border-radius: 6px;
  resize: vertical;
}

.editActions {
  margin-top: 6px;
}

.saveBtn,
.cancelBtn {
  font-size: 13px;
  padding: 4px 8px;
  margin-right: 6px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.saveBtn {
  background-color: #4caf50;
  color: white;
}

.cancelBtn {
  background-color: #ccc;
  color: black;
}
</style>
