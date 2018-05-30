$("#play").click(function () {

    if (player().paused && player().currentTime > 0) {
        resume();
    } else {
        play();
    }

});

$("#pause").click(function () {
    pause();
});

$("#next").click(function () {
    getNextTrack(player().paused);
});

$("#previous").click(function () {
    getPreviousTrack(player().paused);
});

var getNextTrack = function (paused) {
    $.ajax({
        url: "/Home/GetNextTrack",
        method: "POST"
    })
    .done(function (data) {
        updateTrack(data, paused);
    });
}

var getPreviousTrack = function (paused) {
    $.ajax({
        url: "/Home/GetPreviousTrack",
        method: "POST"
    })
    .done(function (data) {
        updateTrack(data, paused);
    });
}

var pause = function () {
    $("#play").removeClass("hidden");
    $("#pause").addClass("hidden");
    player().pause();
    $.ajax({
        url: "/Home/Pause",
        method: "POST"
    })
    .done(function (data) {
        alert("paused");
    });
}

var play = function() {
    $("#play").addClass("hidden");
    $("#pause").removeClass("hidden");
    player().play();
    $.ajax({
        url: "/Home/Play",
        method: "POST"
    })
    .done(function (data) {
        alert("playing");
    });
}

var resume = function () {
    $("#play").addClass("hidden");
    $("#pause").removeClass("hidden");
    player().play();
    $.ajax({
        url: "/Home/Resume",
        method: "POST"
    })
    .done(function (data) {
        alert("resumed");
    });
}

var stop = function() {
    updateTrackData("", "", "");
    player().stop();
    $.ajax({
        url: "/Home/Stop",
        method: "POST"
    })
    .done(function (data) {
        alert("stopped");
    });
}

var updateTrack = function (data, paused) {

    if (!data) {
        stop();
        return;
    }

    updateTrackData(data.Url, data.ArtworkUrl, data.Description);

    player().load();
    if (paused) {
        pause();
    } else {
        play();
    }
}

var updateTrackData = function(url, artworkUrl, description)
{
    $("#mpeg_src").attr("src", url);
    $("#artwork>img").attr("src", artworkUrl);
    $("#description").text(description);
}

var player = function() {
    return document.getElementById("controls");
}

$(function () {
    document.getElementById("controls").onended = function () {
        getNextTrack();
    }
});