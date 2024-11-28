// Main Edit
(function ($) {

    "use strict";

    var app = mshPageApp;

    var pallsA = app.pallsArchiveService;
    var pallsB = app.pallsPublishService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;

    var hotelApi = '/api/hotelapi';
    var hotelAdmin = 'admin/hotels'

    var currentHotelCode = '';

    app.hotelActionService.init({
        deleteApi: `${hotelApi}/HotelDelete`,
        copyApi: `${hotelApi}/HotelCopy`,
        moveApi: `${hotelApi}/HotelMove`,
        listPath: `${hotelAdmin}/HotelsList`,
        codeOnly: true
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${hotelApi}/HotelDeleteBulk`,
        copyBulkApi: `${hotelApi}/HotelCopyBulk`,
        sortListApi: `${hotelApi}/HotelsSort`,
        listPath: `${hotelAdmin}/HotelsList`,
        includeBulkCopy: false
    });

    var inputs = {
        model: 'Hotels',
        name: 'Hotels',
        useHotelCode: false
    }

    var editType = $('#edit-type').val();

    if (editType === "hotel-list") {
        pallsA.init(inputs);
        pallsB.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);
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

    var currentHotelCode = '';  
 
    var optionsPublish = {

        modalActionId: 'publishHotel',
        modalActionTitle: 'Confirm Hotels Publish',
        modalActionBody: 'Confirm you want to publish the current hotels list.',
        modalActionOnCLick: `onclick="window.mshMethods.publishHotelsConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${hotelApi}/HotelsPublish`,

        modalActionedId: 'publishedHotel',
        modalActionedTitle: 'Publish Hotels',
        modalActionedBody: 'The list of hotels was successfully published',
        modalActionedOnCLick: `onclick="window.mshMethods.publishHotelsConfirm()"`,
        modalActionedOk: 'OK',
        actionedConfirmApiUrl: ``,
    }

    var publishPair = new mas.PairOverlay(optionsPublish);

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


    var apiRoot = '/api/hotelapi';

    var options = {
        modalActionId: 'loadHotel',
        modalActionTitle: 'Confirm Hotels Load',
        modalActionBody: 'Confirm load of hotels list',
        modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.loadHotelsConfirm()"`,
        modalActionOk: 'OK',
        loadConfirmApiUrl: `${apiRoot}/HotelsLoad`,

        modalActionedId: 'loadedHotel',
        modalActionedTitle: 'Loaded Hotels',
        modalActionedBody: 'The list of hotels was successfully loaded',
        modalActionedOnCLick: `onclick="window.mshMethods.loadHotelsConfirmed()"`,
        // modalPublishedOk: 'OK',
        modalActionedFooterOk: false,
        modalActionedHideModalEnd: 'window.mshMethods.loadHotelsConfirmed'
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

    var loadPair = new mas.PairOverlay(options);

    var loadSource = '';

    function getLoadList() {
        api.getAsync(`${apiRoot}/HotelsArchiveSelectList`, function (data) {
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

    meth.extendMethods({
    
        loadData: function () {
            getLoadList();
        },
        loadHotelsConfirm: function () {
            loadSource = $('#selected-load').val();
            $(`#${options.modalActionId}`).remove();
            options.actionConfirmApiUrl = `/api/hotelapi/HotelsLoad`;
            options.actionConfirmData = { code: loadSource };
            loadPair.actioned(options);
        },
        loadHotelsConfirmed: function () {
            util.redirectTo('admin/hotels/HotelsList');
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


    var apiRoot = '/api/hotelapi';

    var optionsArchive = {
        modalActionId: 'archiveHotel',
        modalActionTitle: 'Confirm Hotels Archive',
        modalActionBody: 'Confirm archive of hotels list',
        modalActionOnCLick: `id="confirm-archive-ok" onclick="window.mshMethods.archiveHotelsConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${apiRoot}/HotelsArchive`,
        actionConfirmData: {},

        modalActionedId: 'archiveHotel',
        modalActionedTitle: 'Archive Hotels',
        modalActionedBody: 'The list of hotels was successfully archived',
        modalActionedOnCLick: `onclick="window.mshMethods.archiveHotelsConfirmed()"`,

        // modalPublishedOk: 'OK',
       
    }

    var archivePair = new mas.PairOverlay(optionsArchive);

    var loadSource = '';


}(jQuery));


// Lock/Unlock
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


    var apiRoot = '/api/hotelapi';

    var options = {
        modalActionId: 'lockHotel',
        modalActionTitle: 'Lock/Unlock Hotels',
        modalActionBody: 'Confirm lock/unlock of hotels list',
        modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.lockHotelsConfirm()"`,
        modalActionOk: 'OK',
        loadConfirmApiUrl: `${apiRoot}/HotelsLoad`,

        modalActionedId: 'lockedHotel',
        modalActionedTitle: 'Load Hotels',
        modalActionedBody: 'The list of hotels was successfully loaded',
        modalActionedOnCLick: `onclick="window.mshMethods.lockHotelsConfirmed()"`,
        // modalPublishedOk: 'OK',
        modalActionedHideModalEnd: 'window.mshMethods.lockHotelsConfirmed'
    }

    function getLoadBody(optionsHtml) {
        var html = '';
        html += '<p>You can Lock or Unlock one of the records below.</p>';
        html += '<p>Select the record, and whether you want to lock or unlock.</p>';

        html += '<div class="form-group mb-3">';
        html += '<label class="form-label">Select the source to lock/unlock</label>';
        html += `<select class="form-control" id="selected-lock" >`;
        html += optionsHtml;
        html += '</select>';
        html += '</div>';
        html += '<div class="form-group mb-3">';
        html += '<input type="checkbox" id="perform-lock" class="form-check-input"/>&nbsp;'
        html += '<label class="form-label">Set to lock, clear to unlock</label>';
        html += '</div>';
        return html;
    }

    var loadPair = new mas.PairOverlay(options);

    function getLoadList() {
        api.getAsync(`${apiRoot}/HotelsArchiveSelectList`, function (data) {
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

    meth.extendMethods({

        lockHotels: function () {
            getLoadList();
        },
        lockHotelsConfirm: function () {
            var archiveCode = $('#selected-lock').val();
            var performLock = $('#perform-lock').is(':checked');
            options.actionConfirmApiUrl = `${apiRoot}/HotelsLock`;
            options.actionConfirmData = { code: archiveCode };
            loadPair.actioned(options);
        },
        lockHotelsConfirmed: function () {
            //util.redirectTo('admin/hotels/HotelsList');
        },

    });

}(jQuery));
