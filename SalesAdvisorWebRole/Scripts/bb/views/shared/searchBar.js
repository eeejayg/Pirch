
pirch.helpermodels.searchBar = Backbone.Model.extend({
    defaults: {
        // The path to go to when mapIcon is clicked
        mapIconPath: ""
    }
});

pirch.views.searchBar = Marionette.ItemView.extend({
    template: _.template($('#searchBar').html()),
    //events: {
    //    "click .storeMapIcon" : "navStoreMap"
    //},
    //navStoreMap: function (e) {
    //    e.preventDefault();
    //    Backbone.history.navigate("/SiteAreas", { trigger: true });
    //},
    initialize: function () {
        _.bindAll(this);
        this.listenTo(this.model, "change", this.render);
    }
});

