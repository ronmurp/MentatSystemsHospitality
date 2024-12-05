// Performs api ops 
// Depends on:
// - jQuery, axios
//
(function () {

    "use strict";

    if (!window.mshPageApp)
        window.mshPageApp = function () { };

    window.mshPageApp.specialOptionsService = (function ($) {

        var app = mshPageApp;
        var meth = app.methodsService;
        var util = app.utilityService;
        var modal = app.modalService;
        var api = app.apiService;
        var mom = app.momentDateService;
        var htmlS = app.htmlService;

        var options = {
            datesApiAction: 'SpecialOptions',
            tableTargetId: '#table-target',
            saveButtonId: '#save-options',
            editTypeId: '#edit-type',
            editTypeValue: 'item-options'
        };

        var apiRoot = '/api/specialsapi';

        var currentCode = '';
        var currentHotelCode = '';
        
        var itemOptions = [];

        function getTableHtml() {

            var headArray = [
                'Value', 'Text', 'DataValue',
                htmlS.cellIcons([
                    `<a href="javascript:window.mshMethods.addSpecialOption()"><i class="fa-solid fa-plus"></i></a>`
                ])
            ];
           

            var bodyArray = [];
            var i = 0;
            itemOptions.forEach((v) => {


                var rowArray = [
                    `<input type="text" id="Value-${i}" data-msh-index="${i}" name="Value" value="${v.value}" />`,
                    `<input type="text" id="Text-${i}" data-msh-index="${i}" name="Text" value="${v.text}" />`,
                    `<input type="text" id="DataValue-${i}" data-msh-index="${i}" name="DataValue" value="${v.dataValue}" />`,

                    htmlS.cellIcons([
                        `<a href="javascript:window.mshMethods.deleteSpecialOption(${i})"><i class="fa-solid fa-times"></i></a>`
                    ]),

                ];
                bodyArray.push(rowArray);

                i++;
            });

            var html = htmlS.table(headArray, bodyArray);

            return html;

        }

        function loadOptions(code, hotelCode) {
            var url = `${apiRoot}/SpecialOptions?code=${code}&hotelCode=${hotelCode}`;
            api.getAsync(url, (data) => {
                if (data.success) {
                    itemOptions = data.data.options;                 
                    var html = getTableHtml();
                    $(options.tableTargetId).html(html);
                }
            });
        }
        function addSpecialOption() {
            saveValuesLocally();
            itemOptions.push({
                value: '',
                text: '',
                dataValue:'',
            });
            var html = getTableHtml();
            $(options.tableTargetId).html(html);
        }

        function deleteSpecialOption(index) {
            saveValuesLocally();
            const x = itemOptions.splice(index, 1);
            var html = getTableHtml(itemOptions);
            $(options.tableTargetId).html(html);
            // updateInputs();
        }

       

        function saveValuesLocally() {
            var i = 0;
            itemOptions.forEach((v) => {
                v.value = $(`#Value-${i}`).val();
                v.text = $(`#Text-${i}`).val();
                v.dataValue = $(`#DataValue-${i}`).val();
                i++;
            });

        }

        $(options.saveButtonId).on('click', () => {
            saveValuesLocally();
            var d = {
                code: currentCode,
                hotelCode: currentHotelCode,
                options: itemOptions
            };
            var url = `${apiRoot}/SpecialOptions`;
            api.postAsync(url, d, (data) => {
                if (data.success) {
                    loadOptions(currentCode, currentHotelCode);
                }
            });
        });

     

        function init(inputOptions) {
            options = $.extend({}, options, inputOptions);

            var editType = $(options.editTypeId).val();

            if (editType === options.editTypeValue) {
                currentCode = $('#item-code').val();
                currentHotelCode = $('#hotel-code').val();
                loadOptions(currentCode, currentHotelCode);
            }
        }

        meth.extendMethods({
            addSpecialOption: addSpecialOption,
            deleteSpecialOption: deleteSpecialOption
        });       

        return {
            init: init
        }

    })(jQuery);

    window.mshPageApp.specialOptionsService.init({

        datesApiAction: 'SpecialOptions',
        tableTargetId: '#table-target',
        saveButtonId: '#save-options',
        editTypeId: '#edit-type',
        editTypeValue: 'item-options'

    });

}())