import Vue from 'vue'
import App from './App.vue'
import router from './router'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import dayjs from "dayjs"
import 'dayjs/locale/zh-cn'
dayjs.locale('zh-cn')
import mavonEditor from 'mavon-editor'
import 'mavon-editor/dist/css/index.css'
Vue.use(mavonEditor)

import filter from '../common/filter'
 
for (const key in filter) {
    Vue.filter(key,filter[key]);
}


// 引入模拟数据，拦截请求，返回模拟数据，在需要直接的后端请求的时候，请注释之
import './mock'

Vue.use(ElementUI)
Vue.use(dayjs)

Vue.config.productionTip = false

new Vue({
  router,
  render: h => h(App),
}).$mount('#app')

