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

    updateContent(url, selector) {
        UTIL.get(url, function (data) {
            $(selector).html(data);
        });
    },

    isCurrentTab(tabName){
        return $("#tab-" + tabName).length > 0;
    }
}