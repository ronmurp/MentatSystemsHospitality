(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;
    var itemDatesService = app.itemDatesService;

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

    //function changeClassPair(nameEnable, nameFrom, nameTo) {
    //    if ($(`#${nameEnable}`).is(':checked')) {
    //        $(`#${nameFrom}`).removeClass('dim-input');
    //        $(`#${nameTo}`).removeClass('dim-input');
    //    }
    //    else {
    //        $(`#${nameFrom}`).addClass('dim-input');
    //        $(`#${nameTo}`).addClass('dim-input');
    //    }
    //}

    //function updateDatePair(nameFrom, nameTo, isFrom) {

    //    var dateFrom = mom.date($(`#${nameFrom}`).val());
    //    var dateTo = mom.date($(`#${nameTo}`).val());
    //    if (dateTo < dateFrom) {
    //        if (isFrom) {
    //            var d2 = dateFrom.clone().add(1, 'days');
    //            $(`#${nameTo}`).val(d2.format(mom.YMD));
    //        } else {
    //            var d2 = dateTo.clone().add(-1, 'days');
    //            $(`#${nameFrom}`).val(d2.format(mom.YMD));
    //        }
    //    }
    //}

    //function initDatePair(nameFrom, nameTo, nameEnabled) {

    //    if (nameEnabled) {
    //        $(`[name="${nameEnabled}"]`).off('change');
    //        $(`[name="${nameEnabled}"]`).on('change', function () {
    //            changeClassPair(nameEnabled, nameFrom, nameTo)
    //        });
    //    }

    //    $(`[name="${nameFrom}"]`).off('change');
    //    $(`[name="${nameFrom}"]`).on('change', function () {
    //        updateDatePair(nameFrom, nameTo, true);
    //    });

    //    $(`[name="${nameTo}"]`).on('change');
    //    $(`[name="${nameTo}"]`).on('change', function () {
    //        updateDatePair(nameFrom, nameTo, false);
    //    });
    //}

    itemDatesService.initDatePair('StayFrom', 'StayTo');
    itemDatesService.initDatePair('BookFrom', 'BookTo', 'HasBookDates');

}(jQuery));