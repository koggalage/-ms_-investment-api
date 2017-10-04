(function(){
    'use strict'

    angular
        .module('app')
        .controller('AppController', AppController);

    AppController.$inject = ['$scope'];
    function AppController($scope){
        $scope.message = 'Hello';
    }

})();