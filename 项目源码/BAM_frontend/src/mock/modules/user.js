import { Random } from 'mockjs'

let arr = []


for (let i = 0; i < 100; i++) {
    let obj = {
        username: Random.cname(),
        password:Random.cname(),
        isActived: Random.boolean(),
        updatedTime: Random.datetime(),
    }
    arr.push(obj)
}

export function list() {
    return {
        url: '/user/avatar',
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

