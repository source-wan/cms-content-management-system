import Vue from 'vue'
import App from './App.vue'
import router from './router'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
// import './router/mavenEditor.js'
import dayjs from "dayjs"
import 'dayjs/locale/zh-cn'
dayjs.locale('zh-cn')
import mavonEditor from 'mavon-editor'
import 'mavon-editor/dist/css/index.css'
import './style/iconfont/iconfont.css'
Vue.use(mavonEditor)
Vue.use(ElementUI)
Vue.use(dayjs)

Vue.config.productionTip = false

new Vue({
  router,
  render: h => h(App),
}).$mount('#app')
