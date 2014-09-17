

pirch.views.productDetailView = Marionette.ItemView.extend({

    template: _.template($("#productDetail").html()),

    initialize: function (options) {
        _.bindAll(this);
    }
});

pirch.views.productViewWithLegend = Marionette.ItemView.extend({

    template: _.template($("#productItemWithLegend").html()),

    initialize: function (options) {
        _.bindAll(this);

        // href on the product image to the detailed single product view
        this.options.hashPath = "#/" + options.room + "/" + options.category + "/" + options.model.get("ProductTypeGUID");
    },

    events: {
        "click a.pin": "pinItem",
        "click img": "test"
    },

    onRender: function () {
        this.$el.find(".productItemWithLegend a:first-child")[0].href = this.options.hashPath;
    },

    showPinState: function (selectedGUIDs) {
        var guid = this.model.get('ProductTypeGUID');
        if (selectedGUIDs.indexOf(guid) > -1) {
            this.$el.find(".pin").addClass("pinned");
        } else {
            this.$el.find(".pin").addClass("unpinned");
        }
    },

    createProductInstance : function() {
        var productInstance = new pirch.models.productInstance();
        productInstance.set('ProductTypeGUID', this.model.get("ProductTypeGUID"));
        productInstance.set('CustomerId', $.cookie(pirch.constants.COOKIE_ACTIVE_CUSTOMER_ID));
        productInstance.save();
    },

    pinItem: function (e) {
        e.stopPropagation();
        e.preventDefault();
        if ( this.$el.find(".pin").hasClass("unpinned")) {
            this.$el.find(".pin").addClass("pinned").removeClass("unpinned");
            this.createProductInstance();
        } else {
            this.$el.find(".pin").addClass("unpinned").removeClass("pinned");
            this.triggerMethod("product:remove", this.model);
        }
    }

})

pirch.views.roomsView = Marionette.ItemView.extend({
    className: "roomsViewContainer"
})

pirch.views.roomView = Marionette.ItemView.extend({
    className: "roomItem floatH",
    template: _.template($("#roomItem").html())
})

pirch.views.categoryView = Marionette.ItemView.extend({
    template: _.template($("#categoryItem").html()),
    events: {
        "click" : "chooseCategory"
    },
    chooseCategory: function (e) {
        e.stopPropagation();
        this.triggerMethod("category:choose",this.model);
    }
})

