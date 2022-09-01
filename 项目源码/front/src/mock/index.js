import { mock } from 'mockjs'
// import obj from './modules/category'

/*
    1、查找资料，以使在当前项目可以正常使用fs
    2、有没有当前项目或者环境可以用的，让我们可以遍历指定目录的技术

*/

// console.log(obj);

let files = require.context("@/mock/modules", true, /\.js$/).keys();

files.forEach(item => {
    let file = item.slice(2);
    let obj = require('@/mock/modules/' + file);
    // console.log(obj);
    for (let key in obj) {
        let tmp = obj[key]();
        // console.log(tmp.url);
        mock(tmp.url, tmp.method, tmp.res)
    }
    
})


