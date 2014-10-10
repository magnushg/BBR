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

    function sendAutentisertPost(url, successCallback, data) {
        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            beforeSend: setAuthorizationHeader
        })
        .done(successCallback)
        .fail(function(jqXHR, exception) {
            if (jqXHR.status === 0) {
                alert('Not connect.\n Verify Network.');
            } else if (jqXHR.status == 404) {
                alert('Requested page not found. [404]');
            } else if (jqXHR.status == 500) {
                alert('Internal Server Error [500].');
            } else if (exception === 'parsererror') {
                alert('Requested JSON parse failed.');
            } else if (exception === 'timeout') {
                alert('Time out error.');
            } else if (exception === 'abort') {
                alert('Ajax request aborted.');
            } else {
                alert('Uncaught Error.\n' + jqXHR.responseText);
            }
        });
    }
    
    function setAuthorizationHeader(xhr) {
        xhr.setRequestHeader('Authorization', 'Basic Ym91dmV0Om15c2VjcmV0');
    }

    return {
        sendAutentisertRequest: function (url, type, successCallback) {
            sendAutentisertRequest(url, type, successCallback);
        },
        
        sendAutentisertPost: function (url, successCallback, data) {
            sendAutentisertPost(url, successCallback, data);
        },

        sendRequest: function (url, type, successCallback) {
            sendRequest(url, type, successCallback);
        }
    }

}($));