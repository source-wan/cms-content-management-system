  <template>
  <div>
    <el-card class="box-card">
    <el-card class="top-card">
      <div class="search-box">
        <el-dropdown split-button @command="handleCommand" class="el-dropdown">
          {{ this.queryString }}
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="标题">标题</el-dropdown-item>
            <el-dropdown-item command="摘要">摘要</el-dropdown-item>
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
          circle
          icon="el-icon-refresh"
          @click="search()"
        ></el-button>
      </div>
</el-card>
      <el-table :data="tableData" style="width: 100%" class="a">
        <el-table-column
          fixed
          label="序号"
          width="50"
          align="center"
          type="index"
        ></el-table-column>
        <el-table-column
          fixed
          prop="title"
          label="标题"
          width="310"
          align="center"
        ></el-table-column>
        <el-table-column
          fixed
          prop="remark"
          label="摘要"
          width="200"
          align="center"
        ></el-table-column>
          <el-table-column
          fixed
          prop="publishAt"
          label="创建时间"
          width="200"
          align="center"
        ></el-table-column>
        <el-table-column
          fixed
          prop="updatedBy"
          label="操作用户"
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

      <el-dialog title="修改" :visible.sync="dialogFormVisible">
       <el-form
        :label-position="labelPosition"
        label-width="100px"
        :model="formLabelAlign"
      >
        <el-form-item label="标题:">
          <el-input v-model="formLabelAlign.Title"></el-input>
        </el-form-item>
        <el-form-item label="摘要:">
          <el-input v-model="formLabelAlign.Remarks"></el-input>
        </el-form-item>
        <el-form-item label="文章类别：">
            <el-select v-model="formLabelAlign.CategoryId" placeholder="请选择">
            <el-option
              v-for="item in categorys"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            >
            </el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="编写文章:">
          <mavon-editor
            :codeStyle="codeStyle"
            :toolbars="toolbars"
            v-model="formLabelAlign.Content"
            @imgAdd="imgAdd"
            :ishljs="true"
            ref="md"
          />
        </el-form-item>
        <el-button
          type="primary"
          icon="el-icon-circle-check"
          @click="handleAdd"
          >提交</el-button
        >
        <el-button type="primary"
          >取消<i class="el-icon-circle-close el-icon--right"> </i>
        </el-button>
      </el-form>
      </el-dialog>
  </div>
</template>




<script>
import { delArticle,searchArticleByTitle,addArticle,searchArticleByRemarks,postFile, getCategory } from "../api/auth";
import appConfig from '../config/appConfig'

export default {
    data() {
    return {
      labelPosition: "left",
            content: "",
      formLabelAlign: {
        Title: "",
        Remarks: "",
        Content:"",
        CategoryId: "",
      },
      value: '',
      categorys: [],
      codeStyle:'monokai-sublime',//主题
      test_html:undefined,
      toolbars: {
        bold: true, // 粗体
        italic: true, // 斜体
        header: true, // 标题
        underline: true, // 下划线
        strikethrough: true, // 中划线
        mark: true, // 标记
        superscript: true, // 上角标
        subscript: true, // 下角标
        quote: true, // 引用
        ol: true, // 有序列表
        ul: true, // 无序列表
        link: true, // 链接
        imagelink: true, // 图片链接
        code: true, // code
        table: true, // 表格
        fullscreen: true, // 全屏编辑
        readmodel: true, // 沉浸式阅读
        htmlcode: true, // 展示html源码
        help: true, // 帮助
        undo: true, // 上一步
        redo: true, // 下一步
        trash: true, // 清空
        save: true, // 保存（触发events中的save事件）
        navigation: true, // 导航目录
        alignleft: true, // 左对齐
        aligncenter: true, // 居中
        alignright: true, // 右对齐
        subfield: true, // 单双栏模式
        preview: true // 预览
      },
      queryString: "标题",
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
        title:"",
        content:"",
        category:""
      },
      formLabelWidth: "120px",
    };
  },
  methods: {
    imgAdd(pos,file){
      let imgData=new FormData()
      imgData.append('file',file);
      postFile(imgData).then((data)=>{
        console.log(data.data)
        this.$refs.md.$img2Url(pos,`${appConfig.baseUrl}/files/`+data.data.id)
      })
    },
    handleAdd(){
      console.log(this.formLabelAlign)
      addArticle(this.form.id,this.formLabelAlign).then(({data,code})=>{
        console.log(data)
        if(code===1000)
        {
           this.$message.success("编辑成功！");
            this.dialogFormVisible = false;
            this.search();
        } else {
            this.$message.error("Error!");
            this.dialogFormVisible = false;
             this.search();
          }
      })
    },
    
      handleEdit(index, row) {
      (this.form.index = index), (this.form.id = row.id);
      this.formLabelAlign.Title=row.title
      this.formLabelAlign.Remarks=row.remark
      this.formLabelAlign.Content=row.content
      this.dialogFormVisible = true;
    },
    beforeAvatarUpload(files) {
      if (files == null) {
        return false;
      } else {
        const isJPG = files.type === "image/jpeg"||files.type === "image/png";
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
    handleDelete(index, row) {
      delArticle(row.id).then((data) => {
        if (data.code == 1000) {
          this.$message.success("成功删除！");
          this.tableData.splice(index, 1);
        } else {
          this.$message.error("Error！");
        }
      });
    },
    search() {
      if (this.queryString == "标题") {
        searchArticleByTitle(this.pageIndex, this.pageSize, this.state).then(
          ({ data,count }) => {
            console.log(data)
            for (const item of data) {
              item.createdAt = this.transformTime(item.createdAt);
            }
            (this.tableData = data), (this.total = count);
          }
        );
      }
      if (this.queryString == "摘要") {
        searchArticleByRemarks(this.pageIndex, this.pageSize, this.state).then(
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
       getCategory().then((data)=>{

      if(data.code==1000){
        for(let item of data.data)
        {
          this.categorys.push(
            {
              label: item.categoryName.toString(),
              value: item.id.toString()
            }
          )
        }
      }
     })
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

