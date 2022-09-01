import { Random } from 'mockjs'

// 模拟数据库当中的分类表
let arr = []

// 随机100条记录
for (let i = 0; i < 5; i++) {
    let obj = {
        id: Random.uuid(),
        cateName: Random.cname(),
        isActived: Random.boolean(),
        updatedTime: Random.datetime(),
    }
    arr.push(obj)
}

// 获取模拟的表数据
export function getAll() {
    return {
        url: '/article/category',
        method: 'get',
        res: {
            code: 1000,
            msg: '请求成功',
            data: function () {
                // console.log(req);
                return arr
            }
        }
    }
}

// 模拟新增请求，这里会拦截请求
export function postCate() {
    return {
        url: '/article/category',
        method: 'post',
        res: function (req) {

            console.log(req);
            let params = JSON.parse(req.body)
            console.log(params);
            return {
                code: 1000,
                msg: '新增成功',
                data: {
                    id: Random.uuid(),
                    cateName: params.cateName,
                    isActived: true,
                    updatedTime: new Date().toLocaleString().replaceAll('/', '-')
                }
            }
        }
    }
}

// 模拟删除请求，这里会拦截请求
export function delCate() {
    return {
        url: RegExp('/article/category' + '*'),
        method: 'delete',
        res: function (req) {

            console.log(req);

            let id = req.url.replaceAll('/article/category/', '')
            console.log(id);

            arr = arr.filter(item => {
                return item.id !== id
            })
            // arr=tmpArr;
            let params = JSON.parse(req.body)
            console.log(params);
            return {
                code: 1000,
                msg: '删除成功',
                data: null
            }
        }
    }
}

// 模拟新增请求，这里会拦截请求
export function addCate() {
    return {
        url: RegExp('/article/category'),
        method: 'post',
        res: function (req) {
            let params = JSON.parse(req.body)
            let obj = {
                id: Random.uuid(),
                cateName: params.cateName,
                isActived: true,
                updatedTime: new Date().format('yyyy-MM-dd HH:mm:ss'),
            }
            return {
                code: 1000,
                msg: '新增成功',
                data: obj
            }
        }
    }
}

// 模拟编辑请求，这里会拦截请求
export function editCate() {
    return {
        url: RegExp('/article/category'),
        method: 'put',
        res: function (req) {
            let params = JSON.parse(req.body)
            let idx = arr.findIndex(item => {
                return item.id === params.id
            })
            arr[idx].cateName = params.cateName
            return {
                code: 1000,
                msg: '修改成功',
                data: arr[idx]
            }
        }
    }
}