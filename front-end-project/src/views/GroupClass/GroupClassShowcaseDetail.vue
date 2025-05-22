<template>
  <div>
    <TitleCard text="展示用團課" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="展示用團課資料" />
    <div class="sectionTitle"><h3>課程基本資訊</h3></div>
    <div class="courseBasicInformationBox">
      <div class="courseBasicInformationContainer">
        <div class="image">
          <div class="imageInner">
            <img :src="groupClassShowcase.ImageUrl" />
          </div>
        </div>
        <div class="basicInformationContent">
          <div class="top">
            <div class="contentTextBox">
              <label for="">課程名</label><br />
              <span>{{ groupClassShowcase.Name }}</span>
            </div>
            <div class="contentTextBox">
              <label for="">簡介</label><br />
              <span>{{ groupClassShowcase.Summary }}</span>
            </div>
          </div>
          <div class="bottom">
            <div class="contentTextBox">
              <label for="">Icon</label><br />
              <span class="icon">{{ groupClassShowcase.Icon }}</span>
            </div>
            <div class="contentTextBox">
              <label for="">分類</label><br />
              <span>{{ groupClassShowcase.Category }}</span>
            </div>
            <div class="contentTextBox">
              <label for="">排序</label><br />
              <span>{{ groupClassShowcase.Sort }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="courseDetailContentBox">
      <div class="courseDetailContentContainer">
        <div class="detailContent">
          <div class="contentTextBox">
            <label class="lab">內文</label><br />
            <span>{{ groupClassShowcase.DetailContent }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import TitleCard from "@/components/Card/TitleCard";
import SubTitleCard from "@/components/Card/SubTitleCard";
import { groupClassCategoryAndText, groupClassIcon } from "@/utils/groupClass";

export default {
  name: "GroupClassShowcaseDetail",
  components: {
    TitleCard,
    SubTitleCard,
  },
  props: {
    notificationBoxConfirmFlag: Boolean,
  },
  data() {
    return {
      groupClassShowcase: {
        GroupClassShowcaseIdDto: 0,
        Name: "提拉皮斯",
        Summary: "",
        DetailContent: "",
        ImageUrl: "",
        Category: 7,
        Icon: 1,
        Sort: 1,
      },
      AllowGroupClassFlag: false,
    };
  },
  methods: {
    async getShowcaseDetail(groupClassShowcaseId) {
      try {
        let groupClassShowcaseIdDto = {
          GroupClassShowcaseId: groupClassShowcaseId,
        };

        // post
        const response = await this.$axios.post(
          "/api/GroupClassShowcase/GetShowcaseDetail",
          groupClassShowcaseIdDto
        );

        if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
          this.groupClassShowcase = response.data.ApiDataObject;
          this.groupClassShowcase.GroupClassShowcaseIdDto =
            groupClassShowcaseId;

          // 調整顯示格式
          for (
            let index = 0;
            index < groupClassCategoryAndText.length;
            index++
          ) {
            if (
              String(this.groupClassShowcase.Category) ===
              groupClassCategoryAndText[index].value
            )
              this.groupClassShowcase.Category =
                groupClassCategoryAndText[index].text;
          }

          groupClassIcon.forEach((icon) => {
            if (this.groupClassShowcase.Icon.toString() === icon.value)
              this.groupClassShowcase.Icon = icon.text;
          });

          if (!this.groupClassShowcase.DetailContent)
            this.groupClassShowcase.DetailContent = "Ｘ";
          if (!this.groupClassShowcase.Summary)
            this.groupClassShowcase.Summary = "Ｘ";
        } else {
          // 添加監聽器，查看彈窗是否被按確認鍵
          this.unwatchFlag = this.$watch(
            "notificationBoxConfirmFlag",
            (newVal) => {
              if (newVal) {
                let redirectRoute = "/groupClass/showcase";
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
        console.error("取得特定管理者時發生錯誤", error);
      }
    },
  },
  created() {
    if (!this.$route.query.id) {
      this.$router.push("/groupClass/showcase");
      return;
    }
    this.getShowcaseDetail(this.$route.query.id);
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
}

.image {
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
  border-radius: 30px;
  width: 200px;
  max-width: 50%;
  height: 100%;
  display: flex;
  align-items: center;
}

.imageInner {
  width: 12rem;
  border-radius: 0.25rem;
  overflow: hidden;
}

.imageInner img {
  width: 100%;
  height: auto;
  display: flex;
}

.courseBasicInformationBox,
.courseDetailContentBox {
  display: flex;
  justify-content: center;
  margin-bottom: 15px;
}

.courseBasicInformationContainer,
.courseDetailContentContainer {
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

.basicInformationContent,
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

.basicInformationContent {
  min-height: 220px;
}

.top,
.bottom,
.middle {
  display: flex;
  flex-wrap: wrap;
  width: 100%;
  height: 100%;
  padding: 5px;
  gap: 10px 10%;
  word-break: break-word;
}

.top,
.middle {
  padding-bottom: 10px;
  border-bottom: solid #eee 1px;
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

.icon {
  font-size: 30px;
}
</style>