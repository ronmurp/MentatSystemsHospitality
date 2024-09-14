(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/RoomTypeList?hotelCode=${hotelCode}`);
    });

    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/RoomTypeDelete',
        copyApi: '/api/hotelapi/RoomTypeCopy',
        listPath: 'admin/hotels/RoomTypesList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/RoomTypeDeleteBulk',
        copyBulkApi: '/api/hotelapi/RoomTypeCopyBulk',
        sortListApi: '/api/hotelapi/RoomTypesSort',
        listPath: 'admin/hotels/RoomTypesList'
    });


}(jQuery));