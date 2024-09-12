(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    meth.extendMethods({

        confirmDeleteExtra: function (code, hotelCode) {
            var url = `/api/hotelapi/ExtraDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo('admin/hotels/ExtrasList')
            });
        },

        deleteExtra: function (code, hotelCode) {

            modal.showModal('delExtra', "Confirm Delete", `Confirm delete of ${code} ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteExtra('${code}', '${hotelCode}')""`,
                okButtonText: 'OK'
            });
        },

    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/ExtrasList?hotelCode=${hotelCode}`);
    });



}(jQuery));