<template>
  <div>
    <el-container>
      <el-aside width="200px">
        <el-row class="tac">
          <el-col :span="12">
            <corporation-info></corporation-info>
            <nav-menu :menus="menus"></nav-menu>
          </el-col>
        </el-row>
      </el-aside>
      <el-container>
        <el-header>
          <header-info></header-info>
        </el-header>
        <el-main>
          <router-view></router-view>
        </el-main>
        <el-footer>
          <footer-info></footer-info>
        </el-footer>
      </el-container>
    </el-container>
  </div>
</template>
<script>
import FooterInfo from "./FooterInfo.vue";
import HeaderInfo from "./HeaderInfo.vue";
import NavMenu from "./nav/NavMenu.vue";
import CorporationInfo from "./CorporationInfo.vue";
import routes from "../router/routes";
export default {
  components: {
    FooterInfo,
    HeaderInfo,
    NavMenu,
    CorporationInfo,
  },
  data: function () {
    return {};
  },
  methods: {
    handleOpen(key, keyPath) {
      console.log(key, keyPath);
    },
    handleClose(key, keyPath) {
      console.log(key, keyPath);
    },
    // 处理路由定义中的想要在菜单中隐藏的项-第二版
    routesToMenus(routes, path) {
      let arr = [];
      routes.forEach((item) => {
        // 判断当前元素是否需要在菜单中隐藏
        if (item.meta.hidden && item.meta.hidden === true) {
          // 当前元素有children节点，如果有则升格children节点，否则不做其它
          if (item.children && item.children.length > 0) {
            let tmpArr = this.routesToMenus(item.children, item.path);
            arr = arr.concat(tmpArr);
          }
        } else {
          // 这个路径的处理，必须在调用递归方法之前，才会保存最上一层的路径
          if (path) {
            if (path === "/") {
              item.path = path + item.path;
            } else {
              item.path = path + "/" + item.path;
            }
          }
          // 处理当有children节点的时候，递归调用自己
          if (item.children && item.children.length > 0) {
            // console.log(item.path);
            item.children = this.routesToMenus(item.children, item.path);
          }
          arr.push(item);
        }
      });
      // console.log(arr);
      return arr;
    },
    // 处理路由定义中的想要在菜单中隐藏的项-第一版
    processRoutes(routes) {
      /*  
        1、遍历定义的路由
        2、判断当前元素/节点是否需要隐藏，是的话，再判断它是否具有children节点，并且children节点有数据
        3、接上一步，将其children下的元素都往上提一层（升格）
        4、将其装入新的数组
      */
      let arr = [];
      routes.forEach((item) => {
        if (
          item.meta.hidden &&
          item.meta.hidden === true &&
          item.children &&
          item.children.length > 0
        ) {
          item.children.forEach((subItem) => {
            arr.push(subItem);
          });
        } else {
          if (!item.meta.hidden || item.meta.hidden === false) {
            arr.push(item);
          }
        }
      });
      return arr;
    },
  },
  computed: {
    menus: function () {
      // console.log(routes);
      let list = this.routesToMenus(routes, "");
      return list;
    },
  },
};
</script>
<style>
body {
  margin: 0px;
  padding: 0px;
}
.el-header,
.el-footer {
  background-color: #b3c0d1;
  color: #333;
  text-align: center;
  line-height: 60px;
}
.el-main{
  max-height: calc(100vh - 122px);
}
</style>