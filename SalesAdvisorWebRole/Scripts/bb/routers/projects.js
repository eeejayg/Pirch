/* Router for the new customers creation screens*/

console.log("LOADED: project router");
pirch.routers.projects = Backbone.Router.extend({
    routes: {
        "chooseRooms": "chooseRooms",
        "chooseDate": "chooseDate",
        "showProducts": "showProducts",
        "*actions": "defaultRoute"
}
});

pirch.instances.routers.projects = new pirch.routers.projects;
pirch.instances.routers.projects.on('route:defaultRoute', function (actions) {
    pirch.instances.views.chooseProjectType = new pirch.views.chooseProjectType;
});
pirch.instances.routers.projects.on('route:chooseDate', function (actions) {
    if (typeof pirch.instances.models.newProject == "undefined") {
       window.location = '/customers/'+$('#customerID').val()+'/projects'
       return;
    }
    pirch.instances.views.chooseDate = new pirch.views.chooseDate;
});
pirch.instances.routers.projects.on('route:chooseRooms', function (actions) {
    if (typeof pirch.instances.models.newProject == "undefined") {
        window.location = '/customers/' + $('#customerID').val() + '/projects'
        return;
    }
    pirch.instances.views.chooseRooms = new pirch.views.chooseRooms;
});
$(function () {
    Backbone.history.start();
});