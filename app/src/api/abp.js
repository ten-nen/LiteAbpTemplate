import request from '@/utils/request'

export function getConf() {
  return request({
    url: '/abp/application-configuration',
    method: 'get'
  })
}
