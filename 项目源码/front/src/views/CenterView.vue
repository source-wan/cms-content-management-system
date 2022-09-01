

<template>
  <div class="personalCenter">
    <!-- <el-card class="box"> -->
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>个人中心</el-breadcrumb-item>
    </el-breadcrumb>

    <div class="center">
      <el-container>
        <el-main class=".el-main">
          <el-card class="topcard">
            <el-aside width="300px" class="left-show">
              <el-card shadow="hover">
                <div class="userCard">
                  <el-avatar :size="150" :src="imgUrl"></el-avatar>
                  <div class="userInfo-top">
                    <p class="important-font">您好，{{ form.nickName }}！</p>
                  </div>
                </div>         
              </el-card>
            </el-aside>
            <el-aside width="600px">
              <el-card class="right-show">
                <p>个人资料</p>
                <el-form label-width="80px" :model="form">
                  <el-form-item label="用户名" :label-width="formLabelWidth">
                    <el-input
                      v-model="form.username"
                      autocomplete="off"
                    ></el-input>
                  </el-form-item>
                  <el-form-item label="用户昵称" :label-width="formLabelWidth">
                    <el-input
                      v-model="form.nickName"
                      autocomplete="off"
                    ></el-input>
                  </el-form-item>
                  <el-form-item label="用户密码" :label-width="formLabelWidth">
                    <el-input
                      v-model="form.password"
                      autocomplete="off"
                    ></el-input>
                  </el-form-item>
                  <el-form-item label="头像" :label-width="formLabelWidth">
                    <el-upload
                      class="upload-demo"
                      drag
                      :action="upFile"
                      show-file-list
                      :on-success="handleAvatarSuccess"
                      :before-upload="beforeAvatarUpload"
                    >
                      <i class="el-icon-upload"></i>
                      <div class="el-upload__text">
                        将文件拖到此处，或<em>点击上传</em>
                      </div>
                      <div class="el-upload__tip" slot="tip">
                        只能上传jpg文件，且不超过2mb
                      </div>
                    </el-upload>
                  </el-form-item>

                  <el-button-group
                    style="float: right; padding: 3px 0"
                    type="text"
                  >
                    <el-button type="primary" size="medium" round
                      >刷新</el-button
                    >
                  </el-button-group>
                  <el-button-group
                    style="float: right; padding: 3px 0"
                    type="text"
                  >
                    <el-button
                      type="primary"
                      size="medium"
                      round
                      @click="handlePut()"
                      >保存</el-button
                    >
                  </el-button-group>
                </el-form>
              </el-card>
            </el-aside>
          </el-card>
          <el-card class="ArticleLists">
            <el-container>
              <el-main>
                <el-card class="box-card">
                  <div slot="header" class="clearfix">
                    <span style="float: left"><b>已发表文章</b></span>
                  </div>
                  <div class="article">
                      <ArticleTable></ArticleTable>
                  </div>
                </el-card>
              </el-main>
            </el-container>
          </el-card>
        </el-main>
      </el-container>
    </div>
    <!-- </el-card> -->
  </div>
</template>
<script>
import appConfig from "@/config/appConfig";
import { getUser, putUser } from "../api/auth";
import { getToken } from "../utils/auth";
import jwtDecode from "jwt-decode";
import ArticleTable from "./ArticleTable.vue";

export default {
    name: "PersonalCenter",
     components: { ArticleTable },
    data() {
        return {
            id: "",
            username: "",
            nickName: "",
            imgUrl: "",
            upFile:`${appConfig.baseUrl}/files`,
            labelPosition: "top",
            imageUrl: "",
            formLabelWidth: "120px",
            form: {
                index: "",
                id: "",
                nickName: "",
                password: "",
                username: "",
                avatar: "",
            },
        };
    },
    created() {
        var token = getToken();
        this.id = jwtDecode(token).UserId;
        this.username = jwtDecode(token).Username;
        getUser(this.id).then(({ data }) => {
            this.nickName = data.nickName;
            this.imgUrl = `${appConfig.baseUrl}/files/` + data.avatar;
            this.form.id = data.id;
            this.form.username = data.username;
            this.form.password = data.password;
            this.form.nickName = data.nickName;
        });
    },
    methods: {
        handlePut() {
            var model = {
                username: this.form.username,
                password: this.form.password,
                nickName: this.form.nickName,
                avatar: this.form.avatar ? this.form.avatar : null,
            };
            console.log(model);
            putUser(this.form.id, model).then((data) => {
                if (data.code === 1000) {
                    console.log(data);
                    this.$message.success("编辑成功！");
                }
                else {
                    this.$message.error("Error!");
                }
            });
        },
        handleAvatarSuccess(res) {
            var id = res.data.id;
            this.form.avatar = id;
        },
        beforeAvatarUpload(files) {
            if (files == null) {
                return false;
            }
            else {
                const isJPG = files.type === "image/jpeg"||"image/png";
                const isLt2M = files.size / 1024 / 1024 < 2046;
                if (!isJPG) {
                    this.$message.error("上传头像图片只能是 JPG格式!");
                }
                if (!isLt2M) {
                    this.$message.error("上传头像图片大小不能超过 2mb!");
                }
                return isJPG && isLt2M;
            }
        },
    },
   
};
</script>
<style scoped>
.el-icon-arrow-right {
  font-size: 14px;
}

.box {
  margin: auto;
}

.el-aside {
  padding: auto;
}


.personalCenter {
  width: 80%;
  margin: auto;
}

.el-main {
  text-align: center;
  line-height: 160px;
}

.userInfo-top {
  margin: 0px;
}

.userInfo-bottom {
  padding-top: 0px;
  margin: 0px;
  line-height: 10px;
}

.left-show {
  float: left;
  margin-left: 160px;
  margin-right: 100px;
  margin-top: 80px;
  margin-bottom: 100px;
}

.right-show {
  width: 540px;
  margin-top: 30px;

  padding-left: 50px;
}

.ArticleLists {
  margin-top: 10px;
}

.show-card {
  width: 400px;
  height: 100%;
}

.el-header {
  line-height: 60px;
  text-align: center;
}

.el-aside {
  line-height: 44px;
  text-align: center;
}

.el-main {
  line-height: 36px;
  text-align: center;
}

.img {
  float: left;
}

.demo-basic--circle {
  /* align: 'center'; */
  margin-top: 20px;
  margin-left: 20px;
}

.block {
  margin-left: 25px;
  font-weight: bold;
}

.text {
  font-size: 14px;
}

.item {
  margin-bottom: 18px;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}

.box {
  width: 90%;
}

li {
  text-align: left;
}
</style>  
