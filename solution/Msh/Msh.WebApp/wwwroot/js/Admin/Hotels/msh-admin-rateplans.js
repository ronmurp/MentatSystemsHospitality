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
        selectHotel: '#selectHotel',

        stayChangeHotelCode: 'stay-change-hotel-code',
        stayChangeCode: 'stay-change-code',
        stayChangeRatePlanCode: 'stay-change-rpc',
        stayChangeFrom: 'stay-change-from',
        stayChangeTo: 'stay-change-to'
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    var currentStayData = {};

    function getEditStayForm() {

        var d = currentStayData;

        var html = `<p>Change the stay date range for this rate plan.</p>`;
        html += `<div class="form-group">`;
        html += `<label class="form-control">${d.hotelCode}</label>`;
        html += `<label class="form-control mt-3">${d.ratePlanCode}</label>`;

        
        html += `<input id="${ids.stayChangeFrom}" type="date" class="form-control mt-3" onchange="window.mshMethods.changeFrom()" value="${d.stayFrom}" />`;
        html += `<input id="${ids.stayChangeTo}" type="date" class="form-control mt-3" onchange="window.mshMethods.changeTo()" value="${d.stayTo}" />`;

        html += `<input id="${ids.stayChangeHotelCode}" type="hidden"  value="${d.hotelCode}" />`;
        html += `<input id="${ids.stayChangeCode}" type="hidden"  value="${d.code}" />`;
        html += `<input id="${ids.stayChangeRatePlanCode}" type="hidden"  value="${d.ratePlanCode}" />`;
        html += `</div>`;

        return html;

    }

    function changeDate(isFrom) {
        var d = {
            dateFrom: $(`#${ids.stayChangeFrom}`).val(),
            dateTo: $(`#${ids.stayChangeTo}`).val(),
            isFrom: isFrom
        }
        var url = `/api/hotelapi/ChangeDatePair`;
        api.postAsync(url, d, function (data) {
            var dates = data.data;
            $(`#${ids.stayChangeFrom}`).val(dates.dateFrom);
            $(`#${ids.stayChangeTo}`).val(dates.dateTo)
        });
    }

    meth.extendMethods({
        confirmDeleteRatePlan: function (code, hotelCode) {
            var url = `/api/hotelapi/RatePlanDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo(`admin/hotels/RatePlansList?hotelCode=${hotelCode}`)
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
        util.redirectTo(`admin/hotels/RatePlansList?hotelCode=${hotelCode}`);
    });

    app.itemDatesService.initDatePair('StayFrom', 'StayTo');
    app.itemDatesService.initDatePair('BookFrom', 'BookTo', 'HasBookDates');

    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/RatePlanDelete',
        copyApi: '/api/hotelapi/RatePlanCopy',
        moveApi: '/admin/hotels/RatePlanMove',
        listPath: 'admin/hotels/RatePlansList'
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/RatePlanDeleteBulk',
        copyBulkApi: '/api/hotelapi/RatePlanCopyBulk',
        sortListApi: '/api/hotelapi/RatePlansSort',
        listPath: 'admin/hotels/RatePlansList'
    });

    var editType = $('#edit-type').val();

    if (editType === "rate-plan-list") {

        var inputs = {
            model: 'RatePlans',
            name: 'Rate Plans',
            useHotelCode: true
        }

        pallsA.init(inputs);
        pallsB.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);

        meth.extendMethods({

            ratePlanStayChange: function (hotelCode, code, ratePlanCode, stayFrom, stayTo) {

                currentStayData = {
                    hotelCode: hotelCode,
                    code: code,
                    ratePlanCode: ratePlanCode,
                    stayFrom: stayFrom,
                    stayTo: stayTo
                }

                var body = getEditStayForm();
                modal.showModal('stayEditModel', 'Change Stay Dates', body, {
                    okButtonClickScript: 'onclick="window.mshMethods.ratePlanStayChangeConfirm()"',
                    footerOk: true,
                });
            },

            ratePlanStayChangeConfirm: function () {
                var d = {
                    hotelCode: $(`#${ids.stayChangeHotelCode}`).val(),
                    code: $(`#${ids.stayChangeCode}`).val(),
                    ratePlanCode: $(`#${ids.stayChangeRatePlanCode}`).val(),
                    stayFrom: $(`#${ids.stayChangeFrom}`).val(),
                    stayTo: $(`#${ids.stayChangeTo}`).val(),
                }
                var url = `/api/hotelapi/RatePlanStayChange`
                api.postAsync(url, d, function (data) {
                    if (data.success) {
                        util.redirectTo(`admin/hotels/RatePlansList?hotelCode=${d.hotelCode}`)
                    }
                });
            },

            changeFrom: function () {
                changeDate(true);
            },
            changeTo: function () {
                changeDate(false);
            }

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

        modalActionId: 'publishRatePlans',
        modalActionTitle: 'Confirm Rate Plans Publish',
        modalActionBody: 'Confirm publish of rate plans list',
        modalActionOnCLick: `onclick="window.mshMethods.publishDataConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${hotelApi}/RatePlansPublish`,

        modalActionedId: 'publishedRatePlans',
        modalActionedTitle: 'Publish Rate Plans',
        modalActionedBody: 'The list of rate plans was successfully published',
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
            options.actionConfirmApiUrl = `${hotelApi}/RatePlansPublish/${hotelCode}`;
            publishPair.action(options);
        },
        publishDataConfirmX: function () {
            var hotelCode = getHotelCode();
            options.actionConfirmApiUrl = `${hotelApi}/RatePlansPublish/${hotelCode}`;
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
        modalActionId: 'loadRatePlans',
        modalActionTitle: 'Confirm Rate Plans Load',
        modalActionBody: 'Confirm load of rate plans list',
        modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.loadDataConfirm()"`,
        modalActionOk: 'OK',
        loadConfirmApiUrl: `${apiRoot}/RatePlansLoad`,

        modalActionedId: 'loadedRatePlans',
        modalActionedTitle: 'Load Rate Plans',
        modalActionedBody: 'The list of rate plans was successfully loaded',
        modalActionedOnCLick: `onclick="window.mshMethods.loadDataConfirmed()"`,
        // modalPublishedOk: 'OK',
        modalActionedHideModalEnd: 'window.mshMethods.loadDataConfirmed'
    }

    function getLoadBody(optionsHtml) {
        var html = '';
        html += '<p>You can load data from the published content into the editing table, where you can make changes before re-publishing.</p>';
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
        var url = `${apiRoot}/RatePlansArchiveSelectList/${hotelCode}`;
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

        options.actionConfirmApiUrl = `/api/hotelapi/RatePlansLoad/${hotelCode}`;
        options.actionConfirmData = { code: archiveCode };
    }

    meth.extendMethods({
        //loadData: function () {
        //    updateOptions();
        //    getLoadList(options);
        //},
        //loadDataConfirm: function () {
        //    updateOptions();
        //    loadPair.actioned(options);
        //},
        //loadDataConfirmed: function () {
        //    util.redirectTo('admin/hotels/RatePlanslist');
        //},
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
        modalActionTitle: 'Confirm Rate Plans Archive',
        modalActionBody: 'Confirm archive of Rate Plans list',
        modalActionOnCLick: `id="confirm-archive-ok" onclick="window.mshMethods.archiveDataConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${apiRoot}/RatePlansArchive`,
        actionConfirmData: {},

        modalActionedId: 'archivedRatePlans',
        modalActionedTitle: 'Archive Rate Plans',
        modalActionedBody: 'The list of discounts was successfully archived',
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
            options.actionConfirmApiUrl = `/api/hotelapi/RatePlansArchive/${hotelCode}/${archiveCode}`;
            options.actionConfirmData = null;
            archivePair.actioned(options);
        }

    });

}(jQuery));