(function ($) {
    "use strict";

    if (!window.mshPageApp)
        window.mshPageApp = function ($) { };

    window.mshPageApp.routes = (function () {
       
        return {
            Extras: '/Admin/Extras',
            ExtrasApi: '/api/ExtrasApi',
            Specials: '/Admin/Specials',
            SpecialsApi: '/api/SpecialsApi'
        }

    })();

}(jQuery));