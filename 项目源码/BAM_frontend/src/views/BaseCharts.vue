<template>
  <div class="box">
    <el-row :gutter="20">
      <el-col :span="8">
        <div class="grid-content bg-purple">
          <!-- 首页user信息 -->
          <el-card shadow="hover">
            <div class="userCard">
              <el-avatar :size="150" :src="imgUrl"></el-avatar>
              <div class="userInfo">
                <p class="important-font">{{this.username}}</p>
                <p class="secondary-font">管理员</p>
              </div>
            </div>
            <div class="login-info">
              <p>上次登录时间: 2022/07/06 18:16</p>
            </div>
          </el-card>
          <!-- 统计表柱状图 -->
          <el-card style="height: 280px">
            <div style="height: 280px" ref="barEcharts">{{ initBarEcharts() }}</div>
          </el-card>
        </div>
        <!-- 扇形图 -->
        <!-- <div style="width: 100%; height: 265px" ref="pieEcharts">
          {{ initPieEcharts() }}
        </div>-->
      </el-col>
      <el-col :span="16">
        <!-- 六个订单信息 -->
        <div class="num">
          <el-card
            shadow="hover"
            v-for="item in countData"
            :key="item.name"
            :body-style="{ display: 'flex', padding: 0 }"
            class="OrderCard"
          >
            <i class="icon" :class="'el-icon-' + item.icon" :style="{ background: item.color }"></i>
            <div>
              <p class="important-font">{{ item.value }}</p>
              <p class="secondary-font">{{ item.name }}</p>
            </div>
          </el-card>
        </div>
      </el-col>
    </el-row>
  </div>
</template>
 
<script>
import appConfig from "@/config/appConfig";
import * as echarts from "echarts";
import { getUser } from "../api/auth";
import jwtDecode from "jwt-decode";
import { getToken } from "../utils/auth";

export default {
  name: "BaseCharts",


  created() {
 
 

    var token = getToken();
    this.id = jwtDecode(token).UserId;
    this.username = jwtDecode(token).Username;
    getUser(this.id).then(({ data }) => {
      this.imgUrl = `${appConfig.baseUrl}/files/` + data.avatar;
    });
  },
  data() {
    return {
      username: "",
      imgUrl: "",
      value: new Date(),
      countData: [
        {
          name: "在线用户",
          value: 60,
          icon: "success",
          color: "#2ec7c9"
        },
        {
          name: "今日浏览记录",
          value: 500,
          icon: "star-on",
          color: "#ffb980"
        },
        {
          name: "今日发表博客文章数",
          value: 20,
          icon: "s-goods",
          color: "#5ab1ef"
        },
        {
          name: "今日点赞数",
          value: 36,
          icon: "success",
          color: "#2ec7c9"
        },
        {
          name: "今日博客删除数",
          value: 0,
          icon: "star-on",
          color: "#ffb980"
        },
        {
          name: "今日新增用户数",
          value: 10,
          icon: "s-goods",
          color: "#5ab1ef"
        }
      ]
    };
  },
  
  methods: {

    //柱状图
    initBarEcharts() {
      // 新建一个promise对象
      let p = new Promise(resolve => {
        resolve();
      });
      //然后异步执行echarts的初始化函数
      p.then(() => {
        //	此dom为echarts图标展示dom
        let myChart = echarts.init(this.$refs.barEcharts);
        let option = {
          title: {
            text: "统计表"
          },
          tooltip: {},
          legend: {
            data: ["今日数据", "昨日数据"]
          },
          xAxis: {
            data: ["浏览", "发表", "点赞", "评论", "删除", "新增用户"]
          },
          yAxis: {},
          series: [
            {
              name: "今日数据",
              type: "bar",
              data: [5, 20, 36, 10, 10, 20]
            },
            {
              name: "昨日数据",
              type: "bar",
              data: [10, 18, 34, 8, 12, 21]
            }
          ]
        };
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
      });
    },
   
  },
    watch: { // 路由监听：监听路由的变化，从而做出相应操作

           $router(to){
            console.log(11);
              console.log(to);
                console.log(this)
              }
  },
};
</script>
 
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="less" scoped>
.el-card__body {
  padding: 10px;
}
.userCard {
  height: 180px;
  display: flex;
  border-bottom: 2px solid #dcdfe6;
  border-radius: 2px;
}
.userInfo {
  width: auto;
  padding: 6% 0 0 6%;
}
.important-font {
  font-weight: 900;
  font-size: 25px;
  padding-left: 12px;
}
.secondary-font {
  color: #909399;
}
.login-info {
  height: 40px;
  text-align: left;
  color: #909399;
}
.tableInfo {
  margin: 20px 0 0 0;
}
.OrderCard {
  margin: 0 0 60px 10px;
  padding:20px 10px 0 0;
  border: #dcdfe6 1px solid;
  border-radius: 10px;
  i {
    width: 30%;
    line-height: 120px;
    font-size: 30px;
    color: #fff;
  }
  div {
    width: 300px;
  }
}
.el-card {
  border: none;
}
.num {
  display: flex;
  flex-wrap: wrap;
  margin-top: 10px;
  padding-top: 10px;
}
.graph {
  margin: 15px 0 0 0;
}
.el-calendar {
  height: 265px;
}
.num graph {
  display:flex;
}
.box {
  padding: 0px;
  margin: 0px;
  width: 100%;
  height: 100%;
 
}
.icon{
  padding-left: 50px;
}
</style>