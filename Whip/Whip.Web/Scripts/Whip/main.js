var auth = new Auth();
var player = new Player();

$("body").on("click", "[data-whip-url]", function () {
    if ($(this).hasClass("disabled"))
        return;
    UTIL.updateMainContent($(this).attr("data-whip-url"));
});

$("body").on("click", "[data-whip-artist-url]", function () {
    if ($(this).hasClass("disabled"))
        return;
    UTIL.updateContent($(this).attr("data-whip-artist-url"), "#library-artist", true);
});

$("body").on("click", "[data-whip-play-url]", function () {
    if ($(this).hasClass("disabled"))
        return;
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