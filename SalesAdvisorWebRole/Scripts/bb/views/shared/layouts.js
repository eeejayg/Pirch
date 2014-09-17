
pirch.layouts.headerBody = Marionette.Layout.extend({
    template: '#headerBodyLayout',

    regions: {
        header: "#header",
        body: "#body"
    }
});

pirch.layouts.twoHeaderBody = Marionette.Layout.extend({
    template: '#twoHeaderBodyLayout',

    regions: {
        headerTop: "#headerTop",
        headerBottom: "#headerBottom",
        body: "#body"
    }
});

pirch.layouts.fullBodyLayout = Marionette.Layout.extend({
    template: "#fullBodyLayout",
    regions: {
        bodySection: "#bodySection"
    }
});
