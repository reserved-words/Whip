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
            UTIL.updateContent("/CurrentTrack", "#main");
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
    }
}