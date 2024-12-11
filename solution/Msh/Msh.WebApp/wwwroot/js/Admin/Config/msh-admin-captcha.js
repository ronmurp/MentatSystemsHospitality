(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var pallsS = app.pallSupportService;
    var pallsA = app.pallsArchiveService;
    var pallsP = app.pallsPublishService;
    var pallsD = app.pallsDeleteService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;
    var pallsI = app.pallsImportService;

    var itemDatesService = app.itemDatesService;

    var apiRoot = '/api/captchaapi';
    var listPath = 'admin/captcha/config';

  
    var editType = $('#edit-type').val();

    if (editType === "captcha-edit") {

        var inputs = {
            model: 'CaptchaConfig',
            name: 'Captcha Config',
            useHotelCode: false,
            confirmedRedirect: true,
            confirmedRedirectUrl: listPath,
            apiRoot: apiRoot
        }

        // How to add a custom body text to the standard body (code and notes)
        var archiveInputs = $.extend({}, inputs, { modalActionBody: '<p class="mb-2">Test modalActionBody in msh-admin-captcha.js</p>' } );

        pallsA.init(archiveInputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);
        pallsI.init(inputs);

    }

}(jQuery));
