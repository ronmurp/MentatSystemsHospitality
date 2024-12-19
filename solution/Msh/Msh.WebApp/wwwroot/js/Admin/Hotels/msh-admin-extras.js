(function ($) {

    "use strict";
    var app = mshPageApp;
    var routes = app.routes;
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

    var apiRoot = routes.ExtrasApi;
    var controllerPath = routes.Extras;
    var listPath = `${controllerPath}/ExtrasList`;
    
    pallsS.initHotelSelectEvent(listPath);

    if (app.itemDatesService) {
        app.itemDatesService.init({ datesApiAction: 'ExtraDates', apiRoot: apiRoot });
    }
   
    app.hotelActionService.init({
        deleteApi: `${apiRoot}/ExtraDelete`,
        copyApi: `${apiRoot}/ExtraCopy`,
        moveApi: `${controllerPath}/ExtraMove`,
        apiRoot: apiRoot,
        listPath: listPath
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/ExtraDeleteBulk`,
        copyBulkApi: `${apiRoot}/ExtraCopyBulk`,
        sortListApi: `${apiRoot}/ExtrasSort`,
        apiRoot: apiRoot,
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
