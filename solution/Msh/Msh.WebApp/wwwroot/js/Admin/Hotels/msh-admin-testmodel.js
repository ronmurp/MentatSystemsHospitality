(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;
    var modal = app.modalService;

    var props = [];

    meth.extendMethods({

        confirmDeleteTestModel: function (code) {
            var url = `TestModelDelete?code=${code}`;

            api.postAsync(url, null, function (data) {

                util.redirectTo('admin/hotels/TestModelList')
            });
        },

        deleteTestModel: function (code) {

            modal.showModal('delTestModel', "Confirm Delete", `Confirm delete of ${code}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteTestModel('${code}')""`,
                okButtonText: 'OK'
            });

    
        },

    });


}(jQuery));