<template>
  <div>
    <div class="write">
      <el-scrollbar
        style="height: calc(100vh - 210px); width: 100%; overflow-x: hidden"
      >
        <div
          class="blog1"
          v-for="(formdata, index) in this.formdata"
          :key="formdata.id"
        >
          <ul>
            <article class="post-item">
              <section class="post-item-body">
                <div class="post-item-text">
                  <li class="title">
                    <router-link :to="getGoodsHref(formdata.id)">
                      {{ formdata.title }}
                    </router-link>
                  </li>
                  <div class="RichContent">
                    <li>
                      <span class="avatar">
                        <template
                          v-if="
                            imageUrl != null && formdata.authorAvatar != null
                          "
                        >
                          <el-avatar
                            :src="imageUrl + formdata.authorAvatar"
                          ></el-avatar>
                        </template>
                      </span>
                      <span class="remark">
                        {{ formdata.remark }}
                      </span>
                    </li>
                  </div>
                  <li>
                    <footer class="post-item-foot">
                      <span class="name">
                        {{ formdata.authorNickName }}
                      </span>
                      <span class="time">
                        {{ formdata.publishAt }}
                      </span>

                      <span class="rightInfo">
                        <i class="iconfont icon-good" @click="like(index)">
                          {{ formdata.likeCount }}
                        </i>

                        <i class="iconfont icon-bad" @click="unlike(index)">
                          {{ formdata.unlikeCount }}
                        </i>

                        <i
                          class="iconfont icon-favorite"
                          @click="collectionCount(index)"
                        >
                          {{ formdata.collectionCount }}
                        </i>
                        <i class="el-icon-view">
                          {{ formdata.visible }}
                        </i>
                      </span>
                    </footer>
                  </li>
                </div>              
              </section>
            </article>
            <el-divider></el-divider>
          </ul>
        </div>
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
      </el-scrollbar>
    </div>
    <ArticleList />
    
    <div class="block"></div>
  </div>
</template>

<script>
import ArticleList from "../components/ArticleList.vue";
import appConfig from '../config/appConfig'
import {
  getCategoryArticle,
  getCategory,
  queryArticle,
  queryArticleByTitle,
  likeArticleInfo,
  unlikeArticleInfo,
  collectionArticleInfo,
} from "../api/auth";
import { getUrlParamId } from "../utils/auth";

export default {
  components: { ArticleList },
  data() {
    return {
      categorys:[],
      total: 0,
      pageIndex: 0,
      pageSize: 5,
      formdata: [],
      imageUrl: `${appConfig.baseUrl}/files/`,
    };
  },
  //刷新时调用方法显示文章列表
  methods: {
    search() {
      let url = location.search;
      let input = getUrlParamId(url);
      let pathname=location.pathname
      if(pathname=='/news'){
        
        for(let item of this.categorys){
          if(item.label=='新闻'){
            getCategoryArticle(this.pageIndex,this.pageSize,item.value).then((data)=>{

              this.formdata=data.data;
              this.total=data.count;
            })
          }
        }
      }else if(pathname=='/chat'){
            for(let item of this.categorys){
          if(item.label=='杂谈'){
            getCategoryArticle(this.pageIndex,this.pageSize,item.value).then((data)=>{
              console.log(data.data)
              this.formdata=data.data;
              this.total=data.count;
            })
          }
        }
      }else if(pathname=='/tieleone')
      {
         for(let item of this.categorys){
          if(item.label=="C#"){
            getCategoryArticle(this.pageIndex,this.pageSize,item.value).then((data)=>{
              console.log(data.data)
              this.formdata=data.data;
              this.total=data.count;
            })
          }
        }
      }else if(pathname=='/tieletwo')
      {
         for(let item of this.categorys){
          if(item.label=="Vue"){
            getCategoryArticle(this.pageIndex,this.pageSize,item.value).then((data)=>{
              console.log(data.data)
              this.formdata=data.data;
              this.total=data.count;
            })
          }
        }
      }else if(pathname=='/tielethree'){
        for(let item of this.categorys){
          if(item.label=="WebApi"){
            getCategoryArticle(this.pageIndex,this.pageSize,item.value).then((data)=>{
              console.log(data.data)
              this.formdata=data.data;
              this.total=data.count;
            })
          }
        }
      }
      else if (input == null) {
        queryArticle(this.pageIndex, this.pageSize).then(({ data, count }) => {
          for (const item of data) {
            item.publishAt = this.transformTime(item.publishAt);
          }
          this.formdata = data;
          this.total = count;
        });
      } else {
        queryArticleByTitle(this.pageIndex, this.pageSize, input).then(
          ({ data, count }) => {
            for (const item of data) {
              item.publishAt = this.transformTime(item.publishAt);
            }
            this.formdata = data;
            this.total = count;
          }
        );
      }
    },
      
    handleSizeChange(val) {
      this.pageSize = val;
      this.search();
    },
    handleCurrentChange(val) {
      this.pageIndex = val;
      this.search();
    },
    getGoodsHref(val) {
      return `/ArticleInfo?id=` + val;
    },
    transformTime(timestamp) {
      const dayjs = require("dayjs");
      const utc = require("dayjs/plugin/utc");
      dayjs.extend(utc);
      return dayjs.utc(timestamp).format("YYYY/MM/DD HH:mm:ss");
    },
    async like(index) {
      let res = await likeArticleInfo(this.formdata[index].id);
      if (res.code == 1000) {
        this.formdata[index].likeCount++;
      } else {
        this.$message.error({
          title: "点赞失败",
          message: res.msg,
        });
      }
    },
    async unlike(index) {
      let res = await unlikeArticleInfo(this.formdata[index].id);

      if (res.code == 1000) {
        this.formdata[index].likeCount++;
      } else {
        this.$message.error({
          title: "点踩失败",
          message: res.msg,
        });
        // console.error(res);
      }
    },
    async collectionCount(index) {
      let res = await collectionArticleInfo(this.formdata[index].id);

      if (res.code == 1000) {
        this.formdata[index].collectionCount++;
      } else {
        this.$message.error({
          title: "收藏失败",
          message: res.msg,
        });
        // console.error(res);
      }
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
  watch: {
    $route: {
      handler: function () {
        this.search();
      },
    },
  },
};
</script>

<style scoped>
.el-icon-view{
  font-size: 16px;
}
.post-item-foot > * {
  margin-right: 20px;
  vertical-align: middle;
}
.time {
  display: inline-flex;
  fill: #bdbdbd;
  stroke: #bdbdbd;
  height: 20px;
  white-space: nowrap;
  align-items: center;
}
.name {
  color: #0053a1;
  font-size: 13px;
}
.remark {
  display: block;
  color: #555;
  font-size: 15px;
  font-weight: 400;
  line-height: 20px;
  text-overflow: clip;
}
.avatar {
  border-radius: 0;
  padding: 1px;
  border: 1px solid #ccc;
  float: left;
  margin-right: 5px;
  margin-top: 3px;
}
.post-item-foot {
  margin-top: 10px !important;
  display: flex !important;
  align-items: center !important;
  text-align: left;
  font-size: 13px !important;
  color: #555 !important;
}
.RichContent {
  margin-top: 15px;
  width: 750px;
  height: 60px;
}
.title {
  font-weight: bold;
  font-size: 18px;
  font-weight: 600;
  width: 100%;
  color: #005da6;
  text-decoration: underline;
}
a {
  color: inherit;
  text-decoration: none;
}
body {
  background-color: #fff;
  color: #333;
}

.write {
  width: 100%;
  max-height: calc(100vh - 210px);
}
.block {
  text-align: center;
}
.el-divider--horizontal {
  display: block;
  height: 1px;
  width: 100%;
  margin: 15px 0;
}
ul,
li {
  list-style: none;
}
.el-scrollbar__wrap {
  overflow-x: hidden;
}
</style>>


