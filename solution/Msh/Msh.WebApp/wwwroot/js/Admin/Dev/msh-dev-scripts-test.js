(function ($) {

    var app = window.mshPageApp;
    var meth = app.methodsService;
    var modal = app.modalService;
    var mas = app.modalActionService;

    var ids = {
        selectHotel: '#selectHotel'
    }

    var apiRoot = '/api/hotelapi';

    var options = {
        modalActionId: 'confirm-action',
        modalActionTitle: 'Confirm Action',
        modalActionBody: 'Confirm that an action should be carried out.',
        modalActionOnCLick: `id="confirm-load-ok" onclick="window.mshMethods.testConfirmed()"`,
        modalActionOk: 'OK',
        loadConfirmApiUrl: `${apiRoot}/RatePlanSortLoad`,

        modalActionedId: 'confirmed-action',
        modalActionedTitle: 'Confirmed Action',
        modalActionedBody: 'The action has been carried out.',
        modalActionedOnCLick: `id="confirmed-load-ok"onclick="window.mshMethods.testConfirmedDone()"`,
        modalActionedFooterOk: true
        // modalPublishedOk: 'OK',
       
    }

    meth.extendMethods({

        testConfirm: function () {
            loadPair.action(options);
        },
        testConfirmed: function () {
            $(`#${options.modalActionId}`).remove();
            loadPair.actioned(options);
        },
        testConfirmedDone: function () {
            $(`#${options.modalActionedId}`).remove();
            modal.showModal('testFinalDone', 'Test Final Done', 'The final test complete');
        }
    });

    var loadPair = new mas.PairOverlay(options);

    $('#show-modal').on('click', (e) => {

        e.preventDefault();

        var body = '';
        for (var i = 0; i < 50; i++) {
            body += 'Test body';
        }

        modal.showModal('xx1', 'Test Title', body);
    });

    $('#show-overlayl').on('click', (e) => {

        e.preventDefault();

        window.mshMethods.testConfirm();

        
    });

}(jQuery));