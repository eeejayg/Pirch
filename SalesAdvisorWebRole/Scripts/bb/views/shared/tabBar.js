pirch.views.tabBar = Marionette.ItemView.extend({
    template: _.template($('#tabBar').html()),
    templateHelpers: {
        tabWidth: function () {
            return Math.ceil(100 / this.tabs.length);
        }
    },
    initialize: function() {
        this.model.set('selectedTab', this.model.get('selectedTab') || 0);
    },
    events: {
        "click .tab": function (event) {
            var i = $(event.target).index();
            this.model.set('selectedTab', i);
            this.trigger("tabbar:select", i);
            this.render();
        }
    }
});
