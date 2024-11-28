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
        util.redirectTo(`admin/hotels/RoomTypesList?hotelCode=${hotelCode}`);
    });

    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/RoomTypeDelete',
        copyApi: '/api/hotelapi/RoomTypeCopy',
        listPath: 'admin/hotels/RoomTypesList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/RoomTypeDeleteBulk',
        copyBulkApi: '/api/hotelapi/RoomTypeCopyBulk',
        sortListApi: '/api/hotelapi/RoomTypesSort',
        listPath: 'admin/hotels/RoomTypesList'
    });

    var editType = $('#edit-type').val();

    if (editType === "roomtypes-list") {

        var inputs = {
            model: 'RoomTypesList',
            name: 'Room Types',
            useHotelCode: true,
            confirmedRedirect: true,
            confirmedRedirectUrl: 'admin/hotels/RoomTypesList'
        }

      

        pallsA.init(inputs);
        pallsP.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);
        pallsI.init(inputs);

    }


}(jQuery));