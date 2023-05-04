# 操作集配置

---

## 说明

AutomaticWebBrowser 将一元素的所有操作视为一个作业, 在一个作业范围内所有的操作将针对作业制定的元素依次顺序执行. 操作的具体定义如下:

- 操作类型 (type): [AWActionType](AWActionType.md)
- 附加值

在配置文件中使用如下方式表示:

```JSON
// 操作集
"action": [
    // 操作1
    {
        // 操作类型
        "type": "Click"
    },
    // 操作2
    {
        "type": "KeyDown",
        // 操作值, 依据操作类型而定
        "value": {
            "code": "Q"
        }
    },
    ...
]
```

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
                    // 操作集
                    "action": [
                        {
                            "type": "KeyDown",
                            // 操作值, 依据操作类型而定
                            "value": {
                                "code": "Q"
                            }
                        }
                    ]
                }
            ]
        }
    ]
    ...
}
```
