<template>
  <div class="app-container">
    <el-table
      :data="tableData"
      style="width: 100%"
      size="mini"
      :default-sort="{prop: 'date', order: 'descending'}"
      show-summary
      @expand-change="expandChange"
    >
      <el-table-column type="expand">
        <template slot-scope="props">
          <el-table
            v-loading="loading"
            :data="props.row.childData"
            style="width: 100%"
            size="mini"
            :default-sort="{prop: 'date', order: 'descending'}"
            show-summary
          >
            <el-table-column type="expand">
              <template slot-scope="props1">
                <span>{{ props1.row }}</span>
              </template>
            </el-table-column>
            <el-table-column
              prop="date"
              label="日期"
              sortable
              width="180"
              :filters="[{text: '2016-05-01', value: '2016-05-01'}, {text: '2016-05-02', value: '2016-05-02'}, {text: '2016-05-03', value: '2016-05-03'}, {text: '2016-05-04', value: '2016-05-04'}]"
            />
            <el-table-column
              prop="name"
              label="姓名"
              sortable
              width="180"
            />
            <el-table-column
              prop="address"
              label="地址"
              :formatter="formatter"
            />
          </el-table>
        </template>
      </el-table-column>
      <el-table-column
        prop="date"
        label="日期"
        sortable
        width="180"
        :filters="[{text: '2016-05-01', value: '2016-05-01'}, {text: '2016-05-02', value: '2016-05-02'}, {text: '2016-05-03', value: '2016-05-03'}, {text: '2016-05-04', value: '2016-05-04'}]"
      />
      <el-table-column
        prop="name"
        label="姓名"
        sortable
        width="180"
      />
      <el-table-column
        prop="address"
        label="地址"
        :formatter="formatter"
      />
    </el-table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      loading: false,
      tableData: [{
        date: '2016-05-02',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1518 弄'
      }, {
        date: '2016-05-04',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1517 弄'
      }, {
        date: '2016-05-01',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1519 弄'
      }, {
        date: '2016-05-03',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1516 弄'
      }]
    }
  },
  methods: {
    formatter(row, column) {
      return row.address
    },
    expandChange(row) {
      if (!row.childData) {
        this.loading = true
        setTimeout(() => {
          row.childData = [{
            id: 31,
            date: '2016-05-01',
            name: '王小虎',
            address: '上海市普陀区金沙江路1119 弄'
          }, {
            id: 32,
            date: '2016-05-01',
            name: '王小虎',
            address: '上海市普陀区金沙江路 1519 弄'
          }]
          this.loading = false
        }, 2000)
      }
    }
  }
}
</script>
