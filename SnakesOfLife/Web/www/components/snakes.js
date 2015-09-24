/**
 * Angular.js SnakesOfLife Controller
 */

angular.module('SnakesOfLife', ['ngRoute', 'ngAnimate'])
    .config(function($routeProvider) {
        $routeProvider

            .when('/', {
                templateUrl: 'main.html'
            })

            // route for the home page
            .when('/simulate', {
                templateUrl : 'simulate.html',
                controller: 'SnakesOfLifeCtrl'
            })

            // route for the about page
            .when('/research', {
                templateUrl: 'research.html',
                controller: 'researchController'
            })

            // route for the contact page
            .when('/about', {
                templateUrl: 'about.html'
            });
    })
    .controller('SnakesOfLifeCtrl', [
        '$scope', '$http', '$timeout', function($scope, $http, $timeout) {

            $scope.parameters = { NeededAliveNeighborsTurnsToGrow: 5, SnakeCellsForGrow: 5, SnakeLengthForSplit: 5, SnakeLengthToStop: 5, SnakeTurnToDie: 5, SnakeTurnsToShrink: 5 };

            $scope.snakesNum = 0;
            $scope.turnsNum = 0;
            $scope.stopping = false;

            $scope.grass = [['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live']];

            $scope.back = function () {
                $('#nvParams').hide();
                $scope.reset();
                window.history.back();
            };

            $scope.reset = function() {
                $scope.stopping = true;
                $scope.gameGuid = null;
                $scope.turnsNum = 0;
                $scope.snakesNum = 0;
                $scope.started = false;
                $scope.grass = [['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live'], ['Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live', 'Live']];
            };

            // Compute the next step
            $scope.ruleTheWorld = function() {
                // do ajax get
                if ($scope.gameGuid == null)
                    return;

                var dataObj = {};

                $http.put('https://snakesoflife.azurewebsites.net/api/run/' + $scope.gameGuid, dataObj).
                    success(function (data) {
                        if (!$scope.stopping) {
                            $scope.turnsNum++;
                            $scope.snakesNum = data.Snakes.length;
                            //console.log(data.GrassCellsState);
                            $scope.grass = data.GrassCellsState;
                            if (data.HasEnded) {
                                alert("Run Simulation Has Ended");
                                $scope.gameGuid = null;
                                $scope.started = false;
                            }
                        }
                    }).
                    error(function(data, status, headers, config) {
                        console.log(data);
                    });
            };

            // State of 'auto-step' mode
            $scope.started = false;

            // One step
            var step = function() {
                if ($scope.started) {
                    $scope.ruleTheWorld();
                }

                $timeout(step, 200);
            };

            // start 'auto-step' mode
            $scope.start = function() {

                // call ajax to post the parameters
                var dataObj = $scope.parameters;

                // if we allready have a game guid - continue
                if ($scope.gameGuid == null) {

                    // Simple POST request example (passing data) :
                    $http.post('https://snakesoflife.azurewebsites.net/api/run', dataObj).
                        success(function(data) {
                            // we get game guid
                            $scope.gameGuid = data;
                            $scope.turnsNum = 0;
                            $scope.snakesNum = 0;
                            $scope.stopping = false;
                        }).
                        error(function(data, status, headers, config) {
                            console.log("error ocurred - " + data);
                            console.log("Status - " + status);
                            console.log("Headers - " + headers);
                        });
                }

                step();
                $scope.started = true;
            };

            // stop 'auto-step' mode
            $scope.stop = function() {
                $scope.started = false;
            };
        }
    ])
.controller('researchController', [
        '$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

            $scope.started = false;
            $scope.averageTurns = null;
            $scope.parameters = { NeededAliveNeighborsTurnsToGrow: null, SnakeCellsForGrow: null, SnakeLengthForSplit: null, SnakeLengthToStop: null, SnakeTurnToDie: null, SnakeTurnsToShrink: null };

            $scope.back = function () {
                $scope.stop();
                window.history.back();
            };
            // Compute the next step
            $scope.getStatus = function () {
                // do ajax get
                if ($scope.gameGuid == null)
                    return;

                $http.get('https://snakesoflife.azurewebsites.net/api/simulation/' + $scope.gameGuid).
                    success(function (data) {
                        console.log(data);
                        $scope.averageTurns = data.AverageTurns;
                        $scope.parameters = data.Params;
                        $timeout(getSimulationStatus, 2000);
                        //if (!$scope.stopping) {
                        //    $scope.turnsNum++;
                        //    $scope.snakesNum = data.Snakes.length;
                        //    //console.log(data.GrassCellsState);
                        //    $scope.grass = data.GrassCellsState;
                        //    if (data.HasEnded) {
                        //        alert("Run Simulation Has Ended");
                        //        $scope.gameGuid = null;
                        //        $scope.started = false;
                        //    }
                        //}
                    }).
                    error(function (data, status, headers, config) {
                        console.log(data);
                    });
            };

            // One step
            var getSimulationStatus = function () {
                if ($scope.started) {
                    $scope.getStatus();
               
                }
            };

            // start 'auto-step' mode
            $scope.start = function () {

                $scope.started = true;

                // call ajax to post the parameters
                var dataObj = {};

                // if we allready have a game guid - continue
                if ($scope.gameGuid == null) {

                    // Simple POST request example (passing data) :
                    $http.put('https://snakesoflife.azurewebsites.net/api/simulation', dataObj).
                        success(function (data) {
                            // we get game guid
                            $scope.gameGuid = data;
                            getSimulationStatus();
                        }).
                        error(function (data, status, headers, config) {
                            console.log("error ocurred - " + data);
                            console.log("Status - " + status);
                            console.log("Headers - " + headers);
                            $scope.started = false;
                        });
                }
            };

            // stop 'auto-step' mode
            $scope.stop = function () {
                $scope.started = false;
                $scope.gameGuid = null;
            };

        $('[data-toggle="popover"]').popover();
    }
]);