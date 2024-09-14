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
        util.redirectTo(`admin/hotels/ExtrasList?hotelCode=${hotelCode}`);
    });

    if (app.itemDatesService) {
        app.itemDatesService.init({ datesApiAction: 'ExtraDates' });
    }
   
    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/ExtraDelete',
        copyApi: '/api/hotelapi/ExtraCopy',
        listPath: 'admin/hotels/ExtrasList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/ExtraDeleteBulk',
        copyBulkApi: '/api/hotelapi/ExtraCopyBulk',
        sortListApi: '/api/hotelapi/ExtrasSort',
        listPath: 'admin/hotels/ExtrasList'
    });



}(jQuery));
