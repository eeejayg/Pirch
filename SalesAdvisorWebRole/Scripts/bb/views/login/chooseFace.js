$(function () {
    pirch.views.chooseFace = Backbone.View.extend({
        el: "#login",
        template: _.template($('#chooseFace').html()),
        events:{
            "click #faces a": "prepareModel",
        },
        render: function () {
            $(this.el).html(this.template());
            window.picturefill();
        },
        prepareModel: function(e){
            pirch.instances.models.userToLogin = new pirch.models.login;
            var jsonData = {
                id: parseInt(e.target.getAttribute('data-id')),
                name: e.target.getAttribute('data-name'),
                image: e.target.getAttribute('src'),
                UserName: e.target.getAttribute('data-login')
            };
            pirch.instances.models.userToLogin.set(jsonData);
            // set a cookie
            $.cookie(pirch.constants.COOKIE_LAST_LOGGED_IN_USER, JSON.stringify(jsonData), { path: "/", expires: 365 });
        },

        initialize: function () {
            this.render();
        }
    });
});
