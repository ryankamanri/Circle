# “圈”——校友互助平台
## 一. 项目简介

### 1. 项目背景
- 根据自身需求及调研结果，“考研热”、“就业难”的问题是当前社会热点。
- 为让高校学生尽快适应高校学习生活以及尽早做好自身发展规划，创建了校友互助平台。
- 通过以高年级学生为低年级学生传授经验的方式，形成“传、帮、带”的发展平台，使高校在校生利用好“人”的资源，以便高效的获得最贴合自身的经验和高年级学生的有效帮助。


## 二. 项目运行

### 如何运行项目
在导入本项目并且运行之前,你需要下载的包 : 

- MySql.Data

> 运行命令 `dotnet add package MySql.Data`

- Newtonsoft.Json

> 运行命令 `dotnet add package Newtonsoft.Json`

之后运行 `dotnet run`



### 这里的部分文件(夹)作用


#### MVC三剑客

- Model 业务模型,用于描述实体
- Views 视图,用于对外展示模型
- Controllers 控制器,用于沟通视图与模型

#### Services

这里用来写自己所需要的一些服务,并在`StartUp`类中通过`IServicesCollection`对应的容器进行托管

- `SQL.cs` 连接数据库并可利用SQL语句进行基本操作
- `DataBaseContext.cs` 数据库上下文,用于保存部分实体的数据库集合
- `Cookie.cs` 利用Cookie进行认证与授权
- `ICookie.cs` `Cookie.cs`的对外接口

#### Properties
放置`launchSettings.json`项目启动配置文件,一般只改动其中的ip与监听端口

#### wwwroot
用于放置静态文件,通过url可以直接访问到

#### Program.cs与Startup.cs

Program.cs不用说了,主函数都在里面,项目运行的入口,一般也不需要去改.

Startup.cs比较重要,他是项目配置所有**服务**与**中间件**的地方.