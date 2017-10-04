(function(){
    'use strict'

    angular
        .module('app')
        .controller('SessionsController', SessionsController)

    SessionsController.$inject = ['$scope'];

    function SessionsController($scope){
        debugger;
        $scope.message = 'Hi'
    }

})();