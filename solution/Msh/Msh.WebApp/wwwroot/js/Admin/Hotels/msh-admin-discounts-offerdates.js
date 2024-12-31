(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var routes = app.routes;
    var itemDatesService = app.itemDatesService;

    itemDatesService.init({
        datesApiAction: 'DiscountOfferDates',
        apiRoot: routes.DiscountsApi
    });

}(jQuery));