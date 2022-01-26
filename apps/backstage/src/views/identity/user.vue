<template>
  <div class="app-container">
    <div class="filter-container">
      <el-select v-model="listQuery.sorting" placeholder="选择排序" clearable style="width: 140px" class="filter-item" @change="getPagerList">
        <el-option v-for="item in listSortData" :key="item.val" :label="item.key" :value="item.val" />
      </el-select>
      <el-input v-model="listQuery.filter" placeholder="输入搜索关键词" style="width: 200px;" class="filter-item" @keyup.enter.native="getPagerList" />
      <el-select v-model="listQuery.roleName" placeholder="选择角色" clearable class="filter-item" style="width: 130px" @change="getPagerList">
        <el-option v-for="item in roles" :key="item.id" :label="item.name" :value="item.name" />
      </el-select>
      <el-button class="filter-item" type="primary" icon="el-icon-search" @click="getPagerList">
        查询
      </el-button>
      <el-button v-if="hasPermission('Backstage.User.Create')" class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleAdd">
        新增用户
      </el-button>
    </div>

    <el-table v-loading="listLoading" :data="userList" style="width: 100%;margin-top:30px;" border>
      <el-table-column align="center" label="用户名">
        <template slot-scope="{row}">
          {{ row.userName }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="姓名">
        <template slot-scope="{row}">
          {{ row.name }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="手机号">
        <template slot-scope="{row}">
          {{ row.phoneNumber }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="邮箱">
        <template slot-scope="{row}">
          {{ row.email }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="新增时间">
        <template slot-scope="{row}">
          {{ row.creationTime | parseTime('{y}-{m}-{d} {h}:{i}') }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="是否启用">
        <template slot-scope="{row}">
          <el-switch
            v-model="row.isActive"
            active-color="#13ce66"
            :disabled="row.isStatic||!hasPermission('Backstage.User.Update')"
            @change="handleActive(row)"
          />
        </template>
      </el-table-column>
      <el-table-column align="center" label="操作" width="300">
        <template slot-scope="scope">
          <el-button v-if="!scope.row.isStatic&&hasPermission('Backstage.User.Update')" type="primary" size="small" @click="handleEdit(scope)">编辑</el-button>
          <el-button v-if="!scope.row.isStatic&&hasPermission('Backstage.User.SetRoles')" type="primary" size="small" @click="handleSetRoles(scope)">设置角色</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-pagination
      v-show="total>0"
      :current-page="currentPage"
      :page-sizes="[20, 50, 100]"
      :page-size="20"
      layout="total, sizes, prev, pager, next, jumper"
      :total="total"
      style="margin-top:5px;"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
    />

    <el-dialog :visible.sync="dialogVisible" :title="dialogType==='edit'?'编辑用户':'新增用户'">
      <el-form ref="form" :rules="rules" :model="user" label-width="80px" label-position="rigth">
        <el-form-item label="用户名" prop="userName">
          <el-input v-model="user.userName" placeholder="请输入用户名" :disabled="dialogType==='edit'" />
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="user.password" placeholder="请输入密码" />
        </el-form-item>
        <el-form-item label="姓名" prop="name">
          <el-input v-model="user.name" placeholder="请输入姓名" />
        </el-form-item>
        <el-form-item label="手机号" prop="phoneNumber">
          <el-input v-model="user.phoneNumber" placeholder="请输入手机号" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="user.email" placeholder="请输入邮箱" />
        </el-form-item>
      </el-form>
      <div style="text-align:right;">
        <el-button type="danger" @click="dialogVisible=false">取消</el-button>
        <el-button type="primary" :loading="loading" @click="confirmSave">保存</el-button>
      </div>
    </el-dialog>

    <el-dialog :visible.sync="roleDialogVisible" title="设置角色">
      <div>
        <el-checkbox-group v-model="selectedRoles">
          <el-checkbox-button v-for="role in rolesData" :key="role.id" :label="role.name">{{ role.name }}</el-checkbox-button>
        </el-checkbox-group>
      </div>
      <div style="text-align:right;">
        <el-button type="danger" @click="roleDialogVisible=false">取消</el-button>
        <el-button type="primary" :loading="loading" @click="confirmSaveRoles">保存</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { deepClone, parseTime, hasPermission } from '@/utils'
import { getPagerList, addUser, updateUser, getUserRoles, setUserRoles } from '@/api/user'
import { getRoles } from '@/api/role'

const defaultEmptyPassword = '*********'
const defaultUser = {
  id: '',
  userName: '',
  password: '',
  name: '',
  email: '',
  phoneNumber: '',
  creationTime: ''
}

export default {
  data() {
    return {
      loading: false,
      listLoading: false,
      listQuery: { filter: '', roleName: '', sorting: '', kipCount: 0, maxResultCount: 20 },
      listSortData: [
        { key: '新增时间', val: 'CreationTime' },
        { key: '修改时间', val: 'LastModificationTime' }
      ],
      user: Object.assign({}, defaultUser),
      userList: [],
      roles: [],
      dialogVisible: false,
      roleDialogVisible: false,
      dialogType: 'new',
      selectedRoles: [],
      currentPage: 1,
      total: 0,
      rules: {
        userName: [
          { required: true, message: '不能为空', trigger: 'blur' },
          { max: 32, message: '长度不能大于32个字符', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '不能为空', trigger: 'blur' },
          { max: 32, message: '长度不能大于32个字符', trigger: 'blur' }
        ],
        name: [
          { required: true, message: '不能为空', trigger: 'blur' },
          { max: 32, message: '长度不能大于16个字符', trigger: 'blur' }
        ],
        email: [
          { required: true, message: '不能为空', trigger: 'blur' },
          { max: 32, message: '长度不能大于32个字符', trigger: 'blur' },
          { type: 'email', message: '邮箱格式不正确', trigger: ['blur', 'change'] }
        ]
      }
    }
  },
  computed: {
    rolesData() {
      return this.roles
    }
  },
  created() {
    this.getRoles()
    this.getPagerList()
  },
  methods: {
    hasPermission,
    async getRoles() {
      const res = await getRoles()
      this.roles = res
    },
    async getPagerList() {
      this.listLoading = true
      getPagerList(this.listQuery).then(d => {
        this.userList = d.items
        this.total = d.totalCount

        this.listLoading = false
      }).catch(err => {
        console.log(err)
        this.listLoading = false
      })
    },
    formatJson(filterVal) {
      return this.list.map(v => filterVal.map(j => {
        if (j === 'timestamp') {
          return parseTime(v[j])
        } else {
          return v[j]
        }
      }))
    },
    handleAdd() {
      this.user = Object.assign({}, defaultUser)
      this.dialogType = 'new'
      this.dialogVisible = true
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    handleEdit(scope) {
      this.dialogType = 'edit'
      this.dialogVisible = true
      this.user = deepClone(scope.row)
      this.user.password = defaultEmptyPassword
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    async handleActive(row) {
      await updateUser(row.id, { IsActive: row.isActive })

      this.$message({
        type: 'success',
        message: '禁用成功!'
      })
    },
    confirmSave() {
      const isEdit = this.dialogType === 'edit'
      this.$refs['form'].validate((valid) => {
        if (valid) {
          this.loading = true
          if (isEdit) {
            if (this.user.password === defaultEmptyPassword) {
              this.user.password = ''
            }

            updateUser(this.user.id, this.user).then(() => {
              for (let index = 0; index < this.userList.length; index++) {
                if (this.userList[index].id === this.user.id) {
                  this.userList.splice(index, 1, Object.assign({}, this.user))
                  break
                }
              }
              this.dialogVisible = false
              this.$notify({
                title: '用户保存成功',
                dangerouslyUseHTMLString: true,
                message: `
            <div>用户名称: ${this.user.name}</div>
          `,
                type: 'success'
              })
            }).catch(err => {
              console.log(err)
              this.user.password = defaultEmptyPassword
              this.loading = false
            })
          } else {
            addUser(this.user).then(data => {
              this.user.id = data.id
              this.user.creationTime = data.creationTime
              this.userList.push(this.user)

              this.dialogVisible = false
              this.$notify({
                title: '用户保存成功',
                dangerouslyUseHTMLString: true,
                message: `
            <div>用户名称: ${this.user.name}</div>
          `,
                type: 'success'
              })
            }).catch(err => {
              console.log(err)
              this.loading = false
            })
          }
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    handleSizeChange(size) {
      this.listQuery.maxResultCount = size
    },
    handleCurrentChange(currentPage) {
      this.listQuery.kipCount = currentPage * this.listQuery.maxResultCount - 1
      this.listQuery.kipCount = this.listQuery.kipCount > 0 ? this.listQuery.kipCount : 0
      getPagerList()
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    handleSetRoles(scope) {
      this.roleDialogVisible = true
      this.user = deepClone(scope.row)
      this.$nextTick(async() => {
        const userRoles = await getUserRoles(this.user.id)
        this.selectedRoles = userRoles.map(x => x.name)
      })
    },
    confirmSaveRoles() {
      this.loading = true
      console.log(this.selectedRoles)
      setUserRoles(this.user.id, { roleNames: this.selectedRoles }).then(() => {
        this.loading = false
        this.roleDialogVisible = false
        this.$notify({
          title: '设置角色成功',
          dangerouslyUseHTMLString: true,
          type: 'success'
        })
      }).catch(() => {
        this.loading = false
      })
    }
  }
}
</script>

