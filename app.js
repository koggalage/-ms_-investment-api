(function(){

    'use strict'

    var module = angular.module('app',
                                    [
                                        'ui.router'
                                    ]
                                )
                    .config(config)
                    .run(run)

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider){

        $urlRouterProvider.otherwise('/app');

        $stateProvider
            .state('login',{
                url: '/sessions/new',
                templateUrl: '/app-components/sessions/sessions.view.html',
                controller: 'SessionsController'
            })

            .state('app',{
                url:'/app',
                views: {
                    '@':{
                        templateUrl: '/app-components/app/app.view.html',
                    },
                    'header@app':{
                        templateUrl:'/app-components/common/header.html'
                    }
                }
            })

            .state('app.customer',{
                url:'/customer',
                templateUrl:'/app-components/customers/customer.add.customer.html',
                controller:'CustomersController'
            })

            .state('app.contract',{
                url:'/contract/:customer_id',
                templateUrl:'/app-components/contracts/contracts.new.contract.view.html',
                controller:'ContractsController'
            })

    }

    run.$inject = ['$rootScope', '$location', '$http'];
    function run($rootScope, $location, $http){

        $rootScope.isUserLoggedIn = function () {
            if ($rootScope.globals.currentUser) {
                return true;
            }
            else {
                return false;
            }
        }

        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            //start loading bar with white background for state changes 
            //$rootScope.loadingBarChange();

            // redirect to login page if not logged in and trying to access a restricted page
           // var restrictedPage = $.inArray($location.path(), ['/sessions/new', '/register', '/', '']) === -1;
            //var loggedIn = $rootScope.globals.currentUser;
            //if (restrictedPage && !loggedIn) {
                //$location.path('/app');
            //}
        });

    }

})();