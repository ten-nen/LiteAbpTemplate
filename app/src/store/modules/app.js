import Cookies from 'js-cookie'
import { getConf } from '@/api/abp'

const state = {
  sidebar: {
    opened: Cookies.get('sidebarStatus') ? !!+Cookies.get('sidebarStatus') : true,
    withoutAnimation: false
  },
  device: 'desktop',
  abp: null
}

const mutations = {
  TOGGLE_SIDEBAR: state => {
    state.sidebar.opened = !state.sidebar.opened
    state.sidebar.withoutAnimation = false
    if (state.sidebar.opened) {
      Cookies.set('sidebarStatus', 1)
    } else {
      Cookies.set('sidebarStatus', 0)
    }
  },
  CLOSE_SIDEBAR: (state, withoutAnimation) => {
    Cookies.set('sidebarStatus', 0)
    state.sidebar.opened = false
    state.sidebar.withoutAnimation = withoutAnimation
  },
  TOGGLE_DEVICE: (state, device) => {
    state.device = device
  },
  SET_CONF: (state, abp) => {
    state.abp = abp
  },
  CLEAR_CONF: (state) => {
    state.abp = null
  }
}

const actions = {
  toggleSideBar({ commit }) {
    commit('TOGGLE_SIDEBAR')
  },
  closeSideBar({ commit }, { withoutAnimation }) {
    commit('CLOSE_SIDEBAR', withoutAnimation)
  },
  toggleDevice({ commit }, device) {
    commit('TOGGLE_DEVICE', device)
  },
  getConf({ commit, state }) {
    return new Promise((resolve, reject) => {
      getConf().then(d => {
        var abp = d
        abp.auth.isGranted = policyName => {
          return abp.auth.policies[policyName] !== undefined && abp.auth.grantedPolicies[policyName] !== undefined
        }
        abp.auth.isAnyGranted = () => {
          if (!arguments || arguments.length <= 0) {
            return true
          }

          for (var i = 0; i < arguments.length; i++) {
            if (abp.auth.isGranted(arguments[i])) {
              return true
            }
          }

          return false
        }
        abp.auth.areAllGranted = () => {
          if (!arguments || arguments.length <= 0) {
            return true
          }

          for (var i = 0; i < arguments.length; i++) {
            if (!abp.auth.isGranted(arguments[i])) {
              return false
            }
          }

          return true
        }
        abp.setting.get = name => {
          return abp.setting.values[name]
        }
        abp.setting.getBoolean = name => {
          var value = abp.setting.get(name)
          return value === 'true' || value === 'True'
        }
        abp.setting.getInt = name => {
          return parseInt(abp.setting.values[name])
        }
        abp.localization.localize = (key, sourceName) => {
          if (sourceName === '_') { // A convention to suppress the localization
            return key
          }

          sourceName = sourceName || abp.localization.defaultResourceName
          if (!sourceName) {
            abp.log.warn('Localization source name is not specified and the defaultResourceName was not defined!')
            return key
          }

          var source = abp.localization.values[sourceName]
          if (!source) {
            abp.log.warn('Could not find localization source: ' + sourceName)
            return key
          }

          var value = source[key]
          if (value === undefined) {
            return key
          }

          var copiedArguments = Array.prototype.slice.call(arguments, 0)
          copiedArguments.splice(1, 1)
          copiedArguments[0] = value

          return abp.utils.formatString.apply(this, copiedArguments)
        }
        abp.localization.isLocalized = function(key, sourceName) {
          if (sourceName === '_') { // A convention to suppress the localization
            return true
          }

          sourceName = sourceName || abp.localization.defaultResourceName
          if (!sourceName) {
            return false
          }

          var source = abp.localization.values[sourceName]
          if (!source) {
            return false
          }

          var value = source[key]
          if (value === undefined) {
            return false
          }

          return true
        }
        abp.localization.getResource = name => {
          return function() {
            var copiedArguments = Array.prototype.slice.call(arguments, 0)
            copiedArguments.splice(1, 0, name)
            return abp.localization.localize.apply(this, copiedArguments)
          }
        }

        commit('SET_CONF', d)
        resolve(d)
      }).catch(error => {
        reject(error)
      })
    })
  },
  clearConf({ commit, state }) {
    return new Promise((resolve, reject) => {
      commit('CLEAR_CONF')
      resolve()
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
