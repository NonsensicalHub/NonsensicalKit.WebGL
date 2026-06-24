# NonsensicalKit.WebGL 详细介绍

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

- **模块定位**：承载 Web 页面层的启动配置与扩展逻辑，配套 WebBridge 使用。
- **核心入口**：`NonsensicalTemplate`、`custom.js`
- **使用方法**：
  1. 将此包的案例 `NonsensicalTemplate` 导入。
  2. 将 `NonsensicalTemplate` 文件夹移至 `Assets/WebGLTemplates` 文件夹。
  3. 在 `PlayerSetting` -> `Resolution and Presentation` -> `WebGL Template` 中选择 `NonsensicalTemplate`。
  4. 根据业务需求修改模板文件，新增代码建议写入 `custom.js`。

## 第三方模块

### WebGLSupport

- **模块定位**：实现 WebGL 输入框与窗口行为兼容处理。
- **核心入口**：`WebGLInput`、`WebGLWindow`、`WebGLUIToolkitTextField`
