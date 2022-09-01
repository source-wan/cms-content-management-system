<template>
  <div class="box">
    <el-container>
      <div class="Info">
        <!-- <el-aside width="20px" > -->
        <el-card shadow="hover">
          <div class="userCard">
            <el-avatar :size="150" :src="imgUrl"></el-avatar>
            <div class="userInfo">
              <p class="important-font">您好管理员，{{this.nickName}}！</p>
            </div>
          </div>
          <div class="login-info">
            <p>上次登录时间: 2022/07/06 18:16</p>
          </div>
        </el-card>
        <!-- </el-aside> -->
      </div>
      <div class="admin">
        <el-card class="box-card" shadow="hover">
          <div style="margin: 20px"></div>
          <el-form :model="form">
          <el-form-item label="用户名" :label-width="formLabelWidth">
            <el-input v-model="form.username" autocomplete="off"></el-input>
          </el-form-item>

          <el-form-item label="用户密码" :label-width="formLabelWidth">
            <el-input v-model="form.password" autocomplete="off"></el-input>
          </el-form-item>

          <el-form-item label="用户昵称" :label-width="formLabelWidth">
            <el-input v-model="form.nickName" autocomplete="off"></el-input>
          </el-form-item>
          <el-form-item label="是否管理员" :label-width="formLabelWidth">
            <el-radio v-model="form.isadmin" :label="true">是</el-radio>
            <el-radio v-model="form.isadmin" :label="false">否</el-radio>
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
                只能上传jpg/png文件，且不超过500kb
              </div>
            </el-upload>
          </el-form-item>
        </el-form>
          <el-button >取 消</el-button>
          <el-button type="primary" @click="handlePut()">确 定</el-button>
        </el-card>
      </div>
    </el-container>
  </div>
</template>

<script>
import appConfig from '@/config/appConfig';
import { getUser, putUser } from '../api/auth';
import { getToken } from '../utils/auth';
import jwtDecode from 'jwt-decode';

export default {
    created() {
      var token=getToken()
      this.id=jwtDecode(token).UserId
      this.username=jwtDecode(token).Username
      getUser(this.id).then(({data})=>{
        this.nickName=data.nickName
        this.imgUrl=`${appConfig.baseUrl}/files/`+data.avatar
        this.form.id=data.id
        this.form.username=data.username
        this.form.password=data.password
        this.form.nickName=data.nickName
        this.form.isadmin=data.isAdmin
      }) 
  },
  data() {
    return {
      upFile:`${appConfig.baseUrl}/files`,
      id:'',
      username:'',
      nickName:'',
      imgUrl:'',
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
        isadmin: "",
      },
    };
  },
  methods: {
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
    handlePut() {
      var model = {
        username: this.form.username,
        password: this.form.password,
        nickName: this.form.nickName,
        avatar: this.form.avatar ? this.form.avatar : null,
        isAdmin: this.form.isadmin,
      };
        console.log(model);
        putUser(this.form.id, model).then((data) => {
          if (data.code === 1000) {
            console.log(data)
            this.$message.success("编辑成功！");
          } else {
            this.$message.error("Error!");
          }
        });
    },
    handleAvatarSuccess(res) {
      var id = res.data.id;
      this.form.avatar = id;
    },
  },
};
</script>
<style scoped>
.box-card {
  width: 600px;
}

.admin {
  margin-top: 1px;
  margin-left: 150px;
  width: 400px;
  height: 500px;
}
.box {
  padding: 0px;
  margin: 0px;
  width: 100%;
  height: 100%;
  background: url(../assets/white-logo.png);
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



</style>