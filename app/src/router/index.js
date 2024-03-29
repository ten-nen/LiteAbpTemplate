import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/',
    component: Layout,
    redirect: '/home',
    children: [{
      path: 'home',
      name: 'Home',
      component: () => import('@/views/home/index'),
      meta: { title: '主页', icon: 'el-icon-s-home', affix: true }
    }]
  },
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  // {
  //   path: '/report',
  //   component: Layout,
  //   name: 'Report',
  //   meta: { title: '报表管理', icon: 'el-icon-s-data' },
  //   children: [{
  //     path: 'index',
  //     name: 'Index',
  //     component: () => import('@/views/report/index'),
  //     meta: { title: '查看报表' }
  //   }]
  // },
  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true,
    meta: { title: '404', noCache: true }
  },
  {
    path: '/401',
    component: () => import('@/views/401'),
    hidden: true,
    meta: { title: '401', noCache: true }
  }
]

/**
 * asyncRoutes
 * the routes that need to be dynamically loaded based on user roles
 */
export const asyncRoutes = [
  {
    path: '/identity',
    component: Layout,
    name: 'Identity',
    meta: { title: '用户和角色', icon: 'el-icon-user-solid', permissions: ['Api.Users', 'Api.Roles'] },
    children: [
      {
        path: 'user',
        name: 'User',
        component: () => import('@/views/identity/user'),
        meta: { title: '用户管理', permissions: ['Api.Users'] }
      },
      {
        path: 'role',
        name: 'Role',
        component: () => import('@/views/identity/role'),
        meta: { title: '角色管理', permissions: ['Api.Roles'] }
      }
    ]
  },
  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
