import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/user/login',
    method: 'post',
    data
  })
}

export function getPagerList(query) {
  return request({
    url: '/user',
    method: 'get',
    params: query
  })
}

export function addUser(data) {
  return request({
    url: '/user',
    method: 'post',
    data
  })
}

export function updateUser(id, data) {
  return request({
    url: `/user/${id}`,
    method: 'put',
    data
  })
}

export function getCurrentPermissions() {
  return request({
    url: `/user/permissions`,
    method: 'get'
  })
}

export function getUserRoles(id) {
  return request({
    url: `/user/${id}/roles`,
    method: 'get'
  })
}

export function setUserRoles(id, data) {
  return request({
    url: `/user/${id}/roles`,
    method: 'put',
    data
  })
}

