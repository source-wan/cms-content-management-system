<template>
  <div>
    <div v-for="article in info.results" v-bind:key="article.url" id="articles">
      <div>
        <span v-for="tag in article.tags" v-bind:key="tag" class="tag">
          {{ tag }}
        </span>
      </div>
      <div class="article-title">
        {{ article.title }}
      </div>
      <div>{{ formatted_time(article.created) }}</div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "ArticleList",
  data: function () {
    return {
      info: [
        {
          tag: "标题",
          title: "概要",
          formatted_time: "时间",
        },
      ],
    };
  },
  mounted() {
  },
  methods: {
    fetchadat() {
      axios.get("/api/auth").then((response) => (this.info = response.data));
    },
    formatted_time: function (iso_date_string) {
      const date = new Date(iso_date_string);
      return date.toLocaleDateString();
    },
  },
};
</script>

<!-- "scoped" 使样式仅在当前组件生效 -->
<style scoped>
#articles {
  padding: 10px;
}

.article-title {
  font-size: large;
  font-weight: bolder;
  color: black;
  text-decoration: none;
  padding: 5px 0 5px 0;
}

.tag {
  padding: 2px 5px 2px 5px;
  margin: 5px 5px 5px 0;
  font-family: Georgia, Arial, sans-serif;
  font-size: small;
  background-color: #4e4e4e;
  color: whitesmoke;
}
</style>