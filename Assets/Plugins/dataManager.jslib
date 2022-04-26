
mergeInto(LibraryManager.library, {

    StartGame: function() {
        window.StartGame();
    },
    SendGameResult: function(jsonData) {
        window.SetGameResult(Pointer_stringify(jsonData));
    },
    ExitGame: function(jsonData) {
        window.ExitGame(Pointer_stringify(jsonData));
    },
    SendGameEvent:function(jsonData){
        window.SendGameEvent(Pointer_stringify(jsonData));
    }
});
