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
        url: "/Player/Pause",
        method: "POST"
    });
}

var play = function() {
    $("#play").addClass("hidden");
    $("#pause").removeClass("hidden");
    player().play();
    $.ajax({
        url: "/Player/Play",
        method: "POST"
    });
}

var resume = function () {
    $("#play").addClass("hidden");
    $("#pause").removeClass("hidden");
    player().play();
    $.ajax({
        url: "/Home/Resume",
        method: "POST"
    });
}

var skipToPercentage = function () {
    var currentTime = player().currentTime;
    var totalTime = player().duration;
    if (totalTime === 0)
        return;

    $.ajax({
        url: "/Player/SkipToPercentage",
        method: "POST",
        data: { percentage: 100 * currentTime / totalTime }
    });
}

var stop = function() {
    updateTrackData("", "", "");
    player().stop();
    $.ajax({
        url: "/Player/Stop",
        method: "POST"
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
    document.getElementById("controls").onseeking = function () {
        skipToPercentage();
    }
});