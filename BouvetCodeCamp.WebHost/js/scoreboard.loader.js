var loader = (function ($, infrastruktur) {
    'use strict';

    function hentLag(onSuccessCallback) {
        infrastruktur.sendAutentisertRequest("/api/admin/lag/get", "GET", onSuccessCallback);
    }

    function hentPoster(onSuccessCallback) {
        infrastruktur.sendAutentisertRequest("/api/admin/post/get", "GET", onSuccessCallback);
    } 

    function hentInfisertSone(onSuccessCallback) {
        infrastruktur.sendAutentisertRequest("/api/admin/infisert/get", "GET", onSuccessCallback);
    }

    return {
        hentLag: function (onSuccessCallback) {
            hentLag(onSuccessCallback);
        },

        hentPoster: function (onSuccessCallback) {
            hentPoster(onSuccessCallback);
        },

        hentInfisertSone: function (onSuccessCallback) {
            hentInfisertSone(onSuccessCallback);
        }
    }

}($, infrastruktur));