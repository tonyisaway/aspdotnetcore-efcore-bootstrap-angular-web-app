﻿(function () {
    "use strict";

    var tripsController = function ($http) {

        var vm = this;

        vm.trips = [];

        vm.newTrip = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips")
            .then(function(response) {
                    // Success
                    angular.copy(response.data, vm.trips);
                },
                function(error) {
                    // Failure
                    vm.errorMessage = "Failed to load data: " + error;
                })
            .finally(function() {
                vm.isBusy = false;
            });

        vm.addTrip = function() {

            vm.isBusy = false;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
                .then(function (response) {
                    // Success
                    vm.trips.push(response.data);
                    vm.newTrip = {};
                },
                function (error) {
                    // Failure
                    vm.errorMessage = "Failed to save new trip";
                })
            .finally(function () {
                vm.isBusy = false;
            });
        };
    }

    // Getting the module
    angular.module("app-trips", [])
    .controller("tripsController", tripsController);

})();