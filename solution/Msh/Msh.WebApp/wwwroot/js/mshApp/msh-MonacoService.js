// Performs monaco-editor ops
// Depends on:
// - jQuery, axios
//
(function () {

    if (!window.mshApp)
        window.mshApp = function () { };

    window.mshApp.monacoService = (function ($) {

        var app = window.mshApp;
        var util = app.utilityService;

        var programChange = true;
        var editor = null;

        var ids = {
            editorId: 'editor-container',
            saveButtonId: '#save-monaco-data',
            onChange: undefined,
            save: undefined
        };

        var container = {
            height: 500,
            minHeight: 500,
            maxHeight: 2000
        }

        function initEditorContainer() {
            var selectId = '#' + ids.editorId;
            $(selectId)
                .css('height', container.height + 'px')
                .css('min-height', container.minHeight + 'px')
                .css('max-height', container.maxHeight + 'px')
                .css('width', '100%')
                .css('margin-bottom', '5px')
                .css('border', '1px solid black')
                .addClass('monaco-box');

        }

        function init(optionsInput, idsInput) {

            var options = {
                value: 'Hello World',
                language: 'html',
                theme: 'vs-dark',
                automaticLayout: true
            };

            $.extend(options, optionsInput);
            $.extend(ids, idsInput);

            editor = monaco.editor.create(document.getElementById(ids.editorId), options);

            editor.getModel().onDidChangeContent(event => {
                //if (programChange) return;
                programChange = true;
                $(ids.saveButtonId).prop('disabled', false);
                console.log(event);
                if (ids.onChange) {
                    ids.onChange();
                }
                //console.log(window.editor.getValue());
            });

            var myBinding = editor.addCommand(monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_S, function () {
                if (!ids.save || !programChange) return;
                ids.save();
            });

            $(document).on('keydown', (e) => {
                if (e.ctrlKey && (e.which === 83)) {
                    e.preventDefault();
                    return false;
                }
            });

            initEditorContainer();

            return editor;
        }

        function setEditor(editor) {
            editor = editor;
        }

        function setLanguage(language) {
            monaco.editor.setModelLanguage(editor.getModel(), language);
        }

        function setValue(value, language) {
            if (!language) language = 'html';
            programChange = false;
            editor.setValue('');
            setLanguage(language);
            editor.setValue(value);
            $(ids.saveButtonId).prop('disabled', true);
        }
        function getValue(value) {
            return editor.getValue();
        }

        function savedOk() {
            programChange = false;
            $(ids.saveButtonId).prop('disabled', true);
        }

        function setWrap(isOn) {
            var wrap = isOn ? 'on' : 'off';
            editor.updateOptions({ wordWrap: wrap });
        }

        function updateOptions(options) {
            editor.updateOptions(options);
        }

        function increaseHeight() {
            if (container.height < container.maxHeight) {
                container.height += 200;
                $('#' + ids.editorId).css('height', container.height + 'px');
            }
        }
        function reduceHeight() {
            if (container.height > container.minHeight) {
                container.height -= 200;
                $('#' + ids.editorId).css('height', container.height + 'px');
            }
        }

        util.extendIconMethods({
            increaseMonacoHeight: increaseHeight,
            reduceMonacoHeight: reduceHeight
        });

        return {
            init: init,
            setEditor: setEditor,
            setValue: setValue,
            getValue: getValue,
            savedOk: savedOk,
            setWrap: setWrap,
            updateOptions: updateOptions,
            increaseHeight: increaseHeight,
            reduceHeight: reduceHeight
        }

    })(jQuery);

}())