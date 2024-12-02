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

    var ids = {
        selectHotel: '#selectHotel',
    }

    var apiRoot = `/api/rateplantextapi`;
    var listPath = 'admin/hotels/RatePlansTextList';

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

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

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`${listPath}?hotelCode=${hotelCode}`);
    });

    app.itemDatesService.initDatePair('StayFrom', 'StayTo');
    app.itemDatesService.initDatePair('BookFrom', 'BookTo', 'HasBookDates');

    app.hotelActionService.init({
        deleteApi: `${apiRoot}/RatePlanTextDelete`,
        copyApi: `${apiRoot}/RatePlanTextCopy`,
        moveApi: `${apiRoot}/RatePlanTextMove`,
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
