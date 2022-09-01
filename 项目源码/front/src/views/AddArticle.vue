<template>
<el-scrollbar  style="height:calc(100vh - 200px);" >
  <div class="box">
    <div style="margin: 20px"></div>
    <div class="main">
      <el-form
        :label-position="labelPosition"
        label-width="100px"
        :model="formdata"
      >
        <el-form-item label="标题:">
          <el-input v-model="formdata.Title"></el-input>
        </el-form-item>
        <el-form-item label="摘要:">
          <el-input v-model="formdata.Remarks"></el-input>
        </el-form-item>
        <el-form-item label="文章类别：">
          <el-select v-model="formdata.categoryId" placeholder="请选择">
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
            v-model="formdata.Content"
            :ishljs="true"
            ref="md"
            @imgAdd="imgAdd"
            @change="change"
          />
        </el-form-item>
        <el-button
          type="primary"
          icon="el-icon-circle-check"
          @click="outputENter"
          >提交</el-button
        >
        <el-button type="primary"
        @click="handleCancel"
          >取消<i class="el-icon-circle-close el-icon--right"> </i>
        </el-button>
      </el-form>
    </div>
  </div>
  </el-scrollbar>
</template>


<script>
import { addArticle,postFile} from "@/api/auth";
import { getCategory } from '../../../BAM_frontend/src/api/auth';
import appConfig from '../config/appConfig'
export default {
  data() {
    return {
      categorys: [
        // {
        //   value: "e311cc7a-d12c-4859-9863-0bf0ca198cc3",
        //   label: "新闻",
        // },
        // {
        //   value: "c52a2096-b541-41cc-b8c6-ab6107c252b2",
        //   label: "杂谈",
        // },
        // {
        //   value: "f875e624-0a6f-46d2-89bb-6effda3fb2a4",
        //   label: "WebApi",
        // },
        // {
        //   value: "0c3260e9-1eba-4ac4-afbb-5d27d16a626d",
        //   label: "Vue",
        // },
        // {
        //   value: "83337a0e-49a9-4636-80c4-12ccc950ce96",
        //   label: "C#",
        // },
        ],
        value: '',
      html: "",
      configs: {},
      labelPosition: "left",
      formdata: {
        Title: "", //标题
        Content: "", //正文,
        Remarks: "", //摘要
        categoryId:"",
      },
      // categorys:[],
      // value:"",
      //工具栏
      //content: "这里是markdown编辑的内容",
      page_article: undefined,
      html_content: undefined,
      md_content: undefined,
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
        preview: true, // 预览
      },
      codeStyle: "monokai-sublime", //主题
      test_html: undefined,

   
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
    handleCancel(){
      this.$router.push('/')
    },
    outputENter() {
    
        console.log(this.formdata)
        addArticle(this.formdata).then((res)=>{
          console.log(res)
          if (res.code !== 1000) return this.$message.error('文章发布失败')
        this.$message.success('文章发布成功')
        // this.$router.push({ path: '/homepage' })
        })
    },
    change(value, render) {
      // render 为 markdown 解析后的结果
      this.test_html = render;
    },
  },
    created () {
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
.box {
 width: 60%;
  max-height: calc(100vh - 220px);
  margin-left: 40px;
  margin-right: 40px;

}
.el-form {
  height: 800px;
  width: 1000px;
}
.main {
  width: 600px;
 max-height: calc(100vh - 200px);

}
.el-form-item__label {
  padding: 0px;
}
.mavonEditor {
  min-height: 600px;
  min-width: 300px;
}
</style>