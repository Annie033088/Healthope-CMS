<template>
  <div class="subTitleCard">
    <div class="subTitle">
      <div class="goBack btn" @click="redirectBack()">
        <svg
          width="15"
          height="30"
          viewBox="0 0 12 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M2.452 6.58L3.513 5.52L9.292 11.297C9.38516 11.3896 9.45909 11.4996 9.50953 11.6209C9.55998 11.7421 9.58595 11.8722 9.58595 12.0035C9.58595 12.1348 9.55998 12.2649 9.50953 12.3861C9.45909 12.5074 9.38516 12.6174 9.292 12.71L3.513 18.49L2.453 17.43L7.877 12.005L2.452 6.58Z"
            fill="black"
          />
        </svg>
      </div>
      <p>
        {{ text }}
      </p>
    </div>
  </div>
</template>

<script>
export default {
  name: "SubTitleCard",
  props: {
    text: String,
  },
  methods: {
    redirectBack() {
      this.$router.back();
    },
  },
};
</script>

<style scoped>
.subTitle {
  margin-left: 3%;
  font-size: 18px;
  display: flex;
  align-items: center;
  gap: 5px;
}

.subTitleCard {
  padding: 1px;
  background-color: #f0f0f0d3;
  box-shadow: rgba(0, 0, 0, 0.15) 0px 0px 3px 0px inset;
}

.goBack{
  width: 35px;
  height: 35px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.goBack:hover {
  background-color: #fafbfc;
}
</style>