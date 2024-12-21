(function ($) {

    "use strict";

    var app = mshPageApp;
    var routes = app.routes;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService

    var api = app.apiService;

    var pallsS = app.pallSupportService;
    var pallsA = app.pallsArchiveService;
    var pallsP = app.pallsPublishService;
    var pallsD = app.pallsDeleteService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;

    var apiRoot = routes.RatePlansTextApi;
    var controllerRoot = routes.RatePlansTexts;
    var listPath = `${controllerRoot}/RatePlansTextList`;

    pallsS.initHotelSelectEvent(listPath);

    var currentStayData = {};

    meth.extendMethods({
        confirmDeleteRatePlanText: function (code, hotelCode) {
            var url = `${apiRoot}/RatePlanTextDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo(`${listPath}?hotelCode=${hotelCode}`)
            });
        },

        deleteRatePlanText: function (code, hotelCode) {

            modal.showModal('delRatePlan', "Confirm Delete", `Confirm delete of ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteRatePlanText('${code}', '${hotelCode}')""`,
                okButtonText: 'OK'
            });
        },
    });

    app.itemDatesService.initDatePair('StayFrom', 'StayTo');
    app.itemDatesService.initDatePair('BookFrom', 'BookTo', 'HasBookDates');

    app.hotelActionService.init({
        deleteApi: `${apiRoot}/RatePlanTextDelete`,
        copyApi: `${apiRoot}/RatePlanTextCopy`,
        moveApi: `${controllerRoot}/RatePlanTextMove`,
        listPath: listPath
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/RatePlanTextDeleteBulk`,
        copyBulkApi: `${apiRoot}/RatePlanTextCopyBulk`,
        sortListApi: `${apiRoot}/RatePlanTextSort`,
        listPath: listPath
    });

    var editType = $('#edit-type').val();

    if (editType === "rate-plan-text-list") {

        var inputs = {
            model: 'RatePlanText',
            name: 'Rate Plan Text',
            useHotelCode: true,
            confirmedRedirect: true,
            confirmedRedirectUrl: listPath,
            apiRoot: apiRoot
        }

        pallsA.init(inputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);

    }
    
}(jQuery));
