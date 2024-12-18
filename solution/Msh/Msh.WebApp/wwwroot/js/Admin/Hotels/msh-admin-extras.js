﻿(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var pallsS = app.pallSupportService;
    var pallsA = app.pallsArchiveService;
    var pallsP = app.pallsPublishService;
    var pallsD = app.pallsDeleteService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;
    var pallsI = app.pallsImportService;

    var itemDatesService = app.itemDatesService;

    var apiRoot = '/api/extrasapi';
    var listPath = 'admin/hotels/ExtrasList';

    $(ids.selectHotel).on('change', () => {
        var hotelCode = pallsS.getHotelCode();
        util.redirectTo(`${listPath}?hotelCode=${hotelCode}`);
    });

    if (app.itemDatesService) {
        app.itemDatesService.init({ datesApiAction: 'ExtraDates' });
    }
   
    app.hotelActionService.init({
        deleteApi: `${apiRoot}/ExtraDelete`,
        copyApi: `${apiRoot}/ExtraCopy`,
        moveApi: `${apiRoot}/ExtraMove`,
        listPath: listPath
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/ExtraDeleteBulk`,
        copyBulkApi: `${apiRoot}/ExtraCopyBulk`,
        sortListApi: `${apiRoot}/ExtrasSort`,
        listPath: listPath
    });

    var editType = $('#edit-type').val();

    if (editType === "extras-list") {

        var inputs = {
            model: 'Extras',
            name: 'Extras',
            useHotelCode: true,
            confirmedRedirect: true,
            confirmedRedirectUrl: listPath,
            apiRoot: apiRoot
        }

        // How to add a custom body text to the standard body (code and notes)
        var archiveInputs = $.extend({}, inputs, { modalActionBody: '<p class="mb-2">Test modalActionBody in msh-admin-extras.js</p>' } );

        pallsA.init(archiveInputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);
        pallsI.init(inputs);

    }

}(jQuery));
