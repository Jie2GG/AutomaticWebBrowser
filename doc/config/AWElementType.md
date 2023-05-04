# AWElementType 枚举类型

AWElementType 用于指定作业制定页面元素的搜索方式

|名称|说明|需要附加值|备注|
|-|-|-|-|
|None|表示不执行任何操作|
|Window|使用 this.window 对象|否|DOM 文档中的 window 对象, 如果是 iframe, 则使用 contentWindow|
|Document|使用 this.document 对象|否|DOM 文档中的 document 对象, 如果是 iframe, 则使用 contentWindow.document|
|GetElementById|使用 document.getElementById 函数搜索|是|
|GetElementsByName|使用 this.documenet.getElementsByName 函数进行搜索|是|
|GetElementsByTagName|使用 this.documenet.getElementsByTagName 函数进行搜索|是|
|GetElementsByClassName|使用 this.documenet.getElementsByClassName 函数进行搜索|是|
|Evaluate|使用 this.documenet.evaluate 函数进行搜索|是|