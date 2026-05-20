using NonsensicalKit.Core;
using UnityEngine;

[AddComponentMenu("NonsensicalKit/WebGL/InputDeviceSend")]
public class WebInputDeviceSend : MonoSingleton<WebInputDeviceSend>
{
    //自发自接??貌似没有必要
    [SerializeField] private bool m_selfReceive;

    protected override void Awake()
    {
        base.Awake();
        if (m_selfReceive)
        {
            Subscribe<string[]>("WebBridge", "RemoteInput", RemoteInput);
        }
    }

    private void RemoteInput(string[] msg)
    {
        Publish("GetWebRemoteMessage", msg[1]);
    }

    /// <summary>
    ///  创建远程输入设备监听,之后会将硬件设备的信息发送给,host
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public void Create(string host, string port)
    {
        Publish("SendMessageToJS", "RemoteInput", new[] { "Create", host, port });
    }

    public void Stop()
    {
        Publish("SendMessageToJS", "RemoteInput", "Stop");
    }
}
