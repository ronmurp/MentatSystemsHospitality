(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var api = app.apiService;
    var meth = app.methodsService;
    var htmlS = app.htmlService;

    var items = [];


    var options = {
        apiMethod: 'LogXmlConfigEditItems',
        group: "Ows",
        tableTargetId: '#table-target',
        saveButtonId: '#save-owsconfig-maps',
        presetButtonId: '#preset-logxmlconfig-items'
    }

    function getApiPath() {
        return `/api/loggersapi/${options.apiMethod}/${options.group}`;
    }

    function getTableHtml() {
        var cell = `<input type="checkbox" id="bulk-check" onchange="mshMethods.bulkChange()" />`
        var html = '<table>';
        html += `<tr>`;
        html += `<th style="width:220px;">Key</th><th class="text-center">Enabled ${cell}</th>`;
        html += `<th>Filename</th><th>Message</th><th class="text-center">Trace</th>`;
        html += `<th class="text-center" style="width:80px;"><a href="javascript:window.mshMethods.addItem()"><i class="fa-solid fa-plus"></i></a></th>`;
        html += `</tr>`;
        var i = 0;
        items.forEach((v) => {
            var enabled = v.enabled ? 'checked' : '';
            var trace = v.fullTrace ? 'checked' : '';
            html += '<tr>';
            html += `<td><input type="text" id="Key-${i}" data-msh-index="${i}" class="form-control" name="Key" value="${v.key}"/></td>`;
            html += `<td class="text-center"><input type="checkbox" id="Enabled-${i}" data-msh-index="${i}" class="form-control-check text-center" name="Enabled" ${enabled} /></td>`;
            html += `<td><input type="text" id="Filename-${i}" data-msh-index="${i}" class="form-control" name="Filename" value="${v.filename}" /></td>`;
            html += `<td><input type="text" id="MessageName-${i}" data-msh-index="${i}" class="form-control" name="MessageName" value="${v.messageName}" /></td>`;
            html += `<td class="text-center"><input type="checkbox" id="FullTrace-${i}" data-msh-index="${i}" class="form-control-check text-center" name="FullTrace" ${trace} /></td>`;
            html += `<td class="text-center" style="width:80px;"><a href="javascript:window.mshMethods.deleteItem(${i})"><i class="fa-solid fa-times"></i></a></td>`;
            html += '</tr>';
            i++;
        });

        html += '</table>';

        $('#table-target').html(html);

    }

    function loadItems() {
        var url = getApiPath();
        api.getAsync(url, (data) => {
            if (data.success) {
                items = data.data;
                getTableHtml();
            }
        });
    }

    function saveValuesLocally() {
        var i = 0;
        items.forEach((v) => {
            v.key = $(`#Key-${i}`).val();
            v.filename = $(`#Filename-${i}`).val();
            v.messageName = $(`#MessageName-${i}`).val();
            v.enabled = $(`#Enabled-${i}`).is(':checked');
            v.fullTrace = $(`#FullTrace-${i}`).is(':checked');
            i++;
        });
    }

    function addItem() {
        saveValuesLocally();
        items.push({
            key: '',
            filename: '',
            messageName: '',
            enabled: false,
            fullTrace: false
        });
        getTableHtml();
    }

    function deleteItem(index) {
        saveValuesLocally();
        const x = items.splice(index, 1);
        getTableHtml();
    }



    function init(inputOptions) {
        options = $.extend({}, options, inputOptions);

        $(options.saveButtonId).on('click', () => {
            saveValuesLocally();
            var d = {
                items: items
            };

            var url = getApiPath();
            api.postAsync(url, d, (data) => {
                if (data.success) {
                    loadItems();
                }
            });
        });
    }

    function presetItems() {
        var url = `/api/loggersapi/LogXmlConfigEditItemsInit/${options.group}`;
        api.postAsync(url, {}, (data) => {
            if (data.success) {
                loadItems();
            }
        });
    }

    function bulkChange() {
        var checked = $('#bulk-check').is(':checked');
        items.forEach((v) => {
            v.enabled = checked
        });
        getTableHtml();
        $('#bulk-check').prop('checked', checked);
    }
   
    meth.extendMethods({
        addItem: addItem,
        deleteItem: deleteItem,
        presetItems: presetItems,
        bulkChange: bulkChange
    });   

    init({
        apiMethod: $('#api-method').val(),
        group: $('#api-group').val(),
        tableTargetId: '#table-target',
        saveButtonId: '#save-logxmlconfig-items'
    });

    loadItems();

}(jQuery));