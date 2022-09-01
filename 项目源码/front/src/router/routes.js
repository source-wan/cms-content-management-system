let routes = [
    {
        path: "/",
        component: () => import('../components/LayoutBase'),
        meta: {
            title: '首页',
            hidden: true
        },
        redirect: '/homepage',
        children: [
            {
                path: 'homepage',
                component: () => import('../views/MainInfo'),
                meta: {
                    title: '首页'
                },
               
            },
        ]
    },
    {
        path: "/LayoutBase",
        component: () => import('../components/LayoutBase'),
        meta: {
            title: '首页',
            hidden: true
        },
        children: [
            {
                path: "/AddArticle",
                meta: {
                    title: '添加文章',
                    icon: ''
                },
                component: () => import('../views/AddArticle')
            },
            {
                path: "/MyLike",
                meta: {
                    title: '我的点赞',
                    icon: ''
                },
                component: () => import('../views/MyLike')
            },
            {
                path: "/MyCollect",
                meta: {
                    title: '我的收藏',
                    icon: ''
                },
                component: () => import('../views/MyCollect')
            },
            {
                path: "/RecentBrowse",
                meta: {
                    title: '最近浏览',
                    icon: ''
                },
                component: () => import('../views/RecentBrowse')
            },
            {
                path: '/ArticleInfo',
                component: () => import('../views/ArticleInfo'),
                meta: {
                    title: '文章列表',
                },
            },
            {
                path:"/news",
                component:()=>import("../views/MainInfo.vue")
            },
            {
                path:"/chat",
                component:()=>import("../views/MainInfo.vue")
            },
            {
                path:"/tieleone",
                component:()=>import("../views/MainInfo.vue")
            },
            {
                path:"/tieletwo",
                component:()=>import("../views/MainInfo.vue")
            },
            {
                path:"/tielethree",
                component:()=>import("../views/MainInfo.vue")
            }
        ]

    },
    {
        path: "*",
        meta: {
            title: '404',
            icon: ''
        },
        component: () => import('../views/NotFound.vue')
    },

    {
        path: "/login",
        component: () => import("../views/LoginView.vue")
    },
    {
        path: "/register",
        component: () => import("../views/RegisterView.vue")
    },
    {
        path:"/PersionCenter",
        component:()=>import("../views/CenterView.vue")
    }
  
]


export default routes;