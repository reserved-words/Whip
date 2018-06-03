class CurrentPlaylist {
    getNextTrack(done) {
        UTIL.post("/CurrentPlaylist/GetNextTrack", done);
    }

    getPreviousTrack(done) {
        UTIL.post("/CurrentPlaylist/GetPreviousTrack", done);
    }

    update(url, done) {
        UTIL.post(url, done);
    }
}