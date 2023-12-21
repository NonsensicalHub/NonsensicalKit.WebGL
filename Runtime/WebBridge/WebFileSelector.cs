using NonsensicalKit.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.WebGL
{
    /// <summary>
    /// 用于处理webgl选择本地文件的操作
    /// </summary>
    [AddComponentMenu("NonsensicalKit/WebGL/FileSelector")]
    public class WebFileSelector : MonoSingleton<WebFileSelector>
    {
        protected override void Awake()
        {
            base.Awake();
            Subscribe<string[]>("WebBridge", "FileSelected", OnFileSelected);
        }

        public void OpenFileSelector(string type, bool isMultiple = false, string id = "default")
        {
            Publish("SendMessageToJS", "FileSelector", new string[] { type, isMultiple ? "true" : "false", id });
        }

        private void OnFileSelected(string[] values)
        {
            ChoiceFiles(values[1], values[2]);
        }

        private void ChoiceFiles(string nameWithUrl, string id)
        {
            Debug.Log(nameWithUrl);
            string[] array = NonsensicalKit.Tools.JsonTool.DeserializeObject<string[]>(nameWithUrl);

            Debug.Log(NonsensicalKit.Tools.JsonTool.SerializeObject(array));
            List<string> names = new List<string>();
            List<string> urls = new List<string>();
            for (int i = 0; i < array.Length - 1; i += 2)
            {
                names.Add(array[i]);
                urls.Add(array[i + 1]);
            }
            PublishWithID("WebFileSelected", id, names, urls);
            Publish("WebFileSelected", names, urls);
        }
    }
}
