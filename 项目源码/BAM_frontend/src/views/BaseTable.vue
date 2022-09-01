  <template>
  <div>
    <el-card class="box-card">
    <el-card class="top-card">
      <div class="search-box">
        <el-dropdown split-button @command="handleCommand" class="el-dropdown">
          {{ this.queryString }}
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="用户名">用户名</el-dropdown-item>
            <el-dropdown-item command="用户Id">用户Id</el-dropdown-item>
            <el-dropdown-item command="用户昵称">用户昵称</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
        <el-autocomplete
          v-model="state"
          :fetch-suggestions="querySearchAsync"
          placeholder="请输入内容"
          @select="handleSelect"
        ></el-autocomplete>
        <el-button
          type="primary"
          icon="el-icon-search"
          @click="search"
          class="el-search"
          >搜索</el-button
        >
      </div>
      <div class="top-button">
        <el-button
          type="primary"
          icon="el-icon-plus"
          circle
          @click="handleAdd"
        ></el-button>
        <el-button
          type="primary"
          circle
          icon="el-icon-refresh"
          @click="search()"
        ></el-button>
        <el-button type="info" square @click="getAdmin">管理员信息</el-button>
      </div>
</el-card>
      <el-table :data="tableData" style="width: 100%">
        <el-table-column
          fixed
          label="序号"
          width="50"
          align="center"
          type="index"
          
        ></el-table-column>
        <el-table-column
          fixed
          prop="id"
          label="ID"
          width="310"
          align="center"
        ></el-table-column>
        <el-table-column
          fixed
          prop="avatar"
          label="头像"
          width="120"
          align="center"
        >
          <template
            slot-scope="scope"
            v-if="imageUrl != null && scope.row.avatar != null"
          >
            <el-avatar :src="imageUrl + scope.row.avatar"></el-avatar>
          </template>
        </el-table-column>
        <el-table-column
          fixed
          prop="username"
          label="用户名"
          width="200"
          align="center"
        ></el-table-column>
        <el-table-column
          fixed
          prop="createdAt"
          label="注册时间"
          width="180"
          align="center"
        ></el-table-column>
        <el-table-column
          fixed
          prop="password"
          label="用户密码"
          width="200"
          align="center"
        
        ></el-table-column>
        <el-table-column
          fixed
          prop="nickName"
          label="用户昵称"
          width="200"
          align="center"
        ></el-table-column>
        <el-table-column fixed="right" label="操作" align="center">
          <template slot-scope="scope">
            <el-button
              fixed
              type="primary"
              circle
              icon="el-icon-edit"
              @click="handleEdit(scope.$index, scope.row)"
              class="Edit-button"
            ></el-button>
            <el-popconfirm
              title="这是一段内容确定删除吗？"
              @confirm="handleDelete(scope.$index, scope.row)"
            >
              <el-button
                fixed
                type="danger"
                circle
                icon="el-icon-delete"
                slot="reference"
              ></el-button>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>

      <el-dialog title="修改" :visible.sync="dialogFormVisible">
        <el-form :model="form">
          <el-form-item label="用户名" :label-width="formLabelWidth">
            <el-input v-model="form.username" autocomplete="off"></el-input>
          </el-form-item>

          <el-form-item label="用户密码" :label-width="formLabelWidth">
            <el-input v-model="form.password" autocomplete="off" placeholder="密码不少于八位"></el-input>
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
        <div slot="footer" class="dialog-footer">
          <el-button @click="dialogFormVisible = false" >取 消</el-button>
          <el-button type="primary" @click="handlePut()">确 定</el-button>
        </div>
      </el-dialog>
    </el-card>
    <el-pagination
      id="pagination"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
      align="right"
      :page-sizes="[5, 10]"
      :page-size="pageSize"
      layout="total, sizes, prev, pager, next, jumper"
      :total="total"
    >
    </el-pagination>
  </div>
</template>




<script>
import {
  Admin,
  delUser,
  putUser,
  searchUserById,
  searchUserByNick,
  searchUserByName,
} from "@/api/auth";
import appConfig from "@/config/appConfig";
import { addUser } from "../api/auth";

export default {
  methods: {
    getAdmin() {
      Admin(this.pageIndex, this.pageSize).then(({ data, count }) => {
        this.tableData = data;
        this.total = count;
      });
    },
    handleAdd() {
      (this.isEdit = false), (this.dialogFormVisible = true);
      this.form.nickName = null;
      this.form.password = null;
      this.form.username = null;
      this.form.isadmin = null;
    },
    beforeAvatarUpload(files) {
      if (files == null) {
        return false;
      } else {
        const isJPG = files.type === "image/jpeg";
        const isLt2M = files.size / 1024 / 1024 < 0.6;
        if (!isJPG) {
          this.$message.error("上传头像图片只能是 JPG格式!");
        }
        if (!isLt2M) {
          this.$message.error("上传头像图片大小不能超过 500KB!");
        }
        return isJPG && isLt2M;
      }
    },
    handleAvatarSuccess(res) {
      var id = res.data.id;
      this.form.avatar = id;
    },
    errorHandler() {
      return true;
    },
    transformTime(timestamp) {
      const dayjs = require("dayjs");
      const utc = require("dayjs/plugin/utc");
      dayjs.extend(utc);
      return dayjs.utc(timestamp).format("YYYY/MM/DD HH:mm:ss");
    },

    handleCommand(command) {
      this.queryString = command;
    },
    querySearchAsync(queryString, cb) {
      var restaurants = this.restaurants;
      var results = queryString
        ? restaurants.filter(this.createStateFilter(queryString))
        : restaurants;

      clearTimeout(this.timeout);
      this.timeout = setTimeout(() => {
        cb(results);
      }, 3000 * Math.random());
    },
    createStateFilter(queryString) {
      return (state) => {
        return (
          state.value.toLowerCase().indexOf(queryString.toLowerCase()) === 0
        );
      };
    },
    handleSelect(item) {
      console.log(item);
    },
    handleEdit(index, row) {
      (this.form.index = index), (this.form.id = row.id);
      this.isEdit = true;
      this.form.nickName = row.nickName;
      this.form.password = row.password;
      this.form.username = row.username;
      this.form.isadmin = row.isAdmin;
      this.dialogFormVisible = true;
    },
    handleDelete(index, row) {
      delUser(row.id).then((data) => {
        if (data.code == 1000) {
          this.$message.success("成功删除！");
          this.tableData.splice(index, 1);
        } else {
          this.$message.error("Error！");
        }
      });
    },
    handlePut() {
      var model = {
        username: this.form.username,
        password: this.form.password,
        nickName: this.form.nickName,
        avatar: this.form.avatar ? this.form.avatar : null,
        isAdmin: this.form.isadmin,
      };
      if (this.isEdit == true) {
        console.log(model);
        putUser(this.form.id, model).then((data) => {
          if (data.code === 1000) {
            this.$message.success("编辑成功！");
            this.dialogFormVisible = false;
            this.tableData[this.form.index].username = this.form.username;
            this.tableData[this.form.index].password = this.form.password;
            this.tableData[this.form.index].nickName = this.form.nickName;
            this.tableData[this.form.index].avatar = this.form.avatar;
          } else {
            this.$message.error("Error!");
            this.dialogFormVisible = false;
          }
        });
      } else {
        console.log(model);
        addUser(model).then((data) => {
          if (data.code === 1000) {
            this.$message.success("添加成功！");
            this.dialogFormVisible = false;
            this.search();
          } else {
            this.$message.error("Error!");
            this.dialogFormVisible = false;
          }
        });
      }
    },
    search() {
      if (this.queryString == "用户名") {
        searchUserByName(this.pageIndex, this.pageSize, this.state).then(
          ({ data, count }) => {
            for (const item of data) {
              item.createdAt = this.transformTime(item.createdAt);
            }
            (this.tableData = data), (this.total = count);
          }
        );
      }
      if (this.queryString == "用户Id") {
        searchUserById(this.pageIndex, this.pageSize, this.state).then(
          ({ data, count }) => {
            for (const item of data) {
              item.createdAt = this.transformTime(item.createdAt);
            }

            (this.tableData = data), (this.total = count);
          }
        );
      }
      if (this.queryString == "用户昵称") {
        searchUserByNick(this.pageIndex, this.pageSize, this.state).then(
          ({ data, count }) => {
            for (const item of data) {
              item.createdAt = this.transformTime(item.createdAt);
            }
            (this.tableData = data), (this.total = count);
          }
        );
      }
    },
    handleSizeChange(val) {
      this.pageSize = val;
      this.search();
    },

    handleClick(row) {
      console.log(row.username);
    },
    handleCurrentChange(val) {
      this.pageIndex = val;
      this.search();
    },
  },
  created() {
    this.search();
  },
  data() {
    return {
      isEdit: true,
      imageUrl: `${appConfig.baseUrl}/files/`,
      upFile:`${appConfig.baseUrl}/files`,
      queryString: "用户名",
      state: "",
      pageIndex: 0,
      pageSize: 5,
      input: "",
      tableData: [],
      total: 0,
      dialogTableVisible: false,
      dialogFormVisible: false,
      form: {
        index: "",
        id: "",
        nickName: "",
        password: "",
        username: "",
        avatar: "",
        isadmin: "",
      },
      formLabelWidth: "120px",
    };
  },
};
</script>

<style>
.Edit-button{
  margin-right:25px;
}
.search-box{
  float:left
}
.top-button{
  float: right;
}
.top-card{
  margin-bottom: 4px;
  padding-bottom:14px;
}
.handle-input {
  width: 300px;
  display: inline-block;
}
.fenye {
  text-align: center;
}

.el-dropdown-link {
  cursor: pointer;
  color: #409eff;
}
.el-icon-arrow-down {
  font-size: 12px;
}
.el-search {
  margin-left: 15px;
}

</style>

