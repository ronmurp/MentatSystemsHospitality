(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var itemDatesService = app.itemDatesService;

    itemDatesService.init({
        datesApiAction: 'DiscountOfferDates'
    });

}(jQuery));