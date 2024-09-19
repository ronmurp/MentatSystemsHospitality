(function ($) {

    "use strict";

    var app = window.mshPageApp;
    var api = app.apiService;
    var meth = app.methodsService;
    var htmlS = app.htmlService;

    var maps = [];

    var apiMethod = 'owsConfigMaps';
    function getTableHtml() {

        var headArray = [
            'Code', 'Pattern',
            htmlS.cellIcons([
                `<a href="javascript:window.mshMethods.addMap()"><i class="fa-solid fa-plus"></i></a>`
            ])
        ];

        var bodyArray = [];
        var i = 0;

        maps.forEach((v) => {
            var rowArray = [
               
                `<input type="text" id="Code-${i}" data-msh-index="${i}" class="form-control" name="Code" value="${v.code}"/>`,
                `<input type="text" id="Pattern-${i}" data-msh-index="${i}" class="form-control" name="Pattern" value="${v.pattern}" />`,

                htmlS.cellIcons([
                    `<a href="javascript:window.mshMethods.deleteMap(${i})"><i class="fa-solid fa-times"></i></a>`
                ]),
            ];
            bodyArray.push(rowArray);
            i++;
        });
        var html = htmlS.table(headArray, bodyArray);
        return html;
    }

    function loadMaps() {
        var url = `/api/owsapi/${apiMethod}`;
        api.getAsync(url, (data) => {
            if (data.success) {
                maps = data.data;
                var html = getTableHtml();
                $('#table-target').html(html);
            }
        });
    }

    function saveValuesLocally() {
        var i = 0;
        maps.forEach((v) => {
            v.code = $(`#Code-${i}`).val();
            v.pattern = $(`#Pattern-${i}`).val();
            i++;
        });
    }

    function addMap() {
        saveValuesLocally();
        maps.push({
            code: '',
            pattern: ''
        });
        var html = getTableHtml();
        $('#table-target').html(html);
    }

    function deleteMap(index) {
        saveValuesLocally();
        const x = maps.splice(index, 1);
        var html = getTableHtml(maps);
        $('#table-target').html(html);
    }

    $('#save-owsconfig-maps').on('click', () => {
        saveValuesLocally();
        var d = {
            schemaMaps: maps
        };
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
        addMap: addMap,
        deleteMap: deleteMap
    });   

    loadMaps();

}(jQuery));