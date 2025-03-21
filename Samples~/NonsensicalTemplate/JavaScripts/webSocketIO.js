initEvents.push(init);

function init() {
    console.log("websocketIO Init");
    webBridgeEvent["SocketIO"] = socketIOEvent;
}

function socketIOEvent(values) {
    switch (values[0]) {
        case "ConnectSocketIO":
            connectSocketIO(values[1], values[2]);
            break;
        case "AddListener":
            addListener(values[1], values[2]);
            break;
        case "SendMessage":
            sendMessage(values[1], values[2], values[3]);
            break;
        case "SendMessageWithCallback":
            sendMessageWithCallback(values[1], values[2], values[3]);
            break;
        case "Disconnect":
            Disconnect(values[1]);
            break;
        case "DisconnectAll":
            DisconnectAll();
            break;
    }
}

function connectSocketIO(url, id) {
    $().connectSocketIO(url, id);
}
function addListener(eventName, id) {
    $().addListener(eventName, id);
}
function sendMessage(eventName, msg, id) {
    $().sendMessage(eventName, msg, id);
}
function sendMessageWithCallback(eventName, msg, id) {
    $().sendMessageWithCallback(eventName, msg, id);
}
function Disconnect(id) {
    $().Disconnect(id);
}
function DisconnectAll() {
    $().DisconnectAll();
}

$(function () {
    let sockets = new Array();
    $.fn.connectSocketIO = function (url, id) {
        sockets[id] = io(url);
    }
    $.fn.addListener = function (eventName, id) {
        sockets[id].on(eventName, (data) => {
            if (typeof data=="string") {
                sendMessageToUnity("SocketIOMessage", data, eventName, id);
            } else {
                sendMessageToUnity("SocketIOMessage", JSON.stringify(data), eventName, id);
            }
        });
    }
    $.fn.sendMessage = function (eventName, c, id) {
        sockets[id].emit(eventName, JSON.parse(c));
    }
    $.fn.sendMessageWithCallback = function (eventName, c, id) {
        sockets[id].emit(eventName, JSON.parse(c), (callback) => { sendMessageToUnity("SocketIOMessage", callback, eventName, id) });
    }
    $.fn.Disconnect = function (id) {
        sockets[id].disconnect();
        sockets[id] = null;
    }
    $.fn.DisconnectAll = function () {
        sockets.forEach(element => {
            element.disconnect();
        });
    }
});
