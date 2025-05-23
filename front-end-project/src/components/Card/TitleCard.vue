<template>
  <div class="titleCard">
    <h3 class="title" @click="redirect()">{{ text }}</h3>
  </div>
</template>

<script>
export default {
  name: "TitleCard",
  props: {
    text: String,
  },
  methods: {
    redirect() {
      if (this.text === "管理員") {
        if (this.$route.path === "/admin") {
          this.$emit("refreshPage");
        } else {
          this.$router.push("/admin");
        }
      } else if (this.text === "會員") {
        if (this.$route.path === "/member") {
          this.$emit("refreshPage");
        } else {
          this.$router.push("/member");
        }
      }else if(this.text === "教練"){
        if (this.$route.path === "/coach") {
          this.$emit("refreshPage");
        } else {
          this.$router.push("/coach");
        }
      }else if(this.text === "展示用團課"){
        if (this.$route.path === "/groupClass/showcase") {
          this.$emit("refreshPage");
        } else {
          this.$router.push("/groupClass/showcase");
        }
      }
    },
  },
};
</script>

<style scoped>
.title {
  margin-left: 3%;
}

.title:hover {
  cursor: pointer;
}

.titleCard {
  padding: 1px;
  background-color: #e8e8e8;
  box-shadow: rgba(0, 0, 0, 0.15) 0px -1px 3px 0px inset;
}
</style>