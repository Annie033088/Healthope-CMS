<template>
  <footer>
    <p>Healthope &copy; {{year}}</p>
  </footer>
</template>

<script>
export default {
  name: "AppFooter",
  data(){
    return{
      year:2025,
    }
  },
  created(){
    this.year = new Date().getFullYear();
  }
};
</script>

<style scoped>
a {
  color: #333;
}
footer {
  text-align: center;
  margin-top: 5%;
}
</style>