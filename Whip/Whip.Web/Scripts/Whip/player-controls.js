var PlayerControls = {
    
    play: function () {
        $("#play").addClass("hidden");
        $("#pause").removeClass("hidden");
        player().play();
    },
    pause: function () {
        $("#play").removeClass("hidden");
        $("#pause").addClass("hidden");
        player().pause();
    },
    skipToPercentage: function () {
        var currentTime = player().currentTime;
        var totalTime = player().duration;
        return (totalTime === 0) ? 0 : 100 * currentTime / totalTime;
    },
    stop: function () {
        updateTrack(null);
    },
    updateTrack: function (data) {
        if (!data) {
            player().stop();
            disableControls(true);
            return;
        }
        disableControls(false);
        player().load();
    },
    disableControls: function (disable) {
        $("#play").prop("disabled", disable);
        $("#pause").prop("disabled", disable);
        $("#next").prop("disabled", disable);
        $("#previous").prop("disabled", disable);
    },
    player: function () {
        return document.getElementById("controls");
    },
    isPaused: function() {
        return player().paused && player().currentTime > 0;
    }
};