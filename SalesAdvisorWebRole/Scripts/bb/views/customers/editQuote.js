pirch.views.editQuote = Marionette.Layout.extend({
    template: '#editQuotePage',
    regions: {
        top: ".regionTop",
        tabBar: ".regionTabBar",
        tabs: ".regionTabs"
    },

    templateHelpers: function () {
        return this.model.viewHelpers;
    },

    initialize: function () {
        _.bindAll(this);

        // TODO: tabBar is not the right view for this set of tabs. It should be a + button with
        // an auto-sized set of collection labels that may be bigger than the browser width and require
        // horizontal swipe-scrolling.
        this.tabBarView = new pirch.views.tabBar({
            model: new Backbone.Model({
                tabs: [{
                    label: "+"
                }]
            })
        });

        this.on("render", this.rendered);
    },

    updateTabs: function() {
        var collections = this.model.getProductCollections();
        var tabs = [{ label: "+" }];
        collections.each(function (collection) {
            tabs.push({ label: collection.get("name") });
        });
        this.tabBarView.model.set("tabs", tabs);
    },

    rendered: function () {
        this.updateTabs();
        this.tabBar.show(this.tabBarView);
    }
});
