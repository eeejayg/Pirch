
// This could be moved inside the Marionette app once it gets slimmed down and integrated fully with Marionette
pirch.controllers.products = {
    siteAreaVignette: function (storePath, siteAreaGuid) {
        app.siteAreaVignette.stop();
        app.siteAreaVignette.start({ storePath: storePath, siteAreaGuid: siteAreaGuid });
    },
    siteAreas: function ( storePath ) {
        app.siteAreas.stop();
        app.siteAreas.start({ storePath: storePath });
    },
    browseProductDetail: function( room, category, productTypeGUID ) {
        app.browseProductDetail.stop();
        app.browseProductDetail.start({ room: room, category: category, productTypeGUID: productTypeGUID });
    },
    browseProducts: function ( room, category ) {
        app.browseProducts.stop();
        app.browseProducts.start({ room: room, category: category });
    },
    browseRooms: function () {
        app.browseRooms.stop();
        app.browseRooms.start();
    },
    browseCategories: function ( room ) {
        app.browseCategories.stop();
        app.browseCategories.start({ room: room });
    }
};


// The app holds a region for the entire page and is also a repository for models and collections. At some point
// the app may handle its modules - for example the router might call app.newCustomer() which will start the newCustomer
// module etc.

// TODO
// rename as productsApp
app = new Marionette.Application();
app.addRegions({
    containerRegion: "#products"
});

// Initializer gets installed here, then called on app.start()
app.addInitializer(function () {

    // Start routing
    app.router = new pirch.routers.products({ controller: pirch.controllers.products });

    // app.products is a collection of products. Since the products are required for rendering collections,
    // start loading products now and add a flag to show that it's loading. That way a projects page can
    // see that the products aren't loaded, listen to app.products and then re-render itself once the products
    // are loaded.
    this.products = new pirch.collections.products();
    this.productsLoading = true;
    this.products.fetch({
        success: _.bind(function() {
            this.productsLoading = false;
        }, this),
        error: function () {
            alert("Failed to load products collection.");
        }
    });

    this.rooms = new Backbone.Collection(
        [{ name: "kitchen", Image: "" }, { name: "bathroom", Image: "" }, { name: "outdoor", Image: "" }, { name: "laundry", Image: "" }]
    );

});

app.module('siteAreaVignette', function (module, app) {

    module.startWithParent = false;

    module.on("start", function (options) {

        module.options = _.extend(module.options || {},options);
        module.siteAreaModel = new pirch.models.siteArea({
            storePath: module.options.storePath,
            SiteAreaID: module.options.siteAreaGuid
        });
        module.siteAreaModelLoading = true;
        module.siteAreaModel.fetch({
            // TODO endpoints such as this should be defind as global constants like pirch.endpoints["JSON_SiteArea"] or something to that effect ...
            // this is fetched with a specific URL since the model was defined to be used in a collection and a single instance will not construct the URL propertly with urlRoot / idAttribute
            // reference:  http:\/\/stackoverflow.com/questions/7382243/fetching-a-single-backbone-model-from-server
            url: "/json/SiteArea" + "/" + module.options.siteAreaGuid,
            success: _.bind(function() {
                this.siteAreaModelLoading = false;
                module.afterData();
            }, this),
            error: function() {
                alert("Failed to load a specific site area mode.");
            }
        })

    });

    module.afterData = function() {

        module.container = new pirch.layouts.headerBody();
        app.containerRegion.show(module.container);

        module.headerBar = new pirch.views.headerBar({
            model: new pirch.helpermodels.headerBar({
                title: module.siteAreaModel.get('Name')
            })
        })

        //module.siteAreasList = new pirch.views.siteAreasList({
        //    model: new Backbone.Model({ storeName: module.options.storeName }),
        //    collection: module.siteAreas
        //});

        module.container.header.show(module.headerBar);
        //module.container.body.show(module.siteAreasList);
    }

});

app.module('siteAreas', function (module, app) {

    module.startWithParent = false;

    module.on("start", function (options) {

        // get current store info and no store, nav back to get it
        var currentStoreInfo = cookie2object(pirch.constants.COOKIE_STORE_CODE);
        if (!currentStoreInfo) {
            window.assign("login/ChooseStore");
            return;
        }

        module.options = _.extend(module.options || {}, options);
        module.options.storeName = module.options.storePath.replace(/_/g, " ");

        module.siteAreas = new pirch.collections.siteAreas();
        module.siteAreas.fetch({
            success: _.bind(function (returnedCollection) {
                module.afterData();
            }, this),
            error: function () {
                alert("Failed to load site areas.");
                window.assign("login/ChooseStore");
            }
        });

    });

    module.afterData = function () {

        module.container = new pirch.layouts.headerBody();
        app.containerRegion.show(module.container);

        module.headerBar = new pirch.views.headerBar({
            model: new pirch.helpermodels.headerBar({
                title: module.options.storeName
            })
        })

        module.siteAreasList = new pirch.views.siteAreasList({
            model: new Backbone.Model({ storeName: module.options.storeName }),
            itemViewOptions: { storePath: module.options.storePath },
            collection: module.siteAreas
        });
        module.container.header.show(module.headerBar);
        module.container.body.show(module.siteAreasList);

    };

});

app.module('browseRooms', function (module, app) {

    module.startWithParent = false;

    module.on("start", function (options) {

        module.options = _.extend(module.options || {},options);

        module.container = new pirch.layouts.twoHeaderBody();

        app.containerRegion.show(module.container);

        module.headerBar = new pirch.views.headerBar({
            model: new pirch.helpermodels.headerBar({
                title: "Products"
            })
        });

        // get the store path so when mapIcon is clicked it takes you to vignettes
        // for the current store and if store is not set, go back and make user
        // select it
        var currentStoreInfo = cookie2object(pirch.constants.COOKIE_STORE_CODE);
        if (!currentStoreInfo) {
            window.assign("login/ChooseStore");
            return;
        }

        var storePath = currentStoreInfo.StoreCookieStoreName.replace(/ /g, "_");

        module.searchBar = new pirch.views.searchBar({
            model: new pirch.helpermodels.searchBar({
                mapIconPath: "SiteAreas/" + storePath
            })
        });

        module.roomViews = new Backbone.Marionette.CollectionView({
            tagName: "div",
            className: "roomsViewContainer",
            collection: app.rooms,           
            itemView: pirch.views.roomView
        });

        module.container.headerTop.show(module.headerBar);
        module.container.headerBottom.show(module.searchBar);
        module.container.body.show(module.roomViews);
    });

});

// A module can be started by the router at which point it takes over the page and renders its views.
// All views for a module can be declared within the scope of the module.
app.module('browseProducts', function (module, app) {

    // Don't autostart with module (the default) when the parent starts. The parent is the app in this case.
    module.startWithParent = false;

    module.getSelectedProductTypeGUIDs = function () {
        var selectedGUIDs = [];

        var projects = pirch.instances.activeCustomer.get('Projects');
        if (!projects) { return [] }

        projects.each(function (project) {
            var instances = project.get('ProductInstances');
            if (instances) {
                instances.each(function (instance) {
                    selectedGUIDs.push(instance.get('ProductTypeGUID'));
                })
            }
        });

        return _.uniq(selectedGUIDs);
    };

    module.getActiveCustomerRecord = function (CustomerId) {
        if (!CustomerId) { return; }
        pirch.instances.activeCustomer = new pirch.models.customer();
        pirch.instances.activeCustomer.set("Id", CustomerId);
        pirch.instances.activeCustomer.fetch({
            success: function () {
                module.grid.children.each(function (view) {
                    view.showPinState(module.getSelectedProductTypeGUIDs());
                })
            }
        });
    };

    module.removeProductInstance = function (productTypeGUID) {
        pirch.instances.activeCustomer.get('Projects').each(function (project) {
            var instances = project.get('ProductInstances').where({ ProductTypeGUID: productTypeGUID })
            _.each(instances, function (instance) {
                var productInstance = new pirch.models.productInstance();
                productInstance.set('Id', instance.get('Id'));
                productInstance.destroy();
            })
        });
    }

    module.on("start", function (options) {

        module.options = module.options || {};
        module.options = _.extend(module.options,options);

        // Set up layouts and regions to place our views in
        module.container = new pirch.layouts.headerBody();

        // Render this module into the app's container
        app.containerRegion.show(module.container);

        module.headerBar = new pirch.views.headerBar({
            model: new pirch.helpermodels.headerBar({
                title: module.options.room + " / " + module.options.category
            })
        });

        module.grid = new pirch.views.grid({
            className: "gridContainer browseProducts",
            collection: app.products,
            itemView: pirch.views.productViewWithLegend,
            itemViewOptions: module.options,
            cellHeight: 352,
            cellWidth: 256,
            preserveAspectRatio: true
        });
        
        module.grid.on("render", function () {
            setTimeout(function () { window.picturefill(); });
            module.getActiveCustomerRecord($.cookie(pirch.constants.COOKIE_ACTIVE_CUSTOMER_ID));
        });

        module.grid.on("itemview:product:remove", function (productItemWithLegendViewModel) {
            module.removeProductInstance(productItemWithLegendViewModel.model.get('ProductTypeGUID'));
        });

        // .show calls render on the grid
        module.container.header.show(module.headerBar);
        module.container.body.show(module.grid);

        module.grid.layoutGrid();
    });
});

app.module('browseProductDetail', function (module, app) {

    module.startWithParent = false;

    module.on("start", function (options) {

        module.options = module.options || {};
        module.options = _.extend(module.options, options);

        module.fullBodyLayout = new pirch.layouts.fullBodyLayout();

        app.containerRegion.show(module.fullBodyLayout);

        module.productDetailView = new pirch.views.productDetailView();

        module.fullBodyLayout.bodySection.show(module.productDetailView);
    });

});

app.module('browseCategories', function (module, app) {

    module.startWithParent = false;

    module.on("start", function (options) {

        module.options = module.options || {};
        module.options = _.extend(module.options,options);

        module.container = new pirch.layouts.twoHeaderBody();

        app.containerRegion.show(module.container);

        module.headerBar = new pirch.views.headerBar({
            model: new pirch.helpermodels.headerBar({
                title: options.room + " Products"
            })
        });

        // get the store path so when mapIcon is clicked it takes you to vignettes
        // for the current store and if store is not set, go back and make user
        // select it
        var currentStoreInfo = cookie2object(pirch.constants.COOKIE_STORE_CODE);
        if (!currentStoreInfo) {
            window.assign("login/ChooseStore");
            return;
        }

        var storePath = currentStoreInfo.StoreCookieStoreName.replace(/ /g, "_");

        module.searchBar = new pirch.views.searchBar({
            model: new pirch.helpermodels.searchBar({
                mapIconPath: "SiteAreas/" + storePath
            })
        });


        module.categories = new Backbone.Collection(categoryMockData.items[options.room], { model: pirch.models.categoryType });

        module.grid = new pirch.views.grid({
            className: "gridContainer browseCategories",
            collection: module.categories,
            itemView: pirch.views.categoryView,
            cellWidth: 256,
            cellHeight: 256,
            preserveAspectRatio: false
        });

        module.grid.on("itemview:category:choose", function (viewModel) {
            window.gcategorymodel = viewModel;
            var categoryPath = viewModel.model.get('urlPath');
            var roomPath = module.options.room;
            app.router.navigate(roomPath + "/" + categoryPath, { trigger: true });
        });

        module.container.headerTop.show(module.headerBar);
        module.container.headerBottom.show(module.searchBar);
        module.container.body.show(module.grid);

        module.grid.layoutGrid();
    });

});

// Start the app now that it and its modules have been declared.
// This will call the initializer and start the router.
app.start();

// Backbone history start causes the router to kick off and run the module specified by the route
Backbone.history.start({
    root: "/Products"
});

// mock category data

var categoryMockData = {
    basePath: "Images/categories/",
    items: {
        bathroom: [
            { label: "Bath Faucets", urlPath: "bath_faucets", image: "bathroom/bath_faucets.png" },
            { label: "Bath Fillers", urlPath: "bath_fillers", image: "bathroom/bath_fillers.png" },
            { label: "Baths", urlPath: "baths", image: "bathroom/baths.png" },
            { label: "Personal Wellness", urlPath: "personal_wellness", image: "bathroom/personal_wellness.png" },
            { label: "Pet Baths", urlPath: "pet_baths", image: "bathroom/pet_baths.png" },
            { label: "Showerheads", urlPath: "showerheads", image: "bathroom/showerheads.png" },
            { label: "Sink Faucets", urlPath: "sink_faucets", image: "bathroom/sink_faucets.png" },
            { label: "Sinks", urlPath: "sinks", image: "bathroom/sinks.png" }
        ],
        kitchen: [
            { label: "Coffee Systems", urlPath: "coffee_systems", image: "kitchen/coffee_systems.png" },
            { label: "Cooktops", urlPath: "cooktops", image: "kitchen/cooktops.png" },
            { label: "Dishwashers", urlPath: "dishwashers", image: "kitchen/dishwashers.png" },
            { label: "Microwaves", urlPath: "microwaves", image: "kitchen/microwaves.png" },
            { label: "Ovens", urlPath: "ovens", image: "kitchen/ovens.png" },
            { label: "Range Hoods", urlPath: "range_hoods", image: "kitchen/range_hoods.png" },
            { label: "Ranges", urlPath: "ranges", image: "kitchen/ranges.png" },
            { label: "Rangetops", urlPath: "rangetops", image: "kitchen/rangetops.png" },
            { label: "Refrigerators", urlPath: "refrigerators", image: "kitchen/refrigerators.png" },
            { label: "Sink Faucets", urlPath: "sink_faucets", image: "kitchen/sink_faucets.png" }
        ],
        laundry: [
            { label: "Dryers", urlPath: "dryers", image: "laundry/dryers.png" },
            { label: "Faucets", urlPath: "faucets", image: "laundry/faucets.png" },
            { label: "Sinks", urlPath: "sinks", image: "laundry/sinks.png" },
            { label: "Stacked", urlPath: "stacked", image: "laundry/stacked.png" },
            { label: "Washers", urlPath: "washers", image: "laundry/washers.png" }
        ],
        outdoor: [
            { label: "Cookers", urlPath: "cookers", image: "outdoor/cookers.png" },
            { label: "Faucets", urlPath: "faucets", image: "outdoor/faucets.png" },
            { label: "Grills", urlPath: "grills", image: "outdoor/grills.png" },
            { label: "Refrigeration", urlPath: "refrigeration", image: "outdoor/refrigeration.png" },
            { label: "Sinks", urlPath: "sinks", image: "outdoor/sinks.png" },
            { label: "Tap Systems", urlPath: "tap_systems", image: "outdoor/tap_systems.png" }
        ]
    }
}


