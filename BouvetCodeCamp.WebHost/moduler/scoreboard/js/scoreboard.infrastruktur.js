var infrastruktur = (function($) {
    'use strict';

    function sendAutentisertRequest(url, type, successCallback) {
        $.ajax({
            url: url,
            type: type,
            dataType: 'json',
            success: successCallback,
            beforeSend: setAuthorizationHeader
        });
    }

    function setAuthorizationHeader(xhr) {
        xhr.setRequestHeader('Authorization', 'Basic Ym91dmV0Om15c2VjcmV0');
    }

    return {
        sendAutentisertRequest: function (url, type, successCallback) {
            sendAutentisertRequest(url, type, successCallback);
        }
    }

}($));