class CurrentTrack {
    updateLovedStatus() {
        UTIL.post("/CurrentTrack/IsLoved", function (data) {
            if (data.IsLoved) {
                $("#loved").removeClass("hidden");
                $("#notloved").addClass("hidden");
            } else {
                $("#loved").addClass("hidden");
                $("#notloved").removeClass("hidden");
            }
        });
    }

    updateTab() {
        if (UTIL.isCurrentTab("current-track")) {
            UTIL.updateMainContent("/CurrentTrack");
        }
    }

    updateHeader(title, artist) {
        if (title) {
            $("#current-track-artist").text(artist);
            $("#current-track-title").text(title);
        } else {
            $("#current-track-artist").text("");
            $("#current-track-title").text("");
        }
    }

    updateTrackData(trackData) {
        this.updateLovedStatus();
        this.updateTab();
        $("#mpeg_src").attr("src", trackData.Url);
        $("#artwork").attr("src", trackData.ArtworkUrl);
        $("#title").text(trackData.Title);
        $("#artist").text(trackData.Artist);
        $("#album").text(trackData.Album);
        $("#year").text(trackData.Year);
        this.updateHeader(trackData.Title, trackData.Artist);
    }
}