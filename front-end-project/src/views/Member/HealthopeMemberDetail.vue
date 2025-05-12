<template>
  <div>
    <TitleCard text="會員清單" @refreshPage="$emit('refreshPage')" />
    <SubTitleCard text="會員資料" />
    <div class="sectionTitle"><h3>基本資訊</h3></div>
    <div class="memberBasicInformationContainer">
      <div class="memberBasicInformation">
        <div class="avatar">
          <div class="avatarInner">
            <img src="@/assets/logo/avatar1.png" />
          </div>
        </div>
        <div class="basicInformationContent">
          <div class="top">
            <div class="contentTextBox">
              <label for="">姓名</label><br />
              <span>王小小</span>
            </div>
            <div class="contentTextBox">
              <label for="">性別</label><br />
              <span>女</span>
            </div>
            <div class="contentTextBox">
              <label for="">生日</label><br />
              <span>2005-07-09</span>
            </div>
          </div>
          <div class="middle">
            <div class="contentTextBox">
              <label for="">手機</label><br />
              <span>0912-xxx-678</span>
            </div>
            <div class="contentTextBox emailBox">
              <label for="">信箱</label><br />
              <span>qweko123@wwe.abc</span>
            </div>
          </div>
          <div class="bottom">
            <div class="contentTextBox">
              <label for="">身高</label><br />
              <span>159cm</span>
            </div>
            <div class="contentTextBox">
              <label for="">體重</label><br />
              <span>50kg</span>
            </div>
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
  components: {
    TitleCard,
    SubTitleCard,
  },
};
</script>

<style scoped>
.sectionTitle {
  display: flex;
  justify-content: center;
}

.avatar {
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
  border-radius: 30px;
  width: 200px;
  max-width: 50%;
  height: 100%;
  display: flex;
  align-items: center;
}

.avatarInner {
  width: 12rem;
  border-radius: 0.25rem;
  overflow: hidden;
}

.avatarInner img {
  width: 100%;
  height: auto;
  display: flex;
}

.memberBasicInformationContainer {
  display: flex;
  justify-content: center;
}

.memberBasicInformation {
  display: flex;
  position: relative;
  align-items: center;
  padding: 9px;
  width: 1000px;
  max-width: 80%;
  background-color: white;
  border-radius: 35px;
  gap: 9px;
  box-shadow: rgba(10, 37, 64, 0.35) 0px -1px 5px 0px inset;
}

.basicInformationContent {
  display: flex;
  justify-content: space-evenly;
  flex-direction: column;
  align-items: center;
  flex-wrap: wrap;
  overflow: hidden;
  width: 1000px;
  max-width: 100%;
  min-height: 220px;
  background-color: white;
  border-radius: 30px;
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
  /* Content style */
}

.basicInformationContent .top, .bottom, .middle {
  display: flex;
  flex-wrap: wrap;
  width: 100%;
  height: 100%;
  padding: 5px;
  gap: 10px 20%;
  word-break: break-word;
}
.basicInformationContent .top, .middle {
  padding-bottom: 10px;
  border-bottom: solid #eee 1px;
}

.contentTextBox {
  margin-left: 25px;
  margin-bottom: 5px;
}

.contentTextBox label {
  font-size: 20px;
  font-weight: 700;
  color: #6f6f6f;
  text-align: center;
  font-family: "Microsoft JhengHei", "PingFang TC", "Noto Sans TC", sans-serif;
}

.emailBox{
}

@media screen and (min-width: 885px) {
  .emailBox {
  margin-left: -18px;
  }
}
</style>