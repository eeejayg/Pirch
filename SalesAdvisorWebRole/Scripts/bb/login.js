$(function () {
    Backbone.history.start();
    $.removeCookie(pirch.constants.COOKIE_ACTIVE_CUSTOMER_ID);
});

// this re-routes clicks on hrefs so that they
// do not navigate away from the webapp and into
// mobile safari
if (navigator.userAgent.match(/Mobile/) && navigator.userAgent.match(/Safari/)) {
        $(document).on(
            "click",
            "a",
            function (event) {
                var targ = $(event.target).attr("href");
                if (targ) {
                    event.preventDefault();
                    location.href = targ;
                    return false;
                } else {
                    return true;
                }
            }
        );
}
