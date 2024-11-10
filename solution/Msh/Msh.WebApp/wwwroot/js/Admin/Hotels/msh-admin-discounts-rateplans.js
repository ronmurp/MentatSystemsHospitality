(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var api = app.apiService;

    $('#save-button').on('click', function () {
        var selected = [];
        $('input[name="select-codes"]:checked').each(function () {
            // Get the value of the "data-msh-code" attribute and push it to the array
            selected.push($(this).attr('data-msh-code'));
        });

       
        var postData = {
            codeList: selected,
            hotelCode: $('#hotel-code').val(),
            code: $('#item-code').val(),
        }

        var url = $('#save-api-url').val();
        api.post(url, postData, function (data) {
            var y = data;
        });

    });

}(jQuery));