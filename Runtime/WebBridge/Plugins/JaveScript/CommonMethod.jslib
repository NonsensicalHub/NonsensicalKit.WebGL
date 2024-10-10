mergeInto(LibraryManager.library, {
   sendMessageToJS: function (key, values) {
      key = UTF8ToString(key);
      values = UTF8ToString(values);
      sendMessageToJS(key, values);
   }

   , syncDB: function () {
      FS.syncfs(false, function (err) {
         if (err) console.log("syncfs error: " + err);
      });
   }
});