(function($){

    "use strict";

    var app = window.mshPageApp;
    var api = app.apiService;
    var meth = app.methodsService;
    var htmlS = app.htmlService;
    var store = app.storage;
    var mom = app.momentDateService;
    var math = app.mathService;

    var util = {
        setHtml: function (html) {
            $('#data').html(html);
        }
    }

    var sel = {
        dataType: '#dataType',
        sort: '#sort',
        datesHotel: '#dateArrive,#dateDepart,#hotel',
        occupancy: '#adults,#children',

        errorMessage: '#errorMessage',

        systemType: '#systemType',

        dateArrive: '#dateArrive',
        dateDepart: '#dateDepart',
        nights: '#nights',
        hotelCode: '#hotelCode',
        adults: '#adults',
        children: '#children',
        data: '#data',
        date: '#date',

        qType: '#qualifyingType',
        qCode: '#qualifyingCode'
    };

    var funcs = {
        dataType: function () { return $(sel.dataType).val(); },
        sort: function () { return $(sel.sort).val(); }
    }

    function addTh(v, attr) {
        var a = attr && attr.length > 0 ? attr : '';
        return `<th${a}>${v}</th>`
    } function addTd(value) {
        var a = attr && attr.length > 0 ? attr : '';
        return `<td${a}>${v}</td>`
    }

    function setArrive(arriveStr) { $(sel.dateArrive).val(arriveStr); }
    function setDepart(departStr) { $(sel.dateArrive).val(departStr); }

    function dateArrive() { return $(sel.dateArrive).val(); }
    function dateDepart() { return $(sel.dateDepart).val(); }
    function setHtml(html) { $(sel.data).html(html); }

    function onSuccessLoadData(data) {
        //util2.hideProgress();
        if (!data.Success) {
            // util.showError(data.ErrorMessage);
            setHtml("");
            return;
        }

        setArrive(data.ArriveText);
        setDepart(data.DepartText);
        var dataType = funcs.dataType();

        var text = '<table>';

        switch (dataType) {

            case "RoomTypes":
                text = getRoomTypes(data, text);
                break;
            case "RatePlans":
                text = getRatePlans(data, text);
                break;
            case "RoomRates":
            default:
                text = getRoomRates(data, text);
                break;
        }


        text += '</table>';

        setHtml(text);
    }

    function getRoomRates(data, text) {
        text = text + "<tr>"
            + addTd("RoomType")
            + addTd("RatePlan")
            + addTd("Price")
            + addTd("Units")
            + "</tr>";
        data.List.forEach(function (r) {


            text += "<tr>";
            text += addTd(r.RoomTypeCode);
            text += addTd(r.RatePlanCode);
            text += addTd(r.Rate);
            text += addTd(r.Units);


            text += "</tr>";
        });
        return text;
    }

    function getRoomTypes(data, text) {

        text = text + "<tr>"
            + addTd("RoomType")
            + addTd("Units")
            + addTd("Description")
            + "</tr>";


        data.ListRoomTypes.forEach(function (r) {
            text += "<tr>";
            text += addTd(r.RoomTypeCode);
            text += addTd(r.Units);
            text += addTd(r.Description);
            text += "</tr>";
        });
        return text;
    }

    function getRatePlans(data, text) {

        text = text + "<tr>"
            + addTd("RatePlan")
            + addTd("Description")
            + "</tr>";


        data.ListRatePlans.forEach(function (r) {
            text += "<tr>";
            text += addTd(r.RatePlanCode);
            text += addTd(r.Description);
            text += "</tr>";
        });
        return text;
    }

    function getFormData() {

        var req = {
           

            HotelCode: $(sel.hotelCode).val(),
            Arrive: $(sel.dateArrive).val(),
            Nights: $(sel.nights).val(),

            Adults: $(sel.adults).val(),
            Children: $(sel.children).val(),

            DataType: $(sel.dataType).val(),
            Sort: $(sel.sort).val(),

            QualifyingIdType: $(sel.qType).val(),
            QualifyingIdValue: $(sel.qCode).val()
        }

        return req;
    }

    function setFormData(d) {
        var data = d.data;      

        $(sel.hotelCode).val(data.HotelCode);
        $(sel.dateArrive).val(data.Arrive);
        $(sel.nights).val(data.Nights);


        $(sel.adults).val(data.Adults);
        $(sel.children).val(data.Children);
        $(sel.nights).val(data.Nights);

        $(sel.qType).val(data.QualifyingIdType);
        $(sel.qCode).val(data.QualifyingIdValue);

        $(sel.dataType).val(data.DataType);
        $(sel.sort).val(data.Sort);

        updateDates();
    }

    function loadData() {
       
        var req = getFormData();

        api.postAsync('/api/owsapi/Availability', req, onSuccessLoadData);
    };

    var today = mom.today();
    var tomorrow = today.clone().add(1, 'days');
    $('#nights').val('1');
    $('#dateArrive').val(today.format(mom.YMD));
    $('#dateDepart').val(tomorrow.format(mom.YMD));
    $('#dateArrive').attr('min', today.format(mom.YMD));
    $('#dateDepart').attr('min', tomorrow.format(mom.YMD));

    function updateDates() {
        var arrive = mom.date($('#dateArrive').val());
        var nights = math.parseInt($('#nights').val());
        var depart = mom.date($('#dateDepart').val());
        depart = arrive.clone().add(nights, 'days');
        $('#dateDepart').val(depart.format(mom.YMD));
    }
    $('#dateArrive').on('change', function () {
        //var arrive = mom.date($('#dateArrive').val());
        //var nights = math.parseInt($('#nights').val());
        //var depart = mom.date($('#dateDepart').val());
        //depart = arrive.clone().add(nights, 'days');
        //$('#dateDepart').val(depart.format(mom.YMD));
        updateDates();
    });

    $('#nights').on('change', function () {
        var arrive = mom.date($('#dateArrive').val());
        var nights = math.parseInt($('#nights').val());
        var depart = mom.date($('#dateDepart').val());
        depart = arrive.clone().add(nights, 'days');
        $('#dateDepart').val(depart.format(mom.YMD));
    });

    var list = [];
    var storeId = 'ows-raw';

    $('#load-button').on('click', function () {

        loadData();
    });

    $('#save-button').on('click', function () {
        var id = $('#store-id').val();
        var exists = false;
        list.forEach(function (v, i) {
            if (v.id === id) {
                exists = true;
            }
        });
        if (exists)
            return;

        if (id) {
            var data = {
                id: id,
                data: getFormData()
            };
            list.push(data);
            var json = JSON.stringify(list);
            store.saveData(storeId, json);
            fillSelect(list);        }

    });



    function fillSelect(items) {
        var html = '';
        items.forEach(function (v, i) {
            html += '<option value="' + v.id + '">' + v.id + '</option>';
        });
        $('#store-select').html(html);
    }

    function loadStore() {
        var d = store.getData(storeId);
        if (d) {
            list = JSON.parse(d);
            fillSelect(list);
            if (list.length > 0) {

            }
        }
    }

    $('#store-select').on('change', function () {
        var id = $('#store-select').val();
        list.forEach(function (v, i) {
            if (v.id === id) {
                setFormData(v);
            }
        });
        setFormData(data);
    });

    loadStore();

} (jQuery));