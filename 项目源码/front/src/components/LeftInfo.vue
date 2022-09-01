 <!-- 左侧菜单栏 -->
<template>
  <el-menu :default-openeds="['1', '4']" router>
    <el-menu-item index="/AddArticle">
      <i class="el-icon-edit-outline"></i>
      <span slot="title">添加文章</span>
    </el-menu-item>

    <el-menu-item index="/MyLike">
      <i class="el-icon-thumb"></i>
      <span slot="title">我的点赞</span>
    </el-menu-item>

    <el-menu-item index="/MyCollect">
      <i class="el-icon-folder-opened"></i>
      <span slot="title">我的收藏</span>
    </el-menu-item>

    <el-menu-item index="/RecentBrowse">
      <i class="el-icon-view"></i>
      <span slot="title">最近浏览</span>
    </el-menu-item>
  </el-menu>
</template>


        <script>
export default {
  data() {
    return {
      input: "",
    };
  },
};
</script>

<style scoped>
span{
    padding-left:10px;
   
}
.el-menu-item{
    margin-left:-40px
}
.icon-good{
    margin-right:9px ;
}
</style>