# 基于ABP、vue-admin-project 后台管理的小型Web项目模板，已完成了用户和权限快速开始业务开发
### 后端：[ABP](https://github.com/abpframework/abp) 
### 前端：[vue-admin-project](https://github.com/PanJiaChen/vue-admin-template)

### 环境要求：NET6

## 使用步骤
#### 1、安装模板、创建服务端项目
``` cmd
dotnet new --install LiteAbpTemplate 
dotnet new lat -n Demo
...
dotnet run --urls http://localhost:44372
```

#### 2、进入app
``` cmd  
npm install
npm run dev
```

## 效果图
#### api
!["接口"](/imgs/api.png "接口")

#### 用户
!["用户"](/imgs/user.png "用户")

#### 角色
!["角色"](/imgs/role.png "角色")