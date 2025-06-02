<template>
  <div>
    <TitleCard text="條款" @refreshPage="$emit('refreshPage')"></TitleCard>
    <div class="functionColumn">
      <BtnNormal text="新增條款" @click="redirect('/term/add')"></BtnNormal>
      <RadioSelector
        class="statusSelector"
        v-model="selectStatus"
        @change="selectTermByStatus"
        inputTitle="狀態："
        inputType="radioStatus"
        :options="termStatusAndTextOptions"
      />
      <RadioSelector
        class="typeSelector"
        v-model="selectType"
        @change="selectTermByType"
        inputTitle="條款類型："
        inputType="radioType"
        :options="typeOptions"
      />
      <RadioSelector
        class="targetSelector"
        v-model="selectTarget"
        @change="selectTermByTarget"
        inputTitle="適用對象："
        inputType="radioTarget"
        :options="targetOptions"
      />
      <RecordSelector :parentValue.sync="recordPerPage" @change="getTermData" />
      <SvgReset @click="resetSearchingRecord"></SvgReset>
    </div>
    <TableNormal
      class="tableContainer"
      :columns="columns"
      :rows="termList"
      :editBtnFlag="permissionMap.EditTerm"
      :deleteBtnFlag="permissionMap.EditTerm"
      :checkDetailBtnFlag="permissionMap.SelectTerm || permissionMap.EditTerm"
      @goEdit="goEdit"
      @goDelete="deleteTerm"
      @goCheckDetail="goCheckDetail"
      @changeStatus="editStatus"
    >
    </TableNormal>
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
import TitleCard from "@/components/Card/TitleCard";
import BtnNormal from "@/components/Btn/BtnNormal";
import RadioSelector from "@/components/Selector/RadioSelector";
import { termStatusAndText, termStatus } from "@/utils/term";
import RecordSelector from "@/components/Selector/RecordSelector";
import SvgReset from "@/components/Btn/SvgReset";
import TableNormal from "@/components/Table/TableNormal";
import PaginationComponent from "@/components/PaginationComponent";

export default {
  name: "HealthopeTerm",
  components: {
    TitleCard,
    BtnNormal,
    RadioSelector,
    RecordSelector,
    SvgReset,
    TableNormal,
    PaginationComponent,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      selectStatus: "",
      selectType: "",
      selectTarget: "",
      recordPerPage: "8",
      currentPage: 1,
      totalPage: 1,
      searchingPage: 1,
      termList: [{ Name: "會員 - 服務條款" }],
      columns: [
        { label: "名稱", key: "Name" },
        { label: "版本號", key: "Version" },
        { label: "類型", key: "Type" },
        { label: "適用對象", key: "ApplicableTarget" },
        {
          label: "狀態",
          key: "Status",
          type: "dropDownSelector",
          enableFlag: this.permissionMap.EditTerm,
        },
        { label: "條款生效日", key: "EffectiveTime" },
      ],
      typeOptions: [
        { value: "1", text: "服務條款" },
        { value: "2", text: "隱私權政策" },
        { value: "", text: "全部" },
      ],
      targetOptions: [
        { value: "1", text: "會員" },
        { value: "2", text: "教練" },
        { value: "", text: "全部" },
      ],
    };
  },
  methods: {
    redirect(path) {
      if (this.$route.path !== path) this.$router.push(path);
    },
    selectTermByStatus() {
      this.searchingPage = 1;
      this.getTermData();
    },
    resetSearchingRecord() {
      this.selectStatus = "";
      this.selectTarget = "";
      this.selectType = "";
      this.recordPerPage = "8";
      this.searchingPage = 1;
      this.getTermData();
    },
    async getTermData() {
      if (!this.validInput()) return;

      // post 的 dto 變數
      let getTermDto = {
        Type: this.selectType || null,
        Status: this.selectStatus || null,
        ApplicableTarget: this.selectTarget || null,
        RecordPerPage: this.recordPerPage,
        Page: this.searchingPage,
      };
      try {
        // post
        const response = await this.$axios.post(
          "/api/Term/GetTerm",
          getTermDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.currentPage = this.searchingPage;
          this.termList = response.data.ApiDataObject.TermList;

          this.termList.forEach((term) => {
            let type = this.typeOptions.find(
              (typeOption) => typeOption.value === String(term.Type)
            );
            term.Type = type.text;

            let target = this.targetOptions.find(
              (targetOption) =>
                targetOption.value === String(term.ApplicableTarget)
            );
            term.ApplicableTarget = target.text;

            let statusOption = [];
            let statusDraft = 1;
            let statusPublished = 2;
            termStatusAndText.forEach((status) => {
              if (term.Status === Number(status.value))
                statusOption.push(status);

              if (
                Number(status.value) === statusPublished &&
                term.Status === statusDraft
              )
                statusOption.push(status);
            });

            term.Status = {
              Value: String(term.Status),
              Options: statusOption,
              OldValue: String(term.Status),
            };

            const localDate = new Date(term.EffectiveTime);
            term.EffectiveTime = localDate.toLocaleDateString();
            if (
              localDate.getFullYear() === 1900 &&
              localDate.getMonth() === 0 && // 0 = 一月
              localDate.getDate() === 1
            )
              term.EffectiveTime = "未生效";

            term.Version = term.Version === 0 ? "無" : term.Version;
          });

          this.totalPage = response.data.ApiDataObject.TotalPage;
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/";
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
        console.error("取得展示用團課列表時發生錯誤", error);
      }
    },
    selectTermByType() {
      this.searchingPage = 1;
      this.getTermData();
    },
    selectTermByTarget() {
      this.searchingPage = 1;
      this.getTermData();
    },
    searchPage(page) {
      this.searchingPage = page;
      this.getTermData();
    },
    goEdit(row) {
      if (row.Status.Value !== String(termStatus.Draft)) {
        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "請勿修改發布中版本及歷史版本!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }

      if (row.TermId < 1) return;

      this.$router.push({ path: "/term/edit", query: { id: row.TermId } });
    },
    deleteTerm(row) {
      if (row.Status.Value !== String(termStatus.Draft)) {
        // 設定彈窗資料
        this.$notificationBox.notificationBoxFlag = true;
        this.$notificationBox.notificationBoxTitle =
          "請勿刪除發布中版本及歷史版本!";
        this.$notificationBox.notificationBoxErrorCode = 0;
        return;
      }
      // 添加監聽器，查看彈窗是否被按確認鍵
      this.unwatchFlag = this.$watch("notificationBoxConfirmFlag", (newVal) => {
        if (newVal) {
          let redirectRoute = "stop";
          this.$emit("afterConfirmEvent", redirectRoute);

          try {
            this.submitDelTerm(row.TermId);
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
    async submitDelTerm(id) {
      if (id < 1) return;

      try {
        // post
        let termIdDto = {
          TermId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Term/DeleteTerm",
          termIdDto
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
    validInput() {
      if (this.selectType)
        if (this.selectType !== "1" && this.selectType !== "2") return false;

      if (this.selectTarget)
        if (this.selectTarget !== "1" && this.selectTarget !== "2")
          return false;

      if (this.selectStatus)
        if (!Object.values(termStatus).includes(Number(this.selectStatus)))
          return false;

      return true;
    },
    async editStatus(row) {
      if (row.Status.OldValue !== String(termStatus.Draft)) {
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
        this.$notificationBox.notificationBoxTitle = "修改了錯誤的狀態!";
        this.$notificationBox.notificationBoxErrorCode = 0;
      }

      let editTermStauts = {
        TermId: row.TermId,
        UpdateTime: row.UpdateTime,
        Status: row.Status.Value,
      };

      try {
        // post
        const response = await this.$axios.post(
          "/api/Term/EditTermStatus",
          editTermStauts
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.getTermData();
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
        console.error("取得展示用團課列表時發生錯誤", error);
      }
    },
    async goCheckDetail(row){
      if (row.TermId < 1) return;

      this.$router.push({ path: "/term/detail", query: { id: row.TermId } });
    }
  },
  computed: {
    termStatusAndTextOptions() {
      let termStatusAndTextOptions = [...termStatusAndText];
      termStatusAndTextOptions.push({ value: "", text: "全部" });
      return termStatusAndTextOptions;
    },
  },
  created() {
    this.getTermData();
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

.statusSelector,
.typeSelector {
  width: 350px;
}
.targetSelector {
  width: 300px;
}
</style>