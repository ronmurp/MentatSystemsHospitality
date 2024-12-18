(function ($) {
    "use strict";

    if (!window.mshPageApp)
        window.mshPageApp = function ($) { };

    window.mshPageApp.routes = (function () {
       
        return {
            Extras: '/Admin/Extras',
            ExtrasApi: '/api/ExtrasApi'
        }

    })();

}(jQuery));