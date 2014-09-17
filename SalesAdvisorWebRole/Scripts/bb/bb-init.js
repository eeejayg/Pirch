/**
 * For instantiating something as an instance property of something else. If that thing
 * already exists, we just return the already existing instance. If not, we instantiate it.
 * 
 * string path The path of the thing to be instantiated.
 * Object classObject The class of the thing to be instantiated.
 * 
 * Example code:
 *
var bla = new Instantiator();

function testClass(field) {
    this.field = field;
};

testClass.prototype.alertField = function () {
    alert(this.field);
}

bla.instantiate("testClass.foo", testClass, "this is my field");
bla.testClass.foo.alertField();
 */

function Instantiator() {
}

Instantiator.prototype.instantiate = function (path, classObject) {
    var newArgs = [].slice.apply(arguments, [2, arguments.length]);
    // Must create objects corresponding to the path
    var aPath = path.split(".");
    var pathPart;
    var newPathObj = this;
    while ((pathPart = aPath.shift()) != undefined) {
        if (aPath.length <= 0) {
            break;
        }
        newPathObj = newPathObj[pathPart] == undefined ? (newPathObj[pathPart] = {}) : newPathObj[pathPart];
    }
    // Instantiate the object.
    if (newPathObj[pathPart] == undefined) {
        var newObj = newPathObj[pathPart] = new function () { };
        newObj.__proto__ = classObject.prototype;
        classObject.apply(newObj, newArgs);
    }
    return newPathObj[pathPart];
}

var pirch = {};
// CONSTANTS
pirch.app = {};
pirch.constants = {};
pirch.constants.COOKIE_LAST_LOGGED_IN_USER = "LastLoggedInUser";
pirch.constants.COOKIE_ACTIVE_CUSTOMER_ID = "ActiveCustomerId";
pirch.constants.COOKIE_STORE_CODE = 'StoreCookie';
pirch.constants.COOKIE_ACTIVE_CUSTOMER_ID= 'ActiveCustomerId';
pirch.constants.ProjectStatusProject = { Id: 1, Name: "Project" };
pirch.constants.ProjectStatusProposal = { Id: 2, Name: "Proposal" };
pirch.constants.ProjectStatusQuote = { Id: 3, Name: "Quote" };
pirch.constants.RoomCategories = {
    "Kitchen": 1,
    "kitchen": 1,
    "Bathroom": 2,
    "bathroom": 2,
    "Laundry": 4,
    "laundry": 4,
    "Outdoor": 3,
    "outdoor": 3
};
pirch.constants.RoomCategoryIdToName = {
    0: 'other',
    1: 'kitchen',
    2: 'bathroom', 
    3: 'outdoor',
    4: 'laundry'    
}
pirch.app = {};
pirch.data = {};
pirch.models = {};
pirch.helpermodels = {};
pirch.collections = {};
pirch.controllers = {};
pirch.views = {};
pirch.layouts = {};
pirch.routers = {};
pirch.utils = {};
pirch.config = {};
pirch.instances = new Instantiator();
pirch.instances.routers = {};
pirch.instances.models = {};
pirch.instances.views = {};
pirch.instances.collections = {};

//============= Generic Utilities====TODO should be moved to its own unit 'cause not Pirch specific====
cookie2object = function (cookieName) {
    retVal = {};
    var cookieStr = $.cookie(cookieName);
    if (!cookieStr) { return null; }
    var cookieArr = cookieStr.split("&");
    if (!cookieArr) { return null; }
    _.each(cookieArr,function(itm) {
        var itmArr = itm.split("=");
        retVal[itmArr[0]] = itmArr[1];
    })
    return retVal;
}

//============= Utilities================================================================
pirch.utils.showError = function (error) {
    alert(error);
};
//Capitalizes the first letter of the string.
pirch.utils.ucFirst = function(string){
    return string.charAt(0).toUpperCase() + string.slice(1);
}
pirch.utils.image = function (incomingOptions) {
    var options= {
        src: "",
        alt: "",
        class: "",
        id: ""
    }
    $.extend(options, incomingOptions);
 //   console.log('Be forewarned.  This image helper does not use picturefill yet.  It will.  Call window.picturefill after this is rendered. A good test is to fire an alert where you call picturefill.  If the images have already loaded, you are good.  If they havent, keep trying.'  );
    return '<img src="' + pirchGlobals.cdn + options.src + '" alt="' + options.alt.replace('"', '') + '"  class="' + options.class + '" id="' + options.class + '"/>';
}
pirch.utils.cbox = function(html){
    $.colorbox({ html: '<div class="pirchCBoxContent">'+html+'</div>' });
}
pirch.utils.toDate = function (string) {
    // Convert C# date to javascript date
    var result = string.match(/\(([0-9]+)\)/);
    if (result) {
        var t = parseInt(result[1]);
        if (_.isNaN(t)) {
            throw "Error in date string " + string;
        }
        var d = new Date(t);
        return  d.toDateString();
    }
    return null;
}

pirch.utils.queryParam = function (name, url) {
    url = url || window.location.href;
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(url);
    if (!results)
        return "";
    return results[1];
}
//  This function validates a keypress as an acceptable value to add to the money string.
//  returns true/false
pirch.utils.validateKeyPressAsMoneyValue = function (event, money) {
    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39) ||
            (event.keyCode == 190) && (money.split('.').length < 2)) {
        // let it happen, don't do anything
        //  These are utility keys.  Good to go..
        return false;
    }
    else {
        // Ensure that it is a number and stop the keypress
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
            event.preventDefault();
            return false;
        }
    }
    return true;
}
pirch.utils.ensureTwoDecimals = function (e) {
    var bits = e.target.value.split('.');
    if (bits.length < 2) {
        return;
    }
    if (bits[1].length > 2) {
        e.target.value = parseFloat(e.target.value).toFixed(2);
    }
}


pirch.utils.toMoneyString = function(rawMoney){
    //  This function returns the string formatted as money.
    if (isNaN(rawMoney)) {
        numericMoney = rawMoney.replace(/[^0-9\.]+/g, '');
    } else {
        numericMoney = rawMoney;
    }
    var roundToTwo = parseFloat(numericMoney).toFixed(2);
    var moneyString = roundToTwo.toString().replace(' ', '');
    var bits = moneyString.split('.');
    var moneyString = bits[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")+'.'+bits[1];

    if (moneyString.charAt[0] !== '$') {
        moneyString = '$' + moneyString;
    }
    return moneyString;
}
//============= end Utilities================================================================

pirch.config.testData = !!pirch.utils.queryParam("testData");

Backbone.SuperModel = Backbone.Model.extend({
    parse: function (response) {
        _.each(this.propToModel, function (model, prop) {
            if (response[prop]) {
                response[prop] = new model(response[prop], { parse: true });
            }
        });
        _.each(this.propToModelFn, function (model, prop) {
            if (response[prop]) {
                model = model(response[prop]);
                response[prop] = new model(response[prop], { parse: true });
            }
        });
        _.each(this.serverPropToClientProp, function (clientProp, serverProp) {
            response[clientProp] = response[serverProp];
        });

        return response;
    },
    superModelInit: function () {
        this._superModelInitialized = true;
        _.each(this.serverPropToClientProp, function (clientProp, serverProp) {
            this.on("change:" + clientProp, function () {
                this.set(serverProp, this.get(clientProp));
            }, this);
        }, this);
    },
    initialize: function () {
        // If you override initialize in a base class you should also call superModelInit()
        this.superModelInit();
    },
    superModelValidate: function() {
        // DEBUG: Sanity check
        if (!this._superModelInitialized) {
            console.error("This Backbone.SuperModel didn't have it's superModelInit called. If you extend the SuperModel class and override the initialize method, you need to call this.superModelInit from your custom initialize method");
        }
        _.each(this.unsetDefaults, function(data, serverProp) {
            if (this.has(serverProp)) {
                var prop = this.get(serverProp);
                if (prop instanceof Backbone.Collection) {
                    var remove = prop.filter(function (model) {
                        return (model === data || _.isEqual(model.toJSON(), data));
                    });
                    _.each(remove, function (model) {
                        prop.remove(model, { silent: true });
                    });
                } else {
                    if (prop === data || _.isEqual(data, prop)) {
                        this.unset(serverProp, { silent: true });
                    }
                }
            }
        }, this);
    },
    validate: function () {
        return this.superModelValidate();
    }
});

pirch.views.DataBoundItemView = Backbone.Marionette.ItemView.extend({
    events: {
        'change [data-param-name]': 'copyToModel'
    },
    copyToModel: function (e) {
        var parameterName = e.target.getAttribute('data-param-name')
        switch (e.target.getAttribute('type')) {
            case 'checkbox':
                this.model.set(parameterName, $(e.target).is(':checked'));
                break;
            default:
                this.model.set(parameterName, $(e.target).val());
                break;
        }
    }
});

pirch.views.empty = Backbone.View.extend({
    template: _.template("")
});

