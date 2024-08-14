(function ($) {
    "use strict";

    if (!window.mshPageApp)
        window.mshPageApp = function ($) { };

    if (!window.mshMethods)
        window.mshMethods = {};

    window.mshPageApp.mathService = (function () {

        function parseInt(value, defaultValue) {
            var n = value && /^[-+]?(\d+)$/.test(value)
                ? Number(value)
                : defaultValue && /^[-+]?(\d+)$/.test(defaultValue)
                    ? Number(defaultValue)
                    : 0;
            return window.parseInt(n);
        }

        function parseFloat(value, defaultValue) {
            return value && /^[-+]?(\d+)(\.\d+){0,1}$/.test(value)
                ? Number(value)
                : defaultValue && /^[-+]?(\d+)(\.\d+){0,1}$/.test(defaultValue)
                    ? Number(defaultValue)
                    : 0;
        }

        return {
            parseInt: parseInt,
            parseFloat: parseFloat
        }

    })();

    window.mshPageApp.momentDateService = (function () {

        var app = window.mshPageApp;
        var math = app.mathService;
        var api = app.apiService;

        var serverTime = moment().format('YYYY-MM-DD hh:mm:ss');

        function latestServerTime() {
            //api.getAsync('/api/Master/ServerTime', function (data) {
            //    if (data.Success) {
            //        serverTime = data.Data.ServerTime;
            //    }
            //});
        }

        setInterval(function () { latestServerTime() }, 30000);

        latestServerTime();

        return {
            now: () => moment(), //moment(),
            today: () => moment().startOf('day'),
            date: (date) => moment(date).startOf('day'),
            dateTime: function (dateTimeString) {
                return moment.utc(dateTimeString);
            },
            minDateOrToday: (minDate) => {
                var today = moment().startOf('day');
                return minDate.isBefore(today) ? today : minDate;
            },
            firstOfMonth: (date) => moment({ year: date.year(), month: date.month(), day: 1 }).startOf('day'),
            firstOfYearMonth: (year, month) => moment({ year: year, month: month, day: 1 }).startOf('day'),
            firstOfPreviousMonth: (date) => moment({ year: date.year(), month: date.month(), day: 1 }).subtract(1, 'month').startOf('day'),
            lastOfPreviousMonth: (date) => moment({ year: date.year(), month: date.month(), day: 1 }).subtract(1, 'day').startOf('day'),
            firstOfWeek: (date) => moment(date).startOf('week'),

            hoursPassed: function (dateTime) {
                const duration = moment.duration(moment().diff(dateTime));
                const hours = parseInt(duration.asHours());
                return hours;
            },

            validMonth: function (maxMonths, testDate) {
                var d = moment(testDate);
                var today = moment().startOf('day');
                var max = today.clone().add(maxMonths, 'months');
                return d >= today && d <= max;
            },

            monthName: 'MMMM',
            monthName3: 'MMM',
            dayName: 'dddd',
            dayName3: 'ddd',
            dayNumber2: 'DD',
            long: 'ddd, MMM Do YYYY',
            monthNameYear: 'MMMM YYYY',
            YMD: 'YYYY-MM-DD',

            dobInputOff: function ($jqDate) {
                $jqDate.off('keyup');
            },

            dobInput: function (selector) {
                $(selector).mask('00/00/0000');
                //let $jqDate = $(selector);

                //$jqDate.on('keyup', function (ev) {
                //    if (ev.which !== 8) {
                //        let input = $jqDate.val();
                //        let out = input.replace(/\D/g, '');
                //        let len = out.length;

                //        if (len > 1 && len < 4) {
                //            out = out.substring(0, 2) + '/' + out.substring(2, 3);
                //        } else if (len >= 4) {
                //            out = out.substring(0, 2) + '/' + out.substring(2, 4) + '/' + out.substring(4, len);
                //            out = out.substring(0, 10);
                //        }
                //        $jqDate.val(out);
                //    }
                //});

                //return $jqDate;
            },

            dobToDate: function (dobText) {
                var a = dobText.split('/');
                return moment.utc(a[2] + '-' + a[1] + '-' + a[0]).startOf('day');
            }
        }

    })();

    window.mshPageApp.htmlService = (function () {

        var icons = {
            info: '<i class="fa-solid fa-info-circle wbs-icon-blue"></i>',
            warn: '<i class="fa-solid fa-triangle-exclamation wbs-icon-warn"></i>',
            add: '<i class="fa-solid fa-square-plus wbs-icon-blue"></i>',
            trash: '<i class="fa-solid fa-trash wbs-icon-blue"></i>',
            edit: '<i class="fa-solid fa-pen-to-square wbs-icon-blue"></i>',
            view: '<i class="fa-solid fa-eye wbs-icon-blue"></i>',
            rotateLeft: '<i class="fa-solid fa-rotate-left wbs-icon-blue"></i>',
            rotateRight: '<i class="fa-solid fa-rotate-right wbs-icon-blue"></i>',
            refresh: '<i class="fa-solid fa-refresh wbs-icon-blue"></i>',
            check: '<i class="fa-solid fa-check wbs-icon-green"></i>',
            cross: '<i class="fa-solid fa-xmark wbs-icon-red"></i>',
            filter: '<i class="fa-solid fa-filter wbs-icon-blue"></i>',
            sort: '<i class="fa-solid fa-sort wbs-icon-blue"></i>',
            search: '<i class="fa-solid fa-search wbs-icon-blue"></i>',
            upload: '<i class="fa-solid fa-upload wbs-icon-blue"></i>',
            copy: '<i class="fa-solid fa-copy wbs-icon-blue"></i>',
            up: '<i class="fa-solid fa-chevron-up wbs-icon-blue"></i>',
            cart: '<i class="fa-solid fa-cart-shopping wbs-icon-blue"></i>',
            shield: '<i class="fa-solid fa-shield wbs-icon-blue"></i>',
            csv: '<i class="fa-solid fa-file-csv wbs-icon-blue"></i>',
            calendar: '<i class="fa-solid fa-calendar wbs-icon-blue"></i>',
            gbp: '<i class="fa-solid fa-gbp wbs-icon-blue"></i>',
            link: '<i class="fa-solid fa-link wbs-icon-blue"></i>',
            arrowDown: '<i class="fa-solid fa-arrow-down wbs-icon-blue"></i>',
            arrowUp: '<i class="fa-solid fa-arrow-up wbs-icon-blue"></i>',
            personBooth: '<i class="fa-solid fa-person-booth wbs-icon-blue"></i>',
            gift: '<i class="fa-solid fa-gift wbs-icon-blue"></i>',
            suitcaseRolling: '<i class="fa-solid fa-suitcase-rolling wbs-icon-blue"></i>',
            at: '<i class="fa-solid fa-at wbs-icon-blue"></i>',
            loader: '<i class="fa-solid fa-loader wbs-icon-blue"></i>',
            eraser: '<i class="fa-solid fa-eraser wbs-icon-red"></i>',
            circleAdd: '<i class="fa-solid fa-circle-plus wbs-icon-green"></i>',
            userPlus: '<i class="fa-solid fa-user-plus wbs-icon-green"></i>'

        };

        function createElement(tagName, className, id) {
            var ele = document.createElement(tagName);
            if (className) {
                ele.className = className;
            }
            if (id) {
                ele.setAttribute('id', id);
            }

            return ele;
        }

        var defaultSettings = {
            selectId: undefined,
            selectClasses: undefined,
            firstOptionsHtml: undefined, // Text html
            emptyListFirstOptionsHtml: undefined, // What to display if the list is empty
            optionList: undefined,          // The list used to build th options
            optionValueName: undefined,     // The name of the property that provides the value
            optionTextName: undefined,      // The name of the property that provides the text,
            onChangeMethod: undefined,      // The method called when the select is changed.
            dataWbsAttrs: undefined
        };

        function buildSelect(inputSettings) {
            var settings = $.extend({}, defaultSettings, inputSettings);
            var attrs = settings.dataWbsAttrs ? settings.dataWbsAttrs : '';
            var selectF = `<select id="sel-${settings.selectId}" class="${settings.selectClasses}" onchange="${settings.onChangeMethod}" ${attrs}>`;
            selectF += setSelectOptions(settings);
            selectF += '</select>';
            return selectF;
        }

        function setSelectOptions(inputSettings) {
            var settings = $.extend({}, defaultSettings, inputSettings);
            var s = ''; // The options
            if (settings.firstOptionsHtml) s += settings.firstOptionsHtml; // One or more <option ...
            if (settings.optionList && settings.optionList.length > 0) {
                var n = 1;
                settings.optionList.forEach(function (v, i) {

                    var selected = ((settings.selectedValue && settings.selectedValue === v[settings.optionValueName])
                        || (settings.selectedIndex && settings.selectedIndex === n++))
                        ? 'selected="selected"'
                        : '';
                    s += `<option value="${v[settings.optionValueName]}" ${selected}>${v[settings.optionTextName]}</option>`;
                });
            } else if (settings.emptyListFirstOptionsHtml) {
                s += settings.emptyListFirstOptionsHtml;
            }

            return s;
        }

        function iconScriptLink(iconName, script) {
            var link = '<a href="javascript:{Script}">' + icons[iconName] + '</a>';
            if (script) {
                link = link.replace(/{Script}/, script);
            }
            return link;
        }

        function iconScriptLinkSpan(iconName, script, spanId) {
            var link = iconScriptLink(iconName, script);
            var span = '<span id="{Id}">' + link + '</span>';
            if (spanId) {
                span = span.replace(/{Id}/, spanId);
            }
            return span;
        }

        function iconTemplate(iconName) {
            return '<a href="{Script}">' + icons[iconName] + '</a>&nbsp;';
        }

        function filterCell() {

        }

        return {

            icons: icons,
            iconScriptLink: iconScriptLink,
            iconScriptLinkSpan: iconScriptLinkSpan,
            iconTemplate: iconTemplate,

            createElement: createElement,

            div: function (className, id) { return createElement('div', className, id); },
            p: function (className, id) { return createElement('p', className, id); },
            span: function (className, id) { return createElement('i', className, id); },
            i: function (className, id) { return createElement('span', className, id); },
            a: function (className, id) { return createElement('a', className, id); },
            i: function (className, id) { return createElement('i', className, id); },
            h1: function (className, id) { return createElement('h1', className, id); },
            h2: function (className, id) { return createElement('h2', className, id); },
            h3: function (className, id) { return createElement('h3', className, id); },
            h4: function (className, id) { return createElement('h4', className, id); },
            h5: function (className, id) { return createElement('h5', className, id); },
            option: function (value, text, selected) {
                var e = createElement('option');
                e.setAttribute('value', value);
                e.innerText = text;
                if (selected) e.setAttribute('selected', 'selected');
                return e;
            },
            href: 'href',
            id: 'id',
            style: 'style',

            breakpoints: {
                xs: 0,
                sm: 576,
                md: 768,
                lg: 992,
                xl: 120,
                xxl: 1400
            },

            buildSelect: buildSelect,
            setSelectOptions: setSelectOptions
        }

    })();

    window.wbsAdminPageApp.htmlTextService = (function ($) {

        var app = window.wbsAdminPageApp;
        var htmlS = app.htmlService;

        var filterTemplate = htmlS.iconTemplate('filter'); // '<a href="javascript:{Script}"><i class="fa-solid fa-filter"></i></a>&nbsp;';
        var trashTemplate = htmlS.iconTemplate('trash'); // '<a href="javascript:{Script}"><i class="fa-solid fa-trash"></i></a>&nbsp;';
        var inputClass = 'tbl-filter-input';

        return {

            table: (rows) => { return '<table>' + rows + '</table>'; },

            header: (headers) => {
                var html = '<tr>';
                headers.forEach((h) => { html += '<th>' + h + '</th>'; });
                html += '</tr>';
                return html;
            },

            row: (columns) => {
                var html = '<tr>';
                columns.forEach((h) => { html += '<td>' + h + '</td>'; });
                html += '</tr>';
                return html;
            },

            // filters: [ { Header: 'ID', Script: script }]
            filter: (filters) => {
                var html = '<tr>';

                filters.forEach((f) => {
                    var fi = '<td></td>';
                    if (f && f.Header && f.Header.length > 0) {
                        fi = '<td style="padding:1px;"><div>' +
                            filterTemplate.replace('{Script}', f.Script) +
                            '<input type="text" id="tbl-search-{spaceHeader}" value="" class="{inputClass}"'
                                .replace('{spaceHeader}', f.Header)
                                .replace('{inputClass}', inputClass) +

                            '</div></td>';
                    }
                    if (f && f.Icons && f.Icons.length > 0) {
                        fi = '<td>' + f.Icons + '</td>';
                    }
                    html += fi;

                });
                html += '</tr>';
                return html;
            },
            trashTemplate: (trashScript) => {
                return trashTemplate.replace('{Script}', trashScript);
            }

        }

    })(jQuery);


    window.mshPageApp.pageErrorService = (function () {

        var logEnable = true;
        function log(data) {
            if (logEnable)
                console.log(data);
        }
        var staticOptions = {
            errorWrapper: '#page-error-wrapper',
            type: 'text'
        }

        function init(inputOptions) {
            log('init pageErrorService');
            staticOptions = $.extend({}, staticOptions, inputOptions);
        }

        function showErrorMessage(message, inputOptions) {
            var options = staticOptions;
            if (inputOptions)
                options = $.extend({}, staticOptions, inputOptions);
            if (!message) {
                clearErrorMessage(options);
                return;
            }
            if (options.type !== 'html') {
                $(options.errorWrapper).html('<p>' + message + '</p>');
                $('#sticky-error-content').html('<p>' + message + '</p>');
            } else {
                $(options.errorWrapper).html(message);
                $('#sticky-error-content').html(message);
            }
            $(options.errorWrapper).show();
            $('#sticky-error').show();
            stickyErrorInit('sticky-error', 'sticky-error-close');

        }
        function clearErrorMessage(inputOptions) {
            var options = staticOptions;
            if (inputOptions)
                $.extend({}, staticOptions, inputOptions);
            $(options.errorWrapper).html('');
            $(options.errorWrapper).hide();
            $('#sticky-error-content').html('');
            $('#sticky-error').hide();

        }

        function stickyErrorInit(bannerId, closeId) {

            const bannerIdSelector = '#' + bannerId;
            const closeIdSelector = '#' + closeId;

            var header = document.getElementById(bannerId);

            var stickyPosition = header.offsetTop;

            $(closeIdSelector).on('click',
                function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    window.onscroll = null;
                    $(closeIdSelector).off('click');
                    clearErrorMessage();
                });

            var onScroll = function () {

                const diff = (stickyPosition - window.pageYOffset);

                if (window.pageYOffset > header.offsetTop && diff < window.pageYOffset) {

                    $(bannerIdSelector).css('position', 'fixed');
                    $(bannerIdSelector).css('top', 0);

                } else {

                    $(bannerIdSelector).css('position', 'absolute');
                    $(bannerIdSelector).css('top', diff);

                }
            };

            window.onscroll = function () { onScroll() };
        }

        function showStickyErrorMessage(message, inputOptions) {
            var myOptions = options;
            if (inputOptions)
                myOptions = $.extend({}, options, inputOptions);
            if (!message) {
                clearErrorMessage(myOptions);
                return;
            }
            if (myOptions.type !== 'html') {
                $('#sticky-error-content').html('<p>' + message + '</p>');
            } else {
                $('#sticky-error-content').html(message);
            }
            $(myOptions.errorWrapper).show();
            $('#sticky-error').show();
            stickyErrorInit('sticky-error', 'sticky-error-close');

        }

        window.mshMethods = $.extend({},
            window.mshMethods,
            {
                testErrorMessage: function (message) {
                    if (!message) message = 'This is a test error message';
                    showErrorMessage(message, { type: 'text' });
                }
            });

        return {
            init: init,
            showErrorMessage: showErrorMessage,
            clearErrorMessage: clearErrorMessage,
            showStickyErrorMessage: showStickyErrorMessage,
            log: log
        }

    })();


    window.mshPageApp.utilityService = (function () {

        var app = window.mshPageApp;
        var modal = app.modalService;
        var math = app.mathService;
        var err = app.pageErrorService;

        var params = new Proxy(new URLSearchParams(window.location.search), {
            get: (searchParams, prop) => searchParams.get(prop)
        });
        // Get the value of "some_key" in eg "https://example.com/?some_key=some_value"
        var tests = params.tests === 'true'; // "some_value"


        function useTests() {
            return tests;
        }

        function setInt(selector, value) {
            var n = math.parseInt(value);
            return $(selector).val('' + n);
        }

        // return a property getter/setter for int
        function getIntInput(id) {
            return {
                get: function () { return math.parseInt('#' + id); },
                set: function (value) { setInt('#' + id, value); }
            };
        }

        // return a property getter/setter for text
        function getTextInput(id) {
            return {
                get: function () { return $('#' + id).val(); },
                set: function (value) { $('#' + id).val(value); },
                clickOn: function (listener) { $('#' + id).on('click', listener); },
                clickOff: function () { $('#' + id).off('click'); },
                inputOn: function (listener) { $('#' + id).on('input', listener); },
                changeOn: function (listener) { $('#' + id).on('input', listener); }
            };
        }

        // return a property getter/setter for bool
        function getCheckInput(id) {
            return {
                get: function () { return $('#' + id).prop('checked'); },
                set: function (value) { $('#' + id).prop('checked', value); }
            };
        }

        function jquerify(selector) {
            if (!selector) selector = {};
            $('[data-jq-id]').each(function (index, inputElement) {
                var varName = $(inputElement).attr('data-jq-id');
                var id = $(inputElement).attr('id');
                selector[varName] = $('#' + id);


            });
            return selector;
        }


        // arrayObjectIndexOf(arr, "stevie", "hello"); // 1
        function arrayObjectIndexOf(myArray, searchTerm, property) {
            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }
            return -1;
        }

        function findArrayItem(myArray, searchTerm, property) {
            var index = arrayObjectIndexOf(myArray, searchTerm, property);
            if (index === -1)
                return undefined;
            return myArray[index];
        }

        var queryString = (function (a) {
            if (a === "") return {};
            var b = {};
            for (var i = 0; i < a.length; ++i) {
                var p = a[i].split('=');
                if (p.length !== 2) continue;
                b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
            }
            return b;
        })(window.location.search.substr(1).split('&'));

        function getUrl(baseUrl, parameters) {
            var url = baseUrl;
            for (var i = 0; i < parameters.length; i++) {
                var p = parameters[i];
                url += i === 0 ? '?' : '&';
                url += p[0] + '=' + p[1];
            }

            return url;
        }

        function getJavascript(methodName, params) { // gateJavascript('method', [ 2, 'mike' ]) => javascript:method(2, 'mike')
            var js = 'javascript:' + methodName + '(';
            for (var i = 0; i < params.length; i++) {
                var comma = i === 0 ? '' : ', ';
                var item = params[i];
                if (item.type === 'string') {
                    js += comma + "'" + item.value + "'";
                } else {
                    js += comma + "" + item.value + "";
                }
            }
            js += ")";
            return js;
        }

        function selfHide(id, clearHtml) {
            $(id).html('');
            $(id).hide();
        }

        function hasString(message) {
            return (message !== undefined && message !== null && message.length > 0);
        }

        function scrollTop() {
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }

        function copyText(id) {
            var copyText = document.getElementById(id);

            // Select the text field
            copyText.select();
            copyText.setSelectionRange(0, 99999); // For mobile devices

            // Copy the text inside the text field
            navigator.clipboard.writeText(copyText.value);
        }

        return {
            validateEmail: function (email) {
                return (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email));
            },

            hasError: function (data) {
                if (!data.Success) {
                    modal.hideProgress();
                    err.showErrorMessage(data.ErrorMessage);
                    return true;
                }
                return false;
            },

            hasString: hasString,

            showError: function (message) {
                err.showErrorMessage(message);
            },

            showMessage: function (title, message) {
                modal.showMessage(title, message, false);
            },

            hideErrorMessageItems: function () {
                $('.pc-error-message-item').hide();
            },

            getUrl: getUrl,

            disable: function (id, disable) {
                $('#' + id).prop('disabled', disable);
            },

            jquerify: jquerify,

            useTests: useTests,

            arrayObjectIndexOf: arrayObjectIndexOf,

            findArrayItem: findArrayItem,

            getJavascript: getJavascript,

            queryString: queryString,

            extendIconMethods: function (newIconMethods) {
                if (!window.mshMethods)
                    window.mshMethods = {};
                window.mshMethods = $.extend({}, window.mshMethods, { selfHide: selfHide }, newIconMethods);
            },

            scrollTop: scrollTop,
            copyText: copyText
        }
    }());


    // A service that encapsulated actions on a form, based on
    // a number of attributes on the input elements of the form.
    window.mshPageApp.pageFormsService = (function () {

        var app = window.mshPageApp;
        var math = app.mathService;
        var modal = app.modalService;
        var mom = app.momentDateService;

        // var exampleOptions = {
        //    fieldSetId: 'input-set', // the value assigned to the data-wbs-set attribute.

        //    updateButtonId: '#user-update',
        //    cancelButtonId: '#user-cancel',
        //    createButtonId: '#user-add',

        //    updateModelMethod: function (user) {},
        //    cancelModelMethod: function (model) { },        // does anything else to cancel
        //    createModelMethod: function (model) { },        // calls api to create
        //    dataHasChanged: function (hasChanged) { },
        //    isBusy: function () { }                         // disable any buttons that should not be enabled while busy
        // };

        // data-wbs-set="input-set"
        // data-wbs-model="Email"
        // data-wbs-type="bool"

        // A 'class' that can be newed up for different field sets
        // such as a main edit form and a modal create form,
        // with different fieldSetId values
        function FormService(options) {
            if (options === undefined || options === null || options.fieldSetId === undefined) {
                console.error("FormService options not well defined.");
            }
            var self = this;

            var appId = options.appId;
            var appSelector = '[data-' + appId + '-set="' + options.fieldSetId + '"]';
            var appModel = 'data-' + appId + '-model';
            var appType = 'data-' + appId + '-type';

            this.Model = {};
            this.Options = options;

            this.SetModel = setModel;
            this.GetModel = getModel;
            this.HasChanged = false;

            // A spec of fields (inputs) built based on inputs that have a fieldSetId
            this.Fields = getFields(self.Model);



            function getFields(model) {
                var fields = [];
                $(appSelector).each(function (index, inputElement) {
                    var id = $(inputElement).attr('id');
                    var field = {
                        Id: id,
                        Selector: '#' + id,
                        Model: $(inputElement).attr(appModel),
                        Type: $(inputElement).attr(appType)
                    };

                    if (field.Type == undefined)
                        field.Type = 'string';

                    fields.push(field);
                    model[field.Model] = getValue(field);
                });
                watchForChange();
                return fields;
            }

            function setModel(model) {
                self.Model = model;
                self.HasChanged = false;
                setProperties(self.Fields, self.Model);
                dataHasChanged(false);
            }
            function getModel() {
                return getProperties(self.Fields, self.Model);
            }

            $(self.Options.updateButtonId).on('click',
                function (e) {
                    e.preventDefault();
                    var model = getProperties(self.Fields, self.Model); // Page service instance passed in
                    busy();
                    self.Options.updateModelMethod(model);
                });

            $(self.Options.cancelButtonId).on('click',
                function (e) {
                    e.preventDefault();
                    setProperties(self.Fields, self.Model); // Page service instance passed in
                    self.Options.updateModelMethod(self.Model);
                });

            $(self.Options.createButtonId).on('click',
                function (e) {
                    e.preventDefault();
                    self.Options.createModelMethod();
                });


            function watchForChange() {
                $(appSelector).on('input', function (e) {
                    var newModelFromInputs = getProperties(self.Fields, {});
                    self.HasChanged = !deepEqual(self.Model, newModelFromInputs);
                    dataHasChanged(self.HasChanged);
                });
            }


            function busy() {
                modal.showProgress();
                setDisabled(self.Options.updateButtonId, true);
                setDisabled(self.Options.cancelButtonId, true);
                setDisabled(self.Options.createButtonId, true);
                if (self.Options.isBusy) self.Options.isBusy(true);
            }
            function dataHasChanged(value) {
                modal.hideProgress();
                setDisabled(self.Options.updateButtonId, !value);
                setDisabled(self.Options.cancelButtonId, !value);
                setDisabled(self.Options.createButtonId, value);
                // Used by form client code to enable/disable additional fields
                if (self.Options.dataHasChanged) self.Options.dataHasChanged(value);
            }
            function setDisabled(id, value) {
                $(id).prop('disabled', value);
            }

            function getProperties(fields) {
                var model = {};
                fields.forEach(function (field, index) {
                    model[field.Model] = getValue(field);
                });
                return model;
            }

            function setProperties(fields, model) {
                fields.forEach(function (field, index) {
                    setFieldValue(field, model[field.Model]);
                });
            }

            function setFieldValue(field, value) {
                switch (field.Type) {
                    case 'bool':
                    case 'checkbox':
                        $(field.Selector).prop('checked', value);
                        break;
                    default:
                        $(field.Selector).val(value);
                }
            }

            function getValue(field) {

                switch (field.Type) {
                    case 'int':
                        var n = math.parseInt($(field.Selector).val());
                        return Number.isNaN(n) ? 0 : n;
                    case 'float':
                        var m = math.parseFloat($(field.Selector).val());
                        return Number.isNaN(m) ? 0.0 : m;
                    case 'bool':
                    case 'checkbox':
                        return $(field.Selector).prop('checked') === true;
                    default:
                        return $(field.Selector).val();
                }
            }

            function deepEqual(object1, object2) {
                const keys1 = Object.keys(object1);
                const keys2 = Object.keys(object2);
                if (keys1.length !== keys2.length) {
                    return false;
                }
                for (const key of keys1) {
                    const val1 = object1[key];
                    const val2 = object2[key];
                    const areObjects = isObject(val1) && isObject(val2);
                    if (
                        areObjects && !deepEqual(val1, val2) ||
                        !areObjects && val1 !== val2
                    ) {
                        return false;
                    }
                }
                return true;
            }

            function isObject(object) {
                return object != null && typeof object === 'object';
            }

        }

        function trim(str) {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/trim#Polyfill
            return str.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, "");
        }



        function required(value, element, param) {

            //// Check if dependency is met
            //if (!this.depend(param, element)) {
            //    return "dependency-mismatch";
            //}
            //if (element.nodeName.toLowerCase() === "select") {

            //    // Could be an array for select-multiple or a string, both are fine this way
            //    var val = $(element).val();
            //    return val && val.length > 0;
            //}
            //if (this.checkable(element)) {
            //    return this.getLength(value, element) > 0;
            //}
            return value !== undefined && value !== null && value.length > 0;
        }

        function email(value, element) {

            // From https://html.spec.whatwg.org/multipage/forms.html#valid-e-mail-address
            // Retrieved 2014-01-14
            // If you have a problem with this implementation, report a bug against the above spec
            // Or use custom methods to implement your own email validation
            return /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/
                .test(value);
        }

        function errorClass(isValid, label) {
            if (isValid) {
                $(label).removeClass('error');
            } else {
                $(label).addClass('error');
            };
            return isValid;
        }

        function validateChecked(id) {
            var $e = $('#' + id);
            var value = $e.prop('checked');
            var el = $e[0];
            var label = 'label[for="' + id + '"]';
            return errorClass(value, label);
        }

        function validateEmail(id) {
            var $e = $('#' + id);
            var value = $e.val();
            var el = $e[0];
            var label = 'label[for="' + id + '"]';
            return errorClass(email(value, el), label);
        }
        function validateRequired(id) {
            var $e = $('#' + id);
            var value = $e.val();
            var el = $e[0];
            var label = 'label[for="' + id + '"]';
            if (required(value, el)) {
                $(label).removeClass('error');
            } else {
                $(label).addClass('error');
            };
        }

        var validate = {

            validateEmail: validateEmail,
            validateRequired: validateRequired,
            validateChecked: validateChecked,

            // https://jqueryvalidation.org/blank-selector/
            blank: function (a) {
                return !trim("" + $(a).val());
            },

            // https://jqueryvalidation.org/filled-selector/
            filled: function (a) {
                var val = $(a).val();
                return val !== null && !!trim("" + val);
            },

            // https://jqueryvalidation.org/unchecked-selector/
            unchecked: function (a) {
                return !$(a).prop("checked");
            },

            // https://jqueryvalidation.org/jQuery.validator.methods/
            methods: {

                // https://jqueryvalidation.org/required-method/
                required: required,

                // https://jqueryvalidation.org/email-method/
                email: email,

                // https://jqueryvalidation.org/url-method/
                url: function (value, element) {

                    // Copyright (c) 2010-2013 Diego Perini, MIT licensed
                    // https://gist.github.com/dperini/729294
                    // see also https://mathiasbynens.be/demo/url-regex
                    // modified to allow protocol-relative URLs
                    return this.optional(element) ||
                        /^(?:(?:(?:https?|ftp):)?\/\/)(?:(?:[^\]\[?\/<~#`!@$^&*()+=}|:";',>{ ]|%[0-9A-Fa-f]{2})+(?::(?:[^\]\[?\/<~#`!@$^&*()+=}|:";',>{ ]|%[0-9A-Fa-f]{2})*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z0-9\u00a1-\uffff][a-z0-9\u00a1-\uffff_-]{0,62})?[a-z0-9\u00a1-\uffff]\.)+(?:[a-z\u00a1-\uffff]{2,}\.?))(?::\d{2,5})?(?:[/?#]\S*)?$/i
                            .test(value);
                },

                // https://jqueryvalidation.org/date-method/
                date: (function () {
                    var called = false;

                    return function (value, element) {
                        if (!called) {
                            called = true;
                            if (this.settings.debug && window.console) {
                                console.warn(
                                    "The `date` method is deprecated and will be removed in version '2.0.0'.\n" +
                                    "Please don't use it, since it relies on the Date constructor, which\n" +
                                    "behaves very differently across browsers and locales. Use `dateISO`\n" +
                                    "instead or one of the locale specific methods in `localizations/`\n" +
                                    "and `additional-methods.js`."
                                );
                            }
                        }

                        return this.optional(element) || !/Invalid|NaN/.test(new Date(value).toString());
                    };
                }()),

                // https://jqueryvalidation.org/dateISO-method/
                dateISO: function (value, element) {
                    return this.optional(element) ||
                        /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/.test(value);
                },

                // https://jqueryvalidation.org/number-method/
                number: function (value, element) {
                    return this.optional(element) || /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value);
                },

                // https://jqueryvalidation.org/digits-method/
                digits: function (value, element) {
                    return this.optional(element) || /^\d+$/.test(value);
                },

                // https://jqueryvalidation.org/minlength-method/
                minlength: function (value, element, param) {
                    var length = Array.isArray(value) ? value.length : this.getLength(value, element);
                    return this.optional(element) || length >= param;
                },

                // https://jqueryvalidation.org/maxlength-method/
                maxlength: function (value, element, param) {
                    var length = Array.isArray(value) ? value.length : this.getLength(value, element);
                    return this.optional(element) || length <= param;
                },

                // https://jqueryvalidation.org/rangelength-method/
                rangelength: function (value, element, param) {
                    var length = Array.isArray(value) ? value.length : this.getLength(value, element);
                    return this.optional(element) || (length >= param[0] && length <= param[1]);
                },

                // https://jqueryvalidation.org/min-method/
                min: function (value, element, param) {
                    return this.optional(element) || value >= param;
                },

                // https://jqueryvalidation.org/max-method/
                max: function (value, element, param) {
                    return this.optional(element) || value <= param;
                },

                // https://jqueryvalidation.org/range-method/
                range: function (value, element, param) {
                    return this.optional(element) || (value >= param[0] && value <= param[1]);
                },

                // https://jqueryvalidation.org/step-method/
                step: function (value, element, param) {
                    var type = $(element).attr("type"),
                        errorMessage = "Step attribute on input type " + type + " is not supported.",
                        supportedTypes = ["text", "number", "range"],
                        re = new RegExp("\\b" + type + "\\b"),
                        notSupported = type && !re.test(supportedTypes.join()),
                        decimalPlaces = function (num) {
                            var match = ("" + num).match(/(?:\.(\d+))?$/);
                            if (!match) {
                                return 0;
                            }

                            // Number of digits right of decimal point.
                            return match[1] ? match[1].length : 0;
                        },
                        toInt = function (num) {
                            return Math.round(num * Math.pow(10, decimals));
                        },
                        valid = true,
                        decimals;

                    // Works only for text, number and range input types
                    // TODO find a way to support input types date, datetime, datetime-local, month, time and week
                    if (notSupported) {
                        throw new Error(errorMessage);
                    }

                    decimals = decimalPlaces(param);

                    // Value can't have too many decimals
                    if (decimalPlaces(value) > decimals || toInt(value) % toInt(param) !== 0) {
                        valid = false;
                    }

                    return this.optional(element) || valid;
                },

                // https://jqueryvalidation.org/equalTo-method/
                equalTo: function (value, element, param) {

                    // Bind to the blur event of the target in order to revalidate whenever the target field is updated
                    var target = $(param);
                    if (this.settings.onfocusout && target.not(".validate-equalTo-blur").length) {
                        target.addClass("validate-equalTo-blur").on("blur.validate-equalTo",
                            function () {
                                $(element).valid();
                            });
                    }
                    return value === target.val();
                },

                // https://jqueryvalidation.org/remote-method/
                remote: function (value, element, param, method) {
                    if (this.optional(element)) {
                        return "dependency-mismatch";
                    }

                    method = typeof method === "string" && method || "remote";

                    var previous = this.previousValue(element, method),
                        validator,
                        data,
                        optionDataString;

                    if (!this.settings.messages[element.name]) {
                        this.settings.messages[element.name] = {};
                    }
                    previous.originalMessage = previous.originalMessage || this.settings.messages[element.name][method];
                    this.settings.messages[element.name][method] = previous.message;

                    param = typeof param === "string" && { url: param } || param;
                    optionDataString = $.param($.extend({ data: value }, param.data));
                    if (previous.old === optionDataString) {
                        return previous.valid;
                    }

                    previous.old = optionDataString;
                    validator = this;
                    this.startRequest(element);
                    data = {};
                    data[element.name] = value;
                    $.ajax($.extend(true,
                        {
                            mode: "abort",
                            port: "validate" + element.name,
                            dataType: "json",
                            data: data,
                            context: validator.currentForm,
                            success: function (response) {
                                var valid = response === true || response === "true",
                                    errors,
                                    message,
                                    submitted;

                                validator.settings.messages[element.name][method] = previous.originalMessage;
                                if (valid) {
                                    submitted = validator.formSubmitted;
                                    validator.resetInternals();
                                    validator.toHide = validator.errorsFor(element);
                                    validator.formSubmitted = submitted;
                                    validator.successList.push(element);
                                    validator.invalid[element.name] = false;
                                    validator.showErrors();
                                } else {
                                    errors = {};
                                    message = response ||
                                        validator.defaultMessage(element, { method: method, parameters: value });
                                    errors[element.name] = previous.message = message;
                                    validator.invalid[element.name] = true;
                                    validator.showErrors(errors);
                                }
                                previous.valid = valid;
                                validator.stopRequest(element, valid);
                            }
                        },
                        param));
                    return "pending";
                }
            }

        };


        function saveForm() {
            var item = {};
            $('[data-type]').each(function (index, element) {
                var name = $(element).attr('data-name');
                var type = $(element).attr('data-type');
                var id = $(element).attr('id');
                switch (type) {
                    case 'text':
                        item[name] = $('#' + id).val();
                        break;
                    case "date":
                        var val = $('#' + id).val();
                        item[name] = mom.date(val);
                        break;
                    case "dob":
                        var dob = $('#' + id).val();
                        item[name] = mom.dobToDate(dob);
                        break;
                    case "bool":
                    case "checkbox":
                        item[name] = ($('#' + id).prop('checked')) ? true : false;
                        break;
                    case 'int':
                        var number = math.parseInt($('#' + id).val());
                        item[name] = number;
                        break;
                    default:
                        item[name] = $('#' + id).val();
                        break;
                }

            });

            return item;
        }

        return {
            FormService: FormService,
            saveForm: saveForm,
            validate: validate
        };

    })();

    // Tests for deep changes in an object
    window.mshPageApp.changes = (function () {

        var currentValue = null;
        var hasChanged = false;


        function deepEqual(object1, object2) {
            const keys1 = Object.keys(object1);
            const keys2 = Object.keys(object2);
            if (keys1.length !== keys2.length) {
                return false;
            }
            for (const key of keys1) {
                const val1 = object1[key];
                const val2 = object2[key];
                const areObjects = isObject(val1) && isObject(val2);
                if (
                    areObjects && !deepEqual(val1, val2) ||
                    !areObjects && val1 !== val2
                ) {
                    return false;
                }
            }
            return true;
        }

        function isObject(object) {
            return object != null && typeof object === 'object';
        }

        // Public methods ========================================
        function setCurrentValue(value) {
            currentValue = value;
            hasChanged = false;
        }

        function hasDataChanged(newValue) {
            hasChanged = !deepEqual(currentValue, newValue);
            return hasChanged;
        }

        // Return ================================================
        return {
            setCurrentValue: setCurrentValue,
            hasDataChanged: hasDataChanged

        }
    })();


    // Saves and retrieves data from local storage
    window.mshPageApp.storage = (function () {

        var stores =
        {
            adminFitCurrentAgent: "AdminFitCurrentAgent"
        }

        function saveData(storeId, data) {
            localStorage.setItem(storeId, data);
        }
        function getData(storeId) {
            return localStorage.getItem(storeId);
        }

        function saveAgentCode(agentCode) {
            saveData(stores.adminFitCurrentAgent, agentCode);
        }
        function getAgentCode() {
            return getData(stores.adminFitCurrentAgent);
        }

        return {
            stores: stores,
            saveData: saveData,
            getData: getData,
            saveAgentCode: saveAgentCode,
            getAgentCode: getAgentCode
        }

    })();


    window.mshPageApp.dragService = (function () {

        var top1H, bottom1H, shiftInitial;

        var options = {
            topId: '',
            dragId: '',
            bottomId: ''
        };

        function addDraggerStyles(id) {
            $(id).css('width', '100%')
                .css('height', '6px')
                .css('background-color', '#e0e0e0')
                .css('cursor', 'ns-resize')
                .css('position', 'absolute');

        }

        function init(inputOptions) {
            options = inputOptions;
            addDraggerStyles(options.dragId);
            $(options.dragId).draggable({
                axis: "y",
                start: function (event, ui) {
                    shiftInitial = ui.position.top;
                    top1H = $(options.topId).height();
                    bottom1H = $(options.bottomId).height();
                },
                drag: function (event, ui) {
                    var shift = ui.position.top;
                    $(options.topId).height(top1H + shift - shiftInitial);
                    $(options.bottomId).height(bottom1H - shift + shiftInitial);
                    $('#editor-container').height($(options.topId).height() - 10);
                }
            });
        }

        function drag() {


        }

        return {
            init: init,
            drag: drag
        }



    })();


    window.mshPageApp.calendarService = (function () {

        var app = window.mshPageApp;
        var mom = app.momentDateService;
        var modal = app.modalService;
        var html = app.htmlService;
        var util = app.utilityService;

        function CalendarPopMonth(options, events) {

            var self = this;

            this.options = {
                container: document.body,
                overlayId: 'elh-month-cal-overlay',
                overlayClass: 'elh-month-cal-overlay',
                overlayCloseClass: 'elh-month-cal-overlay-close',
                overlayCloseDivId: 'elh-month-cal-overlay-div-close',
                overlayContentId: 'elh-month-cal-overlay-content',
                overlayContentClass: 'elh-month-cal-overlay-content',
                closeEvent: undefined,
                isMobile: true,
                maxMonths: 36
            };

            this.options = $.extend({}, this.options, options);

            this.options.overlaySelectId = '#' + this.options.overlayId;
            this.options.overlayContentSelectId = '#' + this.options.overlayContentId;
            this.options.overlayCloseDivSelectId = '#' + this.options.overlayCloseDivId;

            this.overlay = modal.addStandardOverlay(this.options); // For CalendarPopMonth
            this.firstValidMonth = false;

            this.el = document.querySelector(this.options.overlayContentSelectId);

            this.targetDate = mom.date(options.targetDate);

            this.events = events;

            this.current = this.targetDate.clone().date(1);
            this.selectDate = options.selectDate;

            this.draw();
            this.cal = new modal.Overlay({
                overlaySelectId: this.options.overlaySelectId,
                contentSelectId: this.options.overlayContentSelectId,
                closeSelectId: this.options.overlayCloseDivSelectId,
                content: undefined,
                resetModal: function () {
                    if (self.options.close)
                        self.options.close();
                    self.close();
                }
            });


            this.cal.show();

            if (this.options.noscroll) {
                this.options.noscroll.classList.add('noscroll');
            }
        }

        function checkViewport(days) {

            var viewportWidth = $(window).width();

            var newDays = 7;

            if (viewportWidth > html.breakpoints.md)
                newDays = 14;
            if (viewportWidth > html.breakpoints.lg)
                newDays = 21;
            if (viewportWidth > html.breakpoints.xxl)
                newDays = 28;

            return {
                days: newDays,
                hasChanged: newDays !== days
            };
        }

        CalendarPopMonth.prototype.close = function () {
            if (this.options.noscroll) {
                this.options.noscroll.classList.remove('noscroll');
            }
            this.cal = undefined;
            this.overlay.remove();
        }

        CalendarPopMonth.prototype.draw = function () {
            this.drawHeader();
            this.drawMonth();
        };

        CalendarPopMonth.prototype.drawMobile = function () {
            var self = this;
            this.drawHeaderMobile();
            var d = mom.today();
            var first = mom.firstOfMonth(d);
            var months = 10;
            while (months-- > 0) {
                this.month = undefined;

                var monthName = first.format(mom.monthNameYear);
                var monthDiv = html.div();
                monthDiv.innerText = monthName;
                this.el.appendChild(monthDiv);
                this.current = mom.date(first);
                this.drawMonth();
                first.add(1, 'months');
            }
            var id = 'date-' + self.targetDate.format(mom.YMD);
            document.getElementById(id)
                .scrollIntoView({ behavior: "smooth" });

        };

        CalendarPopMonth.prototype.drawHeaderMobile = function () {
            //var d = this.targetDate.clone(0);
            //var first = mom.firstOfWeek(d);
            //this.week = html.div('week');
            //for (var dt = 0; dt < 7; dt++) {
            //    var thisDay = first.clone().add(dt, 'days');
            //    var thisHead = html.div('day-head col');
            //    thisHead.innerText = thisDay.format(mom.dayName3);
            //    this.week.appendChild(thisHead);
            //}
            //this.week.classList.add('row');
            //this.el.appendChild(this.week);
            //this.scroll = html.div();
            //this.el.appendChild(this.scroll);
        }

        CalendarPopMonth.prototype.drawHeader = function () {
            var self = this;
            if (!this.header) {

                this.header = html.div('header d-flex flex-row justify-content-between', 'header');

                this.headerDiv = html.div('header-container');


                this.title = html.h1();
                this.title.addEventListener('click',
                    function () {
                        if (!self.yearPop) {

                        }
                        self.yearPop = new MonthYearSelector({
                            close: function () {
                                self.yearPop.close();
                            },
                            selectDate: function (date) {
                                //self.options.selectDate(date);
                                self.current = date.clone();
                                self.next = true;
                                self.draw();
                            },
                            maxMonths: self.options.maxMonths
                        });

                    });
                this.headerDiv.appendChild(this.title);

                this.leftContainer = html.div('left-container d-flex flex-column justify-content-center');
                var divLeft = html.div();
                var iconLeft = html.i('fa-solid fa-chevron-left');
                divLeft.appendChild(iconLeft);
                this.leftContainer.appendChild(divLeft);

                this.leftContainer.addEventListener("click",
                    function () {
                        self.prevMonth();
                    });


                this.rightContainer = html.div('right-container d-flex flex-column justify-content-center');
                var divRight = html.div();
                var iconRight = html.i('fa-solid fa-chevron-right');
                divRight.appendChild(iconRight);
                this.rightContainer.appendChild(divRight);

                this.rightContainer.addEventListener('click',
                    function () {
                        self.nextMonth();
                    });

                this.today = html.div('today-picker d-flex flex-column justify-content-center');
                //this.today.setAttribute(html.style, 'cursor: pointer;');
                var span = html.span();
                //span.setAttribute(html.style, 'font-size:smaller;');
                span.innerText = "Today";
                this.today.appendChild(span);

                this.today.addEventListener('click',
                    function () {
                        self.current = mom.firstOfMonth(mom.today());
                        self.next = true;
                        self.draw();
                    });


                this.header.appendChild(this.leftContainer);
                this.header.appendChild(this.today);
                this.header.appendChild(this.headerDiv);
                this.header.appendChild(this.rightContainer);


                this.el.appendChild(this.header);
            }
            this.title.innerHTML = this.current.format(mom.monthNameYear) +
                ' <span><i class="fa-solid fa-chevron-up"></i></span>';

        };

        CalendarPopMonth.prototype.drawMonth = function () {
            var self = this;

            this.firstValidMonth = false;

            if (this.month) {
                this.oldMonth = this.month;
                this.oldMonth.className = "month out " + (self.next ? "next" : "prev");
                this.oldMonth.addEventListener("webkitAnimationEnd",
                    function () {
                        self.oldMonth.parentNode.removeChild(self.oldMonth);
                        self.month = html.div('month');
                        self.backFill();
                        self.currentMonth();
                        self.fowardFill();
                        self.el.appendChild(self.month);
                        window.setTimeout(function () {
                            self.month.className = "month in " + (self.next ? "next" : "prev");
                        },
                            16);
                    });
            } else {
                this.month = html.div('month container');
                this.el.appendChild(this.month);
                this.backFill();
                this.currentMonth();
                this.fowardFill();
                this.month.className = "month new";
            }

            if (!this.firstValidMonth && !this.options.isMobile) {
                this.leftContainer.classList.remove('invalidDate');
            }
        };

        CalendarPopMonth.prototype.currentMonth = function () {
            var clone = this.current.clone();

            while (clone.month() === this.current.month()) {
                this.drawDay(clone);
                clone.add(1, 'days');
            }
        };

        CalendarPopMonth.prototype.backFill = function () {
            var clone = this.current.clone();
            var dayOfWeek = clone.day();

            if (!dayOfWeek) {
                return;
            }

            clone.subtract(dayOfWeek + 1, 'days');

            for (var i = dayOfWeek; i > 0; i--) {
                this.drawDay(clone.add(1, 'days'));
            }
        };

        CalendarPopMonth.prototype.fowardFill = function () {
            var clone = this.current.clone().add(1, 'months').subtract(1, 'days');
            var dayOfWeek = clone.day();

            if (dayOfWeek === 6) {
                return;
            }

            for (var i = dayOfWeek; i < 6; i++) {
                this.drawDay(clone.add(1, 'days'));
            }
        };

        CalendarPopMonth.prototype.getWeek = function (day) {
            if (!this.week || day.day() === 0) {
                this.week = html.div('week row');
                if (this.options.isMobile)
                    this.week.classList.add('row');
                this.month.appendChild(this.week);
            }
        };

        CalendarPopMonth.prototype.drawDay = function (day) {
            var self = this;
            this.getWeek(day);

            var maxDate = mom.today().clone().add(this.options.maxMonths, 'months');

            //Outer Day
            var outer = html.div(this.getDayClass(day));
            outer.setAttribute('data-date', day.clone().format(mom.YMD));
            outer.setAttribute('id', 'date-' + day.clone().format(mom.YMD));
            if (this.current.format('MMM') !== day.format('MMM')) {
                outer.classList.add('not-this-month');
            }
            else if (day.format(mom.YMD) === mom.today().format(mom.YMD)) {
                outer.classList.add('today');
            }
            if (day.isBefore(this.options.minDate)) {
                outer.classList.add('invalidDate');
                if (!this.options.isMobile)
                    this.leftContainer.classList.add('invalidDate');
                this.firstValidMonth = true;
            } else if (day.isAfter(maxDate)) {
                outer.classList.add('invalidDate');
                if (!this.options.isMobile)
                    this.leftContainer.classList.add('invalidDate');
                this.firstValidMonth = true;
            } else {
                outer.addEventListener("click",
                    function () {
                        if (self.selectDate) {
                            var thisDate = this.getAttribute('data-date');
                            var date = mom.date(thisDate);
                            self.selectDate(date);
                        }
                        self.close();
                    });

            }


            var name = html.div("day-name");
            name.innerText = day.format(mom.dayName3);
            var number = html.div('day-number');
            number.innerText = day.format(mom.dayNumber2);
            var events = html.div('day-events');

            this.drawEvents(day, events);

            outer.appendChild(name);
            outer.appendChild(number);
            outer.appendChild(events);
            this.week.appendChild(outer);

        };

        CalendarPopMonth.prototype.getDayClass = function (day) {
            var classes = ["day"];
            if (day.month() !== this.current.month()) {
                classes.push("other");
            } else if (mom.today().isSame(day, "day")) {
                classes.push("today");
            }
            classes.push('col');
            //if (this.options.isMobile) {
            //    classes.push('col');
            //}
            return classes.join(" ");
        };

        CalendarPopMonth.prototype.nextMonth = function () {
            var today = mom.today();
            var max = today.clone().add(this.options.maxMonths, 'months');
            var next = this.current.clone().add(1, 'months');
            if (next > max) return;
            this.current.add(1, 'months');
            this.next = true;
            this.draw();
        };

        CalendarPopMonth.prototype.prevMonth = function () {
            var date = this.current.clone();
            var prevEnd = mom.lastOfPreviousMonth(date);
            if (prevEnd.isBefore(this.options.minDate))
                return;

            this.current.subtract(1, 'months');
            this.next = false;
            this.draw();
        };

        CalendarPopMonth.prototype.drawEvents = function (day, element) {
            if (!this.events) {
                element.classList.add('none');
                return;
            };

            var found = this.events.find((obj) => {
                return obj.date.isSame(day);
            });
            if (found) {
                element.classList.add(found.color);
            } else {
                element.classList.add('none');
            }
        };

        //
        // MonthYearSelector
        //
        function MonthYearSelector(options) {
            var self = this;

            this.options = {
                container: document.body,
                overlayId: 'elh-month-sel-overlay',
                overlayClass: 'elh-month-cal-overlay',
                overlayCloseClass: 'elh-month-cal-overlay-close',
                overlayCloseDivId: 'elh-month-cal-overlay-div',
                overlayContentId: 'elh-month-sel-overlay-content',
                overlayContentClass: 'elh-month-cal-overlay-content elh-month-sel-overlay-content',
                closeEvent: undefined,
                selectDate: function (date) { },
                maxMonths: 36
            };

            this.options = $.extend({}, this.options, options);

            this.options.overlaySelectId = '#' + this.options.overlayId;
            this.options.overlayContentSelectId = '#' + this.options.overlayContentId;
            this.options.overlayCloseDivSelectId = '#' + this.options.overlayCloseDivId;


            this.cal = new modal.Overlay({
                noscroll: undefined,
                overlaySelectId: this.options.overlaySelectId,
                contentSelectId: this.options.overlayContentSelectId,
                closeSelectId: this.options.overlayCloseDivSelectId,
                content: undefined,
                resetModal: function () {
                    if (self.options.close)
                        self.options.close();
                    else
                        self.close();
                }
            });

            this.overlay = modal.addStandardOverlay(this.options); // For MonthYearSelector

            this.el = document.querySelector(this.options.overlayContentSelectId);

            this.targetDate = mom.date(options.targetDate);

            this.draw();

            this.cal.show();
        }

        MonthYearSelector.prototype.close = function () {

            this.cal = undefined;
            this.overlay.remove();
        }

        MonthYearSelector.prototype.draw = function () {
            var self = this;

            this.currentYear = this.targetDate.year();

            var minYear = this.currentYear;
            var maxYear = this.currentYear + Math.floor(this.options.maxMonths / 12);

            var wrapper = html.div('d-flex flex-column justify-content-center');

            this.drawHeader();

            for (var year = minYear; year <= maxYear; year++) {
                this.drawYear(year, wrapper);
            }

            this.el.appendChild(wrapper);

        }

        MonthYearSelector.prototype.selectedValue = function (yearText, monthText) {
            var year = parseInt(yearText);
            var month = parseInt(monthText);
            var date = mom.firstOfYearMonth(year, month);
            this.options.selectDate(date);
            this.close();
        }

        MonthYearSelector.prototype.closeMe = function () {
            this.close();
        }

        MonthYearSelector.prototype.drawHeader = function (year, wrapper) {
            this.header = html.div('header d-flex flex-row justify-content-center');
            this.headerDiv = html.div('header-container');
            this.title = html.h1();
            this.title.innerText = "Select Month";
            this.headerDiv.appendChild(this.title);
            this.header.appendChild(this.headerDiv);
            this.el.appendChild(this.header);
        }

        MonthYearSelector.prototype.drawYear = function (year, wrapper) {
            var self = this;
            var yearBox = this.drawYearBox(year);
            var monthsBox1 = html.div('cal-box1 d-flex flex-row');
            var monthsBox2 = html.div('cal-box2 d-flex flex-row');
            for (var month = 0; month < 12; month++) {
                var date = mom.firstOfYearMonth(year, month).format(mom.monthName3);
                var invalidMonth = (mom.firstOfYearMonth(year, month) < mom.firstOfMonth(mom.today()));
                if (!invalidMonth) {
                    var testMonth = mom.firstOfYearMonth(year, month);
                    invalidMonth = !mom.validMonth(this.options.maxMonths, testMonth);
                }
                var invalidMonthClass = invalidMonth ? ' invalid-sel-month' : '';
                var monthElement = html.div('col-2 sel-month text-center align-middle' + invalidMonthClass);
                monthElement.setAttribute(html.style, '');
                monthElement.setAttribute('data-value-year', year + '');
                monthElement.setAttribute('data-value-month', '' + month);
                monthElement.innerText = date;
                //var monthPara = html.p();
                //monthPara.innerText = date;
                //monthElement.appendChild(monthPara);
                if (!invalidMonth) {
                    monthElement.addEventListener('click', function () {
                        var selectedYear = this.getAttribute('data-value-year');
                        var selectedMonth = this.getAttribute('data-value-month');
                        self.selectedValue(selectedYear, selectedMonth);
                    });
                }

                if (month < 6)
                    monthsBox1.appendChild(monthElement);
                else
                    monthsBox2.appendChild(monthElement);
            }

            wrapper.appendChild(yearBox);
            wrapper.appendChild(monthsBox1);
            wrapper.appendChild(monthsBox2);
        }

        MonthYearSelector.prototype.drawYearBox = function (year) {
            var yearBox = html.div('row');
            var yearHeading = html.div('col-12 text-center year-header');
            yearHeading.innerText = '' + year;
            yearBox.appendChild(yearHeading);
            return yearBox;
        }

        // Week strip calendar
        function Calendar(options, events) {
            var self = this;
            this.type = 'strip';
            this.days = 7;
            this.targetDate = mom.date(options.targetDate);
            this.el = document.querySelector(options.selector);
            this.events = events;
            this.current = this.targetDate.clone();
            this.minDate = mom.date(options.minDate);

            this.days = checkViewport(7).days;

            this.draw();

            $(window).on('resize',
                function () {
                    var vp = checkViewport(self.days, self.viewportWidth);
                    if (vp.hasChanged) {
                        self.days = vp.days;
                        self.draw();
                    }
                });

        }

        Calendar.prototype.SetDate = function (date) {
            //Create Header
            this.targetDate = date;
            var sub = Math.floor(this.days / 2);
            this.current = this.targetDate.clone().subtract(sub, 'days');
            this.draw();

        };

        Calendar.prototype.draw = function () {
            this.drawHeader();
            this.drawWeek();
        };

        Calendar.prototype.drawHeader = function () {
            if (this.type === 'strip') return;
        };

        Calendar.prototype.drawWeek = function () {
            var self = this;
            this.leftDisabled = this.current.clone().startOf('week').isBefore(mom.minDateOrToday(this.minDate));

            if (this.week) {

                this.oldWeek = this.week;
                //this.oldWeek.className = "month out " + (self.next ? "next" : "next");

                self.oldWeek.parentNode.removeChild(self.oldWeek);
                self.week = html.div('row week');
                self.currentWeek();
                self.el.appendChild(self.week);

            } else {

                this.week = html.div('row week');
                this.el.appendChild(this.week);
                this.currentWeek();
            }
        };

        Calendar.prototype.currentWeek = function () {
            var self = this;
            var clone = this.current.clone();
            var date = this.current.clone().startOf('week');
            for (var i = 0; i < self.days; i++) {
                this.drawWeekDay(date);
                date.add(1, 'days');
            }
        }

        Calendar.prototype.currentMonth = function () {
            var clone = this.current.clone();

            while (clone.month() === this.current.month()) {
                this.drawMonthDay(clone);
                clone.add(1, 'days');
            }
        }

        Calendar.prototype.drawMonthDay = function (day) {
            var self = this;
            this.getWeek(day);

            //Outer Day
            var outer = html.div(this.getDayClass(day));
            outer.addEventListener("click",
                function () {
                    //self.openDay(this);
                });

            //Day Name
            var name = html.div("day-name");
            name.innerText = day.format(mom.dayName3);
            //Day Number
            var number = html.div("day-number");
            number.innerText = day.format(mom.dayNumber2);
            //Events
            var events = html.div("day-events");
            this.drawEvents(day, events);

            outer.appendChild(name);
            outer.appendChild(number);
            outer.appendChild(events);
            this.week.appendChild(outer);
        };

        Calendar.prototype.getWeek = function (day) {
            if (!this.week || day.day() === 0) {
                this.week = html.div("week");
                this.month.appendChild(this.week);
            }
        };

        Calendar.prototype.drawWeekDay = function (day) {
            var self = this;

            var selected = '';
            if (mom.date(this.targetDate).isSame(day)) {
                selected = ' selected';
            }

            var minDate = mom.minDateOrToday(this.minDate);
            var disabled = day.isBefore(minDate) ? ' disabled' : '';

            var outer = html.div(this.getDayClass(day) + selected + disabled + ' col');

            var month = html.div("month-name" + selected + disabled);
            month.innerText = day.format(mom.monthName3);
            //Day Name
            var name = html.div("day-name" + selected + disabled);
            name.innerText = day.format(mom.dayName3);
            //Day Number
            var number = html.div("day-number" + selected + disabled);
            number.innerText = day.format(mom.dayNumber2);

            //Events
            var events = html.div("day-events");
            this.drawDayEvents(day, events);

            //outer.appendChild(link);
            outer.appendChild(month);
            outer.appendChild(name);
            outer.appendChild(number);
            outer.appendChild(events);

            outer.setAttribute('onclick', 'mshMethods.calendarSelect("' + day.format(mom.YMD) + '")');
            this.week.appendChild(outer);
        }

        Calendar.prototype.drawDayEvents = function (day, element) {
            if (!this.events) {
                element.classList.add('none');
                return;
            };

            var found = this.events.find((obj) => {
                return obj.date.isSame(day);
            });
            if (found) {
                element.classList.add(found.color);
            } else {
                element.classList.add('none');
            }

        }

        Calendar.prototype.getDayClass = function (day) {
            var classes = ["day"];
            if (day.month() !== this.current.month()) {
                classes.push("other");
            } else if (mom.today().isSame(day, "day")) {
                classes.push("today");
            }
            return classes.join(" ");
        };

        Calendar.prototype.nextWeek = function (events) {
            this.events = events;
            this.current.add(this.days, 'days');
            this.next = true;
            this.draw();
        };

        Calendar.prototype.prevWeek = function (events) {
            if (this.leftDisabled) return;
            this.events = events;
            this.current.subtract(this.days, 'days');
            this.next = false;
            this.draw();
        };

        util.extendIconMethods({
            today: function () {

            }
        });

        return {
            Calendar: Calendar,
            CalendarPopMonth: CalendarPopMonth
        }

    })();


}(jQuery));