initEvents.push(init);

function init() {
    console.log("webIframe Init");
    webBridgeEvent["Iframe"] = iframeEvent;
}

function iframeEvent(values) {

    switch (values[0]) {
        case "Change":
            change(values[1], values[2], values[3], values[4], values[5], values[6]);
            break;
        case "Create":
            create(values[1]);
            break;
        case "Move":
            move(values[1], values[2], values[3], values[4], values[5]);
            break;
        case "SetUrl":
            setUrl(values[1], values[2]);
            break;
        case "Close":
            close(values[1]);
            break;
        case "Info":
            info();
            break;
        case "CloseAll":
            closeAll();
            break;
    }
}

let iframes = new Map();

function change(minX, minY, maxX, maxY, url, id) {
    create(id);
    move(minX, minY, maxX, maxY, id);
    setUrl(url, id);
}
function create(id) {
    if (iframes.has(id) == false) {
        iframes.set(id, new IframeInfo(id));
        createIframe(id,0,0,0,0,"");
    }
}

function move(minX, minY, maxX, maxY, id) {
    if (iframes.has(id)) {
        var iframe = iframes.get(id);
        if (iframe.minX != minX || iframe.minY != minY || iframe.maxX != maxX || iframe.maxY != maxY) {
            iframe.minX = minX;
            iframe.minY = minY;
            iframe.maxX = maxX;
            iframe.maxY = maxY;
            moveIframe(minX, minY, maxX, maxY, id);
        }
    }
}

function setUrl(url, id) {
    if (iframes.has(id)) {
        var iframe = iframes.get(id);
        if (iframe.url != url) {
            iframe.url = url;
            setIframeUrl(url, id);
        }
    }
}

function close(id) {
    if (iframes.has(id)) {
        iframes.delete(id);
        closeIframe(id);
    }
}

function info() {
    var list = [];
    iframes.forEach(function (value, key) {
        list.push(value);
    })
    sendMessageToUnity("IframeList", JSON.stringify(list));
}


function closeAll() {
    iframes.forEach(function (value, key) {
        closeIframe(key);
    })
    iframes = new Map();
}

function createIframe(id) {
    var trueID = "webIframe_" + id;
    var iframe = document.createElement("iframe");
    iframe.id = trueID;
    iframe.style.position = "fixed";
    iframe.style.display = "block"; 
    //iframe.style.pointerEvents = "none"; //鼠标事件穿透，会导致无法和iframe内容交互
    document.body.append(iframe);
}

function setIframeUrl(url, id) {
    var trueID = "webIframe_" + id;
    var iframe = document.getElementById(trueID);
    iframe.src = url;
}

function moveIframe(minX, minY, maxX, maxY, id) {
    var trueID = "webIframe_" + id;
    var iframe = document.getElementById(trueID);

    iframe.style.width = (maxX - minX) * 100.00 + '%';
    iframe.style.height = (maxY - minY) * 100.00 + '%';
    iframe.style.bottom = minY * 100.00 + '%';
    iframe.style.left = minX * 100.00 + '%';

}

function closeIframe(id) {
    var trueID = "webIframe_" + id;
    var iframe = document.getElementById(trueID);
    iframe.remove();
}

class IframeInfo {
    constructor(id, minX, minY, maxX, maxY, url) {
        this.id = id;
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
        this.url = url;
    }
}
