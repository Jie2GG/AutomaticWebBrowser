# 窗体配置配置

## 说明

AutomaticWebBrowser 的窗体配置包含常见窗体的配置选项

- [窗体状态 (state: WindowState)](https://msdn.microsoft.com/zh-cn/library/system.windows.window.windowstate.aspx)
- [窗体启动位置 (startup-location: WindowStartupLocation)](https://msdn.microsoft.com/zh-cn/library/system.windows.window.windowstartuplocation.aspx)
- 窗体左边 (left)
- 窗体顶边 (top)
- 窗体宽度 (width)
- 窗体高度 (height)

#### WindowState 枚举

|名称|备注|
|-|-|
|Normal|还原窗体|
|Minimized|最小化窗口|
|Maximized|最大化窗口|

#### WindowStartupLocation 枚举

|名称|备注|
|-|-|
|Manual|可设置窗体的启动位置，或者使用默认的 Windows 位置|
|CenterScreen|窗体的启动位置位于包含鼠标光标的屏幕的中央|

## Demo

配置文件如下述案例所示:

```JSON
{
    // 浏览器配置
    "browser": {
        // 浏览器窗体配置
        “window”: {
            // 窗体状态
            "state": "Normal",
            // 窗体位置
            "startup-location": "CenterScreen",
            // 窗体左边, 可以传小数
            "left": 0,
            // 窗体顶边, 可以传小数
            "top": 0,
            // 窗体宽度, 可以传小数
            "width": 800,
            // 窗体高度, 可以传小数
            "height": 500
        },
        ...
    },
    ...
}
```
