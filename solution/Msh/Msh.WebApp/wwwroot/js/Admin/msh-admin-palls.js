(function ($) {
    "use strict";

    if (!window.mshPageApp)
        window.mshPageApp = function () { };

    window.mshPageApp.pallSupportService = (function () {


        return {
            apiRoot: '/api/hotelapi',
            getHotelCode: function() {
                var hotelCode = $('#selectHotel').val();
                hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
                return hotelCode;
            },
            getArchiveBody: function () {
                var html = '';
                html += '<div class="form-group mb-3">';
                html += '<input type="text" id="archive-code" class="form-control" />'
                html += '</div>';
                return html;
            },

            getLoadBody: function (optionsHtml) {
                var html = '';
                html += '<p>You can load data from the published content into the editing table, where you can make changes before re-publishing.</p>';
                html += '<p>Only published configuration data is used in customer facing operations.</p>';

                html += '<div class="form-group mb-3">';
                html += '<label class="form-label">Select the source to load</label>';
                html += `<select class="form-control" id="selected-load" >`;
                html += optionsHtml;
                html += '</select>';
                html += '</div>';
                return html;
            }
        }

    }());

    window.mshPageApp.pallsArchiveService = (function () {

        "use strict";

        var app = mshPageApp;
        var meth = app.methodsService;
        var util = app.utilityService;
        var api = app.apiService;
        var mom = app.momentDateService;
        var htmlS = app.htmlService;
        var modal = app.modalService;
        var mas = app.modalActionService;
        var pallss = app.pallSupportService;

        var apiRoot = '/api/hotelapi';

        var initData = {
            model: 'RatePlans',
            name: 'Rate Plans',
            useHotelCode: true
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {
                modalActionId: `archive${initData.model}`,
                modalActionTitle: `Confirm ${initData.name} Archive`,
                modalActionBody: `Confirm archive of ${initData.name} list`,
                modalActionOnCLick: `id="confirm-archive-ok" onclick="window.mshMethods.archiveDataConfirm()"`,
                modalActionOk: 'OK',
                actionConfirmApiUrl: `${apiRoot}/${initData.model}Archive`,
                actionConfirmData: {},

                modalActionedId: `archived${initData.model}`,
                modalActionedTitle: `Archive ${initData.name}`,
                modalActionedBody: `The list of ${initData.name} was successfully archived`,
                modalActionedOnCLick: `onclick="window.mshMethods.archiveDataConfirmed()"`,
            }

            var archivePair = new mas.PairOverlay(options);

            meth.extendMethods({
                archiveData: function () {
                    options.modalActionBody = pallss.getArchiveBody();
                    archivePair.action(options);
                },
                archiveDataConfirm: function () {
                    var archiveCode = $('#archive-code').val();
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';
                    var url = initData.useHotelCode
                        ? `${pallss.apiRoot}/${initData.model}Archive/${hotelCode}/${archiveCode}`
                        : `${pallss.apiRoot}/${initData.model}Archive/${archiveCode}`;
                    options.actionConfirmApiUrl = url;
                    options.actionConfirmData = null;

                    $(`#${options.modalActionId}`).remove();

                    archivePair.actioned(options);
                }
            });
        }

        return {
            init: init
        }

    }());

    window.mshPageApp.pallsPublishService = (function () {

        "use strict";

        var app = mshPageApp;
        var meth = app.methodsService;
        var util = app.utilityService;
        var api = app.apiService;
        var mom = app.momentDateService;
        var htmlS = app.htmlService;
        var modal = app.modalService;
        var mas = app.modalActionService;
        var pallss = app.pallSupportService;

        var initData = {
            model: 'RatePlans',
            name: 'Rate Plans',
            useHotelCode: true
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {

                modalActionId: `publish${initData.model}`,
                modalActionTitle: `Confirm ${initData.name} Publish`,
                modalActionBody: `Confirm publish of ${initData.name} list`,
                modalActionOnCLick: `onclick="window.mshMethods.publishDataConfirm()"`,
                modalActionOk: 'OK',
                actionConfirmApiUrl: '', //`${hotelApi}/${initData.model}Publish`,

                modalActionedId: `published${initData.model}`,
                modalActionedTitle: `Publish ${initData.name}`,
                modalActionedBody: `The list of ${initData.name} was successfully published`,
                modalActionedOnCLick: `onclick="window.mshMethods.publishDataConfirm()"`,
                modalActionedOk: 'OK',
                actionedConfirmApiUrl: ``,
                currentHotelCode: ''
            }

            var publishPair = new mas.PairOverlay(options);

            meth.extendMethods({
                publishData: function () {
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';
                    var url = initData.useHotelCode
                        ? `${pallss.apiRoot}/${initData.model}Publish/${hotelCode}`
                        : `${pallss.apiRoot}/${initData.model}Publish`
                    options.actionConfirmApiUrl = url;
                    publishPair.action(options);              
                },
                publishDataConfirm: function () {
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';
                    var url = initData.useHotelCode
                        ? `${pallss.apiRoot}/${initData.model}Publish/${hotelCode}`
                        : `${pallss.apiRoot}/${initData.model}Publish`
                    options.actionConfirmApiUrl = url;

                    $(`#${options.modalActionId}`).remove();

                 
                    publishPair.actioned(options);
                   
                }
            });
        }

        return {
            init: init
        }

    }());

    window.mshPageApp.pallsLoadService = (function () {

        "use strict";

        var app = mshPageApp;
        var meth = app.methodsService;
        var util = app.utilityService;
        var api = app.apiService;
        var mom = app.momentDateService;
        var htmlS = app.htmlService;
        var modal = app.modalService;
        var mas = app.modalActionService;
        var pallss = app.pallSupportService;

        var initData = {
            model: 'RatePlans',
            name: 'Rate Plans',
            useHotelCode: true
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {

                modalActionId: `load${initData.model}`,
                modalActionTitle: `Confirm ${initData.name} Load`,
                modalActionBody: `Confirm load of ${initData.name} list`,
                modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.loadDataConfirm()"`,
                modalActionOk: 'OK',
                loadConfirmApiUrl: `${pallss.apiRoot}/${initData.model}Load`,

                modalActionedId: `loaded${initData.model}`,
                modalActionedTitle: `Load ${initData.name}`,
                modalActionedBody: `The list of ${initData.name} was successfully loaded`,
                modalActionedOnCLick: `onclick="window.mshMethods.loadDataConfirmed()"`,
                // modalPublishedOk: 'OK',
                modalActionedHideModalEnd: `window.mshMethods.loadDataConfirmed`
            }

            function getLoadList() {

                var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';

                var url = initData.useHotelCode
                    ? `${pallss.apiRoot}/${initData.model}ArchiveSelectList/${hotelCode}`
                    : `${pallss.apiRoot}/${initData.model}ArchiveSelectList/`;
                api.getAsync(url, function (data) {
                    if (data.success) {
                        var list = data.data;
                        var optionsHtml = '';
                        list.forEach((v) => {
                            optionsHtml += `<option value="${v.value}">${v.text}</option>`
                        });
                        options.modalActionBody = pallss.getLoadBody(optionsHtml);
                        loadPair.action(options);
                        return;
                    } else {
                        modal.showError(data.userErrorMessage);

                    }
                })
            }

            function updateOptions() {
                var archiveCode = $('#selected-load').val();
                var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';
                var url = initData.useHotelCode
                    ? `${pallss.apiRoot}/${initData.model}Load/${hotelCode}`
                    : `${pallss.apiRoot}/${initData.model}Load`;
                options.actionConfirmApiUrl = url;
                options.actionConfirmData = { code: archiveCode };
            }

            var loadPair = new mas.PairOverlay(options);

            meth.extendMethods({
                loadData: function () {
                    updateOptions();
                    getLoadList(options);
                },
                loadDataConfirm: function () {
                    $(`#${options.modalActionId}`).remove();
                    updateOptions();
                    loadPair.actioned(options);
                },
                loadDataConfirmed: function () {
                    //util.redirectTo(`admin/hotels/${initData.model}List`);
                },
            });
        }

        return {
            init: init
        }

    }());

    window.mshPageApp.pallsLockService = (function () {

        "use strict";

        var app = mshPageApp;
        var meth = app.methodsService;
        var util = app.utilityService;
        var api = app.apiService;
        var mom = app.momentDateService;
        var htmlS = app.htmlService;
        var modal = app.modalService;
        var mas = app.modalActionService;
        var pallss = app.pallSupportService;

        var initData = {
            model: 'RatePlans',
            name: 'Rate Plans',
            useHotelCode: true
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {
                modalActionId: `lock${initData.model}`,
                modalActionTitle: `Lock/Unlock ${initData.name}`,
                modalActionBody: `Confirm lock/unlock of ${initData.name} list`,
                modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.lockDataConfirm()"`,
                modalActionOk: `OK`,
                loadConfirmApiUrl: `${pallss.apiRoot}/${initData.model}Load`,

                modalActionedId: `locked${initData.model}`,
                modalActionedTitle: `Lock/Unlock ${initData.name}`,
                modalActionedBody: `The lock on ${initData.name} was successfully changed`,
                modalActionedOnCLick: `onclick="window.mshMethods.lockDataConfirmed()"`,
                // modalPublishedOk: `OK`,
                modalActionedHideModalEnd: `window.mshMethods.lockDataConfirmed`
            }

            function getLoadBody(optionsHtml) {
                var html = ``;
                html += `<p>Published and Archived records are locked automatically when saved, to prvent accidental overwrite.</p>`;
                html += `<p>You can Lock or Unlock one of the records below.</p>`;
                html += `<p>Select the record, and whether you want to lock or unlock.</p>`;

                html += `<div class="form-group mb-3">`;
                html += `<label class="form-label">Select the source to lock/unlock</label>`;
                html += `<select class="form-control" id="selected-lock" >`;
                html += optionsHtml;
                html += `</select>`;
                html += `</div>`;
                html += `<div class="form-group mb-3">`;
                html += `<input type="checkbox" id="perform-lock" class="form-check-input"/>&nbsp;`
                html += `<label class="form-label">Set to lock, clear to unlock</label>`;
                html += `</div>`;
                return html;
            }

            function getLoadList(useHotel) {
                var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : ``;
                var url = initData.useHotelCode
                    ? `${pallss.apiRoot}/${initData.model}ArchiveSelectList/${hotelCode}`
                    : `${pallss.apiRoot}/${initData.model}ArchiveSelectList`;
                api.getAsync(url, function (data) {
                    if (data.success) {
                        var list = data.data;
                        var optionsHtml = ``;
                        list.forEach((v) => {
                            optionsHtml += `<option value="${v.value}">${v.text}</option>`
                        });
                        options.modalActionBody = getLoadBody(optionsHtml);
                        loadPair.action(options);
                        return;
                    } else {
                        modal.showError(data.userErrorMessage);

                    }
                })
            }


            var loadPair = new mas.PairOverlay(options);

           
            meth.extendMethods({

                lockData: function (useHotel) {
                    getLoadList(useHotel);
                },
                lockDataConfirm: function (useHotel) {
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : ``;
                    var archiveCode = $(`#selected-lock`).val();
                    var performLock = $(`#perform-lock`).is(`:checked`);
                    $(`#${options.modalActionId}`).remove();
                    var url = initData.useHotelCode
                        ? `${pallss.apiRoot}/${initData.model}Lock/${hotelCode}`
                        : `${pallss.apiRoot}/${initData.model}Lock`;
                    options.actionConfirmApiUrl = url;
                    options.actionConfirmData = { code: archiveCode, isTrue: performLock };
                    loadPair.actioned(options);
                },
                lockDataConfirmed: function () {
                    //util.redirectTo('admin/hotels/HotelsList');
                },

            });        }

        return {
            init: init
        }

    }());


}(jQuery));