using NonsensicalKit.Core;
using UnityEngine;

namespace NonsensicalKit.WebGL
{
    /// <summary>
    /// 用于处理MQTT相关操作
    /// </summary>
    [AddComponentMenu("NonsensicalKit/WebGL/MQTT")]
    public class WebMQTT : MonoSingleton<WebMQTT>
    {
        protected override void Awake()
        {
            base.Awake();
            Subscribe<string[]>("WebBridge", "MQTTMessage", OnMQTTMessage);
            Subscribe<string[]>("WebBridge", "MQTTEvent", OnMQTTEvent);
        }

        public void Connect(string url, string userName, string password)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Connect", url, userName, password });
        }

        public void Subscribe(string url, string topic)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Subscribe", url, topic });
        }

        public void Unsubscribe(string url, string topic)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Unsubscribe", url, topic });
        }

        public void SendMessage(string url, string topic, string msg)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Publish", url, topic, msg });
        }

        public void Close(string url)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Close", url });
        }
        public void CloseAll()
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "CloseAll" });
        }

        private void OnMQTTMessage(string[] values)
        {
            OnMQTTMessage(values[1], values[2], values[3]);
        }
        
        private void OnMQTTEvent(string[] values)
        {
            switch (values[1])
            {
                case "InitCompleted":
                {
                    Publish("MQTTInitCompleted");
                    break;
                }
                case "ConnectSuccess":
                {
                    //2是url，3是客户端id
                    Publish("MQTTConnectSuccess");
                    Publish("MQTTConnectSuccess", values[2]);
                    Publish("MQTTConnectSuccess", values[2], values[3]);
                    break;
                }
            }
        }

        private void OnMQTTMessage(string url, string topic, string message)
        {
            PublishWithID("MQTTMessage", topic, message);
            Publish("MQTTMessage", url, topic, message);
            Publish("MQTTMessage", topic, message);
            Publish("MQTTMessage", message);
        }
    }
}
