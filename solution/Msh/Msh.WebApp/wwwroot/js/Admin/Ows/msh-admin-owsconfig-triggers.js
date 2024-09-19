(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var api = app.apiService;
    var meth = app.methodsService;
    var htmlS = app.htmlService;

    var maps = [];

    var apiMethod = 'owsConfigMaps';

    function getTableHtml() {
        var html = '<table>';

        html += `<tr>`;
        html += `<th style="width:200px;">Code</th><th>Trigger</th><th class="text-center" style="width:80px;">Regex</th><th>User Msg</th>`;
        html += `<th class="text-center" style="width:80px;"><a href="javascript:window.mshMethods.addTrigger()"><i class="fa-solid fa-plus"></i></a></th>`;
        html += `</tr>`;
        var i = 0;
        maps.forEach((v) => {
            var isRegEx = v.isRegEx ? 'checked' : '';
            html += '<tr>';
            html += `<td><input type="text" id="Code-${i}" data-msh-index="${i}" class="form-control" name="Code" value="${v.code}"/></td>`;
            html += `<td><input type="text" id="Trigger-${i}" data-msh-index="${i}" class="form-control" name="Trigger" value="${v.trigger}" /></td>`;
            html += `<td class="text-center"><input type="checkbox" id="IsRegex-${i}" data-msh-index="${i}" class="form-control-check text-center" name="IsRegex" ${isRegEx} /></td>`;
            html += `<td><input type="text" id="UserMessage-${i}" data-msh-index="${i}" class="form-control" name="UserMessage" value="${v.userMessage}" /></td>`;
            html += `<td class="text-center" style="width:80px;"><a href="javascript:window.mshMethods.deleteTrigger(${i})"><i class="fa-solid fa-times"></i></a></td>`;
            html += '</tr>';
            i++;
        });

        html += '</table>';

        $('#table-target').html(html);
        setTimeout(function () {
            var i = 0;
            maps.forEach((v) => {
                $(`#Trigger-${i}`).val(v.trigger);
                i++;
            });
        }, 10);
    }
  
    function loadMaps() {
        var url = `/api/owsapi/${apiMethod}`;
        api.getAsync(url, (data) => {
            if (data.success) {
                maps = data.data;
                getTableHtml();
            }
        });
    }

    function saveValuesLocally() {
        var i = 0;
        maps.forEach((v) => {
            v.code = $(`#Code-${i}`).val();
            v.trigger = $(`#Trigger-${i}`).val();
            v.isRegEx = $(`#IsRegex-${i}`).is(':checked');
            i++;
        });
    }

    function addTrigger() {
        saveValuesLocally();
        maps.push({
            code: '',
            trigger: '',
            isRegEx: false
        });
        getTableHtml();
    }

    function deleteTrigger(index) {
        saveValuesLocally();
        const x = maps.splice(index, 1);
        getTableHtml();
    }

    $('#save-owsconfig-maps').on('click', () => {
        saveValuesLocally();
        var d = {};
        switch (apiMethod) {
            case "OwsConfigEditTriggers":
                d.CriticalErrorTriggers = maps
                break;

            default:
                d.schemeMaps = maps
                break;
        }
       
        var url = `/api/owsapi/${apiMethod}`;
        api.postAsync(url, d, (data) => {
            if (data.success) {
                loadMaps();
            }
        });
    });

    function init() {
        apiMethod = $('#api-method').val();
    }
   
    meth.extendMethods({
        addTrigger: addTrigger,
        deleteTrigger: deleteTrigger
    });   

    init();

    loadMaps();

}(jQuery));