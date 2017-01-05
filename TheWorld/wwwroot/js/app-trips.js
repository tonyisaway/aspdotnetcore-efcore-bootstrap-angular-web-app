(function () {
    "use strict";
    angular.module("app-trips", []);

    var app = angular.module("myApp", ["simpleControls"]);
    app.directive("w3TestDirective", waitCursor);

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);



    angular.bootstrap(document.getElementById("App2"), ["myApp"]);


    function waitCursor() {
        return {
            templateUrl: "/views/waitCursor.html"
        };
    }
})();