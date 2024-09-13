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


    
    meth.extendMethods({

        confirmDeleteHotel: function (hotelCode) {
            var url = `/api/hotelapi/HotelDelete`;
            var d = {
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo('admin/hotels/HotelList')
            });
        },

        deleteHotel: function (hotelCode) {

            modal.showModal('delHotel', "Confirm Delete", `Confirm delete of ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteHotel('${hotelCode}')""`,
                okButtonText: 'OK'
            });
        },
        cancelHotelEdit: function () {
            util.redirectTo('admin/hotels/hotellist');
        },

    });

}(jQuery));