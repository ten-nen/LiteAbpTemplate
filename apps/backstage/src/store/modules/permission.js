import { asyncRoutes, constantRoutes } from '@/router'

/**
 * Use meta.role to determine if the current user has permission
 * @param permissions
 * @param route
 */
function hasPermission(permissions, route) {
  if (route.meta && route.meta.permissions) {
    return permissions.some(permission => route.meta.permissions.includes(permission))
  } else {
    return true
  }
}

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param permissions
 */
export function filterAsyncRoutes(routes, permissions) {
  const res = []

  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(permissions, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, permissions)
      }
      res.push(tmp)
    }
  })

  return res
}

export const clientPermissions = [{
  title: '用户管理',
  name: 'backstage.user',
  permissions: [{
    title: '用户列表',
    name: 'backstage.user.getpager'
  }, {
    title: '新增用户',
    name: 'backstage.user.create'
  }, {
    title: '编辑用户',
    name: 'backstage.user.update'
  }, {
    title: '设置角色',
    name: 'backstage.user.setroles',
    permissions: [{
      title: 'backstage.role.getall',
      name: 'backstage.role.getall'
    }, {
      title: 'backstage.user.getroles',
      name: 'backstage.user.getroles'
    }]
  }]
}, {
  title: '角色管理',
  name: 'backstage.role',
  permissions: [{
    title: '角色列表',
    name: 'backstage.role.getall'
  }, {
    title: '新增角色',
    name: 'backstage.role.create'
  }, {
    title: '编辑角色',
    name: 'backstage.role.update'
  }, {
    title: '删除角色',
    name: 'backstage.role.delete'
  }, {
    title: '授权功能',
    name: 'backstage.role.updatepermissions',
    permissions: [{
      title: 'backstage.role.getpermissions',
      name: 'backstage.role.getpermissions'
    }]
  }]
}]

const state = {
  routes: [],
  addRoutes: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  }
}

const actions = {
  generateRoutes({ commit }, permissions) {
    return new Promise(resolve => {
      const accessedRoutes = filterAsyncRoutes(asyncRoutes, permissions)
      commit('SET_ROUTES', accessedRoutes)
      resolve(accessedRoutes)
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
