using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NonsensicalKit.Core;
using NonsensicalKit.Core.Log;
using NonsensicalKit.Tools;
using UnityEngine;
#if UNITY_WEBGL&& !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

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

#if UNITY_WEBGL&& !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void sendMessageToJS(string key, string values);
        [DllImport("__Internal")]
        private static extern void syncDB();
        [DllImport("__Internal")]
        private static extern int GetLocalStorage(string key);  
        [DllImport("__Internal")]
        private static extern void FreeMemory(int ptr);

#else
        private void sendMessageToJS(string key, string values)
        {
        }

        private void syncDB()
        {
        }
        private int GetLocalStorage(string key)
        {
            return 0;
        }

        private void FreeMemory(int ptr)
        {
            
        }
#endif

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
                    case "UrlHref":
                        IOCC.Set("UrlHref", v);
                        continue;
                    default:
                        Publish("WebBridge", v);
                        PublishWithID("WebBridge", v[0], v);
                        break;
                }
            }
        }

        public void SyncDB()
        {
            if (_running)
            {
                syncDB();
            }
        }

        public void SendMessageToJS(string key)
        {
            if (_running)
            {
                sendMessageToJS(key, string.Empty);
                if (m_logMessage)
                {
                    LogCore.Debug($"WebBridge往JavaScript发送消息：{key}");
                }
            }
        }

        public void SendMessageToJS(string key, string[] values)
        {
            if (_running)
            {
                var str = JsonTool.SerializeObject(values);
                sendMessageToJS(key, str);
                if (m_logMessage)
                {
                    LogCore.Debug($"WebBridge往JavaScript发送消息：{key}：{StringTool.GetSetString(values)}");
                }
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

            string[] values = JsonTool.DeserializeObject<string[]>(str);
            _buffer.Enqueue(values);
        }

        /// <summary>
        /// 从缓存中读取数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public string GetStringFromCache(string key = "ACCESS_TOKEN")
        {
            // WebGL平台才调用浏览器接口
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                int ptr = GetLocalStorage(key);
                if (ptr != 0)
                {
                    string token = Marshal.PtrToStringUTF8((IntPtr)ptr);
                    FreeMemory(ptr); // 释放分配的内存
                    return token;
                }
            }
            return "";
        }
        
        private void HandleUrlQuery(string s)
        {
            string[] values = s.Split(new[] { '?', '&', '=' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> httpArgs = new Dictionary<string, string>();
            for (int i = 0; i < values.Length - 1; i += 2)
            {
                httpArgs.Add(values[i], values[i + 1]);
            }

            IOCC.Set("httpArgs", httpArgs);
        }
        
        
    }
}
