pirch.models.user = Backbone.Model.extend({
    urlRoot: '/json/users',
    defaults: {
        id: null,
        firstName: '',
        lastName: '',
        userName: '',
        role: null,
        title: '',
        imagePath: '',
        __RequestVerificationToken: '',
        image: function () {
            return '/Images/people/'+firstName+'.png';
        }
    }
});



pirch.collections.salesAssociates = Backbone.Collection.extend({
    url: null,
    model: pirch.models.user,
    setStore: function (storeId) {
    //  This collection 
        this.url = '/json/salesassociatesgetbystorecode/' + storeId;
    }
});