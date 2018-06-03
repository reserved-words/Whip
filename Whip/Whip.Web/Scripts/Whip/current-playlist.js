var CurrentPlaylist = {
    getNextTrack: function (done) {
        util.post("/CurrentPlaylist/GetNextTrack", done);
    },
    getPreviousTrack: function (done) {
        util.post("/CurrentPlaylist/GetPreviousTrack", done);
    },
    update: function (url, done) {
        util.post(url, done);
    }
};