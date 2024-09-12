(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    meth.extendMethods({
        deleteRoomType: function (code) {
            var hotelCode = getHotelCode();
            util.redirectTo(`admin/hotels/RoomTypeEditByCode?hotelCode=${hotelCode}&code=${code}&action=delete`);
        },
        cancelRoomTypeEdit: function (hotelCode) {
            util.redirectTo(`admin/hotels/roomtypelist?hotelCode=${hotelCode}`);
        },
        addRoomTypeDate: function () {

        }

    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/ExtrasList?hotelCode=${hotelCode}`);
    });



}(jQuery));