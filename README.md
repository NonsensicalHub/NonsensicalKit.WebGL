# NonsensicalKit.WebGL

`com.nonsensicallab.nonsensicalkit.webgl` 是 NonsensicalKit 的 WebGL 平台能力包，主要提供 Web 模板接入、Unity 与 JavaScript 双向通信，以及 Web 端常见兼容辅助能力。

---

## 核心模块一览

### WebBridge

- **模块定位**：建立 Unity 与浏览器 JS 之间的统一消息通道。
- **核心入口**：`sendMessageToJS`、`SendMessageToUnity`
- **使用方法**：
  1. 统一使用字符串数组作为通信载体，约定字段顺序。
  2. Unity 向 JS 发消息时，先封装事件名与参数。
  3. JS 回传 Unity 时，统一走桥接入口并做参数校验。
  4. 为关键通信事件增加日志，便于联调定位。

### WebGLTemplate

- **模块定位**：承载 Web 页面层的启动配置与扩展逻辑，配套WebBridge使用。
- **核心入口**：`NonsensicalTemplate`、`custom.js`
- **使用方法**：
  1. 将此包的案例`NonsensicalTemplate`导入。
  2. 将`NonsensicalTemplate`文件夹移至`Assets/WebGLTemplates`文件夹。
  3. 在`PlayerSetting`-`Resolution and Presentation`-`WebGlTemplate`中选择`NonsensicalTemplate`
  4. 根据业务需求自行修改NonsensicalTemplate中的文件，新增代码写至custom.js文件中即可

## 第三方模块  

### WebGLSupport

实现webgl输入框的行为兼容处理

---

## 示例

- `NonsensicalTemplate`：WebGL 页面模板与基础桥接能力示例。
- `TransparentBackground`：透明背景渲染示例。
- `WebIframeSample`：Web 页面嵌入与交互示例。
