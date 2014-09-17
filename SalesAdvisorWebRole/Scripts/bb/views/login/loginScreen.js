$(function () {
    pirch.views.loginScreen = Backbone.View.extend({
        el: "#login",
        template: _.template($('#loginScreen').html()),
        events: {
            "keypress input[type=password]": "pwKeypress",
            "click button": "tryLogin"

        },
        render: function () {
            var data = this.model.toJSON();
            $(this.el).html(this.template(data));
        },
        initialize: function () {
            this.model = pirch.instances.models.userToLogin;
            this.allowSubmit = true;
            this.render();
        },
        pwKeypress: function (e) {
            if (e.which !== 13) {
                return;
            }
            this.tryLogin();
        },
        tryLogin: function () {
            this.model.set({
                Password: $('#password').val(),
                __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val()
            });
            var data = this.model.toJSON();
            if (data.Password == "" || this.allowSubmit == false) {
                return;
            }
            this.allowSubmit = false;
            $(this.el).find('#signIn').hide();
            this.signingIn();
            $.ajax({
                url: '/login/loguserin',
                type: "POST",
                data: data,
                success: function (d) {
                    var data = $.parseJSON(d);
                    if (data.success) {
                        window.location = "/home/index";
                    } else {
                        pirch.utils.cbox("Incorrect Password")
                        $('div.loading_word').remove();
                        $('#signIn').show();
                        pirch.instances.views.loginScreen.allowSubmit = true;
                    }
                }
            });
        },
        signingIn: function () {
            $('#signIn').after($('#signingIn').html());
        }
    });
});