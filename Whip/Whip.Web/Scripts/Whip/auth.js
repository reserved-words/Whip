
class Auth {
    
    tryAuthorize() {
        var self = this;
        UTIL.post("/Home/CheckLastFmAuthorized", function (data) {
            if (data) {
                UTIL.showModal(data);
                $("#authorizeLastFm").click(function () {
                    self.authorizeLastFm();
                });
            }
        });
    }

    authorizeLastFm() {
        UTIL.post("/Home/Authorize", function (data) {
            if (data) {
                UTIL.showModal(data);
            } else {
                UTIL.hideModal();
            }
        });
    }
}