<template>
  <div class="titleCard">
    <h2 class="title" @click="redirect()">{{ text }}</h2>
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
      if (this.text === "管理者清單") {
        if (this.$route.path === "/admin") {
        this.$emit("refreshPage")
        } else {
          this.$router.push("/admin");
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