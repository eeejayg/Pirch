//
// This grid control extends Marionette.CollectionView but adds the functionality
// of applying a grid-based layout to the child views. After adding this to a view
// it is advisable to call grid.layoutGrid() so that the grid can measure its container
// and apply the initial layout. After that it will automatically lay out its views on
// window resize.
//
// TODO: Test that iPad orientation change events trigger the re-layout the same as a
// browser window resize
//
// Options that can be passed in the constructor parameter:
//   collection: the collection to render
//   itemView: the view class to create for each item in the collection
//   gridWidth: an initial width of the grid. NOTE: If autoResize is true, this will be
//              overwritten on layout
//   cellWidth: the ideal width of each cell in the grid
//   cellHeight: the ideal height of each cell in the grid
//   fillWidth: true if the grid tiles should expand to fill the width of the grid, false if
//              cellWidth and cellHeight should be static dimensions
//   preserveAspectRatio: true if the cells should keep their aspect ratio when they are scaled
//              to fill the width of the grid
//   autoResize: true if gridWidth should be recalculated when the window size or orientation changes
//   align: if the fillWidth option is false, then align specifies how the contents should be aligned.
//              Possible values are "center", "left" or "right"
pirch.views.grid = Marionette.CollectionView.extend({
    // This class specifies 100% width and position: relative since all children will be absolute
    // positioned within the grid view
    className: "gridContainer",
    tagName: "div",

    defaultOptions: {
        gridWidth: 1000,
        cellWidth: 500,
        cellHeight: 500,
        preserveAspectRatio: true,
        fillWidth: true,
        autoResize: true,
        align: "center"         // one of "center", "left", "right"
    },

    initialize: function () {
        this.options = _.extend(this.defaultOptions, this.options);

        _.bindAll(this, "layoutGrid");
        // equivalent to
        // this.layoutGrid = _.bind(this.layoutGrid, this);

        this.on('render', this.layoutGrid, this);

        if (this.options.autoResize) {
            $(window).resize(this.layoutGrid);
        }

    },

    // Base styles to apply once to each child view
    setCellViewProps: function (view) {
        view.$el.css({
            position: "absolute",
            overflow: "hidden"
        });
    },

    // Apply styles each time the view needs to be laid out within the grid container
    positionView: function (view) {
        // Calculate cell size dynamically based on cellWidth and current gridWidth
        var index = this.collection.indexOf(view.model);
        var count = this.collection.length;
        // xc = number of cells on the horizontally
        var xc = Math.max(1, Math.floor(this.options.gridWidth / this.options.cellWidth));
        // Start with the base size
        var w = this.options.cellWidth;
        var h = this.options.cellHeight;
        var margin = 0;
        if (this.options.fillWidth) {
            // Leave margin at 0, scale width up to fill grid width
            w = this.options.gridWidth / xc;
        } else {
            // Don't resize cells but add margin
            margin = this.options.gridWidth - this.options.cellWidth * xc;
            switch (this.options.align) {
                case "left":
                    margin = 0;
                    break;
                case "right":
                    break;
                case "center":
                    margin = Math.floor(margin * 0.5);
                    break;
                default:
                    throw "Invalid grid alignment option: " + this.options.align;
                    break;
            }
        }

        if (this.options.preserveAspectRatio) {
            // Scaling the width requires the height to be scaled too
            h *= w / this.options.cellWidth;
        }

        var column = index % xc;
        var row = (index - column) / xc;
        view.$el.css({
            top: h * row,
            left: w * column + margin
        });

        view.$el.width(w);
        view.$el.height(h);

        // Return the height of the grid required for this cell to be visible
        return (row + 1) * h;
    },

    buildItemView: function (item, ItemViewType, itemViewOptions) {
        var grid = this;
        var options = _.extend({ model: item }, itemViewOptions);

        // Merge view's existing template helpers with grid template helpers
        //options.templateHelpers = _.extend({}, options.templateHelpers || {}, this.templateHelpers);

        var view = new ItemViewType(options);
        view.on('render', function () {
            grid.setCellViewProps(view);
            grid.positionView(view);
        });

        return view;
    },

    layoutGrid: function () {
        if (this.options.autoResize) {
            this.options.gridWidth = this.$el.width();
        }
        var h = 0;
        this.children.each(function (view) {
            h = Math.max(h, this.positionView(view));
        }, this);
        // Resize to fit contents
        if (h > this.$el.height()) {
            this.$el.height(h);
        }
    }
});
