<template>
  <div class="hello">
    <p>Hello Login?</p>
    <button v-if="!login" @click="postLoginMock">login</button>
    <button v-else @click="postLogoutMock">logout</button>
    <p v-if="login">登入啦</p>
    <p v-else>登出啦</p>
  </div>
</template>

<script>
export default {
  name: "HelloWorld",
  data() {
    return {
      login: false,
    };
  },
  methods: {
    async postLoginMock() {
      const response = await this.$axios.post("/api/Login/LoggedIn");
      if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
        this.login = true;
      }
    },
    async postLogoutMock() {
      const response = await this.$axios.post("/api/Login/LoggedOut");
      if (response.data.ErrorCode === this.$errorCodeDefine.Success) {
        this.login = false;
      }
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
