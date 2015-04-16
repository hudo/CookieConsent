$(function() {
    var cookieConsent = $('#CookieConsent');
    var closeButton = $('#ConsentClose');

    var originalBottomPadding = $('body').css('padding-bottom');

    $('body').css({ 'padding-bottom': '125px' });

    var closeNotification = function () {
        cookieConsent.hide();
        $('body').css({ 'padding-bottom': originalBottomPadding });
    }

    closeButton.on('click', function() {
        document.cookie = 'cookieconsent=true';
        closeNotification();
    });

}) 