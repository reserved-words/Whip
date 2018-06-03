class Player {
    
    constructor() {
        this.playerControls = new PlayerControls();
        this.currentTrack = new CurrentTrack();
        this.currentPlaylist = new CurrentPlaylist();
        this.playerControls.disableControls(true);

        var self = this;

        document.getElementById("controls").onended = function () {
            self.getNextTrack();
        }
        document.getElementById("controls").onseeking = function () {
            self.skipToPercentage();
        }
        $("#play").click(function () {
            self.play();
        });

        $("#pause").click(function () {
            self.pause();
        });

        $("#next").click(function () {
            self.getNextTrack();
        });

        $("#previous").click(function () {
            self.getPreviousTrack();
        });
    }

    play() {
        if (this.playerControls.isPaused()) {
            UTIL.post("/Player/Resume");
        } else {
            UTIL.post("/Player/Play");
        }
        this.playerControls.play();
    }

    pause() {
        this.playerControls.pause();
        UTIL.post("/Player/Pause");
    }

    skipToPercentage() {
        var percentage = this.playerControls.skipToPercentage();
        if (percentage === 0)
            return;
        UTIL.post("/Player/SkipToPercentage", null, { percentage });
    }

    stop () {
        this.currentTrack.updateTrackData(null);
        this.playerControls.stop();
        UTIL.post("/Player/Stop");
    }
    
    updateTrack(data) {
        this.currentTrack.updateTrackData(data);
        this.playerControls.updateTrack(data);
        if (this.playerControls.isPaused()) {
            this.pause();
        } else {
            this.play();
        }
    }

    getNextTrack() {
        var self = this;
        self.currentPlaylist.getNextTrack(function (data) {
            self.updateTrack(data);
        });
    }

    getPreviousTrack() {
        var self = this;
        self.currentPlaylist.getPreviousTrack(function (data) {
            self.updateTrack(data);
        });
    }

    updatePlaylist(url) {
        var self = this;
        self.currentPlaylist.update(url, function (data) {
            self.updateTrack(data);
        });
    }
}