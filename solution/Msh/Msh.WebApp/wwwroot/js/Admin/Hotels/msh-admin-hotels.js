(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;

    meth.extendMethods({
        editHotel: function (hotelCode) {

            var path = `${window.location.origin}/admin/hotels/HotelEditByCode?hotelCode=${hotelCode}&action=edit`;

            window.location.assign(path);
        },
        deleteHotel: function (hotelCode) {
            var path = `${window.location.origin}/admin/hotels/HotelEditByCode?hotelCode=${hotelCode}&action=delete`;
            window.location.assign(path);
        }
    });

    //$('#add-hotel').on('click', (e) => {
    //    e.preventDefault();


    //});

}(jQuery));