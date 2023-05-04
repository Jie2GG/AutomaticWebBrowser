# 浏览器配置

---

## 说明

AutomaticWebBrowser 的浏览器配置可以较为全面的定制浏览器的功能, 例如: 配置浏览器窗体、设置浏览器核心功能等. 具体如下:

- [窗体配置 (window)](AWWindow.md)
- 数据路径 (data-path)
- 启用跟踪防护功能 (enable-tracking-prevention)
- 启用密码自动保存 (enable- password-autosave)
- 启用开发者工具 (enable-dev-tools)
- 启用上下文菜单 (enable-context-menu)
- 其他配置后续逐步增加

## Demo

配置文件如下述案例所示:

```JSON
{
    // 浏览器配置
    "browser": {
        // 浏览器窗体配置
        "window": {...},
        // 数据存储路径, 默认: 程序根路径/data
        "data-path": "./data",
        // 启用跟踪防护功能, 默认: true
        "enable-tracking-prevention": true,
        // 启用密码自动保存, 默认: false
        "enable-password-autosave": false,
        // 启用开发者工具, 默认: false
        "enable-dev-tools": false,
        // 启用上下文菜单, 默认: true
        "enable-context-menu": true
    },
    ...
}
```
