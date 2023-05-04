# AWActionType 枚举类型

在 AutomaticWebBrowser 中, 操作类型中指定的操作均使用 javascript 代码执行, 所有执行结果均为 javascript 代码的执行后产生.

|名称|说明|备注|
|-|-|-|
|None|表示不执行任何操作|
|SetValue|设置元素的 value 属性|
|InputValue|设置元素的 value 属性, 并触发元素的 input 事件|某些框架如 vue 需要触发input事件才算输入数据|
|Click|调用元素的 click 函数|
|KeyDown|触发元素 keydown 事件|
|KeyUp|触发元素 keyup 事件|
|KeyPress|触发元素 keypress 事件|
|MouseDown|触发元素 mousedown 事件|
|MouseUp|触发 mouseup 事件|
|MouseClick|触发 click 事件|和Click不同, 这个事由鼠标事件引发的|
|MouseDbClick|触发 dbclick 事件|
|Play|调用元素的 play 函数|
|Pause|调用元素的 pause 函数|
|WaitEndedEvent|等待元素引发 ended 事件|