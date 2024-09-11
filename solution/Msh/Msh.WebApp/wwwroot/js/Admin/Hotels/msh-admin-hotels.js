(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;

    var hotelApi = '/api/hotelapi/';

    var props = [];

    meth.extendMethods({

        deleteHotel: function (hotelCode) {
            util.redirectTo(`admin/hotels/HotelEditByCode?hotelCode=${hotelCode}&action=delete`);
        },
        cancelHotelEdit: function () {
            util.redirectTo('admin/hotels/hotellist');
        }
       

    });

   

}(jQuery));