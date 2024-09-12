(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;
    var mom = app.momentDateService;
    var htmlS = app.htmlService;

    var currentCode = '';
    var currentHotelCode = '';
    var itemDates = [];

    var itemDate = {
        enabled: false,
        fromDate: mom.today().format(mom.YMD),
        toDate: mom.today().format(mom.YMD),
    }

    function getTableHtml(itemDates) {

        var headArray = [
            'Enabled', 'From', 'To',
            htmlS.cellIcons([
                `<a href="javascript:window.mshMethods.addItemDates()"><i class="fa-solid fa-plus"></i></a>`
            ])
        ];

        var bodyArray = [];
        var i = 0;
        itemDates.forEach((v) => {

            var checked = v.enabled ? `checked` : ``;
         
            var rowArray = [
                `<input type="checkbox" id="Enabled-${i}" data-msh-index="${i}" name="Enabled" ${checked} />`,
                `<input type="date" id="FromDate-${i}" data-msh-index="${i}" name="FromDate" value="${v.fromDate}" />`,
                `<input type="date" id="ToDate-${i}" data-msh-index="${i}" name="ToDate" value="${v.toDate}" />`,
              
                htmlS.cellIcons([
                    `<a href="javascript:window.mshMethods.deleteItemDates(${i})"><i class="fa-solid fa-times"></i></a>`
                ]),

            ];
            bodyArray.push(rowArray);

            i++;
        });

        var html = htmlS.table(headArray, bodyArray);

        return html;

    }

    function changeClass(me) {
        var index = $(me).attr('data-msh-index')
        if ($(`#Enabled-${index}`).is(':checked')) {
            $(`#FromDate-${index}`).removeClass('dim-input');
            $(`#ToDate-${index}`).removeClass('dim-input');
        }
        else {
            $(`#FromDate-${index}`).addClass('dim-input');
            $(`#ToDate-${index}`).addClass('dim-input');
        }
    }


    function updateDates(me, isFrom) {
        var index = $(me).attr('data-msh-index');
        var dateFrom = mom.date($(`#FromDate-${index}`).val());
        var dateTo = mom.date($(`#$ToDate-${index}`).val());
        if (dateTo < dateFrom) {
            if (isFrom) {
                var d2 = dateFrom.clone().add(1, 'days');
                $(`#ToDate-${index}`).val(d2.format(mom.YMD));
            } else {
                var d2 = dateTo.clone().add(-1, 'days');
                $(`#FromDate-${index}`).val(d2.format(mom.YMD));
            }
        }
    }

    function updateInputs() {
        setTimeout(function () {
            for (var i = 0; i < itemDates.length; i++) {
                var stay = $(`#Enabled-${i}`);
                changeClass(stay);
            }
            $('[name="Enabled"]').on('change', function () {
                changeClass(this)
            });
           
            $('[name="FromDate"]').on('change', function () {
                updateDates(this, true);
            });
            $('[name="ToDate"]').on('change', function () {
                updateDates(this, false);
            });

        }, 200);
    }

    function loadDates(code, hotelCode) {
        var url = `/api/hotelapi/ExtraDates?code=${code}&hotelCode=${hotelCode}`;
        api.getAsync(url, (data) => {
            if (data.success) {
                itemDates = data.data.dates;
                //presetDates(data.data.minDate)
                var html = getTableHtml(itemDates);
                $('#table-target').html(html);
                updateInputs();
            }
        });
    }

    meth.extendMethods({
        addItemDates: function () {
            // $('#save-dates').attr('disabled', false);
            itemDates.push(itemDate);
            var html = getTableHtml(itemDates);
            $('#table-target').html(html);
            updateInputs();
        },

        deleteItemDates: function (index) {
            const x = itemDates.splice(index, 1);
            var html = getTableHtml(itemDates);
            $('#table-target').html(html);
            updateInputs();
        }

    });

    $('#save-dates').on('click', () => {
        var i = 0;
        itemDates.forEach((v) => {
            v.enabled = $(`#Enabled-${i}`).is(":checked");

            var fromDate = $(`#FromDate-${i}`).val();
            var toDate = $(`#ToDate-${i}`).val();
            v.fromDate = mom.date(fromDate).format(mom.YMD);
            v.toDate = mom.date(toDate).format(mom.YMD);
            i++;
        });

        var d = {
            code: currentCode,
            hotelCode: currentHotelCode,
            dates: itemDates
        };
        var url = `/api/hotelapi/ExtraDates`;
        api.postAsync(url, d, (data) => {
            if (data.success) {
                loadDates(currentCode, currentHotelCode);
            }
        });
    });

    var editType = $('#edit-type').val();

    if (editType === 'extra-dates') {
        currentCode = $('#item-code').val();
        currentHotelCode = $('#hotel-code').val();
        loadDates(currentCode, currentHotelCode);
    }

}(jQuery));

(function ($) {

    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    function getHotelCode() {
        var hotelCode = $(ids.selectHotel).val();
        hotelCode = hotelCode ? hotelCode : $('#hotelCode').val();
        return hotelCode;
    }

    meth.extendMethods({
        confirmDeleteExtra: function (code, hotelCode) {
            var url = `/api/hotelapi/ExtraDelete`;
            var d = {
                code: code,
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo('admin/hotels/ExtrasList')
            });
        },
        deleteExtra: function (code, hotelCode) {

            modal.showModal('delExtra', "Confirm Delete", `Confirm delete of ${code} ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteExtra('${code}', '${hotelCode}')""`,
                okButtonText: 'OK'
            });
        }
    });

    $(ids.selectHotel).on('change', () => {
        var hotelCode = getHotelCode();
        util.redirectTo(`admin/hotels/ExtrasList?hotelCode=${hotelCode}`);
    });

}(jQuery));