// Manages a select element with FIT Agent Code value + Name text options
// Depends on:
// - api - as a parameter (e.g. wbsAdminPageApp.)
//
(function ($) {

    if (!window.wbsPageApp)
        window.wbsPageApp = function () { };

    // Manages a select element 
    window.wbsPageApp.selectService = (function () {

        function Selector(initialOptions) {

            var latestCount = 0;

            var api = window.wbsPageApp.apiService;

            var self = this;

            this.Options = {
                initialValue: '',
                getSelected: function () { },
                selectId: '#' + 'select-agent',
                apiUrl: '',
                ValuesListName: '',
                hideWhenEmptyMin: 0,
                hideWhenEmptyClass: '',
                showWhenEmptyClass: '',
                reloaded: function (count) { }
            };
            this.LoadSelect = function (options) {
                if (options)
                    $.extend(self.Options, options);
                loadSelect();
            }
            this.GetSelected = function () {
                return getSelectedValue(self.Options);
            }
            this.Count = function () {
                return latestCount;
            }

            function hideEmpty(empty, className) {
                if (className.length > 0 && empty) {
                    $('.' + className).hide();
                } else if (className.length > 0 && !empty) {
                    $('.' + className).show();
                }
            }
            function hideWhenEmpty(count) {
                var emptyMinCount = self.Options.hideWhenEmptyMin;
                var empty = count <= emptyMinCount;
                hideEmpty(empty, self.Options.hideWhenEmptyClass);
                hideEmpty(!empty, self.Options.showWhenEmptyClass);
            }

            function loadSelect() {

                var options = self.Options;
                latestCount = 0;

                $(options.selectId).off('change');

                function gotValues(data) {

                    // Whatever data comes back must be List<ISelectItem>, and
                    // options.ValuesListName contains the name of that list in data (e.g. data.Agents).
                    // Put it into a list that select understands
                    data.Values = data[options.ValuesListName];



                    if (!data.Success || !data.Values || data.Values.length === 0) {
                        // Nothing to show
                        // pageService.ClearProperties();
                        hideWhenEmpty(0);
                        // Inform client of latestCount
                        if (self.Options.reloaded) self.Options.reloaded(0);
                        return;
                    }

                    latestCount = data.Values.length;

                    // Inform client of latestCount
                    if (self.Options.reloaded) self.Options.reloaded(latestCount);

                    hideWhenEmpty(data.Values.length);

                    // Clear the select and add its options as a list of values, selecting the first
                    $(options.selectId).html('');

                    // Figure out which agent to auto-select
                    var selectedValue = data.Values[0].Value; // Default to first
                    if (options.initialValue) {
                        if (data.Values.filter(function (e) { return e.Value === options.initialValue; }).length > 0) {
                            selectedValue = options.initialValue;
                        }
                    }

                    // Add the agents to the select and mark one as selected
                    data.Values.forEach(function (item, index) {
                        var selected = item.Value === selectedValue ? 'selected="selected"' : '';
                        $(options.selectId).append('<option value="' + item.Value + '" ' + selected + ' >' + item.Text + '</option>');

                    });

                    // No point watching for a change if there's no method to act on it
                    if (options.getSelected) {

                        // Display or otherwise use the initially selected agent
                        var value = $(options.selectId).val();
                        options.getSelected(value);

                        // Watch for selection change
                        $(options.selectId).on('change',
                            function () {
                                var value = $(options.selectId).val();
                                options.getSelected(value);
                            });

                    }
                }
                if (options.apiUrl)
                    api.get(options.apiUrl, gotValues);
            }

            function getSelectedValue(options) {
                return $(options.selectId).val();
            }

            // Initialise
            $.extend(this.Options, initialOptions);

        }


        return {
            Selector: Selector
        }

    })();

}(jQuery))