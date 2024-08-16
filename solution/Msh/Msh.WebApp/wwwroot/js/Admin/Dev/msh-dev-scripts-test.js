(function ($) {

    var app = window.mshPageApp;
    var modal = app.modalService;

    $('#show-modal').on('click', (e) => {

        e.preventDefault();

        var body = '';
        for (var i = 0; i < 50; i++) {
            body += 'Test body';
        }

        modal.showModal('xx1', 'Test Title', body);
    });

}(jQuery));