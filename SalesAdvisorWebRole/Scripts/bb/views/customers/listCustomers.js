
pirch.views.oneSalesAssociate= Backbone.Marionette.ItemView.extend({
    template: _.template($('#salesAssociateOne').html()),
    events: {
        "click a": "changeAssociate"
    },
    changeAssociate: function (e) {
        e.preventDefault();
        //this.model.trigger("chooseAssociate", this.model);
    }
});
pirch.views.salesAssociateCollectionView = Backbone.Marionette.CollectionView.extend({
    itemView: pirch.views.oneSalesAssociate,
});

pirch.views.oneListedCustomer = Backbone.Marionette.ItemView.extend({
    tagName: "li",
    template: _.template($('#customerListItem').html()),
    events: {
         "click a": "goToCustomer"
    },
    goToCustomer: function (e) {
        //e.preventDefault();
        //  This removes the link to the Info Edit screen for Beta. 
    }
});

pirch.views.customerListCollectionView = Backbone.Marionette.CollectionView.extend({
    tagName: "ul",
    className: "listView",
    itemView: pirch.views.oneListedCustomer
});

pirch.views.currentAssociate = Backbone.Marionette.ItemView.extend({
    template: _.template($('#currentAssociateTemplate').html())
});