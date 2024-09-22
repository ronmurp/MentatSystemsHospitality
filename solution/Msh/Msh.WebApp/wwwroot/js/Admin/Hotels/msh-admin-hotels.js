(function ($) {

    "use strict";

    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;
    var mom = app.momentDateService;
    var htmlS = app.htmlService;
    var modal = app.modalService;

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

    meth.extendMethods({
        publishHotelsConfirm: function () {
            $('#pubHotel').remove();
            
            api.postAsync('/api/hotelapi/HotelsPublish', {}, function (data) {
                if (data.success) {
                    modal.showModal('published', 'Publish Hotels', 'The list of hotels was successfully published.')
                    return;
                } else {
                    modal.showError(data.userErrorMessage);
                }
            })
        },
        publishHotels: function () {
            modal.showModal('pubHotel', "Confirm Hotels Publish", `Confirm publish of hotels list`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.publishHotelsConfirm()"`,
                okButtonText: 'OK'
            });
        }
    });


}(jQuery));