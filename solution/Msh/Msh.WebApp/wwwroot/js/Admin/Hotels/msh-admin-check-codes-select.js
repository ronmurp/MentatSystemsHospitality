(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var api = app.apiService;
      
    var hotelCode = $('#hotel-code').val();
    var code = $('#item-code').val();

    window.mshPageApp.hotelSelectService.init({
        hotelCode: hotelCode,
        code: code,
        saveApi: '/api/hotelapi/SpecialRoomTypesSave'
    });
   

}(jQuery));
