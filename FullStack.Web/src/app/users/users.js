angular.module('ngBoilerplate.users', [
    'ui.router',
    'placeholders',
    'ui.bootstrap'
])
    .config(function config($stateProvider) {
        $stateProvider.state('users', {
            url: '/users',
            views: {
                "main": {
                    controller: 'UsersCtrl',
                    templateUrl: 'users/users.tpl.html'
                }
            },
            data: {pageTitle: 'Users'},
            resolve: {
                users: ['$http', function ($http) {
                    return $http.get('http://api.fullstack.co.uk/api/accounts/users').then(function (results) {
                        return results.data;
                    });
                }]
            }
        });
    })

    .controller('UsersCtrl', function UsersCtrl($scope, users) {

        $scope.vm = $scope.vm || {};
        $scope.vm.users = users;
    })
;
