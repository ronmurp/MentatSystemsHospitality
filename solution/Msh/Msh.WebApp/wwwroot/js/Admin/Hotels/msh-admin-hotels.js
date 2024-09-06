(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;

    var hotelApi = '/api/hotelapi/';

    var props = [];

    meth.extendMethods({

        addHotel: function () {
            var path = `${window.location.origin}/admin/hotels/HotelEditByCode?hotelCode=&action=add`;
            window.location.assign(path);
        },
        editHotel: function (hotelCode) {
            var path = `${window.location.origin}/admin/hotels/HotelEditByCode?hotelCode=${hotelCode}&action=edit`;
            window.location.assign(path);
        },
        deleteHotel: function (hotelCode) {
            var path = `${window.location.origin}/admin/hotels/HotelEditByCode?hotelCode=${hotelCode}&action=delete`;
            window.location.assign(path);
        },
        cancelHotelEdit: function () {
            var path = `${window.location.origin}/admin/hotels/hotellist`;
            window.location.assign(path);
        },
        getFormData: function () {
            $('#main-form').validate();
            var data = util.getFormData();
            var obj = {};
            props.forEach((v) => {
                var value = $(`#${v.name}`).val();
                switch (v.dataType) {
                    case "System.Boolean":
                        value = value === 'true' ? true : false;
                        break;
                }
                obj[v.name] = value;
            });
            var url = `${hotelApi}hotelsave`;
            
            api.postAsync(url, data, function (d) {
                var x = d;
            });
        }

    });

    function loadConfig() {
        api.getAsync(`${hotelApi}hotelConfig`, (data) => {
            if (data.success) {
                props = data.data;
            }
        });
    }

    loadConfig();

}(jQuery));