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
    if (iframes.has(id)) {
        moveIframe(minX, minY, maxX, maxY, id);
        setIframeUrl(url, id);
    } else {
        iframes.set(id, new IframeInfo(id, minX, minY, maxX, maxY, url));
        createIframe(id);
        moveIframe(minX, minY, maxX, maxY, id);
        setIframeUrl(url, id);
    }
}

function move(minX, minY, maxX, maxY, id) {
    if (iframes.has(id)) {
        var iframe=iframes.get(id);
        iframe.minX = minX;
        iframe.minY = minY;
        iframe.maxX = maxX;
        iframe.maxY = maxY;
        moveIframe(minX, minY, maxX, maxY, id);
    }
}

function setUrl(url, id) {
    if (iframes.has(id)) {
        var iframe=iframes.get(id);
        iframe.url = url;
        setIframeUrl(url, id);
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

    iframe.style.width = (maxX-minX) * 100.00 + '%';
    iframe.style.height = (maxY-minY) * 100.00 + '%';
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
