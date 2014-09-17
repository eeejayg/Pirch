$(function () {
    pirch.views.chooseProjectType = Backbone.View.extend({
        el: "#projects",
        template: _.template($('#BB_TMP_projectType').html()),
        events: {
            "click a": "prepareModel",
        },
        render: function () {
            $(this.el).html(this.template());
        },
        prepareModel: function (e) {
            //  Here, we new up a new model for the project and set the projectType
            if ($(e.target).attr('data-id') == 4) {
                window.location = '/Products';
                return false;
            }
            pirch.instances.models.newProject = new pirch.helpermodels.ProjectCreationWithRooms;
            pirch.instances.models.newProject.set({
                "projectType": $(e.target).attr('data-id'),
            });
            pirch.instances.models.newProject.setCustomer($('#customerID').val());
        },
        initialize: function () {
            this.render();
        }
    });
});