import Vue from 'vue';
import VueRouter from 'vue-router';
import routes from './routes';

const originalPush = VueRouter.prototype.push

VueRouter.prototype.push = function push(location) {
  return originalPush.call(this, location).catch(err => err)
}

Vue.use(VueRouter);

let router = new VueRouter({
    mode: 'history',
    routes
})

export default router;