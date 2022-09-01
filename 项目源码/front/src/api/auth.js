import request from "@/utils/request";

//登录
export function login(formdata) {
    return new Promise(function (resolve, reject) {
        request.post('/login',formdata).then(res => {
            resolve(res)
        }).catch(err => {
            reject(err)
        })
    })
}

//注册
export function regisiter(formdata)
{
    return new Promise(function (resolve,reject)
    {
        request.post('/reg',formdata).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}


export function getUser(id)
{
    return new Promise(function (resolve,reject)
    {
        request.get(`/user/${id}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

export function putUser(id,model){
    return new Promise(function(resolve,reject){
        request.put(`/user/`+id,model).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

//添加文章
export function addArticle(formdata)
{
    return new Promise(function (resolve,reject)
    {
        request.post('/article',formdata).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

//获取文章列表

export function queryArticle(index=1,size=5)
{
    return new Promise(function (resolve,reject)
    {
        request.get(`/article?index=${index}&size=${size}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}
export function queryArticleByTitle(index=1,size=5,title)
{
    return new Promise(function (resolve,reject)
    {
        request.get(`/article?index=${index}&size=${size}&title=${title}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

export function queryArticleByContent(index=1,size=5,Content)
{
    return new Promise(function (resolve,reject)
    {
        request.get(`/article?index=${index}&size=${size}&ontent=${Content}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}
// 获取文章的具体内容
export function getArticleInfo(id)
{
    return new Promise(function (resolve,reject)
    {
        request.get(`/article/${id}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

//给文章点赞
// export function LikeArticleInfo(id)
// {
//     return new Promise (function (resolve , reject)
//     {
//         request.post(`/like/${id}`).then(res=>{
//             resolve(res)
//         }).catch(err=>{
//             reject(err)
//         })
//     })
// }
export function likeArticleInfo(id,model){
    return new Promise(function(resolve,reject){
        request.post(`/like/`+id,model).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}
//给文章点踩
export function unlikeArticleInfo(id)
{
    return new Promise (function (resolve , reject)
    {
        request.post(`/unlike/${id}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

// 收藏文章
export async function collectionArticleInfo(id) {
    return request.post(`/colletcion/${id}`);
}

//获取当前用户喜欢的文章的列表
export async function getLike(index=1,size=5) {
    return request.get(`/like?index=${index}&size=${size}`);
}

//获取热门文章
export async function getHot(){
    return request.get(`/hot`)
}



export function searchArticleByTitle(index=1,size=5,title)
{
    return new Promise(function (resolve,reject)
    {
        request.get(`/article?index=${index}&size=${size}&title=${title}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}


export function searchArticleByRemarks(index=1,size=5,remarks)
{
    return new Promise(function (resolve,reject)
    {
        request.get(`/article?index=${index}&size=${size}&remarks=${remarks}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}
export function delArticle(id,hard=false){
    return new Promise(function(resolve,reject){
        request.delete(`/article/${id}?hard=${hard}`).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

//获取收藏文章
export async function getCollection(index=1,size=5) {
    return request.get(`/collection?index=${index}&size=${size}`);
}

//获取浏览文章
export async function getHistory(index=1,size=5) {
    return request.get(`/history?index=${index}&size=${size}`);
}
export async function getCategory(){
    return request.get(`/category`)
}

export async function postFile(file){
    return request.post(`/files`,file)
}


export function searchArticleById(index=1,size=5,id)
{
    return request.get(`/article?index=${index}&size=${size}&createdBy=${id}`)
}

export function getCategoryArticle(index=1,size=5,id)
{
    return request.get(`/ArticleCategory?index=${index}&size=${size}&Id=${id}`)
}