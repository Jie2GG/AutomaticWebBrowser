# 首页

---

JSON 配置文件是 AutomaticWebBrowser 的核心配置文件, 本文将详细介绍 JSON 配置中包含的配置项目.

## 目录

- [根配置](AWConfig.md)
- [浏览器配置](AWBrowser.md)
- [跟踪器配置](AWLog.md)
- [任务集配置](AWTask.md)

## 清单

- [AWActionType](AWActionType.md)
- [AWConditionType](AWConditionType.md)
- [AWElementType]()
- [AWKeyCode]()
- [AWMouseButtons]()
- [WindowState]()
- [WindowStartupLocation]()

## 案例配置

本案例将定义如何在 [https://www.baidu.com](https://www.baidu.com) 站点首页中进行自动搜索.

```JSON
{
    // 跟踪器配置
    "log": {
        // 输出格式
        "format": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
    },
    // 浏览器配置
    "browser": {
        // 窗体配置
        "window": {
            // 窗体状态
            "state": "Normal",
            // 窗体位置
            "startup-location": "CenterScreen",
            // 宽度
            "width": 800,
            // 高度
            "height": 500
        }
    },
    // 任务集配置
    "task": [
        {
            // 百度首页的地址
            "url": "https://www.baidu.com/",
            // 作业集
            "job": [
                {
                    "name": "输入 海绵宝宝",
                    // 条件
                    "condition": {
                        "type": "Ready"
                    },
                    // 元素
                    "element": {
                        "type": "GetElementById",
                        "value": "kw"
                    },
                    "action": [
                        {
                            "type": "InputValue",
                            "value": "海绵宝宝"
                        }
                    ]
                },
                {
                    "name": "点击 搜索 按钮",
                    "element": {
                        "type": "GetElementById",
                        "value": "su"
                    },
                    "action": [
                        {
                            "type": "Click"
                        }
                    ]
                }
            ]
        }
    ]
}
```
