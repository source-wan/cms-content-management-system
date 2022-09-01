<template>
  <div class="HeaderInfo">
    <el-row class="tac">
      <el-col :span="12">
        <corporation-info></corporation-info>
        <!-- <nav-menu></nav-menu> -->
      </el-col>
    </el-row>
    <!-- background-color="#B3C0D1"  -->

    <el-menu
      class="el-menu-demo"
      mode="horizontal"
      background-color="#fff"
      text-color="black"
      active-text-color="#66ccff"
      router
    >
      <el-menu-item index="/">首页</el-menu-item>
      <el-menu-item index="/news">新闻</el-menu-item>
      <el-menu-item index="/chat">杂谈</el-menu-item>
      <el-submenu index="2">
        <template slot="title">技术专区</template>
        <el-menu-item index="/tieleone">C#</el-menu-item>
        <el-menu-item index="/tieletwo">Vue</el-menu-item>
        <el-menu-item index="/tielethree">WebApi</el-menu-item>
      </el-submenu>
      <el-input
        class="Select"
        placeholder="请输入内容"
        v-model="input"
        clearable
      ></el-input>

      <el-button class="Select" type="primary" icon="el-icon-search" @click="search(input)">搜索</el-button >
      <span class="HeadPhoto">
        <div class="topRight">
          <span class="LoginButton" v-if="!showname">
            <el-button type="primary" @click="submitForm">登录</el-button>
            <el-button type="primary" @click="register">注册</el-button>
          </span>
        <el-submenu index="3">
        <template slot="title">
          <div class="avatarBox">
            <el-avatar :src="imgUrl"></el-avatar>
          </div>
          <div class="rightBox">  
              {{ this.username}}           
              </div>
          </template>
         <el-dropdown-item command="/Center" @click.native="Center">
        个人中心
        </el-dropdown-item >
         <el-dropdown-item command="/logout" @click.native="OutLogin">
        退出登录
        </el-dropdown-item>
      </el-submenu>
        </div>
      </span>
    </el-menu>
  </div>
</template>


<script>
import { clearAllToken } from "@/utils/auth";
import CorporationInfo from "./CorporationInfo.vue";
import { isLogin } from "@/utils/auth";
import jwtDecode from "jwt-decode";
import { getToken } from "../../../BAM_frontend/src/utils/auth";
import { getUser } from "../api/auth";
import appConfig from "@/config/appConfig";
export default {
  components: {
    CorporationInfo,
  },
  data() {
    return {
      username: "",
      imgUrl: ``,
      input: "",
      showname: true,
      submitForm() {
        window.location.href = "/login";
      },
      register() {
        window.location.href = "/register";
      },
      fits: "fill",
      url: "https://fuss10.elemecdn.com/e/5d/4a731a90594a4af544c0c25941171jpeg.jpeg",
    };
  },
  methods: {
    handleCommand(command) {
      this.queryString = command;
    },
    search(input) {
        this.$router.push(`/homepage?title=${input}`);
    },
    OutLogin() {
      var exit = confirm("确认退出登录?");
      if (exit) {
        clearAllToken();
        window.location.href = "/";
      }
    },
    Center() {
      window.location.href = "/PersionCenter";
    },
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    },
  },
  created() {
    this.showname = isLogin();
    let token = getToken();
    if (token !== null) {
      this.id = jwtDecode(token).UserId;
      getUser(this.id).then(({ data }) => {
        this.imgUrl = `${appConfig.baseUrl}/files/` + data.avatar;
      });
      this.username = jwtDecode(token).Username;
    }
  },
};
</script>
<style scoped>

.el-dropdown:hover {
  cursor: pointer;
}

.select {
  width: 60px;
  height: 60px;
  /* background-color: red; */
  margin-top: -135px;
}
.rightBox {
  width: 48px;
  height: 30px;
  /* background-color: rgb(48, 202, 71); */
  margin-left: 50px;
  margin-top: -60px;
}
.avatarBox {
  padding-top: 10px;
  height: 50px;
  width: 50px;
  /* background-color: aqua; */
}
.el-avatar{
  padding-bottom: 4px;
  margin-bottom: 24px;
}
.topRight {
  width: 170px;
  position: absolute;
  top: 0;
  right: 0;
  /* background-color: red; */
}
body,
html {
  height: 100%;
  width: 100%;
}
.HeaderInfo {
  background-color: rgb(248, 235, 235);
  color: black;
  display: flex;
margin-bottom: 10px;
}

.el-input,
.el-input el-input--suffix {
  position: relative;
  font-size: 14px;
  /* display: inline-block; */
  width: 20%;
  margin-left: 40%;
}

.el-menu-demo {
  display: inline-block;
}

.classify {
  margin-left: 1%;
}

.el-menu-demo {
  /* margin-left: 180px; */
  position: relative;
  width: 100%;
  height: 100%;
}
/* .LoginButton {
  padding-left: 20px;
} */
.HeadPhoto {
  padding-left: 30px;
}
.item {
  margin-top: 10px;
  margin-right: 40px;
}
.el-dropdown-link {
  cursor: pointer;
  color: black;
}
.el-icon-arrow-down {
  font-size: 12px;
}
.demonstration {
  display: block;
  color: #8492a6;
  font-size: 14px;
  margin-bottom: 0px;
}
</style>