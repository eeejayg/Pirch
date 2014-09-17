pirch.routers.customers = Backbone.Marionette.AppRouter.extend({
    appRoutes: {
        "newCustomer": "newCustomer",
        ":id/projects": "projects",
        ":id/quotes/:quoteId": "viewQuote",
        ":id/quotes/:quoteId/edit": "editQuote",
        ":id/quotes/:quoteId/view": "viewQuote",
        ":id/info/:childTab": "info",
        ":id/editInfo/:childTab": "infoEdit",
        "*actions": "defaultRoute"
    }
});