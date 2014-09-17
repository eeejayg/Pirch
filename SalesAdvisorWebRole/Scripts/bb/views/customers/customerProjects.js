pirch.views.customerQuote = Marionette.ItemView.extend({
    template: _.template($("#customerDetailQuote").html()),
    events: {
        "click .tempDelete": function (event) {
            this.model.trigger("quote:delete", this.model.id);
            return false;
        },
        "click": function (event) {
            this.model.trigger("quote:click", this.model.id);
            return false;
        }
    },
    templateHelpers: {
        getTotal: function () {
            if (self.model.get('ProductInstances').size()==0) {
                return "0";
            }
            return self.model.get('ProductInstances').getTotal();
        }
    },
    initialize: function () {
        self = this;
    }
});

pirch.views.customerQuotes = Marionette.CompositeView.extend({
    itemView: pirch.views.customerQuote,
    template: function (data) {
        if (data.items.length > 0) {
            return _.template($("#customerDetailQuotes").html());
        } else {
            return _.template($("#customerDetailQuotesEmpty").html());
        }
    },
    serializeData: function () {
        var data = {};

        if (this.model) {
            data = this.model.toJSON();
        } else {
            data = { items: this.collection.toJSON() };
        }

        return data;
    },
    events: {
        "click .newQuote": function () {
            this.collection.trigger("quote:create");
        }
    }
});

pirch.views.customerProposal = Marionette.ItemView.extend({
    template: _.template($("#customerDetailProposal").html()),

    events: {
        "click .tempDelete": function (event) {
            this.model.trigger("proposal:delete", this.model.id);
        }
    }
});

pirch.views.customerProposals = Marionette.CompositeView.extend({
    itemView: pirch.views.customerProposal,
    className: "pagePadding",
    template: function (data) {
        if (data.items.length > 0) {
            return _.template($("#customerDetailProposals").html());
        } else {
            return _.template($("#customerDetailProposalsEmpty").html());
        }
    },
    serializeData: function () {
        var data = {};

        if (this.model) {
            data = this.model.toJSON();
        } else {
            data = { items: this.collection.toJSON() };
        }

        return data;
    },
    events: {
        "click .newProposal": function () {
            this.collection.trigger("proposal:create");
        }
    }
});

pirch.views.customerDetailCollection = Marionette.ItemView.extend({
    template: _.template($("#customerDetailCollection").html()),

    templateHelpers: {
        getProduct: function (id) {
            return app.products.get(id);
        },
        getRoomAddImage: function (room) {
            switch (room.get("RoomCategory").Name) {
                case "Laundry":
                    return "Images/laundry.jpg";
                case "Kitchen":
                    return "Images/kitchen.jpg";
                case "Bathroom":
                    return "Images/bathroom.jpg";
                case "Outdoor":
                    return "Images/outdoor.jpg";
            }
            return null;
        }
    },

    events: {
        "click .viewAll": function () {
            alert("Route to view all collections page");
        }
    }
});

pirch.views.customerDetailCollectionContainer = Marionette.CollectionView.extend({
    itemView: pirch.views.customerDetailCollection
});

pirch.views.customerProject = Marionette.Layout.extend({
    template: _.template($('#customerProject').html()),

    templateHelpers: function() {
        return this.model.viewHelpers;
    },

    regions: {
        tab: ".tabContainer",
        tabContent: ".tabContentContainer"
    },
    viewQuote: function(id){
        var customer = app.currentCustomer();
        //var project = customer.getProjects().get(id);
        Backbone.history.navigate('/' + customer.id + '/quotes/' + id + '/view', { trigger: true });
    },
    editQuote: function(id) {
        var customer = app.currentCustomer();
        //var project = customer.getProjects().get(id);
        Backbone.history.navigate('/' + customer.id + '/quotes/' + id + '/edit', { trigger: true });
    },

    deleteProposalOrQuote: function (id) {
        app.currentCustomer().getProjects().get(id).destroy({
            success: _.bind(function () {
                this.tabChanged();
            }, this)
        })
    },

    createProposal: function () {
        var proposal = this.model.generateProposal();
        // Workaround for oversized post data
        var proposalForPosting = proposal.clone();
        proposalForPosting.unset("Id");
        proposalForPosting.unset("ProjectStatus");
        proposalForPosting.unset("ProductCollections");
        proposalForPosting.unset("ProductInstances");
        proposalForPosting.unset("ProjectType");
        proposalForPosting.unset("Rooms");
        // Save the skinny proposal
        var x = proposalForPosting.save({}, {
            parse: false,
            success: _.bind(function (model, response, options) {
                // Add the real proposal into the customer's projects and update the UI
                proposal.set("Id", parseInt(response.Id));
                app.currentCustomer().getProjects().add(proposal);
                this.tabChanged();
            }, this)
        });
    },

    createQuote: function () {
        var quote = this.model.generateQuote();
        // Workaround for oversized post data
        var quoteForPosting = quote.clone();
        quoteForPosting.unset("Id");
        quoteForPosting.unset("ProjectStatus");
        quoteForPosting.unset("ProductCollections");
        quoteForPosting.unset("ProductInstances");
        quoteForPosting.unset("ProjectType");
        quoteForPosting.unset("Rooms");
        quoteForPosting.set("Name", "Your Proposal");  //  Quotes have been changed to Proposals.
        quoteForPosting.set("CustomerId", app.currentCustomer().get('Id'));
        quoteForPosting.set("FKStatus", 3); // This is a quote Id.  It should be from a global somewhere...
        // Save the skinny quote
        var x = quoteForPosting.save({}, {
            parse: false,
            success: _.bind(function (model, response, options) {
                // Add the real quote into the customer's projects and update the UI
                quote.set("Id", parseInt(response.Id));
                app.currentCustomer().getProjects().add(quote);
                this.tabChanged();
            }, this)
        });
    },

    tabChanged: function (i) {
        if (_.isNumber(i)) {
            this.selectedTab = i;
        } else {
            this.selectedTab = this.selectedTab || 0;
        }

        var collections;
        var projects;
        var view;

        switch (this.selectedTab) {
            //  This is the collections tab
            case 0:
//                collections = this.model.getProductCollections();
//                view = new pirch.views.customerDetailCollectionContainer({
//                    collection: collections
//                });
                view = new pirch.views.comingSoon();
                break;
            case 1:
                //  This is the proposals tab
//                projects = app.currentCustomer().getProjectsByParent(this.model.id, pirch.constants.ProjectStatusProposal.Id);
//                view = new pirch.views.customerProposals({
//                    collection: new Backbone.Collection(projects)
//                });
//                view.listenTo(view.collection, "proposal:create", _.bind(this.createProposal, this));
//                view.listenTo(view.collection, "proposal:delete", _.bind(this.deleteProposalOrQuote, this));
//                view = new pirch.views.comingSoon();
                view = new pirch.views.comingSoon();
                break;
            case 2:
                //  This is the quotes 
                projects = app.currentCustomer().getProjectsByParent(this.model.id, pirch.constants.ProjectStatusQuote.Id);
                view = new pirch.views.customerQuotes({
                    collection: new Backbone.Collection(projects)
                });
                view.listenTo(view.collection, "quote:create", _.bind(this.createQuote, this));
                view.listenTo(view.collection, "quote:delete", _.bind(this.deleteProposalOrQuote, this));
                view.listenTo(view.collection, "quote:click", _.bind(this.viewQuote, this));

                break;
            default:
                //  This is the orders tab
                view = new pirch.views.comingSoon();
                //collections = new Backbone.Collection([]);
                break;
        }

        this.tabContent.close();
        this.tabContent.show(view);
    },

    initialize: function () {
        this.tabBar = new pirch.views.tabBar({
            model: new Backbone.Model({
                tabs: [{
                    label: "COLLECTION"
                }, {
                    label: "CONCEPTS"
                }, {
                    label: "PROPOSALS"
                }, {
                    label: "ORDERS"
                }]
            })
        });

        this.listenTo(this.tabBar, "tabbar:select", this.tabChanged, this);

        this.on("render", function () {
            this.tab.show(this.tabBar);
            this.tabChanged(this.selectedTab);
        }, this);
    }
});

pirch.views.customerProjects = Marionette.CollectionView.extend({
    getItemView: function (item) {
        if (item.get('ProjectStatus').Id == pirch.constants.ProjectStatusProject.Id) {
            return pirch.views.customerProject;
        }
        return pirch.views.empty;
    }
});

pirch.views.customerHeader = Marionette.ItemView.extend({
    template: _.template($('#customerHeader').html()),

    initialize: function() {
        _.bindAll(this);
    }
});


pirch.views.customHeaderWithCustomLink = Marionette.ItemView.extend({
    template: _.template($('#customHeaderWithCustomLink').html()),

    initialize: function () {
        _.bindAll(this);
    }

});
pirch.views.goToProjectCreate = Marionette.ItemView.extend({
    template: _.template($("#goToProjectCreate").html()),
});

pirch.views.comingSoon = Marionette.ItemView.extend({
    template: _.template($("#comingSoon").html()),
    className: "pagePadding"
});


/*
// TODO doesn't work yet.  Unprioritizing because we can't create the name
    
pirch.views.RoomTabForProject = Marionette.ItemView.extend({
    template: _.template($("#RoomTabForProject").html()),
    tagNam
    +e: 'li'
});

pirch.views.RoomTabsForProject = Marionette.CollectionView.extend({
    // TODO doesn't work yet.  Unprioritizing
}) */


pirch.views.productInstanceViewQuote = Marionette.ItemView.extend({
    template: _.template($("#ProductInstanceForQuote").html()),
    tagName: 'li',
    className: "classNameHereForTabs",
    changePrice: function (event) {
        if (event.keyCode == 13) { // ENTER
            $(event.target).blur();
            return;
        }
        pirch.utils.validateKeyPressAsMoneyValue(event, event.target.value);
    },
    deleteProductInstance: function(e){
        e.preventDefault();
        app.quote.trigger('prepareForDelete', this.model);
    },
    events: {
        'click a.deleteProductInstance': 'deleteProductInstance',
        'keydown :input': 'changePrice',
        'keyup :input': 'setTwoDecimals',
        'focus :input': 'setPreChangePrice',
        'blur :input': 'validateAndFormatMoney'

    },
    setPreChangePrice: function (e) {
        this.preChangeValue = this.model.get('Price');
        e.target.value = this.preChangeValue.toFixed(2);
    },
    setTwoDecimals:function(e){
        pirch.utils.ensureTwoDecimals(e);
    },
    templateHelpers: {
        getImage: function () {
            return self.model.getProductType().firstImage();
        },
        getDescription: function () {
            return self.model.getProductType().get('Description');
        },
        getName: function () {
            return self.model.getProductType().get('Name');
        },
        getPrice: function () {
            return self.model.getPrice();
        },
        getManufacturerName: function(){
            return self.model.getProductType().get('Manufacturer').Name;
        }
    },
    validateAndFormatMoney: function (e) {
        this.validatePriceChange(e);
        $(e.target).val(pirch.utils.toMoneyString(e.target.value));
    },
    // This validates a price change for the event
    // Are we happy, or do we need to rollback?
    validatePriceChange: function (event) {
        if (parseFloat(event.target.value) < this.model.getProductType().get('MinimumPrice')) {
            pirch.utils.cbox(pirch.utils.toMoneyString(event.target.value) + " is below the minimum price of " + pirch.utils.toMoneyString(this.model.getProductType().get('MinimumPrice')) + ".  <br/><br />We are rolling back to the last acceptable price of " + pirch.utils.toMoneyString(this.preChangeValue));
            //  we rolled back, so we're done here!
            event.target.value = this.model.get('Price');
            return;
        }
        // Now, re-calculate the total price of the quote.

        this.model.set('Price', parseFloat(event.target.value));
        app.quote.trigger('setInstanceForUpdate', this.model);
        app.quote.trigger('recalculateQuote');

    },
    initialize: function (options) {
        this.model = options.model;
        //  Enables self from the template helpers.  TODO - Why didn't bind work here with templatehelpers?
        self = this;
        this.model.on('change:Price', function () {
            this.$el.find('input').val(pirch.utils.toMoneyString(this.model.get('Price')));
        }, this);
    }
});
pirch.views.productInstancesViewQuote = Marionette.CollectionView.extend({
    itemView: pirch.views.productInstanceViewQuote,
    tagName: 'ul'  
});

pirch.views.projectViewQuoteNav = Marionette.ItemView.extend({
    template: _.template($("#projectViewQuoteNav").html()),     
});

pirch.views.customerDataForQuote = Marionette.ItemView.extend({
    template: _.template($("#customerDataForQuote").html()),
    templateHelpers: {
        showRoles: function () {
            var role = self.model.returnRole();
            if (role.length > 0) {
                return ", " + role;
            }
            return "";
        }
    },
    initialize: function () {
        self = this;
    }
});
pirch.views.quoteEditHeaderBar = Marionette.ItemView.extend({
    template: _.template($('#quoteEditHeaderBar').html()),
    events: {
        'click a.cancel': function(e){
            e.preventDefault();
            // We've updated our model with good change data, but we don't want to save it.
            // Because the cancel is a single page app, we need to re-collect the customers object
            app.currentCustomer().fetch({
                success: function () {
                    Backbone.history.navigate('/' + app.currentCustomer().get('Id') + '/quotes/' + app.quote.quoteId + '/view', { trigger: true });
                }
            });
        },
        'click a.done': function(e){
            e.preventDefault();
            app.quote.trigger('saveQuote');
        }
    }
});

pirch.views.goTo = Marionette.ItemView.extend({
    template: _.template($("#goToTemplate").html())
});


pirch.views.headerWithBackAndCustomText = Marionette.ItemView.extend({
    template: _.template($('#headerWithBackAndCustomText').html()),
    events: {
        'click a': 'goBack'
    },
    goBack: function(e){
        Backbone.history.history.back();
        e.preventDefault();
    },
    initialize: function () {
        _.bindAll(this);
    }
});