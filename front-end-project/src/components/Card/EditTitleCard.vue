<template>
      <div class="card">
        <div class="title">{{title}}</div>
        <div class="content">
          <div class="name">
            <!-- qweqwk90ekqwjepiwqepowqkeopqwejwqoqwewqeqwpejqwepowq! -->
            {{ content }}
          </div>
        </div>
      </div>
</template>

<script>
export default {
    name:"EditTitleCard",
    props:{
        content:{
            Text,
            default:""
        },
        title:{
            Text,
            default:""
        }
    }
}
</script>

<style scoped>
/* 樣式 */
.card {
  width: 30%;
  min-width: 300px;
  background-color: white;
  box-shadow: rgba(216, 216, 216, 0.962) 0px 0px 3px 0px inset;
  border-radius: 5%;
}

.title {
  display: flex;
  justify-content: center;
  background-color: rgb(237, 232, 232);
  font-size: 25px;
  border-radius: 5% 5% 0% 0%;
}

.content {
  padding: 12px 10px;
  margin: 5px;
  display: flex;
  justify-content: center;
}

/* .cardImage {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background: #727272e7;
} */

.name {
  font-size: 25px;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  word-break: break-word;
}
</style>