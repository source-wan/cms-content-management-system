import request from "@/utils/request";


export function login(formdata) {
    return new Promise(function (resolve, reject) {
        request.post('/login',formdata).then(res => {
            resolve(res)
        }).catch(err => {
            reject(err)
        })
    })
}
export function searchUserByName(index=1,size=5,username) {
    return new Promise(function (resolve, reject) {
        request.get(`/user?index=${index}&size=${size}&username=${username}`).then(res => {
            resolve(res.data)
        }).catch(err => {
            reject(err)
        })
    })
}
export function searchUserById(index=1,size=5,id) {
    return new Promise(function (resolve, reject) {
        request.get(`/user?index=${index}&size=${size}&id=${id}`).then(res => {
            resolve(res.data)
        }).catch(err => {
            reject(err)
        })
    })
}
export function searchUserByNick(index=1,size=5,nickName) {
    return new Promise(function (resolve, reject) {
        request.get(`/user?index=${index}&size=${size}&nickName=${nickName}`).then(res => {
            resolve(res.data)
        }).catch(err => {
            reject(err)
        })
    })
}
export function Admin(index=1,size=5,isAdmin=true){
    return new Promise(function (resolve, reject) {
        request.get(`/user?index=${index}&size=${size}&isAdmin=${isAdmin}`).then(res => {
            resolve(res.data)
        }).catch(err => {
            reject(err)
        })
    })
}
export function delUser(id){
    return new Promise(function(resolve,reject){
        request.delete(`/user/${id}`).then(res=>{
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

export function getFile(id){
    return new Promise(function(resolve,reject){
        request.get(`/files/`+id,).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}

export function addUser(model){
    return new Promise(function(resolve,reject){
        request.post(`/reg/`,model).then(res=>{
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
export function addArticle(id,model){
    return new Promise(function(resolve,reject){
        request.put(`/article/${id}`,model).then(res=>{
            resolve(res)
        }).catch(err=>{
            reject(err)
        })
    })
}
export async function getCategory(){
    return request.get(`/category`)
}

export async function postCategory(model) {
    return request.post(`/category`,model);
}
export async function delCategory(id) {
    return request.delete(`/category/${id}`);
}
export async function postFile(file){
    return request.post(`/files`,file)
}