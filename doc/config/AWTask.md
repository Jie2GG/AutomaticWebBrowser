# 任务集配置

---

## 说明

AutomaticWebBrowser 的任务集包含了浏览器在执行自动化任务时的所有指令, 并用简要的结构和数据描述浏览器的任务执行, 并将详细过程记录在跟踪器

在 AutomaticWebBrowser 中, 可包含多个任务, 这些任务将会被顺序执行, 每个任务的信息描述方式保持一致, 任务信息描述:

- 链接地址 (url)
- 加载失败重载次数 (url-reload-count)
- 加载失败重试延迟 (url-reload-delay)
- 自动关闭浏览区 (auto-close)
- [作业集 (job)](AWJob.md)

## Demo

配置文件如下述案例所示:

```JSON
{
    // 任务集
    "task": [
        {
            // 链接地址, 任务开始执行时将会导航到此链接
            "url": "https://www.baidu.com/",
            // 加载失败重试次数, 如果导航失败将重试1次
            "url-reload-count": 1,
            // 加载失败重试延迟, 如果导航失败将延迟1000ms后再重新加载
            "url-reload-delay": 1000,
            // 任务执行完毕(无论成功还是失败)将自动关闭浏览区
            "auto-close": true,
            // 任务包含的作业集
            "job": [...]
        }
    ]
}
```
