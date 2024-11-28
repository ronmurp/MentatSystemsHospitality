(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var pallsA = app.pallsArchiveService;
    var pallsP = app.pallsPublishService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;
    var pallsI = app.pallsImportService;

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
        moveApi: '/admin/hotels/ExtraMove',
        listPath: 'admin/hotels/ExtrasList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/ExtraDeleteBulk',
        copyBulkApi: '/api/hotelapi/ExtraCopyBulk',
        sortListApi: '/api/hotelapi/ExtrasSort',
        listPath: 'admin/hotels/ExtrasList'
    });

    var editType = $('#edit-type').val();

    if (editType === "extras-list") {

        var inputs = {
            model: 'Extras',
            name: 'Extras',
            useHotelCode: true,
            confirmedRedirect: true,
            confirmedRedirectUrl: 'admin/hotels/ExtrasList'
        }

        // How to add a custom body text to the standard body (code and notes)
        var archiveInputs = $.extend({}, inputs, { modalActionBody: '<p class="mb-2">Test modalActionBody in msh-admin-extras.js</p>' } );

        pallsA.init(archiveInputs);
        pallsP.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);
        pallsI.init(inputs);

    }

}(jQuery));
