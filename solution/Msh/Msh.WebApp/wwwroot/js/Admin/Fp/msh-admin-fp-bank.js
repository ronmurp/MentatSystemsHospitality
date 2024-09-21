(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;

    var options = {
        editTypeId: '#edit-type',
        tableTargetId: '#table-target',
        saveButtonId: '#save-bank',
        apiUrl: `/api/fpapi/FpErrorBankList`,

        editTypeValue: '',
        editTypeValueExpected: ''
    }

    var items = [];
  

    function addTh(cell, attrs) {
        var a = attrs ? ` ${attrs}` : ''
        return `<th${a}>${cell}</th>`;
    }

    function addTd(cell, attrs) {
        var a = attrs ? ` ${attrs}` : ''
        return `<td${a}>${cell}</td>`;
    }

    function addPlus(method) {
        return `<th class="text-center" style="width:80px;"><a href="javascript:window.mshMethods.${method}"><i class="fa-solid fa-plus"></i></a></th>`;
    }
    function addDelete(method) {
        return `<th class="text-center" style="width:80px;"><a href="javascript:window.mshMethods.${method}"><i class="fa-solid fa-times"></i></a></th>`;
    }
    function getTableHeader() {
        var html = `<tr>`;
        html += addTh('Code');
        html += addTh('Message');
        html += addPlus('addBank()')
        html += `</tr>`;
        return html;
    }

    function getTableHtml() {
        var html = '<table>';
        html += `<tr>`;
        html += getTableHeader();
        html += `</tr>`;
        var i = 0;
        items.forEach((v) => {
            html += '<tr>';
            html += `<td><input type="text" id="Code-${i}" data-msh-index="${i}" class="form-control" name="Code" value="${v.code}"/></td>`;
            html += `<td><input type="text" id="Message-${i}" data-msh-index="${i}" class="form-control" name="Message" value="${v.message}" /></td>`;
            html += addDelete('deleteBank()')
            html += '</tr>';
            i++;
        });
        html += '</table>';
        $(options.tableTargetId).html(html);
    }

    function saveValuesLocally() {
        var i = 0;
        items.forEach((v) => {
            v.code = $(`#Code-${i}`).val();
            v.message = $(`#Message-${i}`).val();
            i++;
        });

    }
    function addItem() {
        saveValuesLocally();
        items.push({
            code: '',
            message: ''
        });
        getTableHtml();
    }

    function deleteItem(index) {
        saveValuesLocally();
        const x = items.splice(index, 1);
        getTableHtml(items);
    }

    function loadItems() {
        var url = options.apiUrl;
        api.getAsync(url, (data) => {
            if (data.success) {
                items = data.data;
                getTableHtml();
            }
        });
    }

    $(options.saveButtonId).on('click', () => {
        saveValuesLocally();
        var d = {
            errorBankList: items
        };
        var url = options.apiUrl;
        api.postAsync(url, d, (data) => {
            if (data.success) {
                loadItems();
            }
        });
    });

    function init(inputOptions) {
        options = $.extend({}, options, inputOptions);
        options.editTypeValue = $(options.editTypeId).val();
        if (options.editTypeValue === options.editTypeValueExpected) {

            meth.extendMethods({
                addBank: addItem,
                deleteBank: deleteItem
            });

            loadItems();
        }
    }

    init({
        editTypeId: '#edit-type',
        tableTargetId: '#table-target',
        saveButtonId: '#save-bank',
        apiUrl: `/api/fpapi/FpErrorBankList`,
        editTypeValue: '',
        editTypeValueExpected: 'bank-list'
    })

   
   


}(jQuery));