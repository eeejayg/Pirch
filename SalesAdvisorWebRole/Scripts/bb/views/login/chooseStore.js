$(function () {
    pirch.views.chooseStore = Backbone.View.extend({
        el: "#login",
        template: _.template($("#chooseStore").html()),
        render: function () {
            $(this.el).html(this.template());
            window.picturefill();
        },
        initialize: function () {
            this.render();
        },
        events: {
            "click #storeCodes a": "setStoreCode"
        },
        setStoreCode: function (e) {
            e.preventDefault();
            if (this.enabled) {
                this.enabled = false;
                pirch.instances.routers.login.navigate("chooseFace", {trigger: true});
            }
        },
        enabled: true
    });
});