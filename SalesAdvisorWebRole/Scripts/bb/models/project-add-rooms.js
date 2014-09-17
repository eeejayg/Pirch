pirch.helpermodels.ProjectCreationWithRooms = Backbone.Model.extend({
    defaults: {
        allowSubmit: true,
        customerID: null,
        projectType: null,
        dateComplete: null,
        kitchens: 0,
        bathrooms: 0,
        laundry: 0,
        outdoor:0
    },
    increment: function (rcId) {
        //  This should be passed the room category id 
        room = this.getModelNameByRCId(rcId)
        var newVal = this.get(room);
        if (newVal == 1) {
            //  Only allow a single product
            //  anything after this point needs to be re-examined post beta1, including the friendlynames.
            return false;
        }
        newVal++;
        this.set(room, newVal);
        return newVal;
    },
    decrement: function (rcId) {
        room = this.getModelNameByRCId(rcId)
        var newVal = this.get(room);
        if (newVal == 0) {
            return 0;
        }
        newVal--;
        this.set(room, newVal);
        console.log("new val of " + room + " is " + newVal);
        return newVal
    },
    getDataName:function(fn){
        //  This function accepts a "friendly name", which is from the database
        //  and used to display.  We then return the corresponding
        //  data name
        switch (fn) {
            case "Kitchen":
                return 'kitchens';
                break;
            case "Bathroom":
                return "bathrooms"
                break;
            case "Laundry":
                return "laundry";
                break;
            case "Outdoor":
                return "outdoor";
                break;
            default:
                console.log('We missed a type of increment: '+fn);
                break;
        }
    },
    totalRooms: function(){
        return (this.get('outdoor') + this.get('laundry') + this.get('bathrooms') + this.get('kitchens'));
    },
    pluralOrSingular: function (rcId) {
        //  This return plural or singluar text, such as as "Bathrooms" or "Bathroom"
        dataName = this.getModelNameByRCId(rcId);
        var roomCount = this.get(dataName);
        if (roomCount  >1 ) {
            return this.plurals[dataName];
        }
        return this.singular[dataName];
    },
    setCustomer: function(customerID){
        this.set("customerID", customerID);
        this.urlRoot = '/customers/'+customerID+'/projects'
    },
    singular: {
        "kitchens": "Kitchen",
        "bathrooms": "Bathroom",
        "laundry": "Laundry",
        "outdoor": "Outdoor"
    },
    plurals: {
        "kitchens": "Kitchens",
        "bathrooms": "Bathrooms",
        "laundry": "Laundries",
        "outdoor": "Outdoors" 
    },
    getRoomCount: function (rcId) {
        return this.get(this.getModelNameByRCId(rcId));
    },
    getModelNameByRCId: function (rcId) {
        var nameOfCategory = pirch.constants.RoomCategoryIdToName[rcId];
        if (nameOfCategory == 'kitchen') {
            return 'kitchens';
        }
        if (nameOfCategory == 'bathroom') {
            return 'bathrooms';
        }
        return nameOfCategory;
    },
    validate: function (attrs) {
          
    }
});