# QuartzUI.Extension.AspNetCore

Lightweight, injectable UI components based on Quartz.



## 快速使用

### 1. 安装

* [QuartzUI.Extension.AspNetCore](https://www.nuget.org/packages/QuartzUI.Extension.AspNetCore)

``` bash
dotnet add package QuartzUI.Extension.AspNetCore
```



### 2. 注册

#### 2.1 基于文件的持久化存储（默认方式）

* 1.注入QuartzUI：

```csharp
services.AddQuartzUI();  
```

* 2.开启 ClassJob 支持（可选） 

```csharp
 services.AddQuartzClassJobs();  
```

* 3.使用QuartzUI

```csharp
app.UseQuartz();  
```

* 4.配置管理员口令

```json
{
  "QuartzUI": {
    "Token": "task123456",
    "SuperToken": "super123456"
  }
}
```



#### 2.2 基于数据库的持久化存储 


* 1.注入QuartzUI：

```csharp
services.AddQuartzUI(optionsBuilder =>
{
     // 创建 MySQL 数据库连接
     optionsBuilder.UseMySql(Configuration.GetConnectionString("MySql"), b => b.MaxBatchSize(1));
});
```

* 2.开启 ClassJob 支持（可选） 

```csharp
 services.AddQuartzClassJobs();  
```

* 3.使用QuartzUI

```csharp
app.UseQuartz(); 
```

* 4.配置管理员口令

```json
{
  "QuartzUI": {
    "Token": "task123456",
    "SuperToken": "super123456"
  }
}
```


### 3.案例

[QuartzUIDemo](https://github.com/Run2948/QuartzUI.Extension.AspNetCore/tree/main/samples/QuartzUIDemo)