//  The info tab views
pirch.views.email = Backbone.Marionette.ItemView.extend({
    template: _.template($('#emailDisplayTemplate').html()),
});

pirch.views.emailCollectionView = Backbone.Marionette.CollectionView.extend({
    itemView: pirch.views.email,
});

pirch.views.phoneNumber = Backbone.Marionette.ItemView.extend({
    template: _.template($('#phoneNumberDisplayTemplate').html()),
});

pirch.views.phoneNumbersCollectionView = Backbone.Marionette.CollectionView.extend({
    itemView: pirch.views.phoneNumber,
});


pirch.views.address= Backbone.Marionette.ItemView.extend({
    template: _.template($('#addressDisplayTemplate').html())
});

pirch.views.addressesCollectionView = Backbone.Marionette.CollectionView.extend({
    itemView: pirch.views.address
});


pirch.views.email = Backbone.Marionette.ItemView.extend({
    template: _.template($('#emailDisplayTemplate').html())
});

// The Edit tab Views
pirch.views.emailEdit = pirch.views.DataBoundItemView.extend({
    template: _.template($('#emailEditTemplate').html())

});
pirch.views.emailEditCollectionView = Backbone.Marionette.CollectionView.extend({
    itemView: pirch.views.emailEdit
});

pirch.views.phoneNumberEdit = pirch.views.DataBoundItemView.extend({
    template: _.template($('#phoneNumberEditTemplate').html())
});

pirch.views.phoneNumbersEditCollectionView = Backbone.Marionette.CollectionView.extend({
    itemView: pirch.views.phoneNumberEdit
});



pirch.views.addressEdit = pirch.views.DataBoundItemView.extend({
    template: _.template($('#addressEditTemplate').html())
});

pirch.views.addressesEditCollectionView = Backbone.Marionette.CollectionView.extend({
    itemView: pirch.views.addressEdit
});