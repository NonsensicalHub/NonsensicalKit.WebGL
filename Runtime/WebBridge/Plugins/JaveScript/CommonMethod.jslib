mergeInto(LibraryManager.library, {
    sendMessageToJS: function (key, values)
    {
        key = UTF8ToString(key);
        values = UTF8ToString(values);
        sendMessageToJS(key, values);
    }
    , syncDB: function ()
    {
        FS.syncfs(false, function (err)
        {
            if (err) console.log("syncfs error: " + err);
        });
    },
    // 获取本地存储
    GetLocalStorage: function (keyPtr)
    {
        var key = UTF8ToString(keyPtr);
        var value = localStorage.getItem(key);
        if (value)
        {
            var buffer = _malloc(lengthBytesUTF8(value) + 1);
            stringToUTF8(value, buffer, lengthBytesUTF8(value) + 1);
            return buffer;
        }
        return 0;
    },
    // 释放内存
    FreeMemory: function (ptr)
    {
        _free(ptr);
    }
});