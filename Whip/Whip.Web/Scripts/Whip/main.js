var auth = new Auth();
var player = new Player();

$("body").on("click", "[data-whip-url]", function () {
    UTIL.updateMainContent($(this).attr("data-whip-url"));
});

$("body").on("click", "[data-whip-artist-url]", function () {
    UTIL.updateContent($(this).attr("data-whip-artist-url"), "#library-artist");
    window.scrollTo(0,0);
});

$("body").on("click", "button[data-whip-play-url]", function () {
    player.updatePlaylist($(this).attr("data-whip-play-url"));
});

var populateFavouritePlaylists = function () {
    UTIL.get("/Playlists/Favourites", function (data) {
        $("#favourite-playlists").html(data);
    });
}

$(function () {
    auth.tryAuthorize();
    populateFavouritePlaylists();
});