[TOC]

# 安装.net core

本人开发环境为ubunu 20.04 ✖64位  阿里云镜像源

官网安装教程链接: [ubuntu安装.net core 3.1](https://docs.microsoft.com/zh-cn/dotnet/core/install/linux-ubuntu#2004-)

1. 安装镜像源

`wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
`

2. 运行按扎ung命令, 自动下载资源并安装

`sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-3.1
`

3.(可选)安装.net core运行时

`sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y aspnetcore-runtime-3.1
`

 `
 #个人理解就是为了方便部署和运行软件, 一般地我们的程序都会经过编译部署在机器上, 这时程序都被编译成了
 二进制文件, 所以.net core就没有必要安装了, 只需要运行时即可, 减少一部分存储量
 `

## 创建项目基本目录[官网](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-sln)

1. 解决方案 `dotnet new sln`
2. Api层-对外接口 `dotnet new webapi -o Api`
3. Service层-处理业务逻辑 `dotnet new classlib -o Service`
4. Mapper层-对象映射层, 处理实体类对象的验证 业务处理, 是系统中业务的载体 `dotnet new classlib -o Mapper`
5. Models-处理数据库映射关系 `dotnet new classlib -o Models`
6. Dao-数据库访问层 `dotnet new classlib -o Dao`

## 添加类库之间的引用[官网](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-sln)

1. 向解决方案添加类库项目(多个项目空格间隔)

 `dotnet sln add Service/Service.csproj Api/Api.csproj Service/Service.csproj  Models/Models.csproj  Dao/Dao.csproj Mapper/Mapper.csproj`
2. 项目之间的引用 [官网-项目引用命令](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-add-reference)

  Mapper引用Models `dotnet add Mapper/Mapper.csproj reference Models/Models.csproj`
  Dao引用Models `dotnet add  Dao/Dao.csproj reference Models/Models.csproj`
  Service引用Dao `dotnet add Service/Service.csproj reference Dao/Dao.csproj`
  Service引用Mapper `dotnet add Service/Service.csproj reference Mapper/Mapper.csproj`
  Api引用Mapper `dotnet add Api/Api.csproj reference Mapper/Mapper.csproj`
  Api引用Service `dotnet add Api/Api.csproj reference Service/Service.csproj`
3. 测试项目

  在**Models**中新增 `Test.cs` 类 添加如下内容
  ```C#
  using System; 

namespace Models
{

    public class Test
    {
        public static string Showing() {
            return "测试";
        }
    }

}

``` 
在**Mapper**中新增 `TestMapper.cs` 类 添加如下内容
  ```C#
  using System;
using Models;

namespace Mapper
{
    public class TestMapper
    {
        public static string Showing() {
            return Test.Showing();
        }
    }
}

```

在**Dao**中新增 `TestDao.cs` 类 添加如下内容
  ```C#
  using System; 
using Models; 
namespace Dao
{

    public class TestDao
    {
        public static string Showing() {
            return Test.Showing();
        }
    }

}

``` 
在**Service**中新增 `TestService.cs` 类 添加如下内容
  ```C#
  using System;
using Dao;
using Mapper;
namespace Service
{
    public class TestService
    {
        public static string Showing() {
            return TestDao.Showing();
        }

        public static string Show() {
            return TestMapper.Showing();
        }
    }
}

```

在**Api**中新增 `TestController.cs` 类 添加如下内容
  ```C#
  using System; 
using System. Collections. Generic; 
using System. Linq; 
using System. Threading. Tasks; 
using Microsoft. AspNetCore. Mvc; 
using Microsoft. Extensions. Logging; 
using Service; 

namespace Api. Controllers
{

    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return $"show: {TestService.Show()} Showing: {TestService.Showing()}";
        }
    }

}

``` 
4.运行 ` dotnet build` 编译解决方案,若无异常则项目搭建完成,有异常具体处理

5.运行 `dotnet run --project Api/Api.csproj` 运行项目,会生成两个访问地址https、http，随意打开其中一个，在浏览区中输入https://localhost:5001/Test 有结果输出即可
[官网- dotnet run](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-run)

## 添加Swagger作为接口文档生成器[Nuget地址](https://www.nuget.org/packages/Swashbuckle.AspNetCore/) [Swagger官方文档](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

1.运行 `dotnet add Api/Api.csproj package Swashbuckle.AspNetCore --version 5.5.1`
2.在 **Api** `Startup.cs` 类的 `ConfigureServices` 方法中添加

```C#
using Microsoft.OpenApi.Models;

services.AddMvc();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
```

在 `Configure` 方法中添加
```C#
app. UseSwagger(); 
app. UseSwaggerUI(c =>
{

    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = ""; // 配置为api默认起始页,移除"launchSettings.json"中launchUrl属性

}); 

``` 

2. `dotnet build`  `dotnet run --project Api/Api.csproj` 编译解决方案, 运行Api

3. **Swagger**运行后不显示注释解决办法,在**`Api/Api.csproj`*中添加,

```Html
<PropertyGroup Condition="'$(Configuration)|(Platform)'== 'Debug|AnyCPU'">
    <DocumentationFile>bin\Debug\netcoreapp3.1\Api.xml</DocumentationFile>
    <OutputPath>bin\</OutputPath>
  </PropertyGroup>
```

代码含义是指在debug模式下在 `bin\Debug\netcoreapp3.1\` 目录下生成 `Api.xml` 注释文件, 默认会将项目中的注释全部输入到此文件中, 随后在 `Startup.cs` 的 `ConfigureServices` 方法中添加注释文件`c.IncludeXmlComments (Path.Combine (AppContext.BaseDirectory, "Api.xml"), true);`
```C#
services.AddSwaggerGen (c => {
    c.SwaggerDoc ("v1", new OpenApiInfo { Title = "应用程序接口文档", Version = "v1" });
    c.IncludeXmlComments (Path.Combine (AppContext.BaseDirectory, "Api.xml"), true);
});

```

 `dotnet add package AutoMapper --version 10.0.0`
