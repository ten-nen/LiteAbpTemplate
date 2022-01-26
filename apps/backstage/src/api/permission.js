import request from '@/utils/request'

export function getPermissions() {
  return request({
    url: '/backstage/permission',
    method: 'get'
  })
}
