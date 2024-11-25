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

    var ids = {
        selectHotel: '#selectHotel'
    }

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

            var hotelCode = getHotelCode();
            api.postAsync('/api/hotelapi/RatePlanSortAdd', { hotelCode: hotelCode }, function (data) {
                var x = data;
                // Nothing to to, already loaded in the call
                window.location = `/admin/hotels/RatePlanSortList?hotelCode=${hotelCode}`
            });
        }
    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/RatePlanSortList?hotelCode=${hotelCode}`);
    });


    app.hotelActionService.init({
        // deleteApi: '/api/hotelapi/RatePlanDelete',
        // copyApi: '/api/hotelapi/RatePlanCopy',
        moveApi: '/admin/hotels/RatePlanSortMove',
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
            useHotelCode: true
        }

        pallsA.init(inputs);
        pallsB.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);

        meth.extendMethods({


        });
    }
    
}(jQuery));

// Publish
(function ($) {

    "use strict";

    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;
    var mom = app.momentDateService;
    var htmlS = app.htmlService;
    var modal = app.modalService;
    var mas = app.modalActionService;


    var hotelApi = '/api/hotelapi';

    var ids = {
        selectHotel: '#selectHotel'
    }

    var currentHotelCode = '';

    var options = {

        modalActionId: 'publishRatePlanSort',
        modalActionTitle: 'Confirm Rate Plan Sort Publish',
        modalActionBody: 'Confirm publish of rate plan sort list',
        modalActionOnCLick: `onclick="window.mshMethods.publishDataConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${hotelApi}/RatePlanSortPublish`,

        modalActionedId: 'publishedRatePlanSort',
        modalActionedTitle: 'Publish Rate Plan Sort',
        modalActionedBody: 'The list of rate plan sort was successfully published',
        modalActionedOnCLick: `onclick="window.mshMethods.publishDataConfirm()"`,
        modalActionedOk: 'OK',
        actionedConfirmApiUrl: ``,

        currentHotelCode: ''
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    var publishPair = new mas.PairOverlay(options);

    meth.extendMethods({
        publishDataX: function () {
            var hotelCode = getHotelCode();
            options.actionConfirmApiUrl = `${hotelApi}/RatePlanSortPublish/${hotelCode}`;
            publishPair.action(options);
        },
        publishDataConfirmX: function () {
            var hotelCode = getHotelCode();
            options.actionConfirmApiUrl = `${hotelApi}/RatePlanSortPublish/${hotelCode}`;
            publishPair.actioned(options);
        }
    });

}(jQuery));

// Load
(function ($) {

    "use strict";

    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;
    var mom = app.momentDateService;
    var htmlS = app.htmlService;
    var modal = app.modalService;
    var mas = app.modalActionService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    var apiRoot = '/api/hotelapi';

    var options = {
        modalActionId: 'loadRatePlanSort',
        modalActionTitle: 'Confirm Rate Plan Sort Load',
        modalActionBody: 'Confirm load of rate plan sort list.',
        modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.loadDataConfirm()"`,
        modalActionOk: 'OK',
        loadConfirmApiUrl: `${apiRoot}/RatePlanSortLoad`, // this will be updated in loadDataConfirm

        modalActionedId: 'loadedRatePlanSort',
        modalActionedTitle: 'Load Rate Plan Sort',
        modalActionedBody: 'The list of rate plan sort was successfully loaded',
        modalActionedOnCLick: `onclick="window.mshMethods.loadDataConfirmed()"`,
        // modalPublishedOk: 'OK',
        modalActionedFooterOk: false,
        modalActionedHideModalEnd: "window.mshMethods.loadDataConfirmed"
    }

    function getLoadBody(optionsHtml) {
        var html = '';
        html += '<p>You can load data from the Published or Archived content into the editing table, where you can make changes before re-publishing.</p>';
        html += '<p>Only published configuration data is used in customer facing operations.</p>';

        html += '<div class="form-group mb-3">';
        html += '<label class="form-label">Select the source to load</label>';
        html += `<select class="form-control" id="selected-load" >`;
        html += optionsHtml;
        html += '</select>';
        html += '</div>';
        return html;
    }
    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    var loadPair = new mas.PairOverlay(options);

    var loadSource = '';

    function getLoadList() {

        var hotelCode = getHotelCode();
        var url = `${apiRoot}/RatePlanSortArchiveSelectList/${hotelCode}`;
        api.getAsync(url, function (data) {
            if (data.success) {
                var list = data.data;
                var optionsHtml = '';
                list.forEach((v) => {
                    optionsHtml += `<option value="${v.value}">${v.text}</option>`
                });
                options.modalActionBody = getLoadBody(optionsHtml);
                loadPair.action(options);
                return;
            } else {
                modal.showError(data.userErrorMessage);

            }
        })
    }

    function updateOptions() {
        var archiveCode = $('#selected-load').val();
        var hotelCode = getHotelCode();

        options.actionConfirmApiUrl = `/api/hotelapi/RatePlanSortLoad/${hotelCode}`;
        options.actionConfirmData = { code: archiveCode };
    }

    meth.extendMethods({
        loadDataConfirm: function () {
            updateOptions();
            $(`#${options.modalActionId}`).remove();
            loadPair.actioned(options);
        },
        loadDataConfirmed: function () {
            util.redirectTo('admin/hotels/RatePlanSortlist');
        },
    });

}(jQuery));


// Archive
(function ($) {

    "use strict";

    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;
    var mom = app.momentDateService;
    var htmlS = app.htmlService;
    var modal = app.modalService;
    var mas = app.modalActionService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    var apiRoot = '/api/hotelapi';

    var options = {
        modalActionId: 'archiveRatePlans',
        modalActionTitle: 'Confirm Rate Plan Sort Archive',
        modalActionBody: 'Confirm archive of Rate Plan Sort list',
        modalActionOnCLick: `id="confirm-archive-ok" onclick="window.mshMethods.archiveDataConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${apiRoot}/RatePlanSortArchive`,
        actionConfirmData: {},

        modalActionedId: 'archivedRatePlans',
        modalActionedTitle: 'Archive Rate Plan Sort',
        modalActionedBody: 'The list of rate plan sort was successfully archived',
        modalActionedOnCLick: `onclick="window.mshMethods.archiveDataConfirmed()"`,

        // modalPublishedOk: 'OK',

    }

    function getArchiveBody() {
        var html = '';
        html += '<div class="form-group mb-3">';
        html += '<input type="text" id="archive-code" class="form-control" />'
        html += '</div>';
        return html;
    }

    var archivePair = new mas.PairOverlay(options);

    var loadSource = '';


    meth.extendMethods({
        archiveHotels: function () {
            options.modalActionBody = getArchiveBody();
            archivePair.action(options);
        },
        archiveHotelsConfirm: function () {
            var archiveCode = $('#archive-code').val();
            var hotelCode = getHotelCode();
            options.actionConfirmApiUrl = `/api/hotelapi/RatePlanSortArchive/${hotelCode}/${archiveCode}`;
            options.actionConfirmData = null;
            archivePair.actioned(options);
        }

    });

}(jQuery));