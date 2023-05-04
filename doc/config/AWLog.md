# 跟踪器配置

---

## 说明

AutomaticWebBrowser 的日志跟踪模块基于 Serilog 开发, 提供了必要的可配置项目:

- 输出格式 (format)
- 日志等级 (level: LogEventLevel)
- 持久化路径 (save-path)

#### LogEventLevel 枚举

|名称|备注|
|-|-|
|Verbose|详细日志|
|Debug|调试|
|Information|信息|
|Warning|警告|
|Error|错误|
|Fatal|致命错误|

## Demo

配置文件如下述案例所示:

```JSON
{
    // 跟踪器(日志)配置
    "log": {
        // 输出格式
        "format": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        // 日志等级
        "level": "Information",
        // 持久化路径
        "save-path": "./logs"
    }
}
```
