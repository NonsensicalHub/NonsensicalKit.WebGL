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
        }

        public void Connect(string url, string userName, string password)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Connect", url, userName, password });
        }

        public void Subscribe(string topic)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Subscribe", topic });
        }

        public void Publish(string topic, string msg)
        {
            Publish("SendMessageToJS", "MQTT", new string[] { "Publish", topic, msg });
        }

        private void OnMQTTMessage(string[] values)
        {
            OnMQTTMessage(values[1], values[2]);
        }

        private void OnMQTTMessage(string topic, string message)
        {
            PublishWithID("MQTTMessage", topic, message);
            Publish("MQTTMessage", topic, message);
            Publish("MQTTMessage", message);
        }
    }
}
