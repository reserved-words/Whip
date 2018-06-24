class PlayerControls {
    
    play() {
        $(".play").addClass("hidden");
        $(".pause").removeClass("hidden");
        this.player().play();
    }

    pause() {
        $(".play").removeClass("hidden");
        $(".pause").addClass("hidden");
        this.player().pause();
    }

    skipToPercentage() {
        var currentTime = this.player().currentTime;
        var totalTime = this.player().duration;
        return (totalTime === 0) ? 0 : 100 * currentTime / totalTime;
    }

    stop() {
        this.reset();
        $(".play").prop("disabled", false);
    }

    reset() {
        this.disableControls(true);
        this.pause();
        this.player().load();
    }

    updateTrack() {
        this.disableControls(false);
        this.player().load();
    }

    disableControls(disable) {
        $(".play").prop("disabled", disable);
        $(".pause").prop("disabled", disable);
        $(".next").prop("disabled", disable);
        $(".previous").prop("disabled", disable);
    }

    player() {
        return document.getElementById("controls");
    }

    isPaused() {
        return this.player().paused && this.player().currentTime > 0;
    }

    secondsPlayed() {
        return Math.floor(this.player().currentTime);
    }

    totalDuration() {
        var duration = this.player().duration;
        return isNaN(duration) ? 0 : Math.floor(duration);
    }
}