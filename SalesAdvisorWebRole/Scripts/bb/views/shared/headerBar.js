pirch.helpermodels.headerBar = Backbone.Model.extend({
    defaults: {
        // The title text
        title: "",
        // A CSS class to add to the header
        subClass: ""
    }
});

pirch.views.headerBar = Marionette.ItemView.extend({
    template: _.template($('#headerBar').html()),
    initialize: function () {
        _.bindAll(this);
        this.listenTo(this.model, "change", this.render);
    }
});


