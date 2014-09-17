$(function () {
    pirch.views.chooseRooms = Backbone.View.extend({
        el: "#projects",
        template: _.template($('#BB_TMP_chooseRooms').html()),
        events: {
            "click a.increment": "increment",
            "click a.decrement": "decrement",
            "click a#submitRooms": "submitRooms"
        },
        decrement: function (e) {
            e.preventDefault();
            var rcId= $(e.target).attr('data-rooms-category-id');
            this.model.decrement(rcId);
            this.updateRoomCount(rcId);
            if (this.model.totalRooms() == 0) {
                $('#submitRooms').addClass("hidden");
            }
        },
        increment: function (e) {
            e.preventDefault();
            var rcId = $(e.target).attr('data-rooms-category-id');
            this.model.increment(rcId);
            this.updateRoomCount(rcId);
        },
        initialize: function () {
            this.model = pirch.instances.models.newProject;
            this.render();
        },
        render: function () {
            $(this.el).html(this.template());
            window.picturefill();
        },
        submitRooms: function (e) {
            this.model.set('Name', 'Your Collection');
            this.model.save();
            window.location = '/customers';
            e.preventDefault();
        },
        updateRoomCount: function (rcId) {
            //  Room represents the id of the DOM element representing the room.
            var roomCount = this.model.getRoomCount(rcId);
            var friendlyText = '';
            var targetRoom = $('#'+pirch.constants.RoomCategoryIdToName[rcId]);
            if (roomCount > 0) {
// Post Beta                friendlyText = roomCount + " " + this.model.pluralOrSingular(rcId)
// Post Beta                targetRoom.find('a.increment').html('+');
                targetRoom.removeClass('off');
                $('#submitRooms').removeClass("hidden");
            } else {
// Post Beta                friendlyText = this.model.pluralOrSingular(rcId)
                targetRoom.addClass('off');
            }
// Post Beta            targetRoom.find('p').html(friendlyText);
        }
    });
});