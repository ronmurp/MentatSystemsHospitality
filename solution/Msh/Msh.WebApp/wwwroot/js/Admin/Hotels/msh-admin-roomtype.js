(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;

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
            var path = `${window.location.origin}/admin/hotels/RoomTypeEditByCode?hotelCode=${hotelCode}&action=add`;
            window.location.assign(path);
        },
        editRoomType: function (code) {
            var hotelCode = getHotelCode();
            var path = `${window.location.origin}/admin/hotels/RoomTypeEditByCode?hotelCode=${hotelCode}&code=${code}&action=edit`;
            window.location.assign(path);
        },
        deleteRoomType: function (code) {
            var hotelCode = getHotelCode();
            var path = `${window.location.origin}/admin/hotels/RoomTypeEdittByCode?hotelCode=${hotelCode}&code=${code}&action=delete`;
            window.location.assign(path);
        },
        cancelRoomTypeEdit: function () {
            var path = `${window.location.origin}/admin/hotels/roomtypelist`;
            window.location.assign(path);
        }

    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        var path = `${window.location.origin}/admin/hotels/RoomTypeList?hotelCode=${hotelCode}`;
        window.location.assign(path);
    });



}(jQuery));