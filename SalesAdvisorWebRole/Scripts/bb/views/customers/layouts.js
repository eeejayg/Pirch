

pirch.layouts.customerList= Marionette.Layout.extend({
    template: '#customerListLayout',

    regions: {
        currentAssociate: "#currentSalesAssociate",
        selectAssociate: "#selectAssociate",
        customerList: "#customerListView"
    }
});


pirch.layouts.customerDetailLayout = Marionette.Layout.extend({
    template: '#customerDetailLayout',
    regions: {
        primaryTabContainer: "#primaryTabContainer",
        secondaryTabControl: "#secondaryTabControls",
        secondaryTabContainer: "#childTabContainer"
    }
});


pirch.layouts.customerFlatDataLayout = Marionette.Layout.extend({
    template: '#customerFlatDataDetail',
    regions: {
        phoneNumbers: "#displayPhoneNumbers",
        emails: "#displayEmails",
        addresses: "#displayAddresses"
    }
});

pirch.layouts.customerEditLayout = Marionette.Layout.extend({
    template: '#customerFlatDataEdit',
    regions: {
        phoneNumbers: "#phoneNumbersEdit",
        emails: "#emailsEdit",
        addresses: "#addressesEdit"
    },
    events: {
        "click button": "saveCustomer"
    },
    saveCustomer: function (e) {
        this.model.save(null, {
            success: function(){
                pirch.utils.cbox('saved!');

                app.currentCustomer().fetch({
                    success: function () {
                        app.detail.trigger('DisplayCustomerLayout');
                    }
                });
                
            }
        });
    }
});


pirch.layouts.quoteLayout = Marionette.Layout.extend({
    template: '#quoteLayout',
    tagName: 'section',
    className: function (options) {
        return "quote "+this.options.state;
    },
    regions: {
        quoteMenu: "#quoteMenu",
        quoteNavigation: "#quoteNavigation",
        quoteInformation: "#quoteInformation",
        roomTabs: "#roomTabs",
        productInstances: "#productInstances"
    }
});


pirch.layouts.quoteInformationLayout = Marionette.Layout.extend({
    template: '#quoteInformationLayout',
    regions: {
        customerData: "#customerData"
    },
    events: {
        'keydown :input': 'changePrice',
        'keyup :input': 'setTwoDecimals',
        'focus :input': 'setPreChangePrice',
        'blur :input': 'validateMoney'
    },
    changePrice: function (event) {
        if (event.keyCode == 13) { // ENTER
            $(event.target).blur();
            return;
        }
        pirch.utils.validateKeyPressAsMoneyValue(event, event.target.value);
    },
    templateHelpers: {
        roomsCategoryNames: function () {
            //TODO -RoomsCategory goes here
            return "";
        },
        priceTotal: function () {
            return self.priceTotal();
        }
    },
    priceTotal: function(){
        var totalPrice = 0;
        for (var i = 0 ; i < this.model.get('ProductInstances').size() ; i++) {
            totalPrice = totalPrice + parseFloat(this.model.get('ProductInstances').at(i).get('Price'));
        }
        return totalPrice;

    },
    setPreChangePrice: function (e) {
        this.preChangeValue = e.target.value.replace('$', '').replace(',','');
        e.target.value = this.preChangeValue;
    },
    setTwoDecimals: function (e) {
        pirch.utils.ensureTwoDecimals(e);
    },
    validatePriceChange: function (e) {
        app.quote.trigger('seekTargetPrice', {
            targetPrice: e.target.value,
            rollbackPrice: this.preChangeValue
        });
    },
    validateMoney: function (e) {
        this.validatePriceChange(e);
        this.$el.find('input').val(pirch.utils.toMoneyString(this.$el.find('input').val()));
        
    },
    initialize: function () {
        self = this;
        this.on('recalculateQuote', function () {
            this.$el.find('input').val(pirch.utils.toMoneyString(this.priceTotal()));
        });
    },

   
});