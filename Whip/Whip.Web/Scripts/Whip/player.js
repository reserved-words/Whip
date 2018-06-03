
var Player = {
    play: function () {
        if (playerControls.isPaused()) {
            util.post("/Player/Resume");
        } else {
            util.post("/Player/Play");
        }
        playerControls.play();
    },
    pause: function () {
        playerControls.pause();
        util.post("/Player/Pause");
    },
    skipToPercentage: function () {
        var percentage = playerControls.skipToPercentage();
        if (percentage === 0)
            return;
        util.post("/Player/SkipToPercentage", null, { percentage });
    },
    stop: function () {
        currentTrack.updateTrackData(null);
        playerControls.stop();
        util.post("/Player/Stop");
    },
    updateTrack: function (data) {
        currentTrack.updateTrackData(data);
        playerControls.updateTrack(data);
        if (playerControls.isPaused()) {
            pause();
        } else {
            play();
        }
    },
    disableControls: function (disable) {
        playerControls.disableControls(disable);
    },
    getNextTrack: function() {
        currentPlaylist.getNextTrack(function (data) {
            updateTrack(data);
        });
    },
    getPreviousTrack: function () {
        currentPlaylist.getPreviousTrack(function (data) {
            updateTrack(data);
        });
    },
    updatePlaylist: function(url) {
        currentPlaylist.update(url, function (data) {
            updateTrack(data);
        });
    }
}

function Player() {
    this.util = new Util();
    this.playerControls = new PlayerControls();
    this.currentTrack = new CurrentTrack();
    this.currentPlaylist = new CurrentPlaylist();
    document.getElementById("controls").onended = function () {
        getNextTrack();
    }
    document.getElementById("controls").onseeking = function () {
        skipToPercentage();
    }
    $("#play").click(function () {
        play();
    });

    $("#pause").click(function () {
        pause();
    });

    $("#next").click(function () {
        getNextTrack();
    });

    $("#previous").click(function () {
        getPreviousTrack();
    });
}