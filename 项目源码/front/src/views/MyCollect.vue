<template>
  <div>
    <div class="write">
      <el-scrollbar
        style="height: calc(100vh - 210px); width: 100%; overflow-x: hidden"
      >
        <div
          class="blog1"
          v-for="formdata in this.formdata"
          :key="formdata.id"
        >
          <ul>
            <article class="post-item">
              <section class="post-item-body">
                <div class="post-item-text">
                  <li class="title">
                    <a
                      class="post-item-title"
                      :href="getGoodsHref(formdata.articleId)"
                    >
                      {{ formdata.title }}
                    </a>
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
                        {{ formdata.createdAt }}
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
import { getCollection } from "../api/auth";
import appConfig from "../config/appConfig" 

export default {
  components: { ArticleList },
  data() {
    return {
      total: 0,
      pageIndex: 0,
      pageSize: 5,
      formdata: [],
      imageUrl: `${appConfig.baseUrl}/files`,
    };
  },
  methods: {
    search() {
        getCollection(this.pageIndex, this.pageSize).then((data) => {
          for (const item of data.data) {
            item.createdAt = this.transformTime(item.createdAt);
          }
          this.formdata = data.data;
          this.total = data.count;
          console.log(this.formdata)
        });
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
      return `${appConfig.frontUrl}/ArticleInfo?id=` + val;
    },
    transformTime(timestamp) {
      const dayjs = require("dayjs");
      const utc = require("dayjs/plugin/utc");
      dayjs.extend(utc);
      return dayjs.utc(timestamp).format("YYYY/MM/DD HH:mm:ss");
    },
  },
  created() {
    this.search()
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
.el-icon-view {
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
</style>


