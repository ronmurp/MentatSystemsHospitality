
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


    var hotelApi = '/api/hotelapi/';

    var currentHotelCode = '';  

    app.hotelActionService.init({
        deleteApi: '/api/hotelapi/HotelDelete',
        copyApi: '/api/hotelapi/HotelCopy',
        moveApi: '/admin/hotels/HotelMove',
        listPath: 'admin/hotels/HotelList',
        codeOnly: true
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: '/api/hotelapi/HotelDeleteBulk',
        copyBulkApi: '/api/hotelapi/HotelCopyBulk',
        sortListApi: '/api/hotelapi/HotelsSort',
        listPath: 'admin/hotels/HotelList',
        includeBulkCopy: false
    });

 
    var optionsPublish = {

        modalActionId: 'publishHotel',
        modalActionTitle: 'Confirm Hotels Publish',
        modalActionBody: 'Confirm publish of hotels list',
        modalActionOnCLick: `onclick="window.mshMethods.publishHotelsConfirm()"`,
        modalActionOk: 'OK',
        actionConfirmApiUrl: `/api/hotelapi/HotelsPublish`,

        modalActionedId: 'publishedHotel',
        modalActionedTitle: 'Publish Hotels',
        modalActionedBody: 'The list of hotels was successfully published',
        modalActionedOnCLick: `onclick="window.mshMethods.publishHotelsConfirm()"`,
        modalActionedOk: 'OK',
        actionedConfirmApiUrl: ``,
    }

    var optionsLoad = {
        modalActionId: 'loadHotel',
        modalActionTitle: 'Confirm Hotels Load',
        modalActionBody: 'Confirm load of hotels list',
        modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.loadHotelsConfirm()"`,
        modalActionOk: 'OK',
        loadConfirmApiUrl: `/api/hotelapi/HotelsLoad`,

        modalActionedId: 'loadedHotel',
        modalActionedTitle: 'Load Hotels',
        modalActionedBody: 'The list of hotels was successfully loaded',
        modalActionedOnCLick: `onclick="window.mshMethods.loadHotelsConfirmed()"`,
        // modalPublishedOk: 'OK',
    }

    function getBody() {
        var html = '';
        html += '<p>Select the source to load</p>'
        html += '<div class="form-group mb-3">';
        html += `<select class="form-control" id="selected-load" >`;
        html += `<option value="Pub">Published</option>`;
        html += '</select>';
        html += '</div>';
        return html;
    }

    optionsLoad.modalActionBody = getBody();

    var publishPair = new mas.PairOverlay(optionsPublish);
    var loadPair = new mas.PairOverlay(optionsLoad);

    var loadSource = '';

    meth.extendMethods({
        publishHotels: function () {
            publishPair.action();
        },
        publishHotelsConfirm: function () {
            
            publishPair.actioned();
        },
        loadHotels: function () {
            loadPair.action();
        },
        loadHotelsConfirm: function () {
            loadSource = $('#selected-load').val();
            $('#loadHotel').remove();
            api.postAsync(`/api/hotelapi/HotelsLoad`, { code: loadSource }, function (data) {
                if (data.success) {
                    modal.showModal(optionsLoad.modalActionedId, optionsLoad.modalActionedTitle, optionsLoad.modalActionedBody)
                    util.redirectTo('admin/hotels/hotellist')
                    return;
                } else {
                    modal.showError(data.userErrorMessage);
                }
            })
        },
        loadHotelsConfirmed: function () {

        }

    });

}(jQuery));