(function(){
    'use strict'

    angular
    .module('app')
    .controller('ContractsController', ContractsController);

    ContractsController.$inject = ['$scope'];
    function ContractsController($scope){
        $scope.message = 'Hello';


    }

})();