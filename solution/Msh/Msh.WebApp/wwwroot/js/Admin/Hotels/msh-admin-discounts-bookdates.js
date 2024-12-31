(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var routes = app.routes;
    var itemDatesService = app.itemDatesService;

    itemDatesService.init({
        datesApiAction: 'DiscountBookDates',
        apiRoot: routes.DiscountsApi
    });

}(jQuery));