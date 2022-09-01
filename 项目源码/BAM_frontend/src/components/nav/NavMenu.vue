<template>
  <el-menu
    default-active="2"
    class="el-menu-vertical-demo"
    @open="handleOpen"
    @close="handleClose"
    background-color="#545c64"
    text-color="#fff"
    active-text-color="#ffd04b"
    unique-opened
    router
  >
    <nav-menu-item
      v-for="menu in menus"
      :menu="menu"
      :key="menu.path"
    ></nav-menu-item>
  </el-menu>
</template>
<script>
import NavMenuItem from "./NavMenuItem.vue";
export default {
  props: ["menus"],
  components: {
    NavMenuItem,
  },
  methods: {
    handleOpen(key, keyPath) {
      console.log(key, keyPath);
    },
    handleClose(key, keyPath) {
      console.log(key, keyPath);
    },
  },
};
</script>
<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  min-height: 400px;
}
.el-menu {
  /* 计算的时候，减号两边必须有空格 */
  height: calc(100vh - 62px);
  border-right: none;
}
.el-aside {
  overflow: hidden !important;
}
</style>