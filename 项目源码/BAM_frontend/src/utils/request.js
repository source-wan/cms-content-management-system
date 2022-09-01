import Axios from 'axios'
import appConfig from '../config/appConfig'
import { getToken } from './auth'


const request = Axios.create({
    baseURL: appConfig.baseUrl,
    timeout: 5000
})

// 请求拦截器
request.interceptors.request.use(function (config) {
    let token = getToken();
    if (token) {
        config.headers['Authorization'] = 'Bearer ' + token
    }
    return config;
}, function (err) {
    // 对请求错误做些什么
    return Promise.reject(err);
})

// 响应拦截器
request.interceptors.response.use(function (response) {
    // 2xx 范围内的状态码都会触发该函数。
    // 对响应数据做点什么
    return response.data;
}, function (error) {
    // 超出 2xx 范围的状态码都会触发该函数。
    // 对响应错误做点什么
    return Promise.reject(error);
})

export default request