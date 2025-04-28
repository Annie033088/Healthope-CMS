<template>
  <div>
    <TitleCard text="管理者清單"></TitleCard>
    <div class="funtionColumn">
      <BtnNormal text="新增管理者" @click="redirect('/admin/add')"></BtnNormal>
      <div class="inputGroup">
        <input
          type="text"
          required=""
          autocomplete="off"
          placeholder="Account..."
        />
        <svg
          class="btnSearch"
          width="30"
          height="30"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M16.893 16.92L19.973 20M19 11.5C19 13.4891 18.2098 15.3968 16.8033 16.8033C15.3968 18.2098 13.4891 19 11.5 19C9.51088 19 7.60322 18.2098 6.1967 16.8033C4.79018 15.3968 4 13.4891 4 11.5C4 9.51088 4.79018 7.60322 6.1967 6.1967C7.60322 4.79018 9.51088 4 11.5 4C13.4891 4 15.3968 4.79018 16.8033 6.1967C18.2098 7.60322 19 9.51088 19 11.5Z"
            stroke="#B4B4B4"
            stroke-width="1.5"
            stroke-linecap="round"
            stroke-linejoin="round"
          />
        </svg>
      </div>
      <div class="radioContainer">
        <label class="labRadioBox">
          <span class="textStatus">狀態：</span>
        </label>
        <label class="labRadioBox">
          <input type="radio" name="radioStatus" checked="" />
          <span class="textRadio">無</span>
        </label>
        <label class="labRadioBox">
          <input type="radio" name="radioStatus" />
          <span class="textRadio">啟用</span>
        </label>
        <label class="labRadioBox">
          <input type="radio" name="radioStatus" />
          <span class="textRadio">禁用</span>
        </label>
      </div>
      <div class="sortContainer">
        <label class="labSort">排序選項：</label>
        <select class="selectSort" name="sortOption">
          <option value="name">名稱</option>
          <option value="status">狀態</option>
        </select>
        <label class="labRadioBox">
          <input type="radio" name="radioSort" checked="" />
          <span class="textRadio">升序</span>
        </label>
        <label class="labRadioBox">
          <input type="radio" name="radioSort" />
          <span class="textRadio">降序</span>
        </label>
      </div>
      <BtnNormal text="差異紀錄"></BtnNormal>
    </div>
    <div class="adminTable">
      <div class="card">
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
            <div class="cardMainTitle"><span>Name</span></div>
            <div class="cardSubTitle"><span>Status</span></div>
          </div>
          <div class="cardEndText"><span>Identity</span></div>
        </div>

        <div class="cardEditBtn btn">
          <svg
            width="30"
            height="30"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M12 3H5C4.46957 3 3.96086 3.21071 3.58579 3.58579C3.21071 3.96086 3 4.46957 3 5V19C3 19.5304 3.21071 20.0391 3.58579 20.4142C3.96086 20.7893 4.46957 21 5 21H19C19.5304 21 20.0391 20.7893 20.4142 20.4142C20.7893 20.0391 21 19.5304 21 19V12"
              stroke="black"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
            <path
              d="M18.375 2.625C18.7728 2.22717 19.3124 2.00368 19.875 2.00368C20.4376 2.00368 20.9772 2.22717 21.375 2.625C21.7728 3.02282 21.9963 3.56239 21.9963 4.125C21.9963 4.68761 21.7728 5.22717 21.375 5.625L12.362 14.639C12.1245 14.8762 11.8312 15.0499 11.509 15.144L8.636 15.984C8.54995 16.0091 8.45874 16.0106 8.37191 15.9884C8.28508 15.9661 8.20583 15.9209 8.14245 15.8576C8.07907 15.7942 8.03389 15.7149 8.01164 15.6281C7.9894 15.5413 7.9909 15.45 8.016 15.364L8.856 12.491C8.95053 12.169 9.12453 11.8761 9.362 11.639L18.375 2.625Z"
              stroke="black"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BtnNormal from "@/components/Btn/BtnNormal";
import TitleCard from "@/components/Card/TitleCard";

export default {
  name: "HealthopeAdmin",
  components: {
    BtnNormal,
    TitleCard,
  },
  props: {
    text: String,
  },
  methods: {
    redirect(page) {
      if (this.$route.path !==  page) {
        this.$router.push(page);
      }
    },
  },
};
</script>

<style scoped>
/* 功能列 樣式*/
.funtionColumn {
  margin: 15px;
  display: flex;
  flex-wrap: wrap;
}

.inputGroup {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  max-width: 200px;
  border: 1px solid rgb(200, 200, 200);
  background-color: transparent;
  border-radius: 1rem;
  margin-left: 20px;
}

.inputGroup input {
  font-size: 100%;
  padding: 0.6em;
  outline: none;
  border: none;
  background-color: transparent;
  border-radius: 1rem;
  width: 100%;
}
.inputGroup input:hover {
  background-color: #fafbfc;
}

.btnSearch {
  border-end-end-radius: 20px;
  border-top-right-radius: 20px;
  border-left: 1.5px solid rgb(200, 200, 200);
  margin-right: 4px;
}

.btnSearch:hover {
  cursor: pointer;
  background-color: #fafbfc;
}

.btnSearch:active {
  background-color: #edeff2;
  transition: none 0.1s;
}

.radioContainer {
  display: flex;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.25rem;
  width: 300px;
  font-size: 14px;
  margin-left: 20px;
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

.textStatus {
  display: flex;
  justify-content: center;
  padding: 0.5rem 0;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
}

.sortContainer {
  display: flex;
  border-radius: 0.5rem;
  background-color: #eee;
  box-sizing: border-box;
  box-shadow: 0 0 0px 1px rgba(0, 0, 0, 0.06);
  padding: 0.25rem;
  width: 300px;
  font-size: 14px;
  margin-left: 20px;
  margin-right: 20px;
}

.labSort {
  display: flex;
  justify-content: center;
  padding: 0.5rem 0.5rem;
  border-radius: 0.5rem;
  background-color: #fff;
  font-weight: 500;
  margin-right: 5px;
}

.selectSort {
  border: none;
  border-radius: 0.5rem;
  padding: 0.5rem 0.5rem;
  margin-right: 5px;
  background-color: #fafbfc;
}

.selectSort:hover {
  cursor: pointer;
}
</style>

<style scoped>
/* 列表 樣式*/
.adminTable {
  display: flex;
  justify-content: center;
}
.card {
  width: 50%;
  background: #ffff;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
  padding: 12px 10px;
  margin: 15px;
  display: flex;
  justify-content: space-between;
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

.cardSubTitle {
  margin-top: 5px;
}
.cardMainTitle {
  font-size: 20px;
}

.cardSubTitle {
  font-size: 17px;
}

.cardEndText {
  margin-left: 30px;
  font-size: 18px;
}
</style>
