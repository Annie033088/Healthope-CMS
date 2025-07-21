<template>
  <div class="">
    <TitleCard text="條款" @refreshPage="$emit('refreshPage')"></TitleCard>
    <SubTitleCard text="查看條款內容"></SubTitleCard>
    <div class="sectionTitle">
      <h3>條款基本資訊</h3>
      <h3>{{ term.Name + " - " + term.Version }}</h3>
    </div>
    <div class="termDetailContentBox">
      <div class="termDetailContentContainer">
        <div class="detailContent">
          <div class="contentTextBox">
            <label class="lab">版本差異</label><br />
            <span>{{ term.VersionDescription }}</span
            ><br /><br /><br />
            <label class="lab">內文</label><br />
            <span>{{ term.DetailContent }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
export default {
  name: "HealthopeTermDetail",
  components: {
    TitleCard,
    SubTitleCard,
  },
  props: {
    permissionMap: {},
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      term: {
        TermId: 5,
        Name: "會員 - XXX",
        Version: 2,
        VersionDescription: "Q",
        DetailContent: "D",
      },
    };
  },
  methods: {
    async getTermDetail(id) {
      try {
        let termIdDto = {
          TermId: id,
        };

        // post
        const response = await this.$axios.post(
          "/api/Term/GetTermDetail",
          termIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.term = response.data.ApiDataObject;
        } else {
          if (this.unwatchFlag) {
            this.unwatchFlag(); // 確保監聽被移除
            this.unwatchFlag = null;
          }

          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/term";
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
        console.error("取得特定條款時發生錯誤", error);
      }
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/term");
      return;
    }

    this.getTermDetail(this.$route.query.id);
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
}

.termDetailContentBox {
  display: flex;
  justify-content: center;
  margin-bottom: 15px;
}

.termDetailContentContainer {
  display: flex;
  align-items: center;
  padding: 9px;
  width: 1000px;
  max-width: 80%;
  background-color: white;
  border-radius: 35px;
  gap: 9px;
  box-shadow: rgba(10, 37, 64, 0.35) 0px -1px 5px 0px inset;
}

.detailContent {
  display: flex;
  justify-content: space-evenly;
  align-items: center;
  flex-wrap: wrap;
  overflow: hidden;
  width: 1000px;
  max-width: 100%;
  border-radius: 30px;
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
}

.contentTextBox {
  margin-left: 25px;
  margin-bottom: 5px;
  width: 150px;
}

.contentTextBox label {
  font-size: 20px;
  font-weight: 700;
  color: #6f6f6f;
  font-family: "Microsoft JhengHei";
}

.detailContent .contentTextBox {
  padding: 15px;
  width: 100%;
}
</style>