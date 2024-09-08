(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;

  
    var props = [];

    meth.extendMethods({

        addTestModel: function () {
            util.redirectTo('admin/hotels/TestModelAdd');
        },
        editTestModel: function (hotelCode) {
            util.redirectTo(`admin/hotels/TestModelEdit?hotelCode=${hotelCode}`);
        },
        deleteTestModel: function (hotelCode) {
            if (confirm) {
                util.redirectTo(`admin/hotels/TestModelDelete?hotelCode=${hotelCode}`);
            }
        },
        cancelTestModel: function () {
            util.redirectTo('admin/hotels/testmodellist')
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