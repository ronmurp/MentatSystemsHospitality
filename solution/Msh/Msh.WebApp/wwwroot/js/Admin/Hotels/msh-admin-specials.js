(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var pallsA = app.pallsArchiveService;
    var pallsP = app.pallsPublishService;
    var pallsD = app.pallsDeleteService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;

    var itemDatesService = app.itemDatesService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    var apiRoot = `/api/specialsapi`;
    var controllerPath = `admin/hotels`
    var listPath = `${controllerPath}/SpecialsList`;

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`${listPath}?hotelCode=${hotelCode}`);
    });

    if (app.itemDatesService) {
        app.itemDatesService.init({ datesApiAction: 'SpecialDates' });
    }
   
    app.hotelActionService.init({
        deleteApi: `${apiRoot}/SpecialDelete`,
        copyApi: `${apiRoot}/SpecialCopy`,
        moveApi: `${controllerPath}/SpecialMove`,
        listPath: listPath
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/SpecialDeleteBulk`,
        copyBulkApi: `${apiRoot}/SpecialCopyBulk`,
        sortListApi: `${apiRoot}/SpecialsSort`,
        listPath: listPath
    });

    var editType = $('#edit-type').val();

    if (editType === "specials-list") {

        var inputs = {
            model: 'Specials',
            name: 'Specials',
            useHotelCode: true,
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
