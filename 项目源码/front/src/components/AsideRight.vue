<template>
    <div class="hot">
      <h1>热门文章</h1>
      <div class="blog1" v-for="(formdata,index) in this.formdata" :key="formdata.id">
        <ul>
          <div @click="handlehref(formdata.id)">
          <li class="title">
            {{index+1+'.'+formdata.title }}
          </li>
            </div>
        </ul>
      </div>
    </div>
</template>
<script>
import { getHot } from "../api/auth";

export default {
  data() {
    return {
      formdata: [],
    };
  },
  created() {
    getHot().then(({ data }) => {
      this.formdata = data;
    });
  },
  methods:{
    handlehref(id){
    this.$router.push(`/ArticleInfo?id=${id}`)
    }
  }
};
</script>


<style scoped>
.hot {
  margin-left: -40px;
}
.blog1 {
  margin-left: -50px;
}
.hot {
  line-height: 20px;
}
span {
  margin-left: -90px;
}
li {
  line-height: 30px;
}

li {
  font: normal 1.1em Arial, Helvetica, sans-serif;
  color: #666;
}
</style>