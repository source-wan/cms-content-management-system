import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from './routes';


Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  routes,
  
})

// router.beforeEach((to,from,next) => {
//   const isLogin= localStorage.accessToken ? true:false;

//   if(to.path=="/login" || to.path=="/register"){
//     next();
//   }else{
//     isLogin? next(): next("/login");
//   }
// })

export default router
