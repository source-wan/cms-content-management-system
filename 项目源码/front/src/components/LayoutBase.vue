<template>
  <div class="box">
    <el-container>
      <el-card class="top">
        <el-header>
          <header-info></header-info>
        </el-header>
      </el-card>

      <el-card>
        <el-container>
          <el-card class="left">
            <el-aside width="200px">
              <left-info></left-info>
            </el-aside>
          </el-card>
          <el-card class="main">
            <el-container>
              <el-main class="main">
                <router-view></router-view>
              </el-main>
            </el-container>
          </el-card>
          <el-card class="right">
            <el-aside width="200px">
              <aside-right></aside-right>
            </el-aside>
          </el-card>
        </el-container>
      </el-card>
      <el-footer height:100px>
        <footer-info></footer-info>
      </el-footer>
    </el-container>
  </div>
</template>
<script>
import HeaderInfo from "./HeaderInfo.vue";
import LeftInfo from "./LeftInfo.vue";
import FooterInfo from "./FooterInfo.vue";
import AsideRight from "./AsideRight.vue";
export default {
  components: {
    HeaderInfo,
    LeftInfo,
    FooterInfo,
    AsideRight,
  },
  data: function () {
    return {};
  },
};
</script>


<style scoped>
* {
  padding: 0;
  margin: 0;
   background-color:#f0f0f4
}

.footer-font {
  font-size: medium;
  text-align: center;
  line-height: 30px;
  margin-top: 10px;
}

body {
  margin: 0px;
  padding: 0px;
  
}
.top {
  width: 100%;
  margin: 2px 2px;
  padding-top: 5px;
}
.main {
  width: 74%;
  max-height: calc(100vh - 200px);
  margin-left: 40px;
  margin-right: 40px;

}
.box {
  /* position: fixed; */
  display: flex;
  width: 100%;
  height: 100%;
 
}

.el-header {
  padding: 0px 0px !important;
  margin: 0px;
}
.el-header,
.el-footer {
  background-color: #f0f0f4;
}
.el-footer {
  height: 100px;
  padding: 0px 0px !important;
}
.el-aside {
  background-color: #f0f0f4;
  color: #333;
  text-align: center;
  font-size: medium;
  line-height: 200px;
  margin: 0px;
  padding: 0px;
  
}
.right,
.left {
  flex-direction: row;
  width: 200px;
  height: 600px;
    background-color:#f0f0f4
}
.card{
 background-color:#f0f0f4;

}
.el-header {
  background-color: #f0f0f4;
  color: #333;
  line-height: 60px;
  margin: 0px;
}

.el-main {
  background-color: #f0f0f4;
  color: #333;
  line-height: 20px;
  max-height: calc(100vh - 82px);
  padding: 0px;
  margin-top: 2px;
}

body > .el-container {
  margin-bottom: 40px;
}

.el-container:nth-child(5) .el-aside,
.el-container:nth-child(6) .el-aside {
  line-height: 260px;
}

.el-container:nth-child(7) .el-aside {
  line-height: 320px;
}
.el-container {
  height: calc(100vh - 10px);
}
.el-card-define {
  min-height: 100%;
  height: 100%;
}
.el-card-define >>> .el-card__body {
  height: 100%;
}
</style>



