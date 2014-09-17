

pirch.views.siteAreaItem = Backbone.Marionette.ItemView.extend({
    template: "#siteAreaItem",
    initialize: function (options) {
        this.model.set('storePath', options.storePath);
    }
})

pirch.views.siteAreasList = Backbone.Marionette.CompositeView.extend({
    template: "#siteAreasList",
    className: "siteAreasList",
    tagName: "div",
    itemView: pirch.views.siteAreaItem,
    itemViewContainer: ".siteAreaItemContainer"
})

