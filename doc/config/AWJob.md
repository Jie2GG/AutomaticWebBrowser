# 作业集配置

---

## 说明

AutomaticWebBrowser 将一个任务的所有作业都放在该集合中, 包含从任务开始到结束的所有作业, 在作业中可以指定对网页做任何 javascript 可以做到的操作, 基本指令如下:

- 执行条件 (condition)
- 内嵌框架元素 (iframe)
- 页面元素 (element)
- [操作集 (action)](AWAction.md)

#### 执行条件 (condition)

执行条件是指每个作业的操作集开始执行之前必须要满足的条件, 目前只针对 Ready 动作做出同步处理, 具体格式如下:

```JSON
"condition": {
    "type": "Ready"
}
```

条件类型 type 的含义参阅: [AWConditionType](AWConditionType.md)

#### 页面元素 (element) & 内嵌框架元素 (iframe)

页面元素是指 AutomaticWebBrowser 在页面上查找元素时使用的查找方式, 涵盖了大部分 javascript 查找页面元素的方法, 内嵌框架元素的查找和页面元素一致, 仅仅将查找的结果锁定为 iframe, 具体格式如下:

```JSON
// 页面元素
"element": {
    "type": "getElementById",
    "value": "j-search-input"
}

// 内嵌框架元素
"iframe": {
    "type": "getElementById",
    "value": "j-iframe",
    "element": {
        "type": "Evaluate",
        "value": "/html/body/video"
    }
}
```

页面元素 & 内嵌框架元素 type 的含参阅: [AWElementType](AWElementType.md)

## Demo

配置文件如下述案例所示:

```JSON
{
    // 任务集
    "task": [
        {
            ...
            // 作业集
            "job": [
                {
                    // 作业执行条件, 在作业动作执行之前达到此条件才开始执行作业
                    "condition": {
                        // 条件类型
                        "type": "Ready"
                    },
                    // 作业执行目标元素, 作业只针对这一元素(集)执行操作
                    "element": {
                        // 元素的搜索方式, 使用 document.getElementById 方式搜索
                        "type": "GetElementById",
                        // 元素搜素用的值
                        "value": "j-search-input"
                    },
                    // 操作集, 针对 element 中指定的元素将逐一按顺序执行
                    "action": [...]
                },
                {
                    // 如果没有 condition 则表示不需要同步条件
                    // 作业执行的目标元素, 如果使用 iframe, 表示目标元素存在 iframe 中
                    "iframe": {
                        // iframe 的查找类型
                        "type": "GetElementById",
                        // iframe 查找的搜索值
                        "value": "taget-iframe",
                        // 找到 iframe 后将在 iframe 内部执行的作业执行目标元素
                        "element": {
                            "type": "Evaluate",
                            "value": "video"
                        }
                    },
                    "action": [...]
                }
            ]
        }
    ]
}
```
