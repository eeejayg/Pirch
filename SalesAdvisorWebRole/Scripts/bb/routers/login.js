
pirch.routers.login = Backbone.Router.extend({
    routes: {
        "chooseStore": "chooseStore",
        "chooseFace": "chooseFace",
        "loginScreen": "loginScreen",
        "*actions": "defaultRoute"
    }
});

// Initiate the router 
pirch.instances.routers.login = new pirch.routers.login;
pirch.instances.routers.login.on('route:defaultRoute', function (actions) {
    //  Collect session and set the logged on model here and we should 
    //  persist the logged on user.
    try {
        pirch.instances.views.loginScreen = new pirch.views.loginScreen;
    } catch(err){
        pirch.instances.views.chooseFace = new pirch.views.chooseFace;
    }
})
pirch.instances.routers.login.on('route:loginScreen', function (actions) {
    try {
        pirch.instances.views.loginScreen = new pirch.views.loginScreen;
    } catch (err) {
        pirch.instances.routers.login.navigate("chooseFace", {trigger: true});
        //window.location = '/#chooseFace';
    }
});

pirch.instances.routers.login.on('route:chooseFace', function (actions) {
    pirch.instances.views.chooseFace = new pirch.views.chooseFace;
});
pirch.instances.routers.login.on("route:chooseStore", function (actions) {
    pirch.instances.views.chooseStore = new pirch.views.chooseStore;
});
pirch.instances.routers.login.on('route:chooseFace', function (actions) {
    pirch.instances.views.chooseFace = new pirch.views.chooseFace;
});
