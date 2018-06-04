class CurrentPlaylist {
    getNextTrack(done) {
        UTIL.post("/CurrentPlaylist/GetNextTrack", done);
    }

    getPreviousTrack(done) {
        UTIL.post("/CurrentPlaylist/GetPreviousTrack", done);
    }

    updateTab() {
        if (UTIL.isCurrentTab("current-playlist")) {
            UTIL.updateMainContent("/CurrentPlaylist");
        }
    }

    update(url, done) {
        var self = this;
        UTIL.post(url, function(data) {
            done(data);
            self.updateTab();
        });
    }
}