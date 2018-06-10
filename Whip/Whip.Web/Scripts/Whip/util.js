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
        $.ajax({
            url: url,
            method: "POST",
            data: data
        })
        .done(function (data) {
            if (done) {
                done(data);
            }
        });
    },

    get(url, done) {
        $.ajax({
            url: url,
            method: "GET"
        })
        .done(function (data) {
           if (done) {
               done(data);
           }
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

    isCurrentTab(tabName){
        return $("#tab-" + tabName).length > 0;
    }
}