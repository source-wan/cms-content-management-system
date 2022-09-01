import LayoutBase from '../components/LayoutBase'

let routes = [
    {
        path: "/",
        component: () => import('../views/LoginView'),
        meta: {
            title: '登录',
            hidden: true
        },
        redirect: '/login',
       
        children: [
            {
                path: 'login',
                component: () => import('../views/LoginView'),
                meta: {
                    title: '登录',
                    hidden: true
                },
               
            },
        ]
    },
    {
        path: "/LayoutBase",
        component: LayoutBase,
        meta: {
            title: '首页',
            hidden: true,
        },
        redirect: '/LayoutBase/BaseCharts',
        children: [
            {
                path: 'BaseCharts',
                component: () => import('../views/BaseCharts'),
                meta: {
                    title: '博客后台管理系统',
                    hidden:false
                },
            }
        ]
    },
    {

        path: "/article",
        meta: {
            title: '账号管理',
            icon: 'el-icon-user'
        },
        component: () => import('../components/LayoutBase'),
        children: [
            {
                path: "personal",
                meta: {
                    title: '个人中心',
                    icon: ''
                },
                component: () => import('../views/PeopleCenter')
            },
            {

                path: "author",
                meta: {
                    title: "用户管理",
                    icon: ''
                },
                component: () => import('../views/BaseTable')
            },
            {
                path: "publisher",
                meta: {
                    title: '文章管理'
                },
                component: () => import('../views/ArticleTable')
            },
            {
                path:"category",
                meta:{
                    title:'分类管理'
                },
                component: () => import('../views/CategoryTable')
            }
        ],
    },

    {
        path: '/logout',
        meta: {
            hidden: true
        },
        component: () => import('../components/NotFound'),

    },
    {
        path: '*',
        meta: {
            hidden: true
        },
        component: () => import('../components/NotFound')
    },
    
]

export default routes;