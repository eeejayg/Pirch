pirch.models.login = Backbone.Model.extend({
    urlRoot: '/login',
    defaults: {
        id: null,
        name:'',
        Password: '',
        image: '',
        UserName: '',
        __RequestVerificationToken: '',
    }
});
var lastUser = $.parseJSON($.cookie(pirch.constants.COOKIE_LAST_LOGGED_IN_USER));
if (lastUser) {
    pirch.instances.models.userToLogin = new pirch.models.login;
    pirch.instances.models.userToLogin.set(lastUser);
}