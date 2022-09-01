<template>
  <div>
    <el-card class="box-card">
      <el-form :inline="true" class="demo-form-inline">
        <el-form-item label="">
          <el-input placeholder="请输入关键字"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary">查询</el-button>
        </el-form-item>
      </el-form>
      <div>
        <el-button type="primary" icon="el-icon-plus" @click="handleAdd"
          >新增</el-button
        >
        <el-button type="primary" icon="el-icon-refresh" @click="handleRefresh"
          >刷新</el-button
        >
      </div>
    </el-card>
    <el-card>
      <el-table :data="tbData" style="width: 100%">
        <el-table-column label="序号" width="60">
          <template slot-scope="scope">
            {{ scope.$index + 1 }}
          </template>
        </el-table-column>
        <el-table-column label="品类名称" property="cateName">
        </el-table-column>
        <el-table-column label="是否启用" property="isActived">
          <template slot-scope="scope">
            {{ scope.row.isActived ? "启用" : "禁用" }}
          </template>
        </el-table-column>
        <el-table-column label="更新时间" property="updatedTime">
        </el-table-column>
        <el-table-column label="操作">
          <template slot-scope="scope">
            <el-button
              type="primary"
              icon="el-icon-edit"
              size="mini"
              circle
              @click="handleEdit(scope.$index, scope.row)"
            ></el-button>
            <el-button
              type="danger"
              icon="el-icon-delete"
              size="mini"
              circle
              @click="handleDelete(scope.$index, scope.row)"
            ></el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
    <div>
      <el-dialog title="文章类别" :visible.sync="dialogFormVisible">
        <el-form :model="form" ref="cateForm" inline :rules="rules">
          <el-form-item label="id：" prop="id" style="display: none">
            <el-input v-model="form.id" autocomplete="off"></el-input>
          </el-form-item>
          <el-form-item label="类别：" prop="cateName">
            <el-input v-model="form.cateName" autocomplete="off"></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="handleCancel">取 消</el-button>
          <el-button type="primary" @click="handleSave">确 定</el-button>
        </div>
      </el-dialog>
    </div>
  </div>
</template>
<script>
const axios = require("axios");
export default {
  data() {
    return {
      closeOnClickModal: false,
      dialogFormVisible: false,
      form: {
        id: "",
        cateName: "",
      },
      tbData: [],
      rules: {
        cateName: [
          {
            required: true,
            trigger: "blur",
            message: "类别名称不能为空",
          },
        ],
      },
    };
  },
  methods: {
    // 刷新数据
    handleRefresh() {
      this.getData();
    },
    // 新增，这里先模拟请求新增的过程
    handleAdd() {
      this.dialogFormVisible = true;
      // 在对话框加载初始化下一轮，才进行重置表单的动作，要不然会出错
      this.$nextTick(() => {
        this.$refs.cateForm.resetFields();
      });
    },
    handleEdit(index, row) {
      console.log(index, row);
      this.dialogFormVisible = true;
      // 在对话框加载初始化下一轮，才进行重置表单的动作，要不然会出错
      this.$nextTick(() => {
        this.$refs.cateForm.resetFields();
        this.form.id = row.id;
        this.form.cateName = row.cateName;
      });
    },
    handleCancel() {
      this.dialogFormVisible = false;
    },
    handleSave() {
      this.$refs["cateForm"].validate((valid) => {
        if (valid) {
          this.dialogFormVisible = false;
          // 如果id存在并且值不为0，则认为是修改，否则为新增
          if (this.form.id && this.form.id.length > 0) {
            axios.put("/article/category", this.form).then(({ data }) => {
              let idx = this.tbData.findIndex((item) => {
                return item.id === data.data.id;
              });
              // 使用给数组某元素赋值的方式，是没有办法动态响应数据变化
              this.tbData.splice(idx, 1, data.data);
            });
          } else {
            axios.post("/article/category", this.form).then(({ data }) => {
              this.tbData.push(data.data);
            });
          }
        } else {
          this.$message.error('数据验证失败，请检查后重试')
          return false;
        }
      });
    },

    handleDelete(index, row) {
      console.log(index, row);
      axios.delete(`/article/category/${row.id}`).then(({ data }) => {
        //删除成功以后，意味着后端数据库对应的记录已经删除，那么在前端上，也要删除那条数据
        console.log(data);
        if (data.code === 1000) {
          this.tbData.splice(index, 1);
          this.$message.success(data.msg);
        } else {
          this.$message.error(data.msg);
        }
      });
    },
    getData() {
      axios
        .get("/article/category")
        .then(({ data }) => {
          console.log(data);
          this.tbData = data.data;
        })
        .catch((err) => {
          console.log(err);
        });
    },
  },
  created: function () {
    this.getData();
  },
};
</script>
<style>
.el-card__body {
  display: flex !important;
  justify-content: space-between;
}
</style>