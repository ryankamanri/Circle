## 项目简介
...

### 如何运行项目
在导入本项目并且运行之前,你需要下载的包 : 

- MySql.Data

> 运行命令 `dotnet add package MySql.Data`

- Newtonsoft.Json

> 运行命令 `dotnet add package Newtonsoft.Json`

之后运行 `dotnet run`



### 这里的部分文件(夹)作用

#### bin与obj

obj包含该框架在当前项目下的所有生成文件
bin包含该框架所需要的一些依赖
**如非必要务必不要改动这两个文件夹!**


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