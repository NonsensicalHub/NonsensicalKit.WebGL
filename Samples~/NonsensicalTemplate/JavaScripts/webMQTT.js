initEvents.push(init);

function init() {
    console.log("webMQTT Init");
    webBridgeEvent["MQTT"] = MQTTEvent;
}

function MQTTEvent(values) {

    switch (values[0]) {
        case "Connect":
            connect(values[1], values[2], values[3]);
            break;
        case "Subscribe":
            subscribe(values[1]);
            break;
        case "Publish":
            publish(values[1], values[2]);
            break;
        case "End":
            end();
            break;
    }
}
let client;

function connect(url, user, pass) {
    // 客户端ID（注意：客户端不能写死，如果多人用同一个客户端，那么就会出现MQTT一直是断开重连断开重连的问题）
    const clientId = `mqtt_${Math.random().toString(16).slice(3)}`;
    // 连接设置
    let options = {
        clean: true,	// 保留会话
        connectTimeout: 4000,	// 超时时间
        reconnectPeriod: 1000,	// 重连时间间隔
        // 认证信息
        clientId,
        username: user,
        password: pass,
    }
    // 创建客户端
    client = mqtt.connect(url, options);
    // 成功连接后触发的回调
    client.on('connect', () => {
        console.log('MQTT连接成功');
    });
    // 当客户端收到一个发布过来的消息时触发回调
    /** 
     * topic：收到的报文的topic 
     * message：收到的数据包的负载playload 
     * packet：MQTT 报文信息，其中包含 QoS、retain 等信息
     */
    client.on('message', onMessage);
}
function end() {
    if (client != null) {
        client.end()
    }
}

function subscribe(topic) {
    // 订阅主题，这里可以订阅多个主题
    client.subscribe(topic);
}
function publish(topic, msg) {
    // 发送信息给 topic（主题）
    client.publish(topic, msg);
}

function onMessage(topic, message, packet) {
    // 这里有可能拿到的数据格式是Uint8Array格式，可以直接用toString转成字符串
    // let data = JSON.parse(message.toString());
    // console.log("获取到的数据：", message)
    // console.log("数据对应订阅主题：", topic)
    // console.log("获取到的数据包：", packet)
    sendMessageToUnity("MQTTMessage", topic, message.toString());
}
