import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/auth',
    method: 'post',
    data
  })
}

export function getPagerList(query) {
  return request({
    url: '/backstage/user',
    method: 'get',
    params: query
  })
}

export function addUser(data) {
  return request({
    url: '/backstage/user',
    method: 'post',
    data
  })
}

export function updateUser(id, data) {
  return request({
    url: `/backstage/user/${id}`,
    method: 'put',
    data
  })
}

export function getCurrentPermissions() {
  return request({
    url: `/backstage/user/permissions`,
    method: 'get'
  })
}

export function getUserRoles(id) {
  return request({
    url: `/backstage/user/${id}/roles`,
    method: 'get'
  })
}

export function setUserRoles(id, data) {
  return request({
    url: `/backstage/user/${id}/roles`,
    method: 'put',
    data
  })
}

