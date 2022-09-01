<template>
  <div id="app">  
    <!-- <router-link to="/login">开车摩托去拉萨</router-link> -->
    <router-view></router-view>
  </div>
</template>

<script>
// import HelloWorld from './components/HelloWorld.vue'

export default {
  name: 'App',

  // 刷新页面方法
  provide(){
      return{
        reload:this.reload
      }
  },
  components: {
    // HelloWorld
  }
}
</script>

