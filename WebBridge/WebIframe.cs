using NonsensicalKit.Core;
using NonsensicalKit.Tools;
using UnityEngine;

namespace NonsensicalKit.WebGL
{
    [AddComponentMenu("NonsensicalKit/WebGL/Iframe")]
    public class WebIframe : MonoSingleton<WebIframe>
    {
        protected override void Awake()
        {
            base.Awake();
            Subscribe<string[]>("WebBridge", "IframeList", OnListIframe);
        }

        public void Change(float minX, float minY, float maxX, float maxY, string url, string id = "default")
        {
            Publish("SendMessageToJS", "Iframe",
                new[] { "Change", minX.ToString(), minY.ToString(), maxX.ToString(), maxY.ToString(), url, id });
        }

        public void Create(string id = "default")
        {
            Publish("SendMessageToJS", "Iframe", new[] { "Create", id });
        }

        public void Move(float minX, float minY, float maxX, float maxY, string id = "default")
        {
            Publish("SendMessageToJS", "Iframe", new[] { "Move", minX.ToString(), minY.ToString(), maxX.ToString(), maxY.ToString(), id });
        }

        public void SetUrl(string url, string id = "default")
        {
            Publish("SendMessageToJS", "Iframe", new[] { "SetUrl", url, id });
        }

        public void Close(string id = "default")
        {
            Publish("SendMessageToJS", "Iframe", new[] { "Close", id });
        }

        public void Info()
        {
            Publish("SendMessageToJS", "Iframe", new[] { "Info" });
        }

        public void CloseAll()
        {
            Publish("SendMessageToJS", "Iframe", new[] { "CloseAll" });
        }

        private void OnListIframe(string[] values)
        {
            var v = JsonTool.DeserializeObject<IframeInfo[]>(values[1]);

            Publish("IframeInfos", v);
        }
    }

    public class IframeInfo
    {
        public string ID;
        public string MinX;
        public string MinY;
        public string MaxX;
        public string MaxY;
        public string URL;
    }
}
