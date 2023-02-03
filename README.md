# 基于ABP、vue-admin-project，Web后台管理项目模板，已完成了用户和权限快速开始业务开发
### 后端：[ABP](https://github.com/abpframework/abp) 
### 前端：[vue-admin-project](https://github.com/PanJiaChen/vue-admin-template)

### 环境要求：NET6

## 使用步骤
#### 1、安装模板
``` cmd
dotnet new --install LiteAbpTemplate
```

#### 2、创建项目，比如Demo，默认sqlserver数据库，-s mysql 切换mysql数据库
``` cmd
dotnet new lat -n Demo            
```

#### 3、安装ef工具
``` cmd
dotnet tool install --global dotnet-ef        
```

#### 4、把本地sqlserver的sa密码修改为123456（Demo\src\Demo.Api\appsettings.json），然后ef生成数据库
``` cmd
cd Demo\src\Demo.Infrastructure 
dotnet ef migrations add InitialCreate -o ./Migrations
dotnet ef database update         
```

#### 5、最后net run或者(编译后)iis新建站点指向Demo.Web目录，运行查看
``` cmd
cd Demo\src\Demo.Api
dotnet build
dotnet run --urls http://localhost:44372        
```

#### 6、进入app
``` cmd  
cd Demo\app
yarn install
yarn dev
```

## 效果图
#### api
!["接口"](/imgs/api.png "接口")

#### 用户
!["用户"](/imgs/user.png "用户")

#### 角色
!["角色"](/imgs/role.png "角色")
