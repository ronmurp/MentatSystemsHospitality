(function ($) {

    if (!window.mshPageApp)
        window.mshPageApp = function ($) { };

    if (!window.mshMethods)
        window.mshMethods = {};

    window.mshPageApp.captchaService = (function () {

        var api = window.mshPageApp.apiService;
        function performCaptcha(action, key, successCallback, failCallback) {
            grecaptcha.ready(function () {
                grecaptcha.execute(key, { action: action }).then(function (token) {

                    api.postAsync('/api/captcha/VerifyCaptcha', { token: token, action: action }, function (data) {
                        successCallback(data);
                    });

                });
            });
        }

        function captchaClientKeyLoad() {
            api.getAsync('/api/captcha/GetClientKey', (data) => {
                if (data.Success) {
                    $('#captcha-key').val(data.Data.ClientKey);
                }
            });
        }

        captchaClientKeyLoad();

        return {
            performCaptcha: performCaptcha
        }
    }());
}(jQuery));