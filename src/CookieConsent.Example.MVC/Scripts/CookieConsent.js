$(function() {
    var cookieConsent = $('#CookieConsent');
    var closeButton = $('#ConsentClose');

    var closeNotification = function () {
        cookieConsent.hide();
    }

    closeButton.on('click', function() {
        document.cookie = 'cookieconsent=true';
        closeNotification();
    });

}) 