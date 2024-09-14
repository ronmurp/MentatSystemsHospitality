(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    meth.extendMethods({
        confirmDeleteRatePlan: function (code, hotelCode) {
            var url = `/api/hotelapi/RatePlanDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo(`admin/hotels/RatePlanList?hotelCode=${hotelCode}`)
            });
        },

        deleteRatePlan: function (code, hotelCode) {

            modal.showModal('delRatePlan', "Confirm Delete", `Confirm delete of ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteRatePlan('${code}', '${hotelCode}')""`,
                okButtonText: 'OK'
            });
        },
    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/RatePlanList?hotelCode=${hotelCode}`);
    });

    app.itemDatesService.initDatePair('StayFrom', 'StayTo');
    app.itemDatesService.initDatePair('BookFrom', 'BookTo', 'HasBookDates');

    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/RatePlanDelete',
        copyApi: '/api/hotelapi/RatePlanCopy',
        listPath: 'admin/hotels/RatePlanList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/RatePlanDeleteBulk',
        copyBulkApi: '/api/hotelapi/RatePlanCopyBulk',
        sortListApi: '/api/hotelapi/RatePlansSort',
        listPath: 'admin/hotels/RatePlansList'
    });

}(jQuery));