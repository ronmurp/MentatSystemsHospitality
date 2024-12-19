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

    var itemDatesService = app.itemDatesService;

    var apiRoot = routes.SpecialsApi;
    var controllerPath = routes.Specials;
    var listPath = `${controllerPath}/SpecialsList`;

    pallsS.initHotelSelectEvent(listPath);

    if (app.itemDatesService) {
        app.itemDatesService.init({ datesApiAction: 'SpecialDates', apiRoot: apiRoot });
    }
   
    app.hotelActionService.init({
        deleteApi: `${apiRoot}/SpecialDelete`,
        copyApi: `${apiRoot}/SpecialCopy`,
        moveApi: `${controllerPath}/SpecialMove`,
        apiRoot: apiRoot,
        listPath: listPath
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/SpecialDeleteBulk`,
        copyBulkApi: `${apiRoot}/SpecialCopyBulk`,
        sortListApi: `${apiRoot}/SpecialsSort`,
        apiRoot: apiRoot,
        listPath: listPath
    });

    var editType = $('#edit-type').val();

    if (editType === "specials-list") {

        var inputs = {
            model: 'Specials',
            name: 'Specials',
            useHotelCode: true,
            confirmedRedirect: true,
            confirmedRedirectUrl: listPath,
            apiRoot: apiRoot,
            listPath: listPath
        }

        pallsA.init(inputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);

    }


}(jQuery));
