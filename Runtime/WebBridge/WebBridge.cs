using NonsensicalKit.Editor;
using NonsensicalKit.Editor.Log;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NonsensicalKit.WebGL
{
    /// <summary>
    /// 用于规范化与webgl的js代码通讯
    /// 需要挂载在根节点同名对象上
    /// </summary>
    [AddComponentMenu("NonsensicalKit/WebGL/WebBridge", -1)]
    public class WebBridge : MonoSingleton<WebBridge>
    {
        [SerializeField] private bool m_logMessage;

        [DllImport("__Internal")]
        private static extern void sendMessageToJS(string key, string values);

        private Queue<string[]> _buffer = new Queue<string[]>();

        private bool _running;

        protected override void Awake()
        {
            base.Awake();

            gameObject.name = "WebBridge";

            _running = PlatformInfo.Platform == RuntimePlatform.WebGLPlayer;

            Subscribe<string, string[]>("SendMessageToJS", SendMessageToJS);
            Subscribe<string>("SendMessageToJS", SendMessageToJS);
        }

        private void Update()
        {
            while (_buffer.Count > 0)
            {
                var v = _buffer.Dequeue();

                switch (v[0])
                {
                    case "UrlQueryStr":
                        HandleUrlQuery(v[1]);
                        continue;
                    default:
                        Publish("WebBridge", v);
                        PublishWithID("WebBridge", v[0], v);
                        break;
                }
            }
        }

        public void SendMessageToJS(string key)
        {
            if (_running)
            {
                sendMessageToJS(key, string.Empty);
            }
        }

        public void SendMessageToJS(string key, string[] values)
        {
            if (_running)
            {
                var str = NonsensicalKit.Tools.JsonTool.SerializeObject(values);
                sendMessageToJS(key, str);
            }
        }

        /// <summary>
        /// 用于接受javascript的消息
        /// </summary>
        /// <param name="str"></param>
        public void SendMessageToUnity(string str)
        {
            if (m_logMessage)
            {
                LogCore.Debug($"WebBridge收到JavaScript消息：{str}");
            }
            string[] values = NonsensicalKit.Tools.JsonTool.DeserializeObject<string[]>(str);
            _buffer.Enqueue(values);
        }

        private void HandleUrlQuery(string s)
        {
            string[] values = s.Split(new char[] { '?', '&', '=' }, System.StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> httpArgs = new Dictionary<string, string>();
            for (int i = 0; i < values.Length - 1; i += 2)
            {
                httpArgs.Add(values[i], values[i + 1]);
            }
            IOCC.Set("httpArgs", httpArgs);
        }
    }
}
