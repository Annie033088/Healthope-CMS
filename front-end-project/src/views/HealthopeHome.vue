<template>
  <div >
    <h1 class="homeTitle">你好，管理者</h1>
    <h2 class="homeTitle">歡迎來到 Healthope 後台管理網站</h2>
    <div class="imgContainer">
      <img
        class="imgHealthopeLogo"
        src="@/assets/logo/HealthopeLogo-circle-removebg-preview.png"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "HealthopeHome",
  components: {},
  data() {
    return {
      tasks: [],
    };
  },
};
</script>

<style scoped>
.homeTitle {
  display: flex;
  justify-content: center;
}

.imgContainer {
  display: flex;
  justify-content: center;
}

.imgHealthopeLogo {
  max-width: 100%;
  height: auto;
}
</style>