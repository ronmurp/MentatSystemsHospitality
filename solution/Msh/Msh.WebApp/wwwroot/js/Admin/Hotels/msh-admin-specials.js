(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var itemDatesService = app.itemDatesService;

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
        util.redirectTo(`admin/hotels/SpecialsList?hotelCode=${hotelCode}`);
    });

    if (app.itemDatesService) {
        app.itemDatesService.init({ datesApiAction: 'SpecialDates' });
    }
   
    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/SpecialDelete',
        copyApi: '/api/hotelapi/SpecialCopy',
        moveApi: '/admin/hotels/SpecialMove',
        listPath: 'admin/hotels/SpecialsList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/SpecialDeleteBulk',
        copyBulkApi: '/api/hotelapi/SpecialCopyBulk',
        sortListApi: '/api/hotelapi/SpecialsSort',
        listPath: 'admin/hotels/SpecialsList'
    });



}(jQuery));
