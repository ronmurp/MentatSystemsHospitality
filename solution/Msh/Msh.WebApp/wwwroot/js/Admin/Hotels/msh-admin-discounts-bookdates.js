﻿(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var itemDatesService = app.itemDatesService;

    itemDatesService.init({
        datesApiAction: 'DiscountBookDates',
        apiRoot: '/api/discountsapi'
    });

}(jQuery));