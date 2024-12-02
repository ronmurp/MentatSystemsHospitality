(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var pallsA = app.pallsArchiveService;
    var pallsP = app.pallsPublishService;
    var pallsD = app.pallsDeleteService;
    var pallsLoad = app.pallsLoadService;
    var pallsLock = app.pallsLockService;

    var ids = {
        selectHotel: '#selectHotel',

        stayChangeHotelCode: 'stay-change-hotel-code',
        stayChangeCode: 'stay-change-code',
        stayChangeRatePlanCode: 'stay-change-rpc',
        stayChangeFrom: 'stay-change-from',
        stayChangeTo: 'stay-change-to'
    }

    var apiRoot = `/api/rateplanapi`;
    var listPath = 'admin/hotels/RatePlansList';

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

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
        var url = `/api/hotelapi/ChangeDatePair`;
        api.postAsync(url, d, function (data) {
            var dates = data.data;
            $(`#${ids.stayChangeFrom}`).val(dates.dateFrom);
            $(`#${ids.stayChangeTo}`).val(dates.dateTo)
        });
    }

    meth.extendMethods({
        confirmDeleteRatePlan: function (code, hotelCode) {
            var url = `/api/hotelapi/RatePlanDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo(`admin/hotels/RatePlansList?hotelCode=${hotelCode}`)
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

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`${listPath}?hotelCode=${hotelCode}`);
    });

    app.itemDatesService.initDatePair('StayFrom', 'StayTo');
    app.itemDatesService.initDatePair('BookFrom', 'BookTo', 'HasBookDates');

    app.hotelActionService.init({
        deleteApi: `${apiRoot}/RatePlanDelete`,
        copyApi: `${apiRoot}/RatePlanCopy`,
        moveApi: `${apiRoot}/RatePlanMove`,
        listPath: `${apiRoot}/RatePlansList`
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

        pallsA.init(inputs);
        pallsP.init(inputs);
        pallsD.init(inputs);
        pallsLoad.init(inputs);
        pallsLock.init(inputs);

        meth.extendMethods({

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
                        util.redirectTo(`admin/hotels/RatePlansList?hotelCode=${d.hotelCode}`)
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
