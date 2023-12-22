using NonsensicalKit.Core;
using UnityEngine;

namespace NonsensicalKit.WebGL
{
    /// <summary>
    /// 用于处理socketIO相关操作
    /// </summary>
    [AddComponentMenu("NonsensicalKit/WebGL/SocketIO")]
    public  class WebSocketIO : MonoSingleton<WebSocketIO>
    {
        protected override void Awake()
        {
            base.Awake();
            Subscribe<string[]>("WebBridge", "SocketIOMessage", OnSocketIOMessage);
        }

        public void ConnectSocketIO(string url, string id = "default")
        {
            Publish("SendMessageToJS", "SocketIO", new string[] { "ConnectSocketIO", url, id });
        }

        public void SocketIOAddListener(string eventName, string id = "default")
        {
            Publish("SendMessageToJS", "SocketIO", new string[] { "AddListener", eventName, id });
        }

        public void SocketIOSendMessage(string eventName, string msg, string id = "default")
        {
            Publish("SendMessageToJS", "SocketIO", new string[] { "SendMessage", eventName, msg, id });
        }

        public void SocketIOSendMessageWithCallback(string eventName, string msg, string id = "default")
        {
            Publish("SendMessageToJS", "SocketIO", new string[] { "SendMessageWithCallback", eventName, msg, id });
        }

        private void OnSocketIOMessage(string[] values)
        {
            OnSocketIOMessage(values[1], values[2], values[3]);
        }

        private void OnSocketIOMessage(string message, string eventName, string id)
        {
            PublishWithID("SocketIOMessage", id, eventName, message);
            Publish("SocketIOMessage", eventName, message);
            Publish("SocketIOMessage", message);
        }
    }
}
