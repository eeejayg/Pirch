


// This could be moved inside the Marionette app once it gets slimmed down and integrated fully with Marionette
pirch.controllers.customers = {
    collectChildTab: function (name, activeCustomer) {

        pirch.instances.views.customerAbout = new pirch.views.customerAbout({ model: activeCustomer });
            return pirch.instances.views.customerAbout;
    },

    defaultRoute: function (invalidUrlPart) {
        // clear hash if invalid hash path was specified
        if (invalidUrlPart) {
            Backbone.history.navigate("", { replace: true });
        }
        app.customerList.stop();
        app.customerList.start();
    },

    info: function (customerId, childTab) {
        // Setting the app context will load the customer model and update any views that are displaying customer data
        app.context.set("customerId", customerId);
        // Force the detail module to restart if it's already running and supply URL params
        app.detail.stop();
        app.detail.start({ tab: "info", childTab: childTab });
    },

    editQuote: function (customerId, quoteId) {
        app.context.set("customerId", customerId);
        app.quote.stop();
        app.quote.start({
            quoteId: quoteId,
            state: "edit"
        });
    },

    viewQuote: function (customerId, quoteId) {
        app.context.set("customerId", customerId);
        app.quote.stop();
        app.quote.start({
            quoteId: quoteId,
            state: "view"
    });
    },

    infoEdit: function (custId, childTab) {

        // Setting the app context will load the customer model and update any views that are displaying customer data
        app.context.set("customerId", custId);
        // Force the detail module to restart if it's already running and supply URL params
        app.detail.stop();
        app.detail.start({ tab: "infoEdit", childTab: childTab });

 
    },
    newCustomer: function () {
        pirch.instances.views.newCustomer = new pirch.views.newCustomer;
    },
    projects: function (customerId, childTab) {
        // Setting the app context will load the customer model and update any views that are displaying customer data
        app.context.set("customerId", customerId);
        // Force the detail module to restart if it's already running and supply URL params
        app.detail.stop();
        app.detail.start({ tab: "projects", childTab: childTab });
    }

};




// The app holds a region for the entire page and is also a repository for models and collections. At some point
// the app may handle its modules - for example the router might call app.newCustomer() which will start the newCustomer
// module etc.
app = new Marionette.Application();
app.addRegions({
    containerRegion: "#customers"
});

app.addInitializer(function () {

    // Encapsulate all of the data that controls layout/navigation including all vars
    // that come from the URL hash
    this.context = new Backbone.Model({
        customerId: 0,
        customer: new pirch.models.customer(),
        page: null
    });

    app.quoteLightTabHeader = function(options){
        return new pirch.views.twoTabLight({
            model: new Backbone.Model({
                options: [{
                    selected: options.tab == "projects",
                    html: "by room",
                    href: "#"
                }
                , {
                    selected: options.tab == "info",
                //    html: "by delivery",
                    html: "",

                    href: "#"
                }
                ]
            })
        })
    };


    // Add listeners to automatically update the customer when the customerId changes
    this.context.on("change:customerId", function (model) {

        var id = model.get("customerId");
        var c = new pirch.models.customer({ Id: id });
        if (id) {
            c.fetch({
                success: _.bind(function () {
                    this.context.set('customer', c);
                }, this),
                false: function() {
                    alert("Couldn't load customer "+id);
                }
            });
        }
    }, this);

    this.context.on("change:customer", function (customer) {
        // Allow modules to listen for a "change:customer" event on the app
        this.trigger("change:customer");
    }, this);

    // Start routing
    new pirch.routers.customers({ controller: pirch.controllers.customers });

    // Create customers collection and current customer
    this.customers = new pirch.collections.customers();

    // app.products is a collection of products. Since the products are required for rendering collections,
    // start loading products now and add a flag to show that it's loading. That way a projects page can
    // see that the products aren't loaded, listen to app.products and then re-render itself once the products
    // are loaded.
    this.products = new pirch.collections.products();
    this.productsLoading = true;
    this.products.fetch({
        success: _.bind(function () {
            this.productsLoading = false;
        }, this),
        error: function () {
            alert("Failed to load products");
        }
    });

    // Shortuct to current customer via app.currentCustomer()
    this.currentCustomer = function () {
        return this.context.get("customer");
    };
});

// A module can be started by the router at which point it takes over the page and renders its views.
// All views for a module can be declared within the scope of the module.
// 
///  I'm beginning to think these different pages should be different modules ... - BMW 07/02/2013
app.module('detail', function (module, app) {

    // Don't autostart with module (the default) when the parent starts. The parent is the app in this case.
    module.startWithParent = false;

    module.on("start", function (options) {

        
        // Render both headers to module.outerContainer.header and module.container.header
        function showHeader() {
            if (app.currentCustomer().get('Id') == 0) {
                return false;
            }
            module.pageHeader = new pirch.views.customerHeader({
                model: app.currentCustomer()
            });
            module.outerContainer.header.show(module.pageHeader);
            module.detailHeader = new pirch.views.twoTabLight({
                model: new Backbone.Model({
                    options: [{
                        selected: options.tab == "projects",
                        html: "PROJECTS",
                        href: "#/"+app.context.get("customerId")+"/projects"
                    }, {
                        selected: options.tab == "info",
                        html: "INFO",
                        href: "#/"+app.context.get("customerId")+"/info/about"
                    }]
                })
            });
            module.container.header.show(module.detailHeader);
        }

        //=====================================================================================================
        // Render the body which is either customer info or customer projects, depending on the options passed
        // to the start function
        //=====================================================================================================
        function showBody() {
            //=====================================================================================================
            // Tab Control is done here.  
            //=====================================================================================================
            switch (options.tab) {
                case 'projects':
                    //  Deal with customers with no projects
                    showCreateNewProject = function () {
                        module.projectsView = new pirch.views.goToProjectCreate({ model: app.currentCustomer() });
                        module.container.body.show(module.projectsView);

                    }
                    if (app.currentCustomer().getProjects() == null) {
                        showCreateNewProject();
                        break;
                    }
                    if (app.currentCustomer().getProjects().size() == 0) {
                        showCreateNewProject();
                        break;
                    }
                    //  Done dealing with customers without projects
                    pirch.instances.collections.projectsOnlyTypeProject= app.currentCustomer().getProjects();
                    module.projectsView = new pirch.views.customerProjects({
                        collection: pirch.instances.collections.projectsOnlyTypeProject,
                        model: app.currentCustomer()
                    });
                    module.container.body.show(module.projectsView);
                    break;
                case 'infoEdit':
                    self = this;  // Allows the current "this" to be referenced in callbacks.
                    this.loadEditPage = function () {
                        if (app.currentCustomer().get('Id') == 0) {
                            //  sanity check... 
                            return false;
                        }
                        //  Declare our overall layout
                        module.infoLayout = new pirch.layouts.customerDetailLayout();
                        module.container.body.show(module.infoLayout);


                        //  Now the top tab for the edit.  This is a layout
                        customerEditLayout = new pirch.layouts.customerEditLayout({ model: app.currentCustomer() });
                        module.infoLayout.primaryTabContainer.show(customerEditLayout);


                        //  Add in our collection view for emails
                        // Make an empty one if null
                        if (app.currentCustomer().get('Emails') == null) {
                            newEmail = new pirch.models.email({});
                            newEmailCollection = new pirch.collections.emails();
                            newEmailCollection.add(newEmail);
                            app.currentCustomer().set('Emails', newEmailCollection);
                        }
                        emailsCollection = app.currentCustomer().get('Emails');
                        emailEditCollectionView = new pirch.views.emailEditCollectionView({
                            collection: emailsCollection
                        });
                        customerEditLayout.emails.show(emailEditCollectionView);



                        //  Add in our collection view for phone numbers
                        if (app.currentCustomer().get('PhoneNumbers') == null) {
                            phoneNumber = new pirch.models.phoneNumber({});
                            newPhoneNumberCollection = new pirch.collections.phoneNumbers();
                            newPhoneNumberCollection.add(phoneNumber);
                            app.currentCustomer().set('PhoneNumbers', newPhoneNumberCollection);
                        }
                        phoneNumberCollection = app.currentCustomer().get('PhoneNumbers')
                        phoneNumberCollectionView = new pirch.views.phoneNumbersEditCollectionView({
                            collection: phoneNumberCollection
                        });
                        customerEditLayout.phoneNumbers.show(phoneNumberCollectionView);



                        //  And finally addresses
                        if (app.currentCustomer().get('Addresses') == null) {
                            address = new pirch.models.address({});
                            newAddressCollection = new pirch.collections.addresses();
                            newAddressCollection.add(address);
                            app.currentCustomer().set('Addresses', newAddressCollection);
                        }
                        addressCollection = app.currentCustomer().get('Addresses')


                        addressesEditCollectionView = new pirch.views.addressesEditCollectionView({
                            collection: addressCollection
                        });
                        customerEditLayout.addresses.show(addressesEditCollectionView);
                    }
                    module.on('DisplayCustomerLayout', function () {
                        self.loadEditPage();
                    });
                    module.trigger('DisplayCustomerLayout');
                    break;
                default:


                    if (app.currentCustomer().get('Id') == 0) {
                        //  sanity check...  if we have a customer id of 0 then we're not displaying info...
                        //  This shouldn't happen but I don't want to mess with events in projects so close to alpha..  -BMW
                        return false;
                    }
                    $.cookie(pirch.constants.COOKIE_ACTIVE_CUSTOMER_ID, app.currentCustomer().get('Id'));
                    //  This default is the customer info tab
                    //  Start by adding the first layout, that setsup the parent/child tabs
                    module.infoLayout = new pirch.layouts.customerDetailLayout();
                    module.container.body.show(module.infoLayout);

                    // Add in the flat data layout
                    module.customerFlatDataLayout = new pirch.layouts.customerFlatDataLayout({ model: app.currentCustomer() });
                    module.infoLayout.primaryTabContainer.show(module.customerFlatDataLayout);


                    //  input our email collection to the customer data layout
                    emailCollectionView = new pirch.views.emailCollectionView({
                        collection: app.currentCustomer().get('Emails')
                    });
                    module.customerFlatDataLayout.emails.show(emailCollectionView);

                    //  now the phone number...
                    phoneNumberCollectionView = new pirch.views.phoneNumbersCollectionView({
                        collection: app.currentCustomer().get('PhoneNumbers')
                    })
                    module.customerFlatDataLayout.phoneNumbers.show(phoneNumberCollectionView);

                    //  now the addresses...
                    addressCollectionView = new pirch.views.addressesCollectionView({
                        collection: app.currentCustomer().get('Addresses')
                    })
                    module.customerFlatDataLayout.addresses.show(addressCollectionView);

                    break;
            }
        }

        // Set up layouts and regions to place our views in
        module.outerContainer = new pirch.layouts.headerBody();
        module.container = new pirch.layouts.headerBody();

        // Render this module into the app's container
        app.containerRegion.show(module.outerContainer);

        module.outerContainer.body.show(module.container);

        // If a new customer is loaded, re-render the header and body
        app.on('change:customer', showHeader);
        app.on('change:customer', showBody);

        // If any property of the customer changes, re-render the body
        app.currentCustomer().on('change', showBody);

        showHeader();
        showBody();
    });
});

app.module('customerList', function (module, app) {
    module.startWithParent = false;


    this.salesAssociates = new pirch.collections.salesAssociates();
    var StoreCookieQueryString = $.cookie(pirch.constants.COOKIE_STORE_CODE);
    var ActiveUserCookieQueryString = $.cookie(pirch.constants.COOKIE_LAST_LOGGED_IN_USER);
    this.storeCookie = _(StoreCookieQueryString).toQueryParams()
    this.activeUserCookie = _(ActiveUserCookieQueryString).toQueryParams()
    this.salesAssociates.setStore(this.storeCookie.StoreCookieStoreCode);
    this.customers = new pirch.collections.customersByAssociate();

    module.on("start", function (options) {

        //  Start by showing the customer list.  This is the layout for this page.
        module.customerList = new pirch.layouts.customerList();
        app.containerRegion.show(module.customerList);


        //  Setup our syncs to respond to AJAX calls.
        this.salesAssociates.on('sync', function (salesAssociatesReturnValue) {
            salesAssociateView = new pirch.views.salesAssociateCollectionView({ id: "salesAssociateContainer", collection: salesAssociatesReturnValue });

            /// This is fired if one of our models is clicked.  Let re-render it up top so we show that 
            //  a new sales associate has been selected
            salesAssociatesReturnValue.on("chooseAssociate", function (associate) {
                currentAssociate = new pirch.views.currentAssociate({ model: associate });
                module.customerList.currentAssociate.show(currentAssociate);
                window.picturefill();
            });
            module.customerList.selectAssociate.show(salesAssociateView);
            window.picturefill();
        });


        this.customers.on('sync', function(customersCollection){
            
            customerListCollectionView = new pirch.views.customerListCollectionView({ collection: customersCollection });
            module.customerList.customerList.show(customerListCollectionView);

        });


        //  First the sales associates..  Go get them and trigger the sync
        this.salesAssociates.fetch();

        // Now the customers... ditto
        this.customers.setAssociate(22);  //  TODO - Update this with the active user. The backend doesn't work right now, so its Beta...
        this.customers.fetch();


    });

});



//  This is the quote module.  View and Edit are dealt with by showing/hiding classes in order to leverage as much functionality as possible.  
app.module("quote", function (module, app) {
    module.startWithParent = false;

    module.getTitle = function () {
        var customer = app.currentCustomer();
        var name = customer.get("FirstName");
        if (name) {
            return pirch.utils.ucFirst(module.state)+" " + name + "'s Proposal";
        }
        return pirch.utils.ucFirst(module.state) + " Proposal";
    }
    module.on('renderBody', function () {
        module.renderBody();
    })
    module.on('setInstanceForUpdate', function (productInstance) {
        if (module.productInstancesToUpdate.indexOf(productInstance.get('Id')) == -1) {
            module.productInstancesToUpdate.push(productInstance.get('Id'));
        }
    });

    module.on('saveQuote', function () {
        // This is our iterator.  It needs to count
        // how many posts have succeeded.  We return to view when we have
        // recieved a response from everything...
        var successfulPosts = 0;
        var neededPosts = module.productInstancesToUpdate.length + module.productInstancesToDelete.length;
        if(neededPosts == 0){
            module.goToView();
        }
        //  Fire all the posts to update....
        for (var i = 0; i < module.productInstancesToUpdate.length; i++) {

            module.productInstancesForQuote.findWhere({ Id: module.productInstancesToUpdate[i] }).save({}, {
                success: function () {
                    successfulPosts++;
                    console.log(successfulPosts + ' posts have returned.  We need ' + neededPosts);
                    if (successfulPosts == neededPosts) {
                        // We're done!
                        console.log('redirect to view quote');
                        module.goToView();
                    }
                }
            });
        }
        //  Fire the deletes....
        for (var i2 = 0; i2 < module.productInstancesToDelete.length; i2++) {
            new pirch.models.productInstance({ Id: module.productInstancesToDelete[i2] }).destroy({
                success: function () {
                    successfulPosts++;
                    console.log(successfulPosts + ' posts have returned.  We need ' + neededPosts);
                    if (successfulPosts == neededPosts) {
                        // We're done!
                        console.log('redirect to view quote');
                        module.goToView();
                    }
                }
            });
        }
    });

    module.goToView = function () {
        Backbone.history.navigate('/' + app.currentCustomer().get('Id') + '/quotes/' + module.quoteId + '/view', { trigger: true });
    }

    module.on('recalculateQuote', function () {
        module.quoteInformationLayout.trigger('recalculateQuote');
    });

    module.on('prepareForDelete', function (productInstance) {
        if (module.productInstancesToDelete.indexOf(productInstance.get('Id')) == -1) {
            module.productInstancesToDelete.push(productInstance.get('Id'));
            //  Here, we check if this product instance has been queued to update
            //  if so, remove it from the queue.
            var positionOfExistingElement = module.productInstancesToUpdate.indexOf(productInstance.get('Id'));
            
            if ( positionOfExistingElement>= 0) {
                // Consider this as a util...
                module.productInstancesToUpdate.splice(positionOfExistingElement, 1);
            }
            module.productInstancesView.collection.remove(productInstance);
        }
        module.quoteInformationLayout.trigger('recalculateQuote');
    });


    module.getQuote = function (id) {
        return app.currentCustomer().getProjects().get(id);
    }



    module.on("start", function (options) {
        module.quoteId = options.quoteId;
        module.state = options.state;
        // Set up layouts and regions to place our views in
        module.container = new pirch.layouts.twoHeaderBody();

        // Render this module into the app's container
        app.containerRegion.show(module.container);

        //module.container.headerBottom.show(module.detailHeader);
     


        this.listenTo(app, "change:customer", function () {
            module.renderBody();
        });

        //  We are changing product instances throughout this page,
        //  but we don't want to update them all the time.  Here, we keep track of which ones we
        //  need to update.  
        module.productInstancesToUpdate = [];
        module.productInstancesToDelete = [];
        module.whiteTabs = app.quoteLightTabHeader(options);

        module.renderBody();

    });

    module.renderBody = function (options) {
        //  Hey!  We're finally rendering the body for show quote!  
        //  This is a layout with four regions.  The first is empty for beta,
        // but will be a white tab.
        //  The second is basic quote data.
        //  The third is a tabbed list of rooms.
        //  Finally, we have all the product instances.

        pirch.instances.activeQuote = app.currentCustomer().get('Projects').findWhere({Id: parseInt(module.quoteId)});
        if (!pirch.instances.activeQuote) {
           //  No active quote found.  Stahp.
            return false;
        }
        var headerHelperModel = {
            model: new pirch.helpermodels.headerBar({
                title: module.getTitle()

            })
        };
        if (module.state == 'view') {
            var backWithCustomHeader = new Backbone.Model();
            backWithCustomHeader.set('title', 'View ' + app.currentCustomer().get('FirstName') + "'s Proposal");
            backWithCustomHeader.set('link', '/Customers#/'+app.currentCustomer().get('Id')+"/projects");
            module.headerBar = new pirch.views.customHeaderWithCustomLink({ model: backWithCustomHeader });
        } else {
            module.headerBar = new pirch.views.quoteEditHeaderBar(headerHelperModel);
        }
        //  First, we set the top of the page with our common header
        module.container.headerTop.show(module.headerBar);
        // put the tabs back in here....
//        module.container.headerBottom.show(module.whiteTabs);

        


        //  Next, set the layout...
        comingSoon = new pirch.views.comingSoon();
        module.quoteLayout = new pirch.layouts.quoteLayout({ state: module.state});
        module.container.body.show(module.quoteLayout);

        
        //Quote Navigation.
        var navModel  = new Backbone.Model();
        navModel.set('CustomerId', app.currentCustomer().get('Id'));
        navModel.set('QuoteId', module.quoteId);
        module.quoteNavigation = new pirch.views.projectViewQuoteNav({ model: navModel });
        module.quoteLayout.quoteNavigation.show(module.quoteNavigation);




        //  Add in our top-level quote information
        module.quoteInformationLayout = new pirch.layouts.quoteInformationLayout({model: pirch.instances.activeQuote});
        module.quoteLayout.quoteInformation.show(module.quoteInformationLayout);

        //  Notice this is a layout!  That's because it has an inner region where we need to inject customer data
        module.customerDataForQuote = new pirch.views.customerDataForQuote({ model: app.currentCustomer() });
        module.quoteInformationLayout.customerData.show(module.customerDataForQuote);

        //  These are the tabs representing different rooms

//        module.quoteLayout.roomTabs.show(comingSoon);
//        module.RoomTabsForProject = new pirch.views.RoomTabsForProject({ model: pirch.instances.activeQuote });
//        module.quoteLayout.quoteInformation.show(module.RoomTabsForProject);

        //  Finally, the product instances.  This will have to move to a more complex
        //  view post-beta, but its a basic collection view for now.
        module.productInstancesForQuote = pirch.instances.activeQuote.get('ProductInstances')
        module.on('seekTargetPrice', function (goalData, rollback) {
            module.productInstancesForQuote.trigger('seekPriceGoal', goalData);
        })
        if (module.productInstancesForQuote.size() == 0) {
            var goToModel = new Backbone.Model();
            goToModel.set('link', '/products');
            goToModel.set('message', 'Click to add products to your proposal');
            module.productInstancesView = new pirch.views.goTo({ model: goToModel });
        } else{
            module.productInstancesView = new pirch.views.productInstancesViewQuote({ collection: module.productInstancesForQuote });
        }
        module.quoteLayout.productInstances.show(module.productInstancesView);


    }

});

// Start the app now that it and its modules have been declared.
// This will call the initializer and start the router.
app.start();
