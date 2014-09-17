pirch.models.productOption = Backbone.SuperModel.extend({});

pirch.models.categoryType = Backbone.Model.extend({
    defaults: {
        name: '',
        urlPath: '',
        image: ''
    },
    initialize: function () {
        this.set('image', "Images/categories/" + this.get('image'));
    }
});

pirch.models.productType = Backbone.SuperModel.extend({
    defaults: {
        image: '',
        Name: '',
        Manufacturer: ''
    },
    initialize: function () {
        this.set('image',this.firstImage());
    },
    firstImage: function(){
        //  I'm unsure that the path for images is going to remain the same, so 
        // this is built to return the first image.  Call it from templates, then just modify
        //  here if we need to.
        if (this.get('Images')) {
            return this.get('Images')[0]
        } else {
            return "Images/blankWhite.png"
        }
    },
    getDefaultPrice: function(){
        return this.get('ListPrice');
    },
    idAttribute: "ProductTypeGUID"
    // TODO: Add models/collections for the product options once they're defined
    /*
    propToModel: {
        OptionGroups: pirch.collections.optionGroups,
        AvailableAddOns: pirch.collections.availableAddOns
    },
    */

});

pirch.models.siteArea = Backbone.Model.extend({
    urlRoot: "/json/SiteArea",
    idAttribute: "SiteAreaId"
});

pirch.collections.siteAreas = Backbone.Collection.extend({
    url: '/json/SiteArea',
    model: pirch.models.siteArea
});

pirch.collections.products = Backbone.Collection.extend({
    url: '/json/productsgetall',
    model: pirch.models.productType,
    parse: function (response) {
        var models = [];
        _.each(response, function (product) {
            models.push(product);
        });
        return models;
    }
});

pirch.models.productInstance = Backbone.SuperModel.extend({
    urlRoot: '/json/productInstance',
    idAttribute: "Id",
    defaults: {
        productType: null,
        productOptions: [],
        FKRoom: 0,
        FKProject: 0,
        FKParentInstance: 0,
        ProductTypeGUID: null 
    },
    priceWithOptions: function () {
        // TODO: Sum the options also?
        return this.get("Price");
    },
    //  This returns the project model associated with this product instance.
    getProductType: function (val) {
        return app.products.findWhere({ProductTypeGUID: this.get('ProductTypeGUID')});
    },
    getPrice: function(){
        // TODO - This is initializing price creation.  This can all go away.
        if (this.get('Price') !== 0) {
            return parseFloat(this.get('Price'));
        } else {
            if (!this.getProductType()) {
                return '';
            }
            return parseFloat(this.getProductType().getDefaultPrice());
        }
    },
   
    initialize: function () {
        this.superModelInit();
        if (this.get('Id') > 0) {
            if (this.get('Price') == 0) {
                //  If no price has been set before (no deals), then set the price equal to the
                //  price of the product.
                this.set('Price', this.getPrice());
            }
        }
    }
    
});

pirch.collections.productInstances = Backbone.Collection.extend({
    model: pirch.models.productInstance,
    initialize: function () {
        this.on('seekPriceGoal', function (goalData) {
            this.seekPriceGoal(goalData);
        });
        this.allowUpdate = true;
    },
    getTotal: function () {
        var total = 0;
        for (var i = 0 ; i < this.size() ; i++) {
            total = total + this.at(i).get('Price');
        }
        return total;
    },
    seekPriceGoal: function (goalData) {
        //  This is where we seek the goal price.  We need to be able to rollback if we can't hit the price.
        //  A the same time, we need to update if necessary.  To do this we will....
        //  1. Setup a KVP.  This holds the Id of the model and the new price, potentially down to the minimum
        //  3.  Iterate, lower the "current quote price" as possible on each item
        //  4.  
        if (this.allowUpdate == false) {
            return; // defense against 2 events at the same time...
        }
        this.allowUpdate = false;
        console.log("rollback:  "+goalData.rollbackPrice+" goal: "+goalData.targetPrice );
        var savingsNeeded =goalData.rollbackPrice - goalData.targetPrice;
        console.log("savings needed :  " + savingsNeeded);
        var productInstanceToUpdate = [];
        var productInstancePrice = [];
        //  Our quote is higher than our pricing.  Lets just add it to the first instance for now
        if (savingsNeeded < 0) {
            var oldPrice = (this.at(0).get('Price'));
            var newPrice = (parseFloat(oldPrice) - parseFloat(savingsNeeded));
            this.at(0).set('Price', newPrice.toFixed(2));
            app.quote.trigger('setInstanceForUpdate', this.at(0));
            this.allowUpdate = true;
            return;
        }
        //  We are decrementing instances....
        var currentInstance;
        for (var i = 0; i < this.size() ; i++) {
            currentInstance = this.at(i);
            currentInstanceId = currentInstance.get('Id');
            if (savingsNeeded > 0) {
                var possibleSavings = Math.abs(currentInstance.getPrice() - currentInstance.getProductType().get('MinimumPrice'));  
                if (possibleSavings > 0) {
                    if (possibleSavings < savingsNeeded) {
                        productInstanceToUpdate.push(currentInstanceId);
                        productInstancePrice.push(currentInstance.getProductType().get('MinimumPrice'));
                        savingsNeeded = Math.abs(savingsNeeded - possibleSavings);
                        console.log('Discounted by ' + possibleSavings + ' but still ' + savingsNeeded + ' needed');
                    } else {
                        productInstanceToUpdate.push(currentInstanceId);
                        productInstancePrice.push(currentInstance.getPrice() - savingsNeeded);
                        savingsNeeded = 0;
                    }
                }
            } else {
                console.log("We made it! The price is acceptable");
                for (var i = 0; i < productInstanceToUpdate.length; i++) {
                    this.at(i).set('Price', parseFloat(productInstancePrice[i]).toFixed(2));
                    app.quote.trigger('setInstanceForUpdate', this.at(i));
                }
                //  Now, update all the models with our new prices.
                this.allowUpdate = true;
                return;
            }
        }
        pirch.utils.cbox('That price is too low.  Rolling back...');
        $('section.pricingInformation input').val(pirch.utils.toMoneyString(goalData.rollbackPrice));
       this.allowUpdate = true;
    }
    
});
