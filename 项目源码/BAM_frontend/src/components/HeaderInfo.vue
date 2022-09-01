<template>
  <div class="headerInfo">
    <el-avatar :src="imgUrl"></el-avatar>
    <el-dropdown @command="handleCommand">
      <span class="el-dropdown-link">
        {{this.nickName}}<i class="el-icon-arrow-down el-icon--right"></i>
      </span>
      <el-dropdown-menu slot="dropdown">
       <el-dropdown-item command="/logout" @click.native="OutLogin">退出登录</el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>
  </div>
</template>
<script>
import appConfig from '@/config/appConfig'
import {clearAllToken,getToken} from '@/utils/auth'
import jwtDecode from 'jwt-decode'
import { getUser } from '../api/auth'
export default {
  data(){
    return{
      id:'',
      nickName:'',
      imgUrl:'',
    }
  },
  methods:{
    handleCommand:function(arg){
      clearAllToken()

      this.$router.push(arg)
  

    },
       OutLogin() {
      var exit = confirm("确认退出登录?");
      if (exit) {
        clearAllToken();
        window.location.href = "/login";
      }
    },
  },

  created() {
      var token=getToken()
      this.id=jwtDecode(token).UserId
      this.username=jwtDecode(token).Username
      getUser(this.id).then(({data})=>{
        this.imgUrl=`${appConfig.baseUrl}/files/`+data.avatar
        this.nickName=data.nickName
      }) 
  },
}
</script>
<style scoped>
.headerInfo { 
  display:flex;
  justify-content: right;
  align-items: center;
  margin-left: 1150px;
}
.el-avatar{
  margin-right: 5px;
}
</style>