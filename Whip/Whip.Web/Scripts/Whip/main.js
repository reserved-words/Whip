
var util = new Util();
var player = new Player();

$("body").on("click", "a[data-whip-url]", function () {
    util.updateMainContent($(this).attr("data-whip-url"));
});

$("body").on("click", "button[data-whip-play-url]", function () {
    var url = $(this).attr("data-whip-play-url");
    player.updatePlaylist(url);
});

$("body").on("click", "#authorizeLastFm", function () {
    authorizeLastFm();
});

var checkLastFmAuthorized = function () {
    util.post("/Home/CheckLastFmAuthorized", function (data) {
        if (data) {
            util.showModal(data);
        }
    });
}

var authorizeLastFm = function () {
    util.post("/Home/Authorize", function (data) {
        if (data) {
            util.showModal(data);
        } else {
            util.hideModal();
        }
    });
}

var populateFavouritePlaylists = function () {
    util.get("/Playlists/Favourites", function (data) {
        $("#favourite-playlists").html(data);
    });
}

$(function () {
    checkLastFmAuthorized();
    populateFavouritePlaylists();
    player.disableControls(true);
});