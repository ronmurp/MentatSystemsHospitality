(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var api = app.apiService;
      
    var hotelCode = $('#hotel-code').val();
    var code = $('#item-code').val();

    function saveSpecialRoomTypes() {
        var list = [];
        $('[name="room-types"]').each(function(index) {
            var code = $(this).attr('data-msh-code');
            var selected = $(this).is(':checked');
            if (selected) {
                list.push(code);
            }

        });
        var d = {
            hotelCode: hotelCode,
            code: code,
            codeList: list
        };
        api.postAsync(`/api/hotelapi/SpecialRoomTypesSave`, d, (data) => {
            $('#success-alert').addClass('show');
        });
    }

    meth.extendMethods({
        saveSpecialRoomTypes: saveSpecialRoomTypes
       
    });    

   

}(jQuery));
