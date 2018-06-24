var UTIL = {

    hideModal() {
        $("#modal").modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    },

    showModal(content) {
        $("#modal .modal-content").html(content);
        $("#modal").modal({ show: true, backdrop: 'static' });
    },

    post(url, done, data) {
        $("#loading").removeClass("hidden");
        $.ajax({
            url: url,
            method: "POST",
            data: data
        })
        .done(function (data) {
            $("#loading").addClass("hidden");
            if (done) {
                done(data);
            }
        })
        .fail(function (jqXHR, textStatus) {
            $("#loading").addClass("hidden");
            alert("Request failed: " + textStatus);
        });
    },

    get(url, done) {
        $("#loading").removeClass("hidden");
        $.ajax({
            url: url,
            method: "GET"
        })
        .done(function (data) {
            $("#loading").addClass("hidden");
            if (done) {
                done(data);
            }
        })
        .fail(function (jqXHR, textStatus) {
            $("#loading").addClass("hidden");
            alert("Request failed: " + textStatus);
        });
    },

    updateContent(url, selector, scrollToTop) {
        UTIL.get(url, function (data) {
            $(selector).html(data);
            if (scrollToTop) {
                window.scrollTo(0, 0);
            }
        });
    },

    toggleClass(selector, className, add) {
        if (add) {
            $(selector).addClass(className);
        } else {
            $(selector).removeClass(className);
        }
    },

    updateMainContent(url) {
        var self = this;
        UTIL.get(url, function (data) {
            $("#main").html(data);
            $('.navbar-collapse').collapse("hide");
            self.toggleClass("#sidebar", "hidden-xs", !self.isCurrentTab("home"));
            window.scrollTo(0, 0);
        });
    },

    isCurrentTab(tabName) {
        return $("#tab-" + tabName).length > 0;
    }
}