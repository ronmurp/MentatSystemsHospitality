// Performs popupService, progressService
// Depends on:
// - jQuery
//
(function ($) {

    'use strict';

    if (!window.mshPageApp)
        window.mshPageApp = function () { };

    window.mshPageApp.popupService = (function () {

        // Keep track of how often body is set to noscroll by adding noscroll class
        // Allow scrolling again (remove noscroll class) only when there are no more overlays.
        var noscrollCount = 0;
        var overlayStack = [];

        function upScroll() {
            noscrollCount++;
        }

        function downScroll() {
            noscrollCount--;
            if (noscrollCount < 1) // The body is prevented from scrolling as long as there is any open overlay
                $('body').removeClass('noscroll');
        }

        function pushOverlay(overlay) {
            overlayStack.push(overlay);
        }

        function popOverlay() {
            return overlayStack.pop();
        }

        // Removes the popup identified by the overlay's id
        function removePopup(id) {
            downScroll();
            popOverlay();
            $(`#${id}-overlay`).remove(); // hide the overlay
        }

        function buildOverlay(options) {

            var html = `<div id="${options.overlayId}" class="men-overlay">`;

            html += `<div id="${options.popupId}" class="men-popup">`; // Modal container of close and content

            html += `<div id="${options.closeContainerId}">
            <a id="${options.closeId}" class="close" href="javascript:menMethods.removePopup('${options.newId}')">
            <span><i class="fa-solid fa-xmark"></i></span>
            </a>
            </div>`;

            html += `<div id="${options.newId}-content" class="men-content">`;

            html += options.content;

            html += `</div></div></div>`;

            $("body").append(html).addClass('noscroll');

            if (options.iframe) {
                $(`#${options.iframeContainerId}`).append(options.iframe);
            }
            if (options.popupClass) {
                $(`#${options.popupId}`).addClass(options.popupClass);
            }
            if (options.overlayClass) {
                $(`#${options.overlayId}`).addClass(options.overlayClass);
            }
            if (options.noClose) {
                $(`#${options.closeContainerId}`).remove();
            }
            upScroll();
            if (options.zindex) {
                $(`#${options.overlayId}`).css('z-index', options.zindex);
            }
            if (options.height) {
                $(`#${options.popupId}`).css('height', options.height);
            }
            if (options.top) {
                $(`#${options.popupId}`).css('top', options.top);
            }
            $(`#${options.overlayId}`).show();

        }

        function Overlay(options) {

            this.options = {
                newId: 'men-modal',
                noclose: false, // Does not contain the close - e.g. for progress
                overlayId: 'men-modal-overlay',
                popupId: 'men-modal-popup',
                contentId: 'men-modal-content',
                closeId: 'men-modal-close',
                closeContainerId: 'men-modal-close-container',
                content: '<p>Test Content</p>',
                zindex: undefined, // Use z-index in scss, unless defined here
                popupClass: undefined, // An additional class that can define the popup - e.g. dimensions based on 
                overlayClass: undefined,
                removeMethod: 'void(0)', // Default close anchor method. Could be an external method on window: window.menMethods.removeOverlay('id')
                iframe: undefined,
                iframeContainerId: undefined,
                height: undefined,
                top: undefined
            }

            if (options) {
                this.options = $.extend({}, this.options, options);
            }

            if (this.options.newId) {
                this.options.overlayId = `${this.options.newId}-overlay`;
                this.options.popupId = `${this.options.newId}-popup`;
                this.options.contentId = `${this.options.newId}-content`;
                this.options.closeId = `${this.options.newId}-close`;
                this.options.closeContainerId = `${this.options.newId}-close-container`;
            }

            buildOverlay(this.options);

            pushOverlay(this.options);

            if (this.options.closeId && !this.options.noClose) {
                var closeId = this.options.closeId;
                var overlayId = this.options.overlayId;
            }
        }

        Overlay.prototype.getOptions = function () {
            return this.options;
        }

        // Convenient access to methods when the popup or Overlay isn't available
        window.menMethods = $.extend({}, window.menMethods,
            {
                removePopup: removePopup
            });

        return {
            Overlay: Overlay,
            removePopup: removePopup
        }

    })();

    window.mshPageApp.progressService = (function () {

        var app = window.mshPageApp;
        var pop = app.popupService;

        function showProgress(content, id) {
            var newId = id ? id : 'men-progress';
            var popup = new pop.Overlay({
                newId: newId,
                noClose: true,
                content: content,
                top: '20%'
            });
            $('#men-progress-content').css('margin-top', 0);
            return popup.getOptions();
        }

        function hideProgress(id) {
            var newId = id ? id : 'men-progress';
            pop.removePopup(`${newId}`);
        }

        return {
            showProgress: showProgress,
            hideProgress: hideProgress
        };
    }());

}(jQuery))