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

        modalActionId: 'publishDiscounts',
        modalActionTitle: 'Confirm Discounts Publish',
        modalActionBody: 'Confirm publish of discounts list',
        modalActionOnCLick: `onclick="window.mshMethods.publishDiscountsConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${hotelApi}/DiscountsPublish`,

        modalActionedId: 'publishedDiscounts',
        modalActionedTitle: 'Publish Discounts',
        modalActionedBody: 'The list of discounts was successfully published',
        modalActionedOnCLick: `onclick="window.mshMethods.publishDiscountsConfirm()"`,
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
        publishDiscounts: function () {
            var hotelCode = getHotelCode();
            options.actionConfirmApiUrl = `${hotelApi}/DiscountsPublish/${hotelCode}`;
            publishPair.action(options);
        },
        publishDiscountsConfirm: function () {
            var hotelCode = getHotelCode();
            options.actionConfirmApiUrl = `${hotelApi}/DiscountsPublish/${hotelCode}`;
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
        modalActionId: 'loadDiscounts',
        modalActionTitle: 'Confirm Discounts Load',
        modalActionBody: 'Confirm load of discounts list',
        modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.loadDiscountsConfirm()"`,
        modalActionOk: 'OK',
        loadConfirmApiUrl: `${apiRoot}/DiscountsLoad`,

        modalActionedId: 'loadedDiscounts',
        modalActionedTitle: 'Load Discounts',
        modalActionedBody: 'The list of discounts was successfully loaded',
        modalActionedOnCLick: `onclick="window.mshMethods.loadDiscountsConfirmed()"`,
        // modalPublishedOk: 'OK',
        modalActionedHideModalEnd: 'window.mshMethods.loadDiscountsConfirmed'
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
        var url = `${apiRoot}/DiscountsArchiveSelectList/${hotelCode}`;
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
        
        options.actionConfirmApiUrl = `/api/hotelapi/DiscountsLoad/${hotelCode}`;
        options.actionConfirmData = { code: archiveCode };
    }

    meth.extendMethods({
        loadDiscounts: function () {
            updateOptions();
            getLoadList(options);
        },
        loadDiscountsConfirm: function () {
            updateOptions();
            loadPair.actioned(options);
        },
        loadDiscountsConfirmed: function () {
            util.redirectTo('admin/hotels/discountslist');
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
        modalActionId: 'archiveDiscounts',
        modalActionTitle: 'Confirm Discounts Archive',
        modalActionBody: 'Confirm archive of discounts list',
        modalActionOnCLick: `id="confirm-archive-ok" onclick="window.mshMethods.archiveDiscountsConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `${apiRoot}/DiscountsArchive`,
        actionConfirmData: {},

        modalActionedId: 'archivedDiscounts',
        modalActionedTitle: 'Archive Discounts',
        modalActionedBody: 'The list of discounts was successfully archived',
        modalActionedOnCLick: `onclick="window.mshMethods.archiveDiscountsConfirmed()"`,

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
            options.actionConfirmApiUrl = `/api/hotelapi/DiscountsArchive/${hotelCode}/${archiveCode}`;
            options.actionConfirmData = null;
            archivePair.actioned(options);
        }

    });

}(jQuery));
