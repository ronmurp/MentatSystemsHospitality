(function ($) {

    "use strict";
    var app = mshPageApp;
    var routes = app.routes;
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

    var ids = {
        stayChangeHotelCode: 'stay-change-hotel-code',
        stayChangeCode: 'stay-change-code',
        stayChangeRatePlanCode: 'stay-change-rpc',
        stayChangeFrom: 'stay-change-from',
        stayChangeTo: 'stay-change-to'
    }

    var apiRoot = routes.RatePlansApi;
    var controllerRoot = routes.RatePlans;
    var listPath = `${controllerRoot}/RatePlansList`;

    pallsS.initHotelSelectEvent(listPath);

    var currentStayData = {};

    function getEditStayForm() {

        var d = currentStayData;

        var html = `<p>Change the stay date range for this rate plan.</p>`;
        html += `<div class="form-group">`;
        html += `<label class="form-control">${d.hotelCode}</label>`;
        html += `<label class="form-control mt-3">${d.ratePlanCode}</label>`;

        
        html += `<input id="${ids.stayChangeFrom}" type="date" class="form-control mt-3" onchange="window.mshMethods.changeFrom()" value="${d.stayFrom}" />`;
        html += `<input id="${ids.stayChangeTo}" type="date" class="form-control mt-3" onchange="window.mshMethods.changeTo()" value="${d.stayTo}" />`;

        html += `<input id="${ids.stayChangeHotelCode}" type="hidden"  value="${d.hotelCode}" />`;
        html += `<input id="${ids.stayChangeCode}" type="hidden"  value="${d.code}" />`;
        html += `<input id="${ids.stayChangeRatePlanCode}" type="hidden"  value="${d.ratePlanCode}" />`;
        html += `</div>`;

        return html;

    }

    function changeDate(isFrom) {
        var d = {
            dateFrom: $(`#${ids.stayChangeFrom}`).val(),
            dateTo: $(`#${ids.stayChangeTo}`).val(),
            isFrom: isFrom
        }
        var url = `${apiRoot}/ChangeDatePair`;
        api.postAsync(url, d, function (data) {
            var dates = data.data;
            $(`#${ids.stayChangeFrom}`).val(dates.dateFrom);
            $(`#${ids.stayChangeTo}`).val(dates.dateTo)
        });
    }

    meth.extendMethods({
        confirmDeleteRatePlan: function (code, hotelCode) {
            var url = `${apiRoot}/RatePlanDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo(`${listPath}?hotelCode=${hotelCode}`)
            });
        },

        deleteRatePlan: function (code, hotelCode) {

            modal.showModal('delRatePlan', "Confirm Delete", `Confirm delete of ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteRatePlan('${code}', '${hotelCode}')""`,
                okButtonText: 'OK'
            });
        },
    });

    app.itemDatesService.initDatePair('StayFrom', 'StayTo');

    app.itemDatesService.initDatePair('BookFrom', 'BookTo', 'HasBookDates');

    app.hotelActionService.init({
        deleteApi: `${apiRoot}/RatePlanDelete`,
        copyApi: `${apiRoot}/RatePlanCopy`,
        moveApi: `${controllerRoot}/RatePlanMove`,
        listPath: listPath
    });

    app.hotelActionBulkService.init({
        deleteBulkApi: `${apiRoot}/RatePlanDeleteBulk`,
        copyBulkApi: `${apiRoot}/RatePlanCopyBulk`,
        sortListApi: `${apiRoot}/RatePlansSort`,
        listPath: listPath
    });

    var editType = $('#edit-type').val();

    if (editType === "rate-plan-list") {

        var inputs = {
            model: 'RatePlans',
            name: 'Rate Plans',
            useHotelCode: true,
            apiRoot: apiRoot,
            listPath: listPath
        }

        function getHotelSelect(hotelCode) {
            var html = '<select class="form-control" id="copy-hotel">';
            $('[name="hotel-codes"]').each(function (v) {
                var hName = $(this).val();
                var hCode = $(this).attr('data-msh-option');
                if (hCode === hotelCode) {
                    html += `<option value="${hCode}" selected>${hName}</option>`;
                } else {
                    html += `<option value="${hCode}">${hName}</option>`;
                }

            });
            html += '</select>';
            return html;
        }

        pallsA.init(inputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);

        meth.extendMethods({

            // Overrides standard copy because we need a date too
            copyItem: function (code, hotelCode, baseCode) {

                var html = '<div>';
                html += '<p>Change the hotel code, or the item code, or both, to copy the record.</p>';
                var hotelSelect = getHotelSelect(hotelCode);
                html += `<div class="form-group mb-3">${hotelSelect}</div>`;
                html += `<div id="copyBaseCode" class="form-group mb-3">${code}</div>`
                html += `<div class="form-group mb-3"><input id="copyCode" class="form-control" value="${baseCode}" /></div>`,
                html += `<div class="form-group mb-3"><input id="copyDate" class="form-control" value="" placeholder="2025-01-01" /></div>`
                html += `<div id="confirm-error" class="form-group mb-3 d-none text-danger">xxx</div>`
                html += '</div>';
                modal.showModal('copyModalId', "Copy", html, {
                    footerOk: true,
                    okButtonClickScript: `onclick="window.mshMethods.confirmCopyItem('${code}', '${hotelCode}')""`,
                    okButtonText: 'OK'
                });
            },

            confirmCopyItem: function (code, hotelCode) {
                
                var newHotelCode = $('#copy-hotel').val();
                var newCode = $('#copyCode').val();
                var newDate = $('#copyDate').val();
                var copyBaseCode = $('#copyBaseCode').text();

                var url = `/api/RatePlanApi/RatePlanCopy`;
                var d = {
                    code: code,
                    hotelCode: hotelCode,
                    newCode: newCode,
                    newHotelCode: newHotelCode,
                    newDate: newDate,
                    baseCode: copyBaseCode
                }
                api.postAsync(url, d, function (data) {
                    if (!data.success) {
                        $('#confirm-error')
                            .html(data.userErrorMessage)
                            .removeClass('d-none');
                        return;
                    }
                    $('#copyModalId').remove();
                    util.redirectTo(`${listPath}?hotelCode=${hotelCode}`)
                });
            },

            ratePlanStayChange: function (hotelCode, code, ratePlanCode, stayFrom, stayTo) {

                currentStayData = {
                    hotelCode: hotelCode,
                    code: code,
                    ratePlanCode: ratePlanCode,
                    stayFrom: stayFrom,
                    stayTo: stayTo
                }

                var body = getEditStayForm();
                modal.showModal('stayEditModel', 'Change Stay Dates', body, {
                    okButtonClickScript: 'onclick="window.mshMethods.ratePlanStayChangeConfirm()"',
                    footerOk: true,
                });
            },

            ratePlanStayChangeConfirm: function () {
                var d = {
                    hotelCode: $(`#${ids.stayChangeHotelCode}`).val(),
                    code: $(`#${ids.stayChangeCode}`).val(),
                    ratePlanCode: $(`#${ids.stayChangeRatePlanCode}`).val(),
                    stayFrom: $(`#${ids.stayChangeFrom}`).val(),
                    stayTo: $(`#${ids.stayChangeTo}`).val(),
                }
                var url = `${apiRoot}/RatePlanStayChange`
                api.postAsync(url, d, function (data) {
                    if (data.success) {
                        util.redirectTo(`${listPath}?hotelCode=${d.hotelCode}`)
                    }
                });
            },

            changeFrom: function () {
                changeDate(true);
            },
            changeTo: function () {
                changeDate(false);
            }

        });
    }
    
}(jQuery));
