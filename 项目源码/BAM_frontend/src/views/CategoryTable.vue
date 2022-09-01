  <template>
  <div>
    <el-card class="box-card">
    <el-card class="top-card">
      <div class="search-box">
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
          prop="categoryName"
          label="分类名称"
          width="200"
          align="center"
        ></el-table-column>
        <el-table-column fixed="right" label="操作" align="center">
          <template slot-scope="scope">
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
          <el-form-item label="分类名" :label-width="formLabelWidth">
            <el-input v-model="form.categoryName" autocomplete="off"></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="dialogFormVisible = false" >取 消</el-button>
          <el-button type="primary" @click="handlePut()">确 定</el-button>
        </div>
      </el-dialog>
    </el-card>
  </div>
</template>




<script>
import {
  getCategory,

} from "@/api/auth";
import appConfig from "@/config/appConfig";
import { delCategory, postCategory } from '../api/auth';

export default {
  methods: {
    handleAdd() {
      (this.isEdit = false), (this.dialogFormVisible = true);

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
    handleDelete(index, row) {
      console.log(row.id)
      delCategory(row.id).then((data) => {
        console.log(data)
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
        categoryName:this.form.categoryName
      };
      console.log(model)
      postCategory(model).then((data)=>{
        console.log(data)
         if (data.code == 1000) {
          this.$message.success("新增成功");
        } else {
          this.$message.error("Error！");
        }
      })
      this.dialogFormVisible = false
    },
    search() {
     getCategory().then((data)=>{
      console.log(data)
      this.tableData=data.data
      this.total=data.count
     })
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
        categoryName:""
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

