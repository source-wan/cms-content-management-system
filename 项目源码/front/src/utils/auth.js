const ACCESSTOKEN='accessToken'
const REFRESHTOKEN='refreshToken'


// 能获取到token，并且token存在，认为已经登录
export function isLogin() {
    let token = getToken();
    if (token) {
        return true;
    } else {
        return false;
    }
}

// 获取token，默认保存在localStorage中
export function getToken() {
    let token = localStorage.getItem(ACCESSTOKEN);
    return token;
}
// 获取refreshToken，默认保存在localStorage中
export function getRefreshToken() {
    let token = localStorage.getItem(REFRESHTOKEN);
    return token;
}


// 保存token和refreshToken
export function setToken(accessToken,refreshToken) {
    if (accessToken && refreshToken) {
        localStorage.setItem(ACCESSTOKEN,accessToken)
        localStorage.setItem(REFRESHTOKEN,refreshToken)
    }
}

// // 保存refreshToken
// export function setRefreshToken(refreshToken) {
//     if (refreshToken) {
//         localStorage.setItem(REFRESHTOKEN,refreshToken)
//     }q
// }

// 清除token和refreshToken和id
export function clearAllToken(){
    localStorage.removeItem(ACCESSTOKEN)
    localStorage.removeItem(REFRESHTOKEN)
}

// 获取路径上的参数(文章Id)
export function getUrlParamId(url) {
	return (url.split('='))[1];
}