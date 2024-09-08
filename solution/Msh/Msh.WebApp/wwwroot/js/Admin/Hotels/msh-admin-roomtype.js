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

        addRoomType: function () {
            var hotelCode = getHotelCode();
            util.redirectTo(`admin/hotels/RoomTypeEditByCode?hotelCode=${hotelCode}&action=add`);
        },
        editRoomType: function (code) {
            var hotelCode = getHotelCode();
            util.redirectTo(`admin/hotels/RoomTypeEditByCode?hotelCode=${hotelCode}&action=edit`);
        },
        deleteRoomType: function (code) {
            var hotelCode = getHotelCode();
            util.redirectTo(`admin/hotels/RoomTypeEditByCode?hotelCode=${hotelCode}&code=${code}&action=delete`);
        },
        cancelRoomTypeEdit: function () {
            util.redirectTo('admin/hotels/roomtypelist');
         
        }

    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/RoomTypeList?hotelCode=${hotelCode}`);
    });



}(jQuery));