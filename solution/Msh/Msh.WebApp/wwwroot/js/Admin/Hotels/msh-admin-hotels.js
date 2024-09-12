(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;
    var mom = app.momentDateService;
    var htmlS = app.htmlService;
    var modal = app.modalService;

    var hotelApi = '/api/hotelapi/';

    var currentHotelCode = '';

    var minDate = mom.today().format(mom.YMD);

    var hotelDate = {
        stayEnabled: false,
        stayFrom: mom.today().format(mom.YMD),
        stayTo: mom.today().format(mom.YMD),
        bookEnabled: false,
        bookFrom: mom.today().format(mom.YMD),
        bookTo: mom.today().format(mom.YMD)
    }

    function presetDates(date) {
        //hotelDate.StayFrom = mom.date(date).format(mon.YMD);
        //hotelDate.StayTo = mom.date(date).format(mon.YMD);
        //hotelDate.BookFrom = mom.date(date).format(mon.YMD);
        //hotelDate.BookTo = mom.date(date).format(mon.YMD);
    }

    var hotelDates = [];

    function getTableHtml(hotelDates) {

        var headArray = [
            'S.Enabled', 'Stay From', 'Stay To', 'B.Enabled', 'Book From', 'Book To',
            `<a href="javascript:window.mshMethods.addHotelDates()"><i class="fa-solid fa-plus"></i></a>`
        ];

        var bodyArray = [];
        var i = 0;
        hotelDates.forEach((v) => {
            
            var stayChecked = v.stayEnabled ? `checked` : ``;
            var bookChecked = v.bookEnabled ? `checked` : ``;

            var rowArray = [
                `<input type="checkbox" id="StayEnabled-${i}" data-msh-index="${i}" name="StayEnabled" ${stayChecked} />`,
                `<input type="date" id="StayFrom-${i}" data-msh-index="${i}" name="StayFrom" value="${v.stayFrom}" />`,
                `<input type="date" id="StayTo-${i}" data-msh-index="${i}" name="StayTo" value="${v.stayTo}" />`,
                `<input type="checkbox" id="BookEnabled-${i}" data-msh-index="${i}" name="BookEnabled" ${bookChecked} />`,
                `<input type="date" id="BookFrom-${i}" data-msh-index="${i}" name="BookFrom" value="${v.bookFrom}" />`,
                `<input type="date" id="BookTo-${i}" data-msh-index="${i}" name="BookTo" value="${v.bookTo}" />`,
                htmlS.cellIcons([
                    `<a href="javascript:window.mshMethods.deleteHotelDates(${i})"><i class="fa-solid fa-times"></i></a>`
                ]),
               
            ];
            bodyArray.push(rowArray);
       
            i++;
        });

        var html = htmlS.table(headArray, bodyArray);

        return html;

    }

    function changeClass(name, me) {
        var index = $(me).attr('data-msh-index')
        if ($(`#${name}Enabled-${index}`).is(':checked')) {
            var selector = `#StayFrom-${index}`;
            $(`#${name}From-${index}`).removeClass('dim-input');
            $(`#${name}To-${index}`).removeClass('dim-input');
        }
        else {
            $(`#${name}From-${index}`).addClass('dim-input');
            $(`#${name}To-${index}`).addClass('dim-input');
        }

    }

    function updateDates(name, me, isFrom) {
        var index = $(me).attr('data-msh-index');
        var dateFrom = mom.date($(`#${name}From-${index}`).val());
        var dateTo = mom.date($(`#${name}To-${index}`).val());
        if (dateTo < dateFrom) {
            if (isFrom) {
                var d2 = dateFrom.clone().add(1, 'days');
                $(`#${name}To-${index}`).val(d2.format(mom.YMD));
            } else {
                var d2 = dateTo.clone().add(-1, 'days');
                $(`#${name}From-${index}`).val(d2.format(mom.YMD));
            }   
        }
    }

    function updateInputs() {
        setTimeout(function () {
            for (var i = 0; i < hotelDates.length; i++) {
                var stay = $(`#StayEnabled-${i}`);
                var book = $(`#BookEnabled-${i}`);
                changeClass('Stay', stay);
                changeClass('Book', book);

            }
            $('[name="StayEnabled"]').on('change', function () {
                changeClass('Stay', this)
            });
            $('[name="BookEnabled"]').on('change', function () {
                changeClass('Book', this)
            });

            $('[name="StayFrom"]').on('change', function() {
                updateDates('Stay', this, true);
            });
            $('[name="StayTo"]').on('change', function () {
                updateDates('Stay', this, false);
            });

            $('[name="BookFrom"]').on('change', function () {
                updateDates('Book', this, true);
            });
            $('[name="BookTo"]').on('change', function () {
                updateDates('Book', this, false);
            });
        }, 200);
    }
    function loadDates(hotelCode) {
        var url = `/api/hotelapi/HotelDates?hotelCode=${hotelCode}`;
        api.getAsync(url, (data) => {
            if (data.success) {
                hotelDates = data.data.dates;
                presetDates(data.data.minDate)
                var html = getTableHtml(hotelDates);
                $('#table-target').html(html);
                updateInputs();
            }
        });
    }

    meth.extendMethods({

        confirmDeleteHotel: function (hotelCode) {
            var url = `/api/hotelapi/HotelDelete`;
            var d = {
                hotelCode: hotelCode
            }
            api.postAsync(url, d, function (data) {

                util.redirectTo('admin/hotels/HotelList')
            });
        },

        deleteHotel: function (hotelCode) {

            modal.showModal('delHotel', "Confirm Delete", `Confirm delete of ${hotelCode}`, {
                footerOk: true,
                okButtonClickScript: `onclick="window.mshMethods.confirmDeleteHotel('${hotelCode}')""`,
                okButtonText: 'OK'
            });
        },
        cancelHotelEdit: function () {
            util.redirectTo('admin/hotels/hotellist');
        },

        addHotelDates: function () {
            $('#save-dates').attr('disabled', false);
            hotelDates.push(hotelDate);
            var html = getTableHtml(hotelDates);
            $('#table-target').html(html);
            updateInputs();
        },
        deleteHotelDates: function (index) {
            
            const x = hotelDates.splice(index, 1);

            var html = getTableHtml(hotelDates);
            $('#table-target').html(html);
            updateInputs();
        }
       

    });

    $('#save-dates').on('click', () => {
        var i = 0;
        hotelDates.forEach((v) => {
            v.stayEnabled = $(`#StayEnabled-${i}`).is(":checked");
            v.stayFrom = mom.date($(`#StayFrom-${i}`).val()).format(mom.YMD);
            v.stayTo = mom.date($(`#StayTo-${i}`).val()).format(mom.YMD);
            v.bookEnabled = $(`#BookEnabled-${i}`).is(":checked");
            v.bookFrom = mom.date($(`#BookFrom-${i}`).val()).format(mom.YMD);
            v.bookTo = mom.date($(`#BookTo-${i}`).val()).format(mom.YMD);
            i++;
        });

        var d = {
            hotelCode: currentHotelCode,
            dates: hotelDates
        };
        var url = `/api/hotelapi/HotelDates`;
        api.postAsync(url, d, (data) => {
            if (data.success) {
                loadDates(currentHotelCode);
            }
        });
    });

    var editType = $('#edit-type').val();

    if (editType === 'hotel-dates') {
        currentHotelCode = $('#hotel-code').val();
        loadDates(currentHotelCode);
    }
   

}(jQuery));