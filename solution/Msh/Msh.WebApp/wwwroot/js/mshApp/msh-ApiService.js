// Performs api ops 
// Depends on:
// - jQuery, axios
//
(function () {

    "use strict";

    if (!window.mshPageApp)
        window.mshPageApp = function () { };

    window.mshPageApp.apiService = (function ($) {

        function get(url, success, fail) {

            axios.get(url)
                .then(function (response) {
                    if (success) success(response.data);
                })
                .catch(function (error) {
                    if (fail) fail(error);
                    if (error) console.log(error);
                    else console.log('unexpected error ' + url);
                })
                .then(function () {
                    // always executed
                });
        }

        const getAsync = async (url, success, fail) => {
            try {
                const response = await axios.get(url);
                if (success) success(response.data);
            } catch (error) {
                if (fail) fail(error);
                if (error) console.log(error);
                else console.log('unexpected error ' + url);
            }
        };

        function post(url, model, success, fail) {

            axios.post(url, model)
                .then(function (response) {
                    if (success) success(response.data);
                })
                .catch(function (error) {
                    if (fail) fail(error);
                    if (error) console.log(error);
                    else console.log('unexpected error ' + url);
                })
                .then(function () {
                    // always executed
                });
        }
        const postAsync = async (url, model, success, fail) => {
            try {
                const response = await axios.post(url, model);
                if (success) success(response.data);
            } catch (error) {
                if (fail) fail(error);
                if (error) console.log(error);
                else console.log('unexpected error ' + url);
            }
        };

        function put(url, model, success, fail) {

            axios.put(url, model)
                .then(function (response) { if (success) success(response.data); })
                .catch(function (error) {
                    if (fail) fail(error);
                    if (error) console.log(error);
                    else console.log('unexpected error ' + url);
                })
                .then(function () {
                    // always executed
                });
        }

        const putAsync = async (url, model, success, fail) => {
            try {
                const response = await axios.put(url, model);
                if (success) success(response.data);
            } catch (error) {
                if (fail) fail(error);
                if (error) console.log(error);
                else console.log('unexpected error ' + url);
            }
        };

        return {
            get: get,
            post: post,
            put: put,

            getAsync: getAsync,
            putAsync: putAsync,
            postAsync: postAsync
        }
    })(jQuery);

}())