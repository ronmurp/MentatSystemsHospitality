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
        confirmDeleteRoomType: function (code, hotelCode) {
            var url = `/api/hotelapi/RoomTypeDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo('admin/hotels/RoomTypeList')
            });
        },

        deleteRoomType: function (code, hotelCode) {

            modal.showModal('delRoomType', "Confirm Delete", `Confirm delete of ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteRoomType('${code}', '${hotelCode}')""`,
                okButtonText: 'OK'
            });
        },

    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/RoomTypeList?hotelCode=${hotelCode}`);
    });



}(jQuery));