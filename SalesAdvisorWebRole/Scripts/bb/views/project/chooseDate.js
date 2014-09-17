$(function () {
    pirch.views.chooseDate = Backbone.View.extend({
        el: "#projects",
        template: _.template($('#BB_TMP_chooseDate').html()),
        events: {
            "click #saveProject": "saveProject"
        },
        initialize: function () {
            this.model = pirch.instances.models.newProject;
            this.render();
        },
        render: function () {
            $(this.el).html(this.template());
        },
        saveProject: function(e){
            e.preventDefault();
            var date = $('#dateCompleted').val();
            this.model.set('dateComplete', date);
            this.model.save(
                null,
                {
                    success: function (model, data) {
                        if (data.success == "True")
                        {
                            window.location = '/customers/';
                        } else {
                            pirch.utils.showError("There was an error. Please try again later");
                        }
                    },
                    error: function () {
                        pirch.utils.showError("Error. Please try again later");
                        this.allowSubmit = true;
                        console.log("error")
                    }
                }
           );
        }
    });
});