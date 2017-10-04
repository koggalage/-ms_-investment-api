(function(){
    'use strict'

    angular
    .module('app')
    .controller('CustomersController', CustomersController);

    CustomersController.$inject = ['$scope'];
    function CustomersController($scope){
        $scope.message = 'Hello';

        $('.datepicker').datepicker({
            startDate: '-3d'
        });

    }

})();