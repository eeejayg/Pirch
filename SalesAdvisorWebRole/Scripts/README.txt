////////////////////////////////////////////////////////////////////////////////////////
//   Pirch Javascript Readme
//   This is intended to help people understand what we've done with the javascript on this
//   site, learn how its organized, and generally find the stuff you're looking for.
//  
//   With a little luck, we'll be able to stay standard and easily organize our code.  
//
//   Happy Coding,
//   Bryan Wokich
////////////////////////////////////////////////////////////////////////////////////////

In the bb-init file, we create javascript objects for views, models, controllers and routers.  Theres another for instances.  Everything used for backbone should be attached to this object.

As a general rule, one router is tied to one ASP.NET MVC controller.  The main page of the controller is loaded, then the rest of the controller is effectively a single page app through backbone.  

Each action from the MVC controller has a main backbone view.  There is one file per these actions.  There may be sub-views inside of these files, but we should be in a one-to-one relationship between ASP.NET MVC actions and files in /Scripts/bb/views.

Underscore templates are created by ASP.NET MVC partials.  These are rendered in the index of the controller, so they're available throughout the backbone-single-page system.

All the javascript files for single controller should be zipped up in a bundle in AppStart.  One bundle per ASP.NET MVC Controller.
