// Main Edit
(function ($) {

    "use strict";

    var app = mshPageApp;

    var pallsA = app.pallsArchiveService;
    var pallsP = app.pallsPublishService;
    var pallsD = app.pallsDeleteService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;

    var apiRoot = '/api/hotelapi';
    var listPath = 'admin/hotels'

    var currentHotelCode = '';

    app.hotelActionService.init({
        deleteApi: `${apiRoot}/HotelDelete`,
        copyApi: `${apiRoot}/HotelCopy`,
        moveApi: `${apiRoot}/HotelMove`,
        listPath: `${listPath}/HotelsList`,
        codeOnly: true
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/HotelDeleteBulk`,
        copyBulkApi: `${apiRoot}/HotelCopyBulk`,
        sortListApi: `${apiRoot}/HotelsSort`,
        listPath: `${listPath}/HotelsList`,
        includeBulkCopy: false
    });

    var inputs = {
        model: 'Hotels',
        name: 'Hotels',
        useHotelCode: false,
        apiRoot: apiRoot,
        listPath: listPath

    }

    var editType = $('#edit-type').val();

    if (editType === "hotel-list") {
        pallsA.init(inputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);
    }

}(jQuery));
