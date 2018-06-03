
var Util = {
    hideModal: function () {
        $("#modal").modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    },
    showModal: function (content) {
        $("#modal .modal-content").html(content);
        $("#modal").modal({ show: true, backdrop: 'static' });
    },
    post: function (url, done, data) {
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
    get: function (url, done) {
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
    updateMainContent: function (url) {
        get(url, function (data) {
            $("#main").html(data);
        });
    }
}