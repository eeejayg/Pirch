
pirch.models.address = Backbone.Model.extend({
    urlRoot: '/json/address',
    idAttribute: "Id",
    defaults: {
        Id: null,
        AddressLine1: "",
        AddressLine2: null,
        AddressLine3: null,
        PostalCode: "",
        City: "",
        State: "CA",
        Country: "USA"
    }
});

pirch.collections.addresses = Backbone.Collection.extend({
    model: pirch.models.address
});


pirch.models.room = Backbone.Model.extend({
    idAttribute: "Id"
});



pirch.collections.rooms = Backbone.Collection.extend({
    model: pirch.models.room
});

//===================================================================================================
// About productCollections
//===================================================================================================
//
// Projects, quotes and proposals all have two things:
//   - a list of rooms
//   - a list of product instances
//
// A product instance can optionally have a room ID
//
// A product collection is a client-side data-model that groups the product instances
// into named groups. Each of the product instances in these groups shares the same
// room ID, which could be null if the product instances are not associated with any
// room.
//
// Since the productCollections are client side and associated with a project, quote or
// proposal, the projectBase class has an updateProductCollections method that groups
// its product instances into product collections. In the future the updateProductCollections
// method will need to be called when any of the collections or rooms change so that the
// projects productCollection stays in sync with the server side models.
//
//===================================================================================================
pirch.models.productCollection = Backbone.SuperModel.extend({
    defaults: {
        name: "Misc",
        productInstances: null
    },

    // Calculate the price for this collection, including all productInstances
    calcPrice: function () {
        var price = 0;
        this.get("productInstances").each(function (productInstance) {
            price += productInstance.priceWithOptions();
        });
        return price;
    }
});

pirch.collections.productCollections = Backbone.Collection.extend({
    model: pirch.models.productCollection
});

pirch.models.projectBase = Backbone.SuperModel.extend({
    idAttribute: "Id",
    defaults: {
        Name: 'Your Collection',
        Rooms: new pirch.collections.rooms(),
        ProductInstances: new pirch.collections.products(),
        ProductCollections: new pirch.collections.productCollections(),

    },

    propToModel: {
        "Rooms": pirch.collections.rooms,
        "ProductInstances": pirch.collections.productInstances
    },

    viewHelpers: {
        deadlineString: function () {
            return pirch.utils.toDate(this.Deadline);
        },
        totalPrice: function () {
            return this.calcPrice();
        }

    },

    // Calculate the price for this collection, including all productInstances
    calcPrice: function () {
        var price = 0;
        this.get("ProductInstances").each(function (productInstance) {
            price += productInstance.priceWithOptions();
        });
        return price;
    },

    updateProductCollections: function() {
        var collections = this.get("ProductCollections");
        var rooms = this.get("Rooms");

        var miscCollection = [];
        var roomCollections = {};

        var instances = this.get('ProductInstances');

        instances.each(function (instance) {
            var room = instance.get("FKRoom");
            var c;
            if (room) {
                c = roomCollections[room] || [];
                roomCollections[room] = c;
            } else {
                c = miscCollection;
            }
            c.push(instance);
        });

        if (miscCollection.length) {
            // Add a misc collection
            collections.add(new pirch.models.productCollection({
                name: "Misc",
                productInstances: miscCollection
            }));
        }

        _.each(roomCollections, function (collection, room) {
            room = rooms.get(room);
            var name = room.get('Name');
            if (!name) {
                var cat = room.get('RoomCategory');
                if (cat) {
                    name = cat.Name;
                }
                if (!name) {
                    name = "Unknown Room";
                }
            }
            collections.add(new pirch.models.productCollection({
                name: name,
                room: room,
                productInstances: collection
            }));
        });
    },

    getProductCollections: function() {
        return this.get("ProductCollections");
    },

    initialize: function () {
        // Base class
        this.superModelInit();

        this.viewHelpers.totalPrice = _.bind(this.viewHelpers.totalPrice, this);

        var collections = new pirch.collections.productCollections();
        this.set("ProductCollections", collections);

        this.updateProductCollections();
    },

    projectStatusForClass: function() {
        throw "Derived class must define this function to return the correct ProjectStatus type for itself";
    },

    validate: function () {
        this.set("ProjectStatus", this.projectStatusForClass(), { silent: true });
    },

    generateProposal: function () {
        var obj = new pirch.models.proposal(this.toJSON());
        obj.unset("Id");
        obj.set("FKParentProject", this.id);
        obj.validate();
        return obj;
    },

    generateQuote: function () {
        var obj = new pirch.models.quote(this.toJSON());
        obj.unset("Id");
        obj.set("FKParentProject", this.id);
        obj.validate();
        return obj;
    }
});


pirch.models.email = Backbone.SuperModel.extend({
    defaults: {
        Id: null,
        Address: ''
    }
});


pirch.models.phoneNumber = Backbone.SuperModel.extend({
    defaults: {
        Id: null,
        PhoneNumber: ''
    }
});



pirch.collections.emails = Backbone.Collection.extend({
    model: pirch.models.email
});

pirch.collections.phoneNumbers = Backbone.Collection.extend({
    model: pirch.models.phoneNumber
});


pirch.models.project = pirch.models.projectBase.extend({
    urlRoot: "json/project",

    projectStatusForClass: function() {
        return pirch.constants.ProjectStatusProject;
    }
});


pirch.models.proposal = pirch.models.projectBase.extend({
    urlRoot: "/json/proposal",

    projectStatusForClass: function () {
        return pirch.constants.ProjectStatusProposal;
    }
});


pirch.models.quote = pirch.models.projectBase.extend({
    urlRoot: "/json/quote",

    projectStatusForClass: function () {
        return pirch.constants.ProjectStatusQuote;
    }
});


// This is a polymorphic collection of projects, proposals and quotes
pirch.collections.projects = Backbone.Collection.extend({
    model: function (attrs, options) {
        switch (attrs.ProjectStatus.Id) {
            case pirch.constants.ProjectStatusProject.Id:
                return new pirch.models.project(attrs, options);
            case pirch.constants.ProjectStatusProposal.Id:
                return new pirch.models.proposal(attrs, options);
            case pirch.constants.ProjectStatusQuote.Id:
                return new pirch.models.quote(attrs, options);
        }
        throw "Unrecognized project type " + attrs.ProjectStatus.Name;
    }
});





pirch.models.customer = Backbone.SuperModel.extend({
    urlRoot: '/json/customers',
    idAttribute:"Id",
    defaults: {
        Id: 0,
        allowSubmit: true,
        FirstName: '',
        LastName: '',
        Addresses: pirch.collections.addresses,
        Emails: pirch.collections.emails,
        PhoneNumbers: pirch.collections.phoneNumbers,
        Projects: new pirch.collections.projects(),
        image: '',
        __RequestVerificationToken: '',
    },
    propToModel: {
        "Addresses": pirch.collections.addresses,
        "Emails": pirch.collections.emails,
        "PhoneNumbers": pirch.collections.phoneNumbers
    },
    propToModelFn: {
        "Projects": function (data) {
            return pirch.collections.projects;
        }
    },
    unsetDefaults: {
        //  This methodology was creating FOUCs.  I removed it for the time being and are gracefully dealing with the null posts server-side.  I don't know if there's another reason
        //  to une this. I left it in case we need it for something else.  - BMW
//        "Addresses": pirch.collections.addresses.prototype.model.prototype.defaults,
//        "Emails": pirch.collections.emails.prototype.model.prototype.defaults,
//        "PhoneNumbers": pirch.collections.phoneNumbers.prototype.model.prototype.defaults
    },
    validate: function (attrs) {
        if (!attrs.FirstName && !attrs.LastName) {
            return "First or Last Name is mandatory";
        }
        return this.superModelValidate();
    },
    customerTypes: function () {
        return ['Owner', 'Architect', 'Designer', 'Builder', 'Vendor', 'Realtor']
    },
    getProjects: function () {
        return this.get("Projects");
    },

    getProjectsByParent: function (parentId, statusId) {
        //  This will only get one level of parent.  Fine for beta.
                return this.get("Projects").filter(function (projectBase) {
            return projectBase.get('FKParentProject') == parentId && (!statusId || projectBase.get('ProjectStatus').Id == statusId);
        });
    },

    getProjectsArray: function () {
        return this.get("Projects").filter(function (projectBase) {
            return projectbase.get('ProjectStatus').Id == pirch.constants.ProjectStatusProject.Id;
        });
    },
    getProposalsArray: function () {
        return this.get("Projects").filter(function (projectBase) {
            return projectbase.get('ProjectStatus').Id == pirch.constants.ProjectStatusProposal.Id;
        });
    },
    getQuotesArray: function () {
        return this.get("Projects").filter(function (projectBase) {
            return projectbase.get('ProjectStatus').Id == pirch.constants.ProjectStatusQuote.Id;
        });
    },
    returnRole: function () {
        var customerTypes = this.customerTypes();
        for (var i = 0; i < customerTypes.length; i++) {
            if (this.get(customerTypes[i])) {
                return customerTypes[i];
            }
        }
        return "";
     }
});

pirch.collections.customers = Backbone.Collection.extend({
    model: pirch.models.customer,
    url: '/json/customers',
});

pirch.collections.customersByAssociate = Backbone.Collection.extend({
    model: pirch.models.customer,
    url: '/json/customersbyassociate',
    setAssociate: function (associateId) {
        this.url = '/json/customersbyassociate/'+associateId;
        this.associateId = associateId;
    }

});


