//  This is straight backbone.  TODO - Convert to Marionette.

$(function () {
    pirch.views.newCustomer = Backbone.View.extend({
        el: "#customers",
        events: {
            "click button": "save",
            "change #customerTypes input": "unselectAll",
            "click label": "selectMe"

        },
        template: _.template($('#newCustomer').html()),
        render: function() {
            $(this.el).html(this.template());
        },
        unselectAll: function(e){
            $(e.target).siblings('input').attr('checked', false);
        },
        selectMe: function (e) {
            $('div#customerTypes label').removeClass('selected');
            $(e.target).addClass('selected');
        },
        save: function () {
            if (this.allowSubmit == false) {
                return; 
            }
            this.allowSubmit = false;
            this.model = new pirch.models.customer
            this.model.on("invalid", function (model, error) {
                pirch.instances.views.newCustomer.allowSubmit = true;
                pirch.utils.showError(error);
            });


            email = new pirch.models.email();
            email.set('Address', $('#email').val());
            emCollection = new pirch.collections.emails();
            emCollection.add(email);

            phoneNumber = new pirch.models.phoneNumber();
            phoneNumber.set('PhoneNumber', $('#phone').val());
            pnCollection = new pirch.collections.phoneNumbers();
            pnCollection.add(phoneNumber);
            if($('#mobile').is(':checked')){
                phoneNumber.set('FKType', 1);
            } else  {
                phoneNumber.set('FKType', 2);
            }

            //  TODO - validation.  How are we going to validate phone number?
            this.model.save({
                FirstName: $('#firstName').val(),
                LastName: $('#lastName').val(),
                Owner: $('#Owner').is(':checked'),
                Architect: $('#Architect').is(':checked'),
                Designer: $('#Designer').is(':checked'),
                Builder: $('#Builder').is(':checked'),
                Vendor: $('#Vendor').is(':checked'),
                Realtor: $('#Realtor').is(':checked'),
                Emails: emCollection,
                PhoneNumbers: pnCollection
            },
            {
                success: function (model, data) {
                    $.cookie(pirch.constants.COOKIE_ACTIVE_CUSTOMER_ID, app.currentCustomer().get('Id'));
                    window.location = '/customers/' + data.customerID + '/projects/'
                },
                error: function () {
                    this.allowSubmit = true;
                    console.log("error")
                }

            });
        },
        initialize: function () {
            this.allowSubmit = true;
            this.render();
        }
    });



});

