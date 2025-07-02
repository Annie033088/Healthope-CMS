<template>
  <div class="tableContainer">
    <table class="">
      <thead>
        <tr>
          <th>類型</th>
          <th>發票號碼</th>
          <th>狀態</th>
          <th>金額</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="invoice in invoices" :key="invoice.ElectronicInvoiceId">
          <td>{{ invoice.Category.Text }}</td>
          <td>{{ invoice.InvoiceNumber || "（尚未取得）" }}</td>
          <td>
            <span :class="['status', invoice.Status.Class]">
              {{ invoice.Status.Text }}
            </span>
          </td>
          <td>{{ invoice.TotalAmount }}</td>
          <td>
            <div
              v-if="
                invoice.BtnRetryFlag ||
                invoice.BtnVoidFlag ||
                invoice.BtnDiscountFlag
              "
              class="operationBtn"
            >
              <button
                v-if="invoice.BtnRetryFlag"
                @click="retryPrintInvoice(invoice)"
                class="retryButton"
              >
                補開
              </button>
              <button
                v-if="invoice.BtnDiscountFlag"
                @click="discountInvoice(invoice)"
                class="retryButton"
              >
                折讓
              </button>
              <button
                v-if="invoice.BtnVoidFlag"
                @click="voidInvoice(invoice)"
                class="retryButton"
              >
                作廢
              </button>
            </div>
            <span v-else class="noAction">—</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import {
  electronicInvoiceStatus,
  electronicInvoiceStatusAndText,
  electronicInvoiceCategoryAndText,
} from "@/utils/electronicInvoice";
export default {
  name: "OrderInvoiceTable",
  props: {
    invoices: {
      type: Array,
      required: true,
    },
  },
  methods: {
    retryPrintInvoice(invoice) {
      this.$emit("retryPrintInvoice", invoice);
    },
    discountInvoice(invoice) {
      this.$emit("discountInvoice", invoice);
    },
    voidInvoice(invoice) {
      this.$emit("voidInvoice", invoice);
    },
  },
  created() {
    this.invoices.forEach((invoice) => {
      if (invoice.Status.Value === electronicInvoiceStatus.PendingDiscount) {
        invoice.BtnDiscountFlag = true;
        invoice.Status.Class = "pending";
      }

      if (invoice.Status.Value === electronicInvoiceStatus.PendingVoid) {
        invoice.BtnVoidFlag = true;
        invoice.Status.Class = "pending";
      }

      if (invoice.Status.Value === electronicInvoiceStatus.Fail) {
        invoice.BtnRetryFlag = true;
        invoice.Status.Class = "failed";
      }

      if (invoice.Status.Value === electronicInvoiceStatus.Success) {
        invoice.Status.Class = "success";
      }

      electronicInvoiceStatusAndText.forEach((status) => {
        if (Number(status.value) === invoice.Status.Value) {
          invoice.Status.Text = status.text;
        }
      });
      electronicInvoiceCategoryAndText.forEach((category) => {
        if (Number(category.value) === invoice.Category.Value) {
          invoice.Category.Text = category.text;
        }
      });
    });
  },
};
</script>

<style scoped>
.status {
  padding: 4px 8px;
  border-radius: 4px;
  font-weight: bold;
}
.status.success {
  color: #2e7d32;
  background-color: #e0f2f1;
}
.status.failed {
  color: #c62828;
  background-color: #ffebee;
}
.status.pending {
  color: #6d4c41;
  background-color: #fff3e0;
}

.retryButton {
  background-color: #1976d2;
  color: white;
  border: none;
  padding: 5px 12px;
  border-radius: 4px;
  cursor: pointer;
}
.retryButton:hover {
  background-color: #1565c0;
}
.retryButton:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
.noAction {
  color: #999;
}
.operationBtn {
  display: flex;
  gap: 5px;
}

.tableContainer {
  overflow-x: auto;
  max-width: 1000px;
  margin: 0 auto; /* 置中 */
}

table {
  width: 100%;
  min-width: 600px;
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

tbody tr {
  border-bottom: 1px solid #e5e7eb;
}

tbody td {
  color: #1f2937;
}
</style>
