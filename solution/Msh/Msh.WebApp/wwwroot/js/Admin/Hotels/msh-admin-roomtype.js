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
    var pallsI = app.pallsImportService;


    var ids = {
        selectHotel: '#selectHotel'
    }

    var apiRoot = '/api/roomtypeapi'
    var listPath = 'admin/hotels/RoomTypesList';

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`${listPath}?hotelCode=${hotelCode}`);
    });

    app.hotelActionService.init({
        deleteApi: `${apiRoot}/RoomTypeDelete`,
        copyApi: `${apiRoot}/RoomTypeCopy`,
        listPath: listPath
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/RoomTypeDeleteBulk`,
        copyBulkApi: `${apiRoot}/RoomTypeCopyBulk`,
        sortListApi: `${apiRoot}/RoomTypesSort`,
        listPath: listPath
    });

    var editType = $('#edit-type').val();

    if (editType === "roomtypes-list") {

        var inputs = {
            model: 'RoomType',
            name: 'Room Types',
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
        pallsI.init(inputs);

    }


}(jQuery));