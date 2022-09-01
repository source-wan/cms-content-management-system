<template>
<div>
  <el-scrollbar
        style="height: calc(100vh - 210px); width: 100%; overflow-x: hidden">
<div v-html="blog">
</div>
</el-scrollbar>
</div>
</template>

<script>
import { marked } from "marked"
import { getUrlParamId } from '../utils/auth';
import { getArticleInfo } from '../api/auth';
export default {
    data() {
        return {
            blog: ''
        }
    },
    methods:{
        init(){
            var url = location.search;
            var id = getUrlParamId(url)
            getArticleInfo(id).then(({data})=>{
                  this.blog=marked(data.content)
            })
        }
    },
    created(){
        this.init();
    },
     watch: {
    $route: {
      handler: function () {
        this.init();
      },
    },
  },
};
</script>

<style scoped>
body {
  background-color: rgb(233, 238, 243);
  color: #333;
}

.write {
  width: 100%;
  max-height: calc(100vh - 210px);
}
.block {
  text-align: center;
}
ul,
li {
  margin: 0;
  padding: 0;
  list-style: none;
}
.el-scrollbar__wrap {
  overflow-x: hidden;
}
</style>>
