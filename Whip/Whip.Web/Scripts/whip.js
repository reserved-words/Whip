$("body").on("click", "a[data-whip-url]", function () {
    updateMainContent($(this).attr("data-whip-url"));
});

$("body").on("click", "button[data-whip-play-url]", function () {
    updateCurrentPlaylist($(this).attr("data-whip-play-url"));
});

$("body").on("click", "#authorizeLastFm", function () {
    authorizeLastFm();
});

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
        url: "/CurrentPlaylist/GetNextTrack",
        method: "POST"
    })
    .done(function (data) {
        updateTrack(data, paused);
    });
}

var getPreviousTrack = function (paused) {
    $.ajax({
        url: "/CurrentPlaylist/GetPreviousTrack",
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

var play = function () {
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
        url: "/Player/Resume",
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

var stop = function () {
    updateTrackData(null);
    disableControls();
    player().stop();
    $.ajax({
        url: "/Player/Stop",
        method: "POST"
    });
}

var updateMainContent = function (url) {
    $.ajax({
        url: url,
        method: "GET"
    })
    .done(function (data) {
        $("#main").html(data);
    });
}

var updateCurrentPlaylist = function (url) {
    $.ajax({
        url: url,
        method: "POST"
    })
    .done(function (data) {
        updateTrack(data, false);
    });
}

var updateTrack = function (data, paused) {

    if (!data) {
        stop();
        return;
    }

    disableControls(false);
    updateTrackData(data);

    player().load();
    if (paused) {
        pause();
    } else {
        play();
    }

    $.ajax({
        url: "/CurrentTrack/IsLoved",
        method: "POST"
    })
    .done(function (data) {
        if (data.IsLoved) {
            $("#loved").removeClass("hidden");
            $("#notloved").addClass("hidden");
        } else {
            $("#loved").addClass("hidden");
            $("#notloved").removeClass("hidden");
        }
    });
}

var updateTrackData = function (trackData) {
    $("#mpeg_src").attr("src", trackData.Url);
    $("#artwork").attr("src", trackData.ArtworkUrl);
    $("#title").text(trackData.Title);
    $("#artist").text(trackData.Artist);
    $("#album").text(trackData.Album);
    $("#year").text(trackData.Year);
}

var player = function () {
    return document.getElementById("controls");
}

var checkLastFmAuthorized = function () {
    $.ajax({
        url: "/Home/CheckLastFmAuthorized",
        method: "POST"
    })
    .done(function (data) {
        if (data) {
            showModal(data);
        }
    });
}

var authorizeLastFm = function () {
    $.ajax({
        url: "/Home/Authorize",
        method: "POST"
    })
    .done(function (data) {
        if (data) {
            showModal(data);
        } else {
            hideModal();
        }
    });
}

var hideModal = function() {
    $("#modal").modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}

var showModal = function(content) {
    $("#modal .modal-content").html(content);
    $("#modal").modal({ show: true, backdrop: 'static' });
}

var disableControls = function(disable) {
    $("#play").prop("disabled", disable);
    $("#pause").prop("disabled", disable);
    $("#next").prop("disabled", disable);
    $("#previous").prop("disabled", disable);
}

$(function () {
    checkLastFmAuthorized();
    $.ajax({
        url: "/Playlists/Favourites",
        method: "GET"
    })
    .done(function (data) {
        $("#favourite-playlists").html(data);
    });
    document.getElementById("controls").onended = function () {
        getNextTrack();
    }
    document.getElementById("controls").onseeking = function () {
        skipToPercentage();
    }
    disableControls(true);
});