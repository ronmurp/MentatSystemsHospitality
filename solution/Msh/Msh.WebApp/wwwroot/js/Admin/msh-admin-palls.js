(function ($) {
    "use strict";

    if (!window.mshPageApp)
        window.mshPageApp = function () { };

    window.mshPageApp.pallSupportService = (function () {

   
        function isValidPathCode(code) {
            const regex = /^[a-zA-Z0-9\-_\.]+$/;
            return regex.test(code);
        }

        return {
            apiRoot: '/api/hotelapi',
            getHotelCode: function() {
                var hotelCode = $('#selectHotel').val();
                hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
                return hotelCode;
            },
            getArchiveBody: function (options) {
                var html = '';
                if (options && options.modalActionBody) {                   
                    html += options.modalActionBody;                
                }
                html += '<div class="form-group mb-3">';
                html += `<p>Enter a name for the archived data.</p>`
                html += '<input type="text" id="archive-code" class="form-control" /><br />'
                html += `<label>Notes</label><br />`
                html += '<textarea id="archive-notes" class="form-control" ></textarea>'
                html += '</div>';
                return html;
            },

            getDeleteBody: function (optionsHtml) {
                var html = '';
             
                html += '<div class="form-group mb-3">';
                html += '<label class="form-label">Select the archive to delete</label>';
                html += `<select class="form-control" id="selected-load" >`;
                html += optionsHtml;
                html += '</select>';
                html += '</div>';
                
                return html;
            },


            getPublishBody: function (options) {
                var html = '';
                if (options && options.modalActionBody) {
                    html += options.modalActionBody;
                }
                html += '<div class="form-group mb-3">';
                html += `<label>Notes</label><br />`
                html += '<textarea id="publish-notes" class="form-control" ></textarea>'
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
            },

            isValidPathCode: isValidPathCode
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

        var initData = {
            model: 'RatePlans',
            name: 'Rate Plans',
            useHotelCode: true,
            modalActionBody: '',
            apiRoot: pallss.apiRoot
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {
                modalActionId: `archive${initData.model}`,
                modalActionTitle: `Confirm ${initData.name} Archive`,
                modalActionBody: initData.modalActionBody ? initData.modalActionBody : '',
                modalActionOnCLick: `id="confirm-archive-ok" onclick="window.mshMethods.archiveDataConfirm()"`,
                modalActionOk: 'OK',
                actionConfirmApiUrl: `${initData.apiRoot}/${initData.model}Archive`,
                actionConfirmData: {},

                modalActionedId: `archived${initData.model}`,
                modalActionedTitle: `Archive ${initData.name}`,
                modalActionedBody: `The list of ${initData.name} was successfully archived`,
                modalActionedOnCLick: `onclick="window.mshMethods.archiveDataConfirmed()"`,
            }

            var archivePair = new mas.PairOverlay(options);

            meth.extendMethods({
                archiveData: function () {
                    options.modalActionBody = initData.modalActionBody ? initData.modalActionBody : '',
                    options.modalActionBody = pallss.getArchiveBody(options);
                    archivePair.action(options);
                },
                archiveDataConfirm: function () {
                    var archiveCode = $('#archive-code').val();
                    if (!pallss.isValidPathCode(archiveCode)) {
                        modal.showModal('invalid-code', "Invalid Code", "The code is invalid");
                        return;
                    }
                    var archiveNotes = $('#archive-notes').val();
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';
                    var url = initData.useHotelCode
                        ? `${initData.apiRoot}/${initData.model}Archive/${hotelCode}/${archiveCode}`
                        : `${initData.apiRoot}/${initData.model}Archive/${archiveCode}`;
                    options.actionConfirmApiUrl = url;
                    options.actionConfirmData = { notes: archiveNotes };

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
            useHotelCode: true,
            modalActionBody: '',
            apiRoot: pallss.apiRoot
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {

                modalActionId: `publish${initData.model}`,
                modalActionTitle: `Confirm ${initData.name} Publish`,
                modalActionBody: initData.modalActionBody ? initData.modalActionBody : '',
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
                    options.modalActionBody = initData.modalActionBody ? initData.modalActionBody : '',
                    options.modalActionBody = pallss.getPublishBody(options);
                    var url = initData.useHotelCode
                        ? `${initData.apiRoot}/${initData.model}Publish/${hotelCode}`
                        : `${initData.apiRoot}/${initData.model}Publish`
                    options.actionConfirmApiUrl = url;
                    publishPair.action(options);              
                },
                publishDataConfirm: function () {
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';
                    var publishNotes = $('#publish-notes').val();
                    var url = initData.useHotelCode
                        ? `${initData.apiRoot}/${initData.model}Publish/${hotelCode}`
                        : `${initData.apiRoot}/${initData.model}Publish`
                    options.actionConfirmApiUrl = url;
                    options.actionConfirmData = { notes: publishNotes };

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
            useHotelCode: true,
            apiRoot: pallss.apiRoot
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {

                modalActionId: `load${initData.model}`,
                modalActionTitle: `Confirm ${initData.name} Load`,
                modalActionBody: `Confirm load of ${initData.name} list`,
                modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.loadDataConfirm()"`,
                modalActionOk: 'OK',
                loadConfirmApiUrl: `${initData.apiRoot}/${initData.model}Load`,

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
                    ? `${initData.apiRoot}/${initData.model}ArchiveSelectList/${hotelCode}`
                    : `${initData.apiRoot}/${initData.model}ArchiveSelectList/`;
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
                    ? `${initData.apiRoot}/${initData.model}Load/${hotelCode}`
                    : `${initData.apiRoot}/${initData.model}Load`;
                options.actionConfirmApiUrl = url;
                options.actionConfirmData = { code: archiveCode, hotelCode: hotelCode };
            }

            var loadPair = new mas.PairOverlay(options);

            meth.extendMethods({
                loadData: function () {
                    updateOptions();
                    getLoadList(options);
                },
                loadDataConfirm: function () {             
                    updateOptions();
                    $(`#${options.modalActionId}`).remove();
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


    window.mshPageApp.pallsDeleteService = (function () {

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
            useHotelCode: true,
            apiRoot: pallss.apiRoot
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {

                modalActionId: `delete${initData.model}`,
                modalActionTitle: `Confirm ${initData.name} Archive Delete`,
                modalActionBody: `Confirm delete of ${initData.name} archive`,
                modalActionOnCLick: `id="confirm-delete-ok" onclick="window.mshMethods.deleteDataConfirm()"`,
                modalActionOk: 'OK',
                loadConfirmApiUrl: `${initData.apiRoot}/${initData.model}Delete`,

                modalActionedId: `deleted${initData.model}`,
                modalActionedTitle: `Delete ${initData.name}`,
                modalActionedBody: `The list of ${initData.name} archive was successfully deleted`,
                modalActionedOnCLick: `onclick="window.mshMethods.deleteDataConfirmed()"`,
                // modalPublishedOk: 'OK',
                modalActionedHideModalEnd: `window.mshMethods.deleteDataConfirmed`
            }

            function getLoadList() {

                var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';

                var url = initData.useHotelCode
                    ? `${initData.apiRoot}/${initData.model}ArchiveSelectList/${hotelCode}`
                    : `${initData.apiRoot}/${initData.model}ArchiveSelectList/`;
                api.getAsync(url, function (data) {
                    if (data.success) {
                        var list = data.data;
                        // For deletes, remove the Pub option
                        if (list.length === 1 && list[0].value === 'Pub') {
                            modal.showModal('delete-archive', 'Delete Archive', 'There are no archived records to delete.');
                            return;
                        }
                        var optionsHtml = '';
                        optionsHtml += `<option value="">Select ...</option>`
                        list.forEach((v) => {
                            if(v.value !== 'Pub')
                                optionsHtml += `<option value="${v.value}">${v.text}</option>`
                        });
                        options.modalActionBody = pallss.getDeleteBody(optionsHtml);
                        loadPair.action(options);
                        return;
                    } else {
                        modal.showError(data.userErrorMessage);
                    }
                })
            }

            function updateOptions() {
                var archiveCode = $('#selected-load').val();
                if (!archiveCode) {
                    return;
                }
                var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : '';
                var url = initData.useHotelCode
                    ? `${initData.apiRoot}/${initData.model}ArchiveDelete/${hotelCode}/${archiveCode}`
                    : `${initData.apiRoot}/${initData.model}ArchiveDelete/${archiveCode}`;
                options.actionConfirmApiUrl = url;
                options.actionConfirmData = undefined;
            }

            var loadPair = new mas.PairOverlay(options);

            meth.extendMethods({
                deleteData: function () {
                    updateOptions();
                    getLoadList(options);
                },
                deleteDataConfirm: function () {
                    updateOptions();
                    $(`#${options.modalActionId}`).remove();
                    loadPair.actioned(options);
                },
                deleteDataConfirmed: function () {
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
            useHotelCode: true,
            apiRoot: pallss.apiRoot
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {
                modalActionId: `lock${initData.model}`,
                modalActionTitle: `Lock/Unlock ${initData.name}`,
                modalActionBody: `Confirm lock/unlock of ${initData.name} list`,
                modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.lockDataConfirm()"`,
                modalActionOk: `OK`,
                loadConfirmApiUrl: `${initData.apiRoot}/${initData.model}Load`,

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
                    ? `${initData.apiRoot}/${initData.model}ArchiveSelectList/${hotelCode}`
                    : `${initData.apiRoot}/${initData.model}ArchiveSelectList`;
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
                        ? `${initData.apiRoot}/${initData.model}Lock/${hotelCode}`
                        : `${initData.apiRoot}/${initData.model}Lock`;
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

    window.mshPageApp.pallsImportService = (function () {

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
            useHotelCode: true,
            apiRoot: pallss.apiRoot
        }
        function init(inputs) {

            initData = $.extend({}, initData, inputs);

            var options = {
                modalActionId: `import${initData.model}`,
                modalActionTitle: `Import ${initData.name}`,
                modalActionBody: `Confirm import of ${initData.name} list`,
                modalActionOnCLick: `id="confirm-import-ok" onclick="window.mshMethods.importDataConfirm()"`,
                modalActionOk: `OK`,
                loadConfirmApiUrl: `${initData.apiRoot}/${initData.model}Import`,

                modalActionedId: `imported${initData.model}`,
                modalActionedTitle: `Import ${initData.name}`,
                modalActionedBody: `The import of ${initData.name} was successful.`,
                modalActionedOnCLick: `onclick="window.mshMethods.importDataConfirmed()"`,
                // modalPublishedOk: `OK`,
                modalActionedHideModalEnd: `window.mshMethods.importDataConfirmed`,

                confirmedRedirect: initData.confirmedRedirect,
                confirmedRedirectUrl: initData.confirmedRedirectUrl
            }

            function getImportBody(optionsHtml) {
                var html = ``;
                html += `<p>Edit records are locked automatically when imported, to prvent accidental overwrite from another import.</p>`;
               

                html += `<div class="form-group mb-3">`;
               
                html += `</div>`;
                html += `<div class="form-group mb-3">`;
               
                html += `</div>`;
                return html;
            }

            function getImport(useHotel) {
                var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : ``;
                var url = initData.useHotelCode
                    ? `${initData.apiRoot}/${initData.model}ArchiveSelectList/${hotelCode}`
                    : `${initData.apiRoot}/${initData.model}ArchiveSelectList`;
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

                importData: function (useHotel) {
                    //getLoadList(useHotel);
                    initData.modalActionBody = getImportBody(undefined);
                    loadPair.action(options);
                },
                importDataConfirm: function (useHotel) {
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : ``;
                  
                    $(`#${options.modalActionId}`).remove();
                    var url = initData.useHotelCode
                        ? `${initData.apiRoot}/${initData.model}Import/${hotelCode}`
                        : `${initData.apiRoot}/${initData.model}Import`;
                    options.actionConfirmApiUrl = url;
                    options.actionConfirmData = {  };
                    loadPair.actioned(options);
                },
                importDataConfirmed: function () {
                    var hotelCode = initData.useHotelCode ? pallss.getHotelCode() : ``;
                    if (options.confirmedRedirect) {
                        var url = initData.useHotelCode
                            ? `${options.confirmedRedirectUrl}?hotelCode=${hotelCode}`
                            : `${options.confirmedRedirectUrl}`;
                        util.redirectTo(url);
                    }
                },

            });
        }

        return {
            init: init
        }

    }());

}(jQuery));