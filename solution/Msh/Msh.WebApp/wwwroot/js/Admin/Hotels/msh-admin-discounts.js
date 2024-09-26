(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var pallsA = app.pallsArchiveService;
    var pallsB = app.pallsPublishService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;

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
        util.redirectTo(`admin/hotels/DiscountsList?hotelCode=${hotelCode}`);
    });

    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/DiscountDelete',
        copyApi: '/api/hotelapi/DiscountCopy',
        moveApi: '/admin/hotels/DiscountMove',
        listPath: 'admin/hotels/DiscountList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/DiscountDeleteBulk',
        copyBulkApi: '/api/hotelapi/DiscountCopyBulk',
        sortListApi: '/api/hotelapi/DiscountsSort',
        listPath: 'admin/hotels/DiscountsList'
    });

    var inputs = {
        model: 'Discounts',
        name: 'Discounts',
        useHotelCode: true
    }

    pallsA.init(inputs);
    pallsB.init(inputs);
    pallsLoad.init(inputs);
    pallsLock.init(inputs);

}(jQuery));

