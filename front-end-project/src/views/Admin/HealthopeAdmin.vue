<template>
  <div>
    <TitleCard
      text="管理者清單"
      @refreshPage="$emit('refreshPage')"
    ></TitleCard>
    <div class="funtionColumn">
      <BtnNormal text="新增管理者" @click="redirect('/admin/add')"></BtnNormal>
      <SelectInput
        @select="selectAdminByAccount()"
        placeholder="Account..."
        v-model="searchAccount"
      ></SelectInput>
      <div class="statusContainer">
        <label class="labStatus">狀態： </label>
        <label class="labRadioBox">
          <input
            type="radio"
            name="radioStatus"
            value=""
            v-model="selectStatus"
            @change="selectAdminByStatus()"
          />
          <span class="textRadio">無</span>
        </label>
        <label class="labRadioBox">
          <input
            type="radio"
            name="radioStatus"
            value="true"
            v-model="selectStatus"
            @change="selectAdminByStatus()"
          />
          <span class="textRadio">啟用</span>
        </label>
        <label class="labRadioBox">
          <input
            type="radio"
            name="radioStatus"
            value="false"
            v-model="selectStatus"
            @change="selectAdminByStatus()"
          />
          <span class="textRadio">停用</span>
        </label>
      </div>
      <div class="sortContainer">
        <label class="labSort">排序選項：</label>
        <select
          class="selectSortOrder"
          v-model="selectSortOption"
          @change="getAdminData()"
        >
          <option value="">無</option>
          <option value="account">帳號</option>
          <option value="status">狀態</option>
        </select>
        <label class="labRadioBox">
          <input
            type="radio"
            name="radioSort"
            value="ascending"
            v-model="selectSortOrder"
            @change="getAdminData()"
          />
          <span class="textRadio">升序</span>
        </label>
        <label class="labRadioBox">
          <input
            type="radio"
            name="radioSort"
            value="descending"
            v-model="selectSortOrder"
            @change="getAdminData()"
          />
          <span class="textRadio">降序</span>
        </label>
      </div>
      <div class="selectRecordContainer">
        <label class="labRecord">顯示：</label>
        <select
          class="selectRecord"
          v-model="recordPerPage"
          @change="getAdminData()"
        >
          <option value="8">8</option>
          <option value="12">12</option>
          <option value="16">16</option>
        </select>
        <label class="">筆</label>
      </div>
      <BtnNormal text="差異紀錄"></BtnNormal>
    </div>
    <div class="adminTable">
      <div
        class="card"
        v-for="(admin, index) in adminList"
        :key="index"
        @click="goEditAdmin(admin.AdminId)"
      >
        <div class="cardContent">
          <div class="cardImage">
            <svg
              width="50"
              height="50"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M12 4C13.0609 4 14.0783 4.42143 14.8284 5.17157C15.5786 5.92172 16 6.93913 16 8C16 9.06087 15.5786 10.0783 14.8284 10.8284C14.0783 11.5786 13.0609 12 12 12C10.9391 12 9.92172 11.5786 9.17157 10.8284C8.42143 10.0783 8 9.06087 8 8C8 6.93913 8.42143 5.92172 9.17157 5.17157C9.92172 4.42143 10.9391 4 12 4ZM12 14C16.42 14 20 15.79 20 18V20H4V18C4 15.79 7.58 14 12 14Z"
                fill="white"
              />
            </svg>
          </div>
          <div class="cardTitle">
            <div class="cardMainTitle">
              <span>{{ admin.Account }}</span>
            </div>
            <div class="cardSubTitle">
              <span>狀態：{{ admin.Status ? "啟用" : "停用" }}</span>
            </div>
          </div>
          <div class="cardEndText">
            <span>{{ adminIdentityToText(admin.Identity) }}</span>
          </div>
        </div>

        <div class="" @click.stop="delAdmin(admin.AdminId)">
          <svg
            class="btn btnDeleteAdmin"
            width="40"
            height="40"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M7.616 20C7.168 20 6.78667 19.8426 6.472 19.528C6.15733 19.2133 6 18.8323 6 18.385V5.99998H5V4.99998H9V4.22998H15V4.99998H19V5.99998H18V18.385C18 18.845 17.846 19.2293 17.538 19.538C17.23 19.8466 16.8453 20.0006 16.384 20H7.616ZM17 5.99998H7V18.385C7 18.5643 7.05767 18.7116 7.173 18.827C7.28833 18.9423 7.436 19 7.616 19H16.385C16.5383 19 16.6793 18.936 16.808 18.808C16.9367 18.68 17.0007 18.5386 17 18.384V5.99998ZM9.808 17H10.808V7.99998H9.808V17ZM13.192 17H14.192V7.99998H13.192V17Z"
              fill="#AA7F7F"
            />
          </svg>
        </div>
      </div>
    </div>
    <div>
      <PaginationComponent
        @searchPage="searchPage"
        :currentPage="currentPage"
        :totalPage="totalPage"
      ></PaginationComponent>
    </div>
  </div>
</template>

<script>
import BtnNormal from "@/components/Btn/BtnNormal";
import TitleCard from "@/components/Card/TitleCard";
import PaginationComponent from "@/components/PaginationComponent";
import SelectInput from "@/components/Input/SelectInput";
import adminIdentityToText from "../../utils/globalSetting";

export default {
  name: "HealthopeAdmin",
  components: {
    BtnNormal,
    TitleCard,
    PaginationComponent,
    SelectInput,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
    text: String,
  },
  data() {
    return {
      adminList: [],
      selectStatus: "",
      selectSortOrder: "ascending",
      selectSortOption: "",
      recordPerPage: "8",
      searchAccount: "",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
    };
  },
  methods: {
    adminIdentityToText,
    async goEditAdmin(adminId) {
      if (adminId < 1) return;
      this.$router.push({ path: "/admin/edit", query: { id: adminId } });
    },
    redirect(page) {
      if (this.$route.path !== page) {
        this.$router.push(page);
      }
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getAdminData();
    },
    selectAdminByAccount() {
      this.searchingPage = 1;
      this.searchAccount = this.searchAccount.trim();
      if (this.searchAccount === "") {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入長度需至少 2 位數";
        this.$notificationBox.notificationBoxErrorCode = 5;
        return;
      }
      this.getAdminData();
    },
     selectAdminByStatus() {
      this.searchingPage = 1;
      this.getAdminData();
    },
    async getAdminData() {
      // 驗證參數
      if (!(this.searchAccount.length > 1 || this.searchAccount === "")) {
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle = "輸入長度需至少 2 位數";
        this.$notificationBox.notificationBoxErrorCode = 5;
        return;
      }
      if (
        !(
          this.selectStatus === "true" ||
          this.selectStatus === "false" ||
          this.selectStatus === ""
        )
      )
        return;
      if (
        !(
          this.selectSortOrder === "ascending" ||
          this.selectSortOrder === "descending"
        )
      )
        return;
      if (
        !(
          this.selectSortOption === "account" ||
          this.selectSortOption === "status" ||
          this.selectSortOption === ""
        )
      )
        return;
      if (
        !(
          this.recordPerPage === "8" ||
          this.recordPerPage === "12" ||
          this.recordPerPage === "16"
        )
      )
        return;
      if (this.searchingPage < 1) return;

      // post 的 dto 變數
      let getADminDto = {
        Status: this.selectStatus || null,
        SortOrder: this.selectSortOrder,
        SortOption: this.selectSortOption || null,
        RecordPerPage: this.recordPerPage,
        SearchAccount: this.searchAccount || null,
        Page: this.searchingPage,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Admin/GetAdmin",
          getADminDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.adminList = response.data.ApiDataObject.AdminList;
          this.totalPage = response.data.ApiDataObject.TotalPage;
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
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
        console.error("取得管理者列表時發生錯誤", error);
      }
    },
    delAdmin(id) {
      // 添加監聽器，查看彈窗是否被按確認鍵
      this.unwatchFlag = this.$watch("notificationBoxConfirmFlag", (newVal) => {
        if (newVal) {
          let redirectRoute = "stop";
          this.$emit("afterConfirmEvent", redirectRoute);

          try {
            this.submitDelAdmin(id);
          } catch (error) {
            console.error("刪除管理員時發生錯誤", error);
          } finally {
            this.unwatchFlag(); // 確保監聽被移除
            this.unwatchFlag = null;
          }
        }
      });

      // 設定彈窗資料
      this.$notificationBox.notificationBoxFlag = true;
      this.$notificationBox.notificationBoxTitle = "此操作不可修改，確認刪除?";
      this.$notificationBox.notificationBoxCancelFlag = true;
      this.$notificationBox.notificationBoxErrorCode = 0;
    },
    async submitDelAdmin(id) {
      if (id < 1) return;

      try {
        // post
        let adminIdDto = {
          AdminId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Admin/DeleteAdmin",
          adminIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.$emit("refreshPage");
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
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
        console.error("刪除失敗", error);
      }
    },
  },
  created() {
    this.getAdminData();
  },
};
</script>

<style scoped>
/* 功能列 樣式*/
.funtionColumn {
  margin: 15px;
  display: flex;
  flex-wrap: wrap;
  gap: 10px 20px;
}

.labRadioBox {
  flex: 1 1 auto;
  text-align: center;
  justify-content: center;
}

.labRadioBox input {
  display: none;
}

.labRadioBox .textRadio {
  display: flex;
  cursor: pointer;
  justify-content: center;
  border-radius: 0.5rem;
  padding: 0.5rem 0;
  transition: all 0.15s ease-in-out;
}

.labRadioBox input:checked + .textRadio {
  background-color: #fff;
  font-weight: 600;
}

.labStatus,
.labSort,
.labRecord {
  padding: 0.5rem 0.5rem;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
}

.sortContainer,
.selectRecordContainer,
.statusContainer {
  display: flex;
  align-items: center;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.25rem;
  width: 300px;
  font-size: 14px;
}

.selectSortOrder,
.selectRecord {
  border: none;
  border-radius: 0.5rem;
  padding: 0.5rem 0.5rem;
  margin-right: 5px;
  background-color: #fafbfc;
}

.selectSort:hover,
.selectRecord:hover {
  cursor: pointer;
}

.selectRecordContainer {
  width: 150px;
}
</style>

<style scoped>
/* 列表 樣式*/
.adminTable {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
}

.card {
  width: 50%;
  min-width: 300px;
  background: #ffff;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
  padding: 12px 10px;
  margin: 5px;
  display: flex;
  justify-content: space-between;
  cursor: pointer;
}

.card:hover {
  background: rgba(255, 255, 255, 0.668);
}

.cardContent {
  display: flex;
}

.cardImage {
  width: 50px;
  height: 50px;
  background: #727272e7;
  border-radius: 10%;
}

.cardTitle {
  margin-left: 20px;
}

.cardMainTitle {
  font-size: 20px;
}

.cardSubTitle {
  margin-top: 5px;
  font-size: 16px;
  color: #828282;
}

.cardEndText {
  margin-left: 30px;
  font-size: 18px;
  color: #aa7f7f;
}

.btnDeleteAdmin:hover {
  background-color: #fffffffe;
  border-radius: 50%;
}
</style>
