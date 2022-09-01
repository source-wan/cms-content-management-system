<template>
  <body>
    <div class="img">
      <div class="loginContainer">
        <div class="box">
          <el-card slot="header" header="欢迎使用">
            <el-form
              shadow="always"
              :model="formData"
              status-icon
              :rules="rules"
              ref="ruleForm"
              label-width="100px"
              class="demo-ruleForm"
              @keyup.enter.native="submitForm()"
            >
              <div class="label">
                <el-form-item label="用户名" prop="username">
                  <el-input
                    prefix-icon="el-icon-user-solid"
                    type="text"
                    v-model="formData.username"
                    autocomplete="off"
                  ></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="password">
                  <el-input
                    prefix-icon="el-icon-lock"
                    type="password"
                    v-model="formData.password"
                    autocomplete="off"
                  ></el-input>
                </el-form-item>
              </div>
              <el-form-item>
                <div class="button">
                  <Vcode
                    :show="isShow"
                    @success="success"
                    @close="close"
                    :imgs="[Img1]"
                  />
                  <el-button type="primary" @click="submitForm">登录</el-button>
                  <el-button class="admin" type="primary" @click="resetForm"
                    >注册</el-button
                  >
                </div>
              </el-form-item>
            </el-form>
          </el-card>
        </div>
      </div>
    </div>
  </body>
</template>

<script>
import Vcode from "vue-puzzle-vcode";
import { login } from "../api/auth";
import { setToken } from "../utils/auth";
import Img1 from "../assets/blogwhite.jpg";
export default {
  data() {
    return {
      Img1,
      isShow: false,
      formData: {
        username: "",
        password: "",
      },
      rules: {
        username: [
          { required: true, message: "请输入用户名", trigger: "blur" },
        ],
        password: [{ required: true, message: "请输入密码", trigger: "blur" }],
      },
    };
  },
  components: {
    Vcode,
  },
  methods: {
    success(msg) {
      this.isShow = false;
      console.log(msg);
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          login(this.formData)
            .then((data) => {
              let res = data;
              console.log(res);
              if (res.code === 1000) {
                setToken(res.data.accessToken, res.data.refreshToken);
                this.$router.push("/");
              } else {
                this.$message({ type: "error", message: res.msg });
              }
            })
            .catch((err) => {
              console.log(err);
            });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    close() {
      this.isShow = false;
    },
    submitForm() {
      if (this.formData.username != null && this.formData.password != null) {
        this.isShow = true;
      }
    },
    resetForm() {
      window.location.href = "./register";
    },
  },
};
</script>
<style scoped>
.loginContainer .el-card__body {
  width: 500px;
  padding: 0%;
  margin: 0%;
}
.el-card {
  margin: 0px;
  padding: 0px;
  width: 400px;
  height: 300px;
}
.box {
  height: 350px;
  width: 400px;
  border-radius: 30px;
}
.el-button {
  margin: 2px;
  margin-right: 80px;
}

.admin {
  margin-left: -70px;
}
.loginContainer {
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 35px;
  height: 95vh;
  opacity: 0.8;
  text-align: center;
  padding-left: 15%;
  margin-top: 20px;
}

#register {
  position: relative;
  right: 220px;
}

body,
html {
  padding: 0px;
  margin: 0px;
  width: 100%;
  height: 100%;
  background: url(../assets/logo.png);
  background-position: center center;
  /* 背景图不平铺 */
  background-repeat: no-repeat;
  /* 当内容高度大于图片高度时，背景图像的位置相对于viewport固定 */
  background-attachment: fixed;
  /* 让背景图基于容器大小伸缩 */
  background-size: cover;
  /* 设置背景颜色，背景图加载过程中会显示背景色 */
  background-color: #464646;
}

/* 给边框设置弧度 */
div {
  border-radius: 20px;
}
</style>
