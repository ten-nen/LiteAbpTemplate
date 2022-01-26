import request from '@/utils/request'

export function getRoles() {
  return request({
    url: '/backstage/role',
    method: 'get'
  })
}

export function addRole(data) {
  return request({
    url: '/backstage/role',
    method: 'post',
    data
  })
}

export function updateRole(id, data) {
  return request({
    url: `/backstage/role/${id}`,
    method: 'put',
    data
  })
}

export function deleteRole(id) {
  return request({
    url: `/backstage/role/${id}`,
    method: 'delete'
  })
}

export function getRolePermissions(id) {
  return request({
    url: `/backstage/role/${id}/permissions`,
    method: 'get'
  })
}

export function updateRolePermissions(id, data) {
  return request({
    url: `/backstage/role/${id}/permissions`,
    method: 'put',
    data
  })
}
