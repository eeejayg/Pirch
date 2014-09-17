
pirch.routers.products = Backbone.Marionette.AppRouter.extend({
    appRoutes: {
        "SiteAreas/:storePath/:siteAreaGuid": "siteAreaVignette",
        "SiteAreas/:storePath": "siteAreas",
        ":room/:category/:guid": "browseProductDetail",
        ":room/:category": "browseProducts",
        ":room": "browseCategories",
        "*anything": "browseRooms"
    }
});



