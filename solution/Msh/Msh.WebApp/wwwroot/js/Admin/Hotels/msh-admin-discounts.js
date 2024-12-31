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

    var apiRoot = routes.DiscountsApi;
    var listRoot = routes.Discounts;
    var listPath = `${listRoot}/DiscountsList`

    $('#selectHotel').on('change', () => {
        var hotelCode = pallsS.getHotelCode();
        util.redirectTo(`${listPath}?hotelCode=${hotelCode}`);
    });

    app.hotelActionService.init({
        deleteApi: `${apiRoot}/DiscountDelete`,
        copyApi: `${apiRoot}/DiscountCopy`,
        moveApi: `${apiRoot}/DiscountMove`,
        listPath: `${apiRoot}/DiscountList`
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/DiscountDeleteBulk`,
        copyBulkApi: `${apiRoot}/DiscountCopyBulk`,
        sortListApi: `${apiRoot}/DiscountsSort`,
        listPath: listPath
    });

    var editType = $('#edit-type').val();

    if (editType === "discounts-list") {

        var inputs = {
            model: 'Discounts',
            name: 'Discounts',
            useHotelCode: true,
            apiRoot: apiRoot,
            confirmedRedirect: true,
            confirmedRedirectUrl: listPath
        }

        pallsA.init(inputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);
    }

}(jQuery));

