﻿(function ($) {
    "use strict";
    var app = mshPageApp;
    var meth = app.methodsService;
    var util = app.utilityService;
    var modal = app.modalService;
    var api = app.apiService;
    var htmls = app.htmlService;

    var options = {
        editTypeId: '#edit-type',
        tableTargetId: '#table-target',
        saveButtonId: '#save-bank',
        apiUrl: `/api/fpapi/FpErrorBankList`,

        editTypeValue: '',
        editTypeValueExpected: ''
    }

    var items = [];
  
    function getTableHeader() {
        var html = htmls.addTh('Code');
        html += htmls.addTh('Message');
        html += htmls.addPlus('addBank()')
        return htmls.addTr(html);
    }

    function getTableHtml() {
        var html = '<table>';
        
        html += getTableHeader();
        
        var i = 0;
        items.forEach((v) => {
            var cells = htmls.addTdText(i, 'Code', v.code, '');
            cells += htmls.addTdText(i, 'Message', v.message, '');
            cells += htmls.addDelete(`deleteBank()`);
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