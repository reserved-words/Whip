var CurrentTrack = {
    updateLovedStatus: function () {
        util.post("/CurrentTrack/IsLoved", function (data) {
            if (data.IsLoved) {
                $("#loved").removeClass("hidden");
                $("#notloved").addClass("hidden");
            } else {
                $("#loved").addClass("hidden");
                $("#notloved").removeClass("hidden");
            }
        });
    },
    updateTrackData: function (trackData) {
        updateLovedStatus();
        $("#mpeg_src").attr("src", trackData.Url);
        $("#artwork").attr("src", trackData.ArtworkUrl);
        $("#title").text(trackData.Title);
        $("#artist").text(trackData.Artist);
        $("#album").text(trackData.Album);
        $("#year").text(trackData.Year);
    }
};