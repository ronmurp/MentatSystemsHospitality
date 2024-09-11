(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var api = app.apiService;
    var mom = app.momentDateService;

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
        
        var html = '<table>';
        html += '<tr>';

        html += `<th>S.Enabled</th>`;
        html += `<th>Stay From</th>`;
        html += `<th>Stay To</th>`;
        html += `<th>B.Enabled</th>`;
        html += `<th>Book From</th>`;
        html += `<th>Book To</th>`;
        html += `<th><a href="javascript:window.mshMethods.addHotelDates()"><i class="fa-solid fa-plus"></i></a></th>`;

        html += '</tr>';
        var i = 0;
        hotelDates.forEach((v) => {
            var stayChecked = v.stayEnabled ? `checked` : ``;
            var bookChecked = v.bookEnabled ? `checked` : ``;
            html += '<tr>';
            html += `<td><input type="checkbox" id="StayEnabled-${i}" ${stayChecked} /></td>`;
            html += `<td><input type="date" id="StayFrom-${i}" value="${v.stayFrom}" /></td>`;
            html += `<td><input type="date" id="StayTo-${i}" value="${v.stayTo}" /></td>`;
            html += `<td><input type="checkbox" id="BookEnabled-${i}" ${bookChecked} /></td>`;
            html += `<td><input type="date" id="BookFrom-${i}" value="${v.bookFrom}" /></td>`;
            html += `<td><input type="date" id="BookTo-${i}" value="${v.bookTo}" /></td>`;
            html += `<td><a href="javascript:window.mshMethods.deleteHotelDates(${i})"><i class="fa-solid fa-times"></i></a></td>`
            html += '</tr>';
            i++;
        });
        html += '</table>';
        return html;

    }
    function loadDates(hotelCode) {
        var url = `/api/hotelapi/HotelDates?hotelCode=${hotelCode}`;
        api.getAsync(url, (data) => {
            if (data.success) {
                hotelDates = data.data.dates;
                presetDates(data.data.minDate)
                var html = getTableHtml(hotelDates);
                $('#table-target').html(html);
            }
        });
    }

    meth.extendMethods({

        confirmDeleteHotel: function (hotelCode) {
            var url = `/api/hotelapi/HotelDelete`;
            api.postAsync(url, hotelCode, function (data) {

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
        },
        deleteHotelDates: function (index) {
            
            const x = hotelDates.splice(index, 1);

            var html = getTableHtml(hotelDates);
            $('#table-target').html(html);
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