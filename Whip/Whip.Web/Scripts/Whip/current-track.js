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
            $("#current-track-content").text(artist + " - " + title);
            $("#current-track-header div").removeClass("hidden");
        } else {
            $("#current-track-content").text("");
            $("#current-track-header div").addClass("hidden");
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