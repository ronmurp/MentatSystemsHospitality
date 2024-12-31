(function ($) {
    "use strict";
    var app = mshPageApp;
    var routes = app.routes;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;
    var htmls = app.htmlService;
    var sel = app.hotelSelectService;

    var options = {
        editTypeId: '#edit-type',
        tableTargetId: '#table-target',
        saveButtonId: '#save-config-state',
        apiUrl: `/api/fpapi/FpErrorBankList`,

        editTypeValue: '',
        editTypeValueExpected: '',
        apiRoot: routes.DiscountsApi
    }

    var items = [];
    var types = [];

    function getSelectHtml(optionList, selectedValue) {
        var matchValue = selectedValue ? selectedValue : '';
        var html = '';
       
        html += `<option value="">Select ...</option>`
        optionList.forEach((v) => {
            var selectedOption = v.value === matchValue ? 'selected' : '';
            html += `<option value="${v.value}" ${selectedOption}>${v.text}</option>`;
        });
       
        return html;
    }

    function getTableHeader() {
        var html = htmls.addTh('Type');
        html += htmls.addTh('Message');
        html += htmls.addThIcon('add', `window.mshMethods.addDiscountError()`, 'class="text-center" style="width:60px;"');
        return htmls.addTr(html);
    }

    function getTableHtml() {
        var html = '<table>';
        html += getTableHeader();
        var i = 0;
        items.forEach((v) => {

            var selectList = getSelectHtml(types, v.errorType);
            var cells = '';
            // (i, name, value, tdAttrs, inputAttrs)
            cells += htmls.addTdSelect(i, 'Type', selectList, 'style="width:200px;"', '');
            cells += htmls.addTdText(i, 'Message', v.text, '', '');
            // (i, name, iconName, script, tdAttrs, inputAttrs)
            cells += htmls.addTdIcon('times', `window.mshMethods.deleteDiscountError(${i})`, 'class="text-center" style="width:60px;"');

            html += htmls.addTr(cells);
            i++;
        });
        html += '</table>';
        $(options.tableTargetId).html(html);
    }

    function saveValuesLocally() {
        var i = 0;
        items.forEach((v) => {
            v.code = $(`#Code-${i}`).val();
            v.description = $(`#Description-${i}`).val();
            i++;
        });

    }
    function addItem() {
        saveValuesLocally();
        items.push({
            code: '',
            description: ''
        });
        getTableHtml();
    }

    function deleteItem(index) {
        saveValuesLocally();
        const x = items.splice(index, 1);
        getTableHtml(items);
    }

    function loadItems() {
        var hotelCode = sel.getHotelCode();
        var code = sel.getItemCode();
        var url = `${options.apiRoot}/DiscountErrors?hotelCode=${hotelCode}&code=${code}`;
        api.getAsync(url, (data) => {
            if (data.success) {
                items = data.data.errors;
                types = data.data.types
                getTableHtml();
            }
        });
    }

   

    function init(inputOptions) {

        options = $.extend({}, options, inputOptions);
        options.editTypeValue = $(options.editTypeId).val();

        $(options.saveButtonId).on('click', () => {
            saveValuesLocally();
            var hotelCode = sel.getHotelCode();
            var code = sel.getItemCode();
            var d = {
                errors: items,
                hotelCode: hotelCode,
                code: code
            };
            var url = options.apiUrl;
            api.postAsync(url, d, (data) => {
                if (data.success) {
                    loadItems();
                }
            });
        });

        if (options.editTypeValue === options.editTypeValueExpected) {

            meth.extendMethods({
                addDiscountError: addItem,
                deleteDiscountError: deleteItem
            });

            loadItems();
        }
    }

    init({
        editTypeId: '#edit-type',
        tableTargetId: '#table-target',
        saveButtonId: '#save-discount-errors',
        apiUrl: `${routes.DiscountsApi}/DiscountErrors`,
        editTypeValue: '',
        editTypeValueExpected: 'discount-errors'
    })


}(jQuery));