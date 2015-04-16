$(function() {
    var cookieConsent = $('#CookieConsent');
    var closeButton = $('#ConsentClose');

    if (cookieConsent.length === 0) return;

    var originalBottomPadding = $('body').css('padding-bottom');

    $('body').css({ 'padding-bottom': cookieConsent.height()+'px' });

    var closeNotification = function () {
        cookieConsent.hide();
        $('body').css({ 'padding-bottom': originalBottomPadding });
    }

    closeButton.on('click', function() {
        document.cookie = 'cookieconsent=true';
        closeNotification();
    });

}) 