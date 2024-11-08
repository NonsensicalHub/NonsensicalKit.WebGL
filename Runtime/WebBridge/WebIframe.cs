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
            Publish("SendMessageToJS", "Iframe", new string[] { "Change", minX.ToString(), minY.ToString(), maxX.ToString(), maxY.ToString(), url, id });
        }

        public void Move(float minX, float minY, float maxX, float maxY, string id = "default")
        {
            Publish("SendMessageToJS", "Iframe", new string[] { "Move", minX.ToString(), minY.ToString(), maxX.ToString(), maxY.ToString(), id });
        }

        public void SetUrl(string url, string id = "default")
        {
            Publish("SendMessageToJS", "Iframe", new string[] { "SetUrl", url, id });
        }

        public void Close(string id = "default")
        {
            Publish("SendMessageToJS", "Iframe", new string[] { "Close", id });
        }

        public void Info()
        {
            Publish("SendMessageToJS", "Iframe", new string[] { "Info" });
        }

        public void CloseAll()
        {
            Publish("SendMessageToJS", "Iframe", new string[] { "CloseAll" });
        }

        private void OnListIframe(string[] values)
        {
            //TODO:获取当前管理的Iframe信息

            var v = JsonTool.DeserializeObject<IframeInfo[]>(values[1]);

            Publish<IframeInfo[]>("IframeInfos", v);
        }
    }

    public class IframeInfo
    {
       public string id;
       public string minX;
       public string minY;
       public string maxX;
       public string maxY;
       public string url;
    }

}