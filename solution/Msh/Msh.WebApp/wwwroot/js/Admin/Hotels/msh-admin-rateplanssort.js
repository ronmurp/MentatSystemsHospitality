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

    var ids = {
        selectHotel: '#selectHotel'
    }

    var apiRoot = routes.RatePlansSortApi;
    var controllerRoot = routes.RatePlansSort;
    var listPath = `${controllerRoot}/RatePlanSortList`;

    pallsS.initHotelSelectEvent(listPath);

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    meth.extendMethods({

        reloadRatePlans: function () {
            modal.showModal('delRatePlan', "Confirm Reload", `Confirm reload of rate plans. This will add missing rate plans and remove unused rate plans.`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmReloadRatePlans()"`,
                okButtonText: 'OK'
            });
        },

        confirmReloadRatePlans: function () {

            var hotelCode = pallsS.getHotelCode();
            api.postAsync(`${apiRoot}/RatePlanSortAdd`, { hotelCode: hotelCode }, function (data) {
                var x = data;
                // Nothing to to, already loaded in the call
                window.location = `${listPath}?hotelCode=${hotelCode}`
            });
        }
    });

    


    app.hotelActionService.init({
        // deleteApi: '/api/hotelapi/RatePlanDelete',
        // copyApi: '/api/hotelapi/RatePlanCopy',
        moveApi: `${controllerRoot}/RatePlanSortMove`,
        // listPath: 'admin/hotels/RatePlansList'
    });

    //app.hotelActionBulkService.init({
    //    deleteBulkApi: '/api/hotelapi/RatePlanDeleteBulk',
    //    copyBulkApi: '/api/hotelapi/RatePlanCopyBulk',
    //    sortListApi: '/api/hotelapi/RatePlansSort',
    //    listPath: 'admin/hotels/RatePlansList'
    //});

    var editType = $('#edit-type').val();

    if (editType === "rate-plan-sort-list") {

        var inputs = {
            model: 'RatePlanSort',
            name: 'Rate Plan Sort',
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
