var loader = (function ($, infrastruktur) {
    'use strict';

    function lastInnLag(onSuccessCallback) {
        infrastruktur.sendAutentisertRequest("/api/admin/lag/get", "GET", onSuccessCallback);
    }

    function lastInnPoster(onSuccessCallback) {
        infrastruktur.sendAutentisertRequest("/api/admin/post/get", "GET", onSuccessCallback);
    } 

    function lastInnInfisertSone(onSuccessCallback) {
        infrastruktur.sendAutentisertRequest("/api/admin/infisert/get", "GET", onSuccessCallback);
    }

    return {
        lastInnLag: function (onSuccessCallback) {
            lastInnLag(onSuccessCallback);
        },

        lastInnPoster: function (onSuccessCallback) {
            lastInnPoster(onSuccessCallback);
        },

        lastInnInfisertSone: function (onSuccessCallback) {
            lastInnInfisertSone(onSuccessCallback);
        }
    }

}($, infrastruktur));